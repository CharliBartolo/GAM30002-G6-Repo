using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IConditions
{
    private float deltaTime = 0.0f;
    [HideInInspector]
    public enum PlayerState { ControlsDisabled, MoveAndLook, MoveOnly }

    [System.Serializable]
    public struct playerMovementSettings
    {
        public float movementSpeed;
        public float speedCapSmoothFactor;
        public float jumpStrength;
        public float airSpeed;
        public Vector2 velocityCap;
        public float antiGravMod;
        [Range(0f, 50f)] public float stoppingForce;
        [Range(0f, 50f)] public float reverseForce;

        public playerMovementSettings(float movementSpeed, float speedCapSmoothFactor, float jumpStrength, float airSpeed, Vector2 velocityCap, float antiGravMod, float stoppingForce, float reverseForce)
        {
            this.movementSpeed = movementSpeed;
            this.speedCapSmoothFactor = speedCapSmoothFactor;
            this.jumpStrength = jumpStrength;
            this.airSpeed = airSpeed;
            this.velocityCap = velocityCap;
            this.antiGravMod = antiGravMod;
            this.stoppingForce = stoppingForce;
            this.reverseForce = reverseForce;
        }
    }

    [Header("Player Control Settings")]
    public playerMovementSettings currentMovementSettings = new playerMovementSettings(20f, 15f, 10f, 0.02f, new Vector2(8f, 20f), 1f, 1.2f, 1.5f);
    public float interactRange = 2f;
    public float timeBetweenFootsteps = 1f;
    private float currentTimeBetweenFootsteps = 0f;
    public float timeBeforeFrictionReturns = 0.2f;
    private float currentTimeBeforeFrictionReturns = 0f;
    private Vector3 horizVelocity;
    private Vector3 vertVelocity;
    private List<ContactPoint> contactPoints;

    // Mouse Control Settings   
    public Vector2 mouseSensitivity = new Vector2(1, 1);
    public Vector2 mouseSmoothing = new Vector2(2, 2);
    const float MIN_X = 0.0f;
    const float MAX_X = 360.0f;
    const float MIN_Y = -90.0f;
    const float MAX_Y = 90.0f;
    private Vector2 _mouseAbsolute;
    private Vector2 _mouseSmooth;

    [Header("Player Condition Control Settings")]
    public playerMovementSettings baseMovementSettings = new playerMovementSettings(20f, 15f, 10f, 0.02f, new Vector2(8f, 20f), 1f, 1.2f, 1.5f);
    public playerMovementSettings hotMovementSettings = new playerMovementSettings(20f, 3f, 14f, 0.15f, new Vector2(6f, 10f), 1f, 1.2f, 1.5f);
    public playerMovementSettings coldMovementSettings = new playerMovementSettings(24f, 15f, 10f, 0.02f, new Vector2(16f, 20f), 1f, 1.2f, 1.5f);

    [Header("Player State Settings")]
    public bool isGravityEnabled = true;
    public bool isGunEnabled = true;   
    [SerializeField] 
    private bool isGrounded;
    private bool isClimbing = false;

    private bool isWalking = false;
    public float walkingMultiplier = 0.5f;
    private RaycastHit groundedHit;
    private RaycastHit emptyRaycast;
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
    public AudioManager audioManager;
    public PhysicMaterial icyPhysicMaterial; //Physics mat for slippery effect
    public PhysicMaterial regularPhysicMaterial;


    private void Awake()
    {
        playerRB = GetComponent<Rigidbody>();
        playerTemp = GetComponent<TemperatureStateBase>();
        _activeConditions = new List<IConditions.ConditionTypes>();
        contactPoints = new List<ContactPoint>();
        playerInventory = new List<string>();

        emptyRaycast = new RaycastHit();

        playerInput.actions.FindAction("Interact").performed += context => Interact(context);
        playerInput.actions.FindAction("Interact").canceled += ExitInteract;
        playerInput.actions.FindAction("Jump").performed += Jump;
        playerInput.actions.FindAction("Shoot").performed += raygunScript.FireBeam;
        playerInput.actions.FindAction("Shoot").canceled += raygunScript.FireBeam;
        playerInput.actions.FindAction("Swap Beam").performed += raygunScript.SwapBeam;

        playerInput.ActivateInput();

        LockCursor();
    }

    private void Start()
    {
        SetShootingEnabled(isGunEnabled);

        if (isGunEnabled)
        {
            playerInventory.Add("Raygun");
            GetComponent<GunFXController>().EquipTool();
        }


        playerRB.angularDrag = 100f;
    }

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
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
        horizVelocity = Vector3.Scale(playerRB.velocity, new Vector3(1f, 0f, 1f));
        vertVelocity = Vector3.Scale(playerRB.velocity, new Vector3(0f, 1f, 0f));

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

        if (audioManager != null)
            PlayFootstepSound();
        VelocityClamp();
        SetPlayerFriction();
        ClamberLedge();
        //ContactPoint groundContact = default(ContactPoint);
        //bool grounded = FindGround(out groundContact, contactPoints);
        contactPoints.Clear();
    }

    // Input functions
    void MovePlayer(Vector2 stickMovementVector)
    {
        // Translate 2d analog movement to 3d vector movement            
        Vector3 movementVector = new Vector3(stickMovementVector.x, 0f, stickMovementVector.y);

        //Walk button changes
        if (!isWalking)
        {
            playerInput.actions.FindAction("Walk").performed +=
            walk =>
            {
                isWalking = true;
                //Debug.Log("True");
            };
        }
        else
        {
            playerInput.actions.FindAction("Walk").canceled +=
            walk =>
            {
                isWalking = false;
                //Debug.Log("False");
            };
        }
        
        if (isWalking && isGrounded)
        {               
            //Debug.Log("Normal: " + movementVector);
            movementVector *= walkingMultiplier;
            //Debug.Log("Walk: " + movementVector);
     
        }        
        
        movementVector = transform.TransformDirection(movementVector);
        //Debug.Log("Normal2: " + movementVector); 

        // If movement vector greater than one, reduce magnitude to one, otherwise leave untouched (in case of analog stick input)
        if (movementVector.magnitude > 1f)
        {
            movementVector = movementVector.normalized;
        } 

        if (isGrounded)
        {           
            // Player's movement input projected on surface, to enable moving up / down ramps 
            movementVector = Vector3.ProjectOnPlane(movementVector, groundedHit.normal);

            // Opposite gravity force, calculated based on incline, so gravity doesn't stop you from moving up and down
            Vector3 inverseGravProportion = -Physics.gravity * (1 + Vector3.Dot(Physics.gravity.normalized, groundedHit.normal.normalized));

            
            playerRB.AddForce(movementVector * currentMovementSettings.movementSpeed + inverseGravProportion, ForceMode.Acceleration);
               
            //Debug.DrawRay(transform.position, movementVector * 100);
            //Debug.Log("Inverse Grav component is :" + inverseGravProportion);
        }
        else
        {                     
            playerRB.AddForce(movementVector * currentMovementSettings.movementSpeed * currentMovementSettings.airSpeed, ForceMode.Acceleration);
        }        
    }

    void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            //Debug.Log("Jump attempted");
            //playerRB.AddForce(Vector3.up * jumpStrength, ForceMode.VelocityChange);
            playerRB.AddForce(Vector3.up * currentMovementSettings.jumpStrength, ForceMode.VelocityChange);
        }
    }
    
    private void OnCollisionStay(Collision other) 
    {  
        contactPoints.AddRange(other.contacts);      
    }

    private void OnCollisionEnter(Collision other) 
    {
        contactPoints.AddRange(other.contacts);
    }

    void ClamberLedge()
    {
        //Debug.Log("Collision is happening");
        //Vector3 normal = other.GetContact(0).normal;
        Vector3 horForward = playerCam.transform.forward;
        horForward.y = 0f;
        horForward.Normalize();

        foreach(ContactPoint cp in contactPoints)
        {
            // If we're hitting the wall at no more than a 45 degree angle...
            if (Vector3.Angle(horForward, -cp.normal) <= 45)
            {
                bool ledgeAvailable = true;
                
                RaycastHit hit;

                // If falling too fast, can't climb
                if (vertVelocity.magnitude < -10f)
                {
                    ledgeAvailable = false;
                }

                // If the wall's too tall, can't climb
                if (Physics.Raycast(playerCam.transform.position + Vector3.up * 0.5f, -cp.normal, out hit, 1, LayerMask.GetMask("Default")))
                {
                    ledgeAvailable = false;
                }

                if (ledgeAvailable)
                {
                    //Debug.Log("Ledge is available!");                
                    Vector3 currentPos = playerCam.transform.position + Vector3.up * 0.5f + Vector3.down * 0.05f;
                    while (!Physics.Raycast(currentPos, -cp.normal, out hit, 1, LayerMask.GetMask("Default")))
                    {
                        currentPos += Vector3.down * 0.05f;
                        if (currentPos.y < playerCam.transform.position.y - 2f)
                            break;
                    }
                    
                    if (playerInput.actions.FindAction("Jump").ReadValue<float>() > 0f && isClimbing == false)
                    {
                        // Cancel all vertical velocity, apply a big upward force once
                        isClimbing = true;
                        playerRB.velocity = horizVelocity;
                        //Debug.Log("Direction force is applied in: " + (currentPos - transform.position));
                        Vector3 forceDir = Vector3.Max(currentPos - transform.position, Vector3.zero) * 10f;
                        playerRB.AddForce(forceDir, ForceMode.VelocityChange);
                        Invoke("ClimbingCooldownReset", 1f);
                    }                    
                }
            }
        }        
    }

    void ClimbingCooldownReset()
    {
        isClimbing = false;
    }

    bool FindGround(out ContactPoint groundCP, List<ContactPoint> contactPoints)
    {
        groundCP = default(ContactPoint);
        bool found = false;

        foreach (ContactPoint cp in contactPoints)
        {
            // Pointing in some upward direction
            if (cp.normal.y > 0.0001f && (found == false || cp.normal.y > groundCP.normal.y))
            {
                groundCP = cp;
                found = true;
            }
        }

        return found;
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
                        SetShootingEnabled(playerInventory.Contains("Raygun"));
                        if (playerInventory.Contains("Raygun"))
                            GetComponent<GunFXController>().EquipTool();
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

    // Sound Functions Below
    void PlayFootstepSound()
    {
        //Debug.Log(currentTimeBetweenFootsteps);
        if (horizVelocity.magnitude > 0f && isGrounded)
        {
            currentTimeBetweenFootsteps -= 5f * horizVelocity.magnitude * Time.deltaTime;
        }   
        else
        {
            currentTimeBetweenFootsteps = 0.01f;
        }

        if (currentTimeBetweenFootsteps <= 0f)
        {
            audioManager.Play("Footstep");
            
            currentTimeBetweenFootsteps = timeBetweenFootsteps;
        }
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
            groundedHit = emptyRaycast;
        //Debug.DrawRay(groundChecker.position, Vector3.down * GetComponent<CapsuleCollider>().height);
    } 

    /// <summary>
    /// Clamps the Velocity based on the player's current movement settings. Horizontal velocity clamp
    /// only occurs on the ground, and scales based on movement inputs (as joysticks provide incremental input.)
    /// </summary>
    void VelocityClamp()
    {    
        if (isGrounded)
        {
            // Multiply horizontal velocity cap by movement input to limit top speed based on varying stick inputs / walk button
            float movementInputMultiplier = playerInput.actions.FindAction("Movement").ReadValue<Vector2>().magnitude;

            if (movementInputMultiplier > 1)
            {
                movementInputMultiplier = 1f;
            }

            if (isWalking)
            {
                movementInputMultiplier = 1 * walkingMultiplier;
            }

            if (horizVelocity.magnitude > currentMovementSettings.velocityCap.x * movementInputMultiplier && currentMovementSettings.velocityCap.x > 0f)
            { 
                // First Exp value = smoothing factor, higher values = no smooth, lower = more smooth 
                horizVelocity = Vector3.Lerp(horizVelocity, horizVelocity.normalized * currentMovementSettings.velocityCap.x * movementInputMultiplier,
                    1 - Mathf.Exp(-currentMovementSettings.speedCapSmoothFactor * Time.deltaTime));                                                 
            }
        }   
        
        if (vertVelocity.magnitude > currentMovementSettings.velocityCap.y && currentMovementSettings.velocityCap.y > 0f)
        { 
            // First Exp value = smoothing factor, higher values = no smooth, lower = more smooth 
            vertVelocity = Vector3.Lerp(vertVelocity, vertVelocity.normalized * currentMovementSettings.velocityCap.y, 
                1 - Mathf.Exp(-currentMovementSettings.speedCapSmoothFactor * Time.deltaTime));            
        }

        playerRB.velocity = horizVelocity + vertVelocity;
    }

    // TODO: Use forces instead of friction for player control
    void SetPlayerFriction()
    {        
        if (isGrounded)
        {
            Vector3 movementDirection = new Vector3 (playerInput.actions.FindAction("Movement").ReadValue<Vector2>().x, 0f, playerInput.actions.FindAction("Movement").ReadValue<Vector2>().y);       
            
            float moveDirDotProd = Vector3.Dot(transform.TransformVector(movementDirection.normalized), horizVelocity.normalized);

            // Use DOT product as sliding scale of force I.e. Between 0 and 1, apply less force, between -1 and 0, apply most force
            // Basically, closer DOT is to 1, the closer the player is to moving in the same direction as their current velocity.
            if (moveDirDotProd >= 0f && moveDirDotProd < 0.9f)
            {
                playerRB.velocity = Vector3.Lerp(transform.TransformDirection(movementDirection.normalized) * horizVelocity.magnitude, horizVelocity, 
                    Time.deltaTime * 50 / currentMovementSettings.stoppingForce) + vertVelocity;
            }
            else if (moveDirDotProd < 0f)
            {
                playerRB.velocity = Vector3.Lerp(Vector3.zero, horizVelocity, Time.deltaTime * 50 / currentMovementSettings.reverseForce) + vertVelocity;
            }
        }

        /*
        // Player should have no friction when either airborne or providing movement input.
        // Should probably enable friction if trying to move opposite current direction?
        if (!isGrounded || playerInput.actions.FindAction("Movement").ReadValue<Vector2>().magnitude > 0)
        {
            //Debug.Log("Friction disabled");
            regularPhysicMaterial.staticFriction = 0f;
            regularPhysicMaterial.dynamicFriction = 0f;
            currentTimeBeforeFrictionReturns = timeBeforeFrictionReturns;
        }
        else
        {
            currentTimeBeforeFrictionReturns -= Time.deltaTime;

            if (currentTimeBeforeFrictionReturns <= 0f)
            {
                //Debug.Log("Friction enabled");
                regularPhysicMaterial.staticFriction = playerFriction[0];
                regularPhysicMaterial.dynamicFriction = playerFriction[1];
            }
        }   
        */
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
        //float msec = deltaTime * 1000f;
        float fps = 1.0f / deltaTime;
        //float avgFramerate = Time.frameCount / Time.time;

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
        GUILayout.Label(fps + " frames per second");

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
            currentMovementSettings = hotMovementSettings;
            // movementSpeed = baseMovementSpeed * hotMoveSpeedMod;            
            // jumpStrength = baseJumpStrength * hotJumpStrengthMod;
            // airSpeed = baseAirSpeed * hotAirSpeedMod;  
            // velocityCap = baseVelocityCap * hotVelocityCapMod;    
        }
        // If player is cold and NOT hot
        else if (ActiveConditions.Contains(IConditions.ConditionTypes.ConditionCold) && !ActiveConditions.Contains(IConditions.ConditionTypes.ConditionHot))
        {   
            currentMovementSettings = coldMovementSettings;
            // movementSpeed = baseMovementSpeed * coldMoveSpeedMod;            
            // jumpStrength = baseJumpStrength * coldJumpStrengthMod;
            // airSpeed = baseAirSpeed * coldAirSpeedMod; 
            // velocityCap = baseVelocityCap * coldVelocityCapMod;          
        }
        else
        {
            currentMovementSettings = baseMovementSettings;
            // movementSpeed = baseMovementSpeed;            
            // jumpStrength = baseJumpStrength;
            // airSpeed = baseAirSpeed;  
            // velocityCap = baseVelocityCap;    
        }
    }

    public void UpwardForce()
    {
        if (GetComponent<Rigidbody>() != null)
        {
            GetComponent<Rigidbody>().AddForce(-Physics.gravity * currentMovementSettings.antiGravMod * Time.deltaTime * 25f, ForceMode.Acceleration);
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

