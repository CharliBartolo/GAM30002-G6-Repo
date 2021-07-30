using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IConditions
{
    [HideInInspector]
    public enum PlayerState {ControlsDisabled, MoveAndLook, MoveOnly}    

    [Header("Player Control Settings")]    
    public float movementSpeed = 20f;
    public float speedCapSmoothFactor = 20f;
    public float jumpStrength = 10f;
    public float airSpeed = 0.02f; 
    public Vector2 velocityCap = new Vector2 (6f, 20f); 
    public float hotGravMod = 1f;
    public float interactRange = 2f;
    private Vector3 horizVelocity;
    private Vector3 vertVelocity;
    private float[] playerFriction = new float[2];

    [Header("Player Condition Control Settings")]
    public float baseMovementSpeed = 20f;
    public float coldMoveSpeedMod = 1.2f;
    public float hotMoveSpeedMod = 1f;

    public float baseAirSpeed = 0.02f;
    public float coldAirSpeedMod = 1f;
    public float hotAirSpeedMod = 5f;

    public float baseJumpStrength = 10f;
    public float hotJumpStrengthMod = 1.4f;
    public float coldJumpStrengthMod = 1f;

    public Vector2 baseVelocityCap = new Vector2 (8f, 20f);
    public Vector2 coldVelocityCapMod = new Vector2 (2f, 1f);
    public Vector2 hotVelocityCapMod = new Vector2 (0.75f, 0.5f);   

    public float baseSpeedCapSmoothFactor = 15f;
    public float hotSpeedCapSmoothMod = 0.2f; 
    public float coldSpeedCapSmoothMod = 1f; 

    [Header("Player State Settings")]
    public bool isGravityEnabled = true;
    public bool isGunEnabled = true;
    [SerializeField] private bool isGrounded;
    private bool isClimbing = false;
    private RaycastHit groundedHit;
    public PlayerState playerControlState = PlayerState.MoveAndLook;
    public List<string> playerInventory;
    [SerializeField] private List<IConditions.ConditionTypes> _activeConditions;
    private bool isConditionChanging = false;    

    [Header("References")]    
    public Camera playerCam;
    public RayCastShootComplete raygunScript;
    public PlayerInput playerInput;
    private Rigidbody playerRB;
    public Transform groundChecker; 
    private TemperatureStateBase playerTemp;   
    private InteractableBase currentInteractingObject;
    public PauseController PC;
    public PhysicMaterial icyPhysicMaterial; //Physics mat for slippery effect
    public PhysicMaterial regularPhysicMaterial;

    [Header("Mouse Control Settings")]    
    public Vector2 mouseSensitivity = new Vector2 (2, 2);
    public Vector2 mouseSmoothing = new Vector2 (3,3);        
    const float MIN_X = 0.0f;
    const float MAX_X = 360.0f;
    const float MIN_Y = -90.0f;
    const float MAX_Y = 90.0f;    
    private Vector2 _mouseAbsolute;
    private Vector2 _mouseSmooth;    

    private void Awake() 
    {   
        playerFriction[0] = regularPhysicMaterial.staticFriction;
        playerFriction[1] = regularPhysicMaterial.dynamicFriction;
        playerRB = GetComponent<Rigidbody>();
        playerTemp = GetComponent<TemperatureStateBase>();
        _activeConditions = new List<IConditions.ConditionTypes>();
        playerInventory = new List<string>();  

        baseMovementSpeed = movementSpeed;            
        baseJumpStrength = jumpStrength;
        baseAirSpeed = airSpeed;
        baseVelocityCap = velocityCap;               

        playerInput.actions.FindAction("Interact").performed += context => Interact(context);
        playerInput.actions.FindAction("Interact").canceled += ExitInteract;
        playerInput.actions.FindAction("Jump").performed += Jump;
        playerInput.actions.FindAction("Shoot").performed += raygunScript.FireBeam;
        playerInput.actions.FindAction("Shoot").canceled += raygunScript.FireBeam;
        playerInput.actions.FindAction("Swap Beam").performed += raygunScript.SwapBeam;

        playerInput.ActivateInput();      

        if (isGunEnabled)
            playerInventory.Add("Raygun");
        LockCursor();
    }

    private void Start()
    {
        SetShootingEnabled(playerInventory.Contains("Raygun"));
    }

    private void Update() 
    {
        //Debug.Log(playerRB.velocity.magnitude);

        if (PC != null)
        {
            if (PC.GetPause())
            {
                playerControlState = PlayerState.ControlsDisabled;
            }
            else
            {
                playerControlState = PlayerState.MoveAndLook;
            }
        }
        
        switch (playerControlState)
        {
            case (PlayerState.ControlsDisabled):
                if (playerInput.currentActionMap == playerInput.actions.FindActionMap("Player"))
                    playerInput.currentActionMap = playerInput.actions.FindActionMap("Menu");
                break;
            case (PlayerState.MoveAndLook):
                if (playerInput.currentActionMap == playerInput.actions.FindActionMap("Menu"))
                    playerInput.currentActionMap = playerInput.actions.FindActionMap("Player");                
                MouseLook(playerInput.actions.FindAction("Look").ReadValue<Vector2>());                
                break;
            case (PlayerState.MoveOnly):
                if (playerInput.currentActionMap == playerInput.actions.FindActionMap("Menu"))
                    playerInput.currentActionMap = playerInput.actions.FindActionMap("Player");                
                //ResetMouse();
                break;
            default:
                if (playerInput.currentActionMap == playerInput.actions.FindActionMap("Menu"))
                    playerInput.currentActionMap = playerInput.actions.FindActionMap("Player");
                playerControlState = PlayerState.MoveAndLook;
                break;
        }       

        // TODO: Add distance + rotation restriction on interacting, so can't keep interacting if too far / not looking at it  
        if (currentInteractingObject != null)
        {
            currentInteractingObject.OnInteracting();
        }
    }

    private void FixedUpdate() 
    {        
        ExecuteConditions();
        RemoveConditionsIfReturningToNeutral();       

        switch (playerControlState)
        {
            case (PlayerState.ControlsDisabled):                
                break;
            case (PlayerState.MoveAndLook):                
                GroundedCheck();                
                MovePlayer(playerInput.actions.FindAction("Movement").ReadValue<Vector2>());
                break;
            case (PlayerState.MoveOnly):                
                GroundedCheck();
                MovePlayer(playerInput.actions.FindAction("Movement").ReadValue<Vector2>());
                //ResetMouse();
                break;
            default:               
                break;
        }

        VelocityClamp();
        SetPlayerFriction();
    }

    // Input functions 

    void MovePlayer(Vector2 stickMovementVector)
    {
        // Translate 2d analog movement to 3d vector movement            
        Vector3 movementVector = new Vector3 (stickMovementVector.x, 0f, stickMovementVector.y);   
        movementVector = transform.TransformDirection(movementVector);

        // Reflect along surface if grounded?
        if (isGrounded)
        {
            movementVector = Vector3.ProjectOnPlane(movementVector, groundedHit.normal);
            Debug.DrawRay(transform.position, movementVector * 100);
        }
        else
        {            
            movementVector *= airSpeed;
        }

         // If movement vector greater than one, reduce magnitude to one, otherwise leave untouched (in case of analog stick input)
        if (movementVector.magnitude > 1f)
        {
            movementVector = movementVector.normalized;
        } 

        playerRB.AddForce(movementVector * movementSpeed, ForceMode.Acceleration);
    }

    void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            //Debug.Log("Jump attempted");
            playerRB.AddForce(Vector3.up * jumpStrength, ForceMode.VelocityChange);
        }
    }
    
    private void OnCollisionStay(Collision other) 
    {
        //Debug.Log("Collision is happening");
        Vector3 normal = other.GetContact(0).normal;
        Vector3 horForward = playerCam.transform.forward;
        horForward.y = 0f;
        horForward.Normalize();

        // If we're hitting the wall at no more than a 45 degree angle...
        if (Vector3.Angle(horForward, -normal) <= 45)
        {
            bool ledgeAvailable = true;
            
            RaycastHit hit;
            // If the wall's too tall, don't climb
            if (Physics.Raycast(playerCam.transform.position + Vector3.up * 0.5f, -normal, out hit, 1, LayerMask.GetMask("Default")))
            {
                ledgeAvailable = false;
            }

            if (ledgeAvailable)
            {
                Debug.Log("Ledge is available!");                
                Vector3 currentPos = playerCam.transform.position + Vector3.up * 0.5f + Vector3.down * 0.05f;
                while (!Physics.Raycast(currentPos, -normal, out hit, 1, LayerMask.GetMask("Default")))
                {
                    currentPos += Vector3.down * 0.05f;
                    if (currentPos.y < playerCam.transform.position.y - 2f)
                        break;
                }

                
                if (playerInput.actions.FindAction("Jump").ReadValue<float>() > 0f && isClimbing == false)
                {
                    //isClimbing = true;
                    //Debug.Log("Direction force is applied in: " + (currentPos - transform.position));
                    playerRB.AddForce(Vector3.up * 30, ForceMode.Impulse);
                }
                
            }
        }


    }
    
    void SetShootingEnabled(bool setToEnable)
    {
        if (setToEnable)
        {
            playerInput.actions.FindAction("Shoot").Enable();
            playerInput.actions.FindAction("Swap Beam").Enable();
            //raygunScript.gameObject.SetActive(true);
        }
        else
        {
            playerInput.actions.FindAction("Shoot").Disable();
            playerInput.actions.FindAction("Swap Beam").Disable();
            //raygunScript.gameObject.SetActive(false);
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward * interactRange, out RaycastHit hit) && 
            hit.collider.gameObject.GetComponent<InteractableBase>() != null)
        {
            //Debug.Log("Interactable object found, attempting interaction.");
            currentInteractingObject = hit.collider.gameObject.GetComponent<InteractableBase>();

            currentInteractingObject.OnInteractEnter(playerInput);

            switch (currentInteractingObject.pInteractionType)
            {
                case InteractableBase.InteractionType.Carry:
                    break;
                case InteractableBase.InteractionType.RotateOnly:
                    playerControlState = PlayerState.MoveOnly;
                    break;
                // Use object, trigger exit interaction, and remove object from script.
                case InteractableBase.InteractionType.Use:
                    if (currentInteractingObject.GetComponent<CollectInteractable>() != null) 
                    {
                        playerInventory.Add(currentInteractingObject.GetComponent<CollectInteractable>().itemName);
                    }                   
                    ExitInteract(context);
                    break;
                default:
                    break;
            }
            //currentInteractingObject.OnInteractEnter(controls);
        }
        else
        {
            //Debug.Log("Interaction failed, as no object was found capable of being interacted with.");
        }
    }

    private void ExitInteract(InputAction.CallbackContext context)
    {
        if (currentInteractingObject != null)
        {
            if (currentInteractingObject.pInteractionType == InteractableBase.InteractionType.RotateOnly)
            {
                playerControlState = PlayerState.MoveAndLook;
            }

            currentInteractingObject.OnInteractExit();
            currentInteractingObject = null;
        }
    }

    // Mouse Functions below
    Vector2 MouseSmooth(Vector2 deltaParam)
    {
        Vector2 mouseDelta = deltaParam;
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2 (mouseSensitivity.x * mouseSmoothing.x / 10f, mouseSensitivity.y * mouseSmoothing.y / 10f));

        // Interpolate mouse movement over time to smooth delta
        _mouseSmooth.x = Mathf.Lerp(_mouseSmooth.x, mouseDelta.x, 1f / mouseSmoothing.x);
        _mouseSmooth.y = Mathf.Lerp(_mouseSmooth.y, mouseDelta.y, 1f / mouseSmoothing.y);

        return _mouseSmooth;
    }

    void ResetMouse()
    {
        //_mouseAbsolute = Vector2.zero;
        _mouseSmooth = Vector2.zero;
    }

    private void MouseClamp()
    {
         // Manages and clamps X axis rotation        
        if (_mouseAbsolute.x < MIN_X)
            _mouseAbsolute.x += MAX_X;
        else if (_mouseAbsolute.x > MAX_X)
            _mouseAbsolute.x -= MAX_X;

        // Manages and clamps Y axis rotation
        if (_mouseAbsolute.y < MIN_Y)
            _mouseAbsolute.y = MIN_Y;
        else if (_mouseAbsolute.y > MAX_Y)
            _mouseAbsolute.y = MAX_Y;
    }

    void MouseLook(Vector2 deltaParam)
    {
        _mouseAbsolute += MouseSmooth(deltaParam);
        MouseClamp();

        transform.rotation = Quaternion.Euler(0f, _mouseAbsolute.x, 0f);
        playerCam.transform.rotation = Quaternion.Euler(-_mouseAbsolute.y, transform.eulerAngles.y, transform.eulerAngles.z);      
    }

    // Utility Functions below
    void GroundedCheck()
    {
        //isGrounded = Physics.CheckSphere(groundChecker.position, 0.01f, -1, QueryTriggerInteraction.Ignore);
        //isGrounded = Physics.SphereCast(groundChecker.position, GetComponent<CapsuleCollider>().radius, Vector3.down, out RaycastHit hit, 1f);
        isGrounded = Physics.SphereCast(transform.position, GetComponent<CapsuleCollider>().radius - 0.01f, 
            Vector3.down, out RaycastHit hit, (GetComponent<CapsuleCollider>().height / 2 + 0.01f));

        if (isGrounded)
            groundedHit = hit;
        else
            groundedHit = new RaycastHit();
        //Debug.DrawRay(groundChecker.position, Vector3.down * GetComponent<CapsuleCollider>().height);
    } 

    void VelocityClamp()
    {
        horizVelocity = Vector3.Scale(playerRB.velocity, new Vector3 (1f, 0f, 1f));
        vertVelocity = Vector3.Scale(playerRB.velocity, new Vector3 (0f, 1f, 0f));
        
        if (horizVelocity.magnitude > velocityCap.x && velocityCap.x > 0f)
        { 
            // First Exp value = smoothing factor, higher values = no smooth, lower = more smooth 
            horizVelocity = Vector3.Lerp(horizVelocity, horizVelocity.normalized * velocityCap.x, 1 - Mathf.Exp(-speedCapSmoothFactor * Time.deltaTime));            
        }

        if (vertVelocity.magnitude > velocityCap.y && velocityCap.y > 0f)
        { 
            // First Exp value = smoothing factor, higher values = no smooth, lower = more smooth 
            vertVelocity = Vector3.Lerp(vertVelocity, vertVelocity.normalized * velocityCap.y, 1 - Mathf.Exp(-speedCapSmoothFactor * Time.deltaTime));            
        }

        playerRB.velocity = horizVelocity + vertVelocity;
    }

    void SetPlayerFriction()
    {
        if (isGrounded)
        {
            if (regularPhysicMaterial.staticFriction < playerFriction[0] || regularPhysicMaterial.dynamicFriction < playerFriction[1])
            {
                regularPhysicMaterial.staticFriction = Mathf.Clamp(regularPhysicMaterial.staticFriction +
                    Mathf.Lerp(0f, playerFriction[0], Time.deltaTime), 0, playerFriction[0]);
                regularPhysicMaterial.dynamicFriction = Mathf.Clamp(regularPhysicMaterial.dynamicFriction +
                    Mathf.Lerp(0f, playerFriction[1], Time.deltaTime), 0, playerFriction[1]);
            }            
        }
        else
        {
            regularPhysicMaterial.staticFriction = 0f;
            regularPhysicMaterial.dynamicFriction = 0f;
        }
    }
 
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10f, 10f, Screen.width, Screen.height));
        string stringToShow = "Player Inventory: ";

        if (playerInventory.Count > 0)
        {            
            for (int i = 0; i < playerInventory.Count - 1; i++)
            {
                stringToShow += playerInventory[i] + ", ";
            }
            stringToShow += playerInventory[playerInventory.Count - 1];
        }
        else
        {
            stringToShow += "None";
        }
        
        GUILayout.Label(stringToShow);

        GUILayout.EndArea();
    }

    public void AddCondition(IConditions.ConditionTypes nameOfCondition)
    {
        if (!ActiveConditions.Contains(nameOfCondition))
        {
            _activeConditions.Add(nameOfCondition);
            isConditionChanging = true;
        }        
    }

    public void RemoveCondition(IConditions.ConditionTypes nameOfCondition)
    {
        if (ActiveConditions.Contains(nameOfCondition))
        {
            _activeConditions.Remove(nameOfCondition);
            isConditionChanging = true;
        }       
    }

    // In future, could edit list to store separate condition timers for each. For now, using isReturningToNeutral works.
    private void RemoveConditionsIfReturningToNeutral()
    {
        if (playerTemp.isReturningToNeutral)
        {
            RemoveCondition(IConditions.ConditionTypes.ConditionHot);
            RemoveCondition(IConditions.ConditionTypes.ConditionCold);
        }
    }

    public List<IConditions.ConditionTypes> ActiveConditions
    {
        get => _activeConditions;
        set
        {
            _activeConditions = value;
            isConditionChanging = true;
        }        
    }

    public void ExecuteConditions()
    {
        // If player is hot and NOT cold
        if (ActiveConditions.Contains(IConditions.ConditionTypes.ConditionHot) && !ActiveConditions.Contains(IConditions.ConditionTypes.ConditionCold))
        {
            UpwardForce();
            //ResetSlip();
        }

        // If player is cold and NOT hot
        else if (ActiveConditions.Contains(IConditions.ConditionTypes.ConditionCold) && !ActiveConditions.Contains(IConditions.ConditionTypes.ConditionHot))
        {
            /*
            // If player is making any movement inputs, remove friction, otherwise re-enable it.
            if (playerInput.actions.FindAction("Movement").ReadValue<Vector2>().magnitude > 0f)
                IcySlip();
            else
                ResetSlip();
            */
        }
        
        //if (!_prevConditions.Equals(_activeConditions))
        if (isConditionChanging)
        {
            Debug.Log("Conditions have changed!");
            UpdateControlSettingsBasedOnConditions();
            isConditionChanging = false;
        }    
    }

    public void UpdateControlSettingsBasedOnConditions()
    {
        if (ActiveConditions.Contains(IConditions.ConditionTypes.ConditionHot) && !ActiveConditions.Contains(IConditions.ConditionTypes.ConditionCold))
        {
            movementSpeed = baseMovementSpeed * hotMoveSpeedMod;            
            jumpStrength = baseJumpStrength * hotJumpStrengthMod;
            airSpeed = baseAirSpeed * hotAirSpeedMod;  
            velocityCap = baseVelocityCap * hotVelocityCapMod;    
        }
        // If player is cold and NOT hot
        else if (ActiveConditions.Contains(IConditions.ConditionTypes.ConditionCold) && !ActiveConditions.Contains(IConditions.ConditionTypes.ConditionHot))
        {   
            movementSpeed = baseMovementSpeed * coldMoveSpeedMod;            
            jumpStrength = baseJumpStrength * coldJumpStrengthMod;
            airSpeed = baseAirSpeed * coldAirSpeedMod; 
            velocityCap = baseVelocityCap * coldVelocityCapMod;          
        }
        else
        {
            movementSpeed = baseMovementSpeed;            
            jumpStrength = baseJumpStrength;
            airSpeed = baseAirSpeed;  
            velocityCap = baseVelocityCap;    
        }
    }

    public void UpwardForce()
    {
        if (GetComponent<Rigidbody>() != null)
        {
            GetComponent<Rigidbody>().AddForce(-Physics.gravity * hotGravMod * Time.deltaTime * 25f, ForceMode.Acceleration);
        }
    }

    public void IcySlip()
    {
        if (GetComponent<Collider>() != null)
        {
            GetComponent<Collider>().material = icyPhysicMaterial;            
        }
    }

    public void ResetSlip()
    {
        if (GetComponent<Collider>() != null)
        {
            GetComponent<Collider>().material = regularPhysicMaterial;
        }
    }
}

