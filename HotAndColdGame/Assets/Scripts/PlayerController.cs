using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState {ControlsDisabled, MoveAndLook, MoveOnly}
    public PlayerState playerControlState = PlayerState.MoveAndLook;
    public bool _useRigidBody = true;
    public List<string> playerInventory;
    public RayCastShootComplete raygunScript;

    public PlayerFPControls controls;    
    public float movementSpeed = 10f;
    public float interactRange = 2f;
    public bool isGravityEnabled = true;
    public bool isGunEnabled = true;
    private bool isGrounded;
    private Vector3 playerVelocity;

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

    private void Awake() 
    {   
        //playerCharController = GetComponent<CharacterController>();
        playerRB = GetComponent<Rigidbody>();

        playerInventory = new List<string>();
        controls = new PlayerFPControls();
        controls.Player.Interact.performed += context => Interact(context);
        controls.Player.Interact.canceled += ExitInteract;
        controls.Player.Jump.performed += Jump;

        controls.Player.Shoot.performed += raygunScript.FireBeam;
        controls.Player.Shoot.canceled += raygunScript.FireBeam;
        controls.Player.SwapBeam.performed += raygunScript.SwapBeam;
        controls.Enable(); 

        if (isGunEnabled)
            playerInventory.Add("Raygun");
        LockCursor();
    }

    private void Update() 
    {   
        switch (playerControlState)
        {
            case (PlayerState.ControlsDisabled):
                //ResetMouse();
                break;
            case (PlayerState.MoveAndLook):
                GroundedCheck();
                MouseLook(controls.Player.Look.ReadValue<Vector2>());
                MovePlayer(controls.Player.Movement.ReadValue<Vector2>());
                break;
            case (PlayerState.MoveOnly):
                GroundedCheck();
                MovePlayer(controls.Player.Movement.ReadValue<Vector2>());
                //ResetMouse();
                break;
            default:
                playerControlState = PlayerState.MoveAndLook;
                break;
        }        

        // TODO: Add distance + rotation restriction on interacting, so can't keep interacting if too far / not looking at it 
   
        SetShootingEnabled(playerInventory.Contains("Raygun"));

        if (currentInteractingObject != null)
        {
            currentInteractingObject.OnInteracting();
        }      
    }

    // Input functions

    void MovePlayer(Vector2 stickMovementVector)
    {
        // Translate 2d analog movement to 3d vector movement
        //Debug.Log(stickMovementVector);
 
        playerVelocity = playerRB.velocity;            
        Vector3 movementVector = new Vector3 (stickMovementVector.x, 0f, stickMovementVector.y);
        movementVector = transform.TransformDirection(movementVector).normalized;
        movementVector = movementVector.normalized;

        playerRB.AddForce(movementVector * movementSpeed, ForceMode.Acceleration);
    
    }

    void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            //Debug.Log("Jump attempted");
            playerRB.AddForce(Vector3.up * 6f, ForceMode.VelocityChange);
        }
    }

    void SetShootingEnabled(bool setToEnable)
    {
        if (setToEnable)
        {
            controls.Player.Shoot.Enable();
            controls.Player.SwapBeam.Enable();
            raygunScript.gameObject.SetActive(true);
        }
        else
        {
            controls.Player.Shoot.Disable();
            controls.Player.SwapBeam.Disable();
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

            currentInteractingObject.OnInteractEnter(controls);

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
        isGrounded = Physics.CheckSphere(groundChecker.position, 0.01f, -1, QueryTriggerInteraction.Ignore);
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
}
