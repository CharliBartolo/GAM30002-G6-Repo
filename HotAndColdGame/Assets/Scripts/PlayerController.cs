using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IConditions
{
    public enum PlayerState {ControlsDisabled, MoveAndLook, MoveOnly}
    public PlayerState playerControlState = PlayerState.MoveAndLook;
    public bool _useRigidBody = true;
    public List<string> playerInventory;
    public RayCastShootComplete raygunScript;

    //public PlayerFPControls controls;
   // public InputActionMap controls;
    public PlayerInput playerInput;    
    public float movementSpeed = 50f;
    public float velocityCap = 8f;
    public float interactRange = 2f;
    public bool isGravityEnabled = true;
    public bool isGunEnabled = true;
    [SerializeField]private bool isGrounded;
    private Vector3 lateralVelocity;

    //Mouse Control Variables
    public Camera playerCam;
    public Vector2 mouseSensitivity = new Vector2 (2, 2);
    public Vector2 mouseSmoothing = new Vector2 (3,3);        
    const float MIN_X = 0.0f;
    const float MAX_X = 360.0f;
    const float MIN_Y = -90.0f;
    const float MAX_Y = 90.0f;    
    private Vector2 _mouseAbsolute;
    private Vector2 _mouseSmooth;

    private Rigidbody playerRB;
    public Transform groundChecker;    
    //private CharacterController playerCharController;
    private InteractableBase currentInteractingObject;

    //Pause
    public PauseController PC;
    public bool pauseFunctionality = true;

    private void Awake() 
    {   
        //playerCharController = GetComponent<CharacterController>();
        playerRB = GetComponent<Rigidbody>();

        playerInventory = new List<string>();          
        //controls = new PlayerFPControls();
        
        //controls.Player.Interact.performed += context => Interact(context);
        //controls.Player.Interact.canceled += ExitInteract;
        //controls.Player.Jump.performed += Jump;
        //controls.Player.Shoot.performed += raygunScript.FireBeam;
        //controls.Player.Shoot.canceled += raygunScript.FireBeam;
        //controls.Player.SwapBeam.performed += raygunScript.SwapBeam;

        //controls.Enable(); 

        playerInput.actions.FindAction("Interact").performed += context => Interact(context);
        playerInput.actions.FindAction("Interact").canceled += ExitInteract;
        playerInput.actions.FindAction("Jump").performed += Jump;
        playerInput.actions.FindAction("Shoot").performed += raygunScript.FireBeam;
        playerInput.actions.FindAction("Shoot").canceled += raygunScript.FireBeam;
        playerInput.actions.FindAction("Swap Beam").performed += raygunScript.SwapBeam;

        playerInput.ActivateInput();      

        if (isGunEnabled)
            playerInventory.Add("Raygun");
        //LockCursor();
    }

    private void Update() 
    {
        if (pauseFunctionality)
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
                //ResetMouse();
                break;
            case (PlayerState.MoveAndLook):
                GroundedCheck();
                //MouseLook(controls.Player.Look.ReadValue<Vector2>());
                //MovePlayer(controls.Player.Movement.ReadValue<Vector2>());
                MouseLook(playerInput.actions.FindAction("Look").ReadValue<Vector2>());
                MovePlayer(playerInput.actions.FindAction("Movement").ReadValue<Vector2>());
                break;
            case (PlayerState.MoveOnly):
                GroundedCheck();
                MovePlayer(playerInput.actions.FindAction("Movement").ReadValue<Vector2>());
                //ResetMouse();
                break;
            default:
                playerControlState = PlayerState.MoveAndLook;
                break;
        }

        // TODO: Add distance + rotation restriction on interacting, so can't keep interacting if too far / not looking at it 
        SetShootingEnabled(playerInventory.Contains("Raygun"));
        VelocityCap();

        if (currentInteractingObject != null)
        {
            currentInteractingObject.OnInteracting();
        }        
    }

    // Input functions

    void MovePlayer(Vector2 stickMovementVector)
    {
        // Translate 2d analog movement to 3d vector movement        
            
        Vector3 movementVector = new Vector3 (stickMovementVector.x, 0f, stickMovementVector.y);

        // If movement vector greater than one, reduce magnitude to one, otherwise leave untouched (in case of analog stick input)
        //movementVector = transform.TransformDirection(movementVector).normalized;
        movementVector = transform.TransformDirection(movementVector);
        if (movementVector.magnitude > 1f)
        {
            movementVector = movementVector.normalized;
        }
                

        // If airborne, dampen movement force
        if (!isGrounded)
        {
            movementVector = movementVector.normalized * 0.02f;
        }  
        else
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
            playerRB.AddForce(Vector3.up * 12f, ForceMode.VelocityChange);
        }
    }

    void SetShootingEnabled(bool setToEnable)
    {
        if (setToEnable)
        {
            playerInput.actions.FindAction("Shoot").Enable();
            playerInput.actions.FindAction("Swap Beam").Enable();
            raygunScript.gameObject.SetActive(true);
        }
        else
        {
            playerInput.actions.FindAction("Shoot").Disable();
            playerInput.actions.FindAction("Swap Beam").Disable();
            raygunScript.gameObject.SetActive(false);
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
        isGrounded = Physics.SphereCast(groundChecker.position, GetComponent<CapsuleCollider>().radius, Vector3.down, out RaycastHit hit, 1f);
        Debug.DrawRay(groundChecker.position, Vector3.down * GetComponent<CapsuleCollider>().height);
    } 

    void VelocityCap()
    {
        lateralVelocity = Vector3.Scale(playerRB.velocity, new Vector3 (1f, 0f, 1f));
        Vector3 downwardVelocityVector = Vector3.Scale(playerRB.velocity, new Vector3 (0f, 1f, 0f));

        if (lateralVelocity.magnitude > velocityCap)
        {
            playerRB.velocity = Vector3.Lerp(lateralVelocity.normalized * velocityCap + downwardVelocityVector, playerRB.velocity, Time.deltaTime);            
            //playerRB.velocity = Vector3.Lerp(playerRB.velocity.normalized * velocityCap, playerRB.velocity, Time.deltaTime);
            //playerRB.velocity = playerRB.velocity.normalized * velocityCap;
        }
    }

    public void AddCondition(string conditionToAdd)
    {
        ActiveConditions.Add(conditionToAdd);
    }

    public void RemoveCondition(string conditionToRemove)
    {
        ActiveConditions.Remove(conditionToRemove);
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

    public List<string> ActiveConditions
    {
        get;
        set;
    }
}
