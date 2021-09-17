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
        [Range(0f, 50f)] public float stoppingForce;
        [Range(0f, 50f)] public float reverseForce;

        public playerMovementSettings(float movementSpeed, float speedCapSmoothFactor, float jumpStrength, float airSpeed, Vector2 velocityCap, float antiGravMod, float stoppingForce, float reverseForce)
        {
            this.movementSpeed = movementSpeed;
            this.speedCapSmoothFactor = speedCapSmoothFactor;
            this.jumpStrength = jumpStrength;
            this.airSpeed = airSpeed;
            this.velocityCap = velocityCap;
            this.stoppingForce = stoppingForce;
            this.reverseForce = reverseForce;
        }
    }

    [Header("Player Control Settings")]
    public playerMovementSettings currentMovementSettings = new playerMovementSettings(20f, 15f, 10f, 0.5f, new Vector2(8f, 20f), 1f, 1.2f, 1.5f);
    public float interactRange = 2f;   
    public float coyoteTimer = 0.5f;
    private float currentCoyoteTimer; 
    private float jumpBufferTimer = 0f;
    public float maxStepHeight = 0.4f;        // The maximum a player can set upwards in units when they hit a wall that's potentially a step
    public float stepSearchOvershoot = 0.01f; // How much to overshoot into the direction a potential step in units when testing. High values prevent player from walking up tiny steps but may cause problems.

    [Range(0f, 90f)]public float slopeWalkingLimit = 45f;
    public float antiGravMod = 1f;  
    
    //public float timeBeforeFrictionReturns = 0.2f;
    //private float currentTimeBeforeFrictionReturns = 0f;

    private ContactPoint ledgeContact;
    private Vector3 ledgePos;
    private Vector3 horizVelocity;
    private Vector3 vertVelocity;
    private Vector3 prevVelocity;
    private List<ContactPoint> contactPoints;

    [Header("Player Condition Control Settings")]
    public playerMovementSettings baseMovementSettings = new playerMovementSettings(20f, 15f, 10f, 0.5f, new Vector2(8f, 20f), 1f, 1.2f, 1.5f);
    public playerMovementSettings hotMovementSettings = new playerMovementSettings(20f, 3f, 14f, 0.75f, new Vector2(6f, 10f), 1f, 1.2f, 1.5f);
    public playerMovementSettings coldMovementSettings = new playerMovementSettings(24f, 15f, 10f, 0.5f, new Vector2(16f, 20f), 1f, 1.2f, 1.5f);
    public playerMovementSettings bothMovementSettings = new playerMovementSettings(24f, 15, 14, 0.75f, new Vector2 (16f, 10f), 1f, 1.2f, 1.5f);

    [Header("Player State Settings")]
    public bool isGravityEnabled = true;
    public bool isGunEnabled = true;   
    [SerializeField] 
    public bool isGrounded;
    private bool isClimbing = false;

    private bool isWalking = false;
    public float walkingMultiplier = 0.5f;
    private ContactPoint groundContactPoint;
    public PlayerState playerControlState = PlayerState.MoveAndLook;
    public PlayerState playerControlState_copy = PlayerState.MoveAndLook;
    public List<string> playerInventory;
    [SerializeField] private List<IConditions.ConditionTypes> _activeConditions;
    private bool isConditionChanging = false;
    private bool isPaused = false;
    private bool isInitialised = false;

    [Header("References")]    
    public RayCastShootComplete raygunScript;
    public PlayerInput playerInput;
    public PlayerMouseLook playerMouseLook;
    public PlayerSoundControl playerSoundControl;
    public PlayerCameraControl playerCamControl;
    private Rigidbody playerRB;
    //public Transform groundChecker;
    private TemperatureStateBase playerTemp;
    private InteractableBase currentInteractingObject;
    public PauseController PC;
    //public AudioManager audioManager;
    public PhysicMaterial icyPhysicMaterial; //Physics mat for slippery effect
    public PhysicMaterial regularPhysicMaterial;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody>();
        playerTemp = GetComponent<TemperatureStateBase>();
        playerMouseLook = GetComponent<PlayerMouseLook>();   
        playerSoundControl = GetComponent<PlayerSoundControl>();
        playerCamControl = GetComponent<PlayerCameraControl>();     
        _activeConditions = new List<IConditions.ConditionTypes>();
        contactPoints = new List<ContactPoint>();
        playerInventory = new List<string>();        

        playerInput.actions.FindAction("Interact").performed += context => Interact(context);
        playerInput.actions.FindAction("Interact").canceled += ExitInteract;
        playerInput.actions.FindAction("Jump").performed += JumpInput;
        playerInput.actions.FindAction("Shoot").performed += raygunScript.FireBeam;
        playerInput.actions.FindAction("Shoot").canceled += raygunScript.FireBeam;
        playerInput.actions.FindAction("Swap Beam").performed += raygunScript.SwapBeam;

        playerInput.ActivateInput();
        currentMovementSettings = baseMovementSettings;

        LockCursor();
    }

    private void Start()
    {
        SetShootingEnabled(isGunEnabled);        

        if (isGunEnabled)
        {
            playerInventory.Add("Raygun");
            GetComponent<GunFXController>().EquipTool(false);
        }

        playerRB.angularDrag = 100f;
        regularPhysicMaterial.dynamicFriction = 0f;
        regularPhysicMaterial.staticFriction = 0f;
        regularPhysicMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
        jumpBufferTimer = 0f;
        currentCoyoteTimer = coyoteTimer;

        playerMouseLook.ResetMouse(transform);
        //playerMouseLook.MouseAbsolute = new Vector2 (transform.eulerAngles.y, 0f);   

        // set initial stating state
        playerControlState = PlayerState.ControlsDisabled;
        // set controls to enable after a delay

        StartCoroutine(EnableControls(1f));
    }

    IEnumerator EnableControls(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerControlState = PlayerState.MoveAndLook;
        isInitialised = true;
    }

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        playerCamControl.UpdateFOVBasedOnSpeed(playerRB.velocity.magnitude);        
        //Debug.Log(playerRB.velocity.magnitude);

        if (PC != null)
        {
            if (PC.GetPause())
            {
                if(!isPaused)
                {
                    Debug.Log("PAUSED");
                    playerControlState_copy = playerControlState;
                    playerControlState = PlayerState.ControlsDisabled;
                    UnlockCursor();
                    isPaused = true;
                }
            }
            else
            {

                if (isPaused)
                {
                    playerControlState = playerControlState_copy;
                    LockCursor();
                    isPaused = false;
                }
                //playerControlState = PlayerState.MoveAndLook;
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
                playerMouseLook.MouseLook(playerInput.actions.FindAction("Look").ReadValue<Vector2>(), playerCamControl.playerCam);
                //ProgressStepCycle(1.5f);
                break;
            case (PlayerState.MoveOnly):
                if (playerInput.currentActionMap == playerInput.actions.FindActionMap("Menu"))
                    playerInput.currentActionMap = playerInput.actions.FindActionMap("Player");
                //ProgressStepCycle(1.5f);
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
        Vector3 stepUpOffset = default(Vector3);

        ExecuteConditions();
        RemoveConditionsIfReturningToNeutral();
                
        isGrounded = FindGround(out groundContactPoint, contactPoints);  
        Jump();  

        switch (playerControlState)
        {
            case (PlayerState.ControlsDisabled):
                break;
            case (PlayerState.MoveAndLook):
                MovePlayer(playerInput.actions.FindAction("Movement").ReadValue<Vector2>());               
                break;
            case (PlayerState.MoveOnly):
                MovePlayer(playerInput.actions.FindAction("Movement").ReadValue<Vector2>());
                //ResetMouse();
                break;
            default:
                break;
        } 

        if (GameMaster.instance.audioManager != null)
            playerSoundControl.CalculateTimeToFootstep(horizVelocity, (currentCoyoteTimer > 0 || isGrounded));
        ToggleGravity(!isGrounded);
        VelocityClamp();
        SetPlayerFrictionAndDrag();
        if (FindStep(out stepUpOffset) && currentCoyoteTimer > 0)
            StepUp(stepUpOffset);
        ClamberLedge();
        CoyoteTimeTick();
        contactPoints.Clear();
        prevVelocity = playerRB.velocity;
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
        
        if (isWalking && (coyoteTimer > 0 || isGrounded))
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
            movementVector = Vector3.ProjectOnPlane(movementVector, groundContactPoint.normal);

            // Opposite gravity force, calculated based on incline, so gravity doesn't stop you from moving up and down
            //Vector3 inverseGravProportion = -Physics.gravity * (1 + Vector3.Dot(Physics.gravity.normalized, groundContactPoint.normal.normalized));
            //Vector3 inverseGravProportion = -Physics.gravity;
            
            playerRB.AddForce(movementVector * currentMovementSettings.movementSpeed + -groundContactPoint.normal, ForceMode.Acceleration);
            //Debug.DrawRay(transform.position, movementVector * 100);
            //Debug.Log("Inverse Grav component is :" + inverseGravProportion);
        }
        else if (currentCoyoteTimer > 0)
        {
            playerRB.AddForce(movementVector * currentMovementSettings.movementSpeed, ForceMode.Acceleration);
        }
        else
        {      
            if ((horizVelocity + movementVector).magnitude <= currentMovementSettings.velocityCap.x || 
                Vector3.Dot(horizVelocity, movementVector) <= 0)
            {
                playerRB.AddForce(movementVector * currentMovementSettings.movementSpeed * currentMovementSettings.airSpeed, ForceMode.Acceleration);
            }              
        }        
    }

    void Jump()
    {
        if ((isGrounded || currentCoyoteTimer > 0) && jumpBufferTimer > 0) 
        {
            // If moving down, cancel downward force.
            if (playerRB.velocity.y < 0f)
            {
                playerRB.velocity = horizVelocity;
                vertVelocity = Vector3.zero;
                Debug.Log("Player's velocity is now: " + playerRB.velocity);
                
            }
                
            transform.position += Vector3.up * 0.1f;   //To remove grounded contact
            contactPoints.Clear();
            playerRB.AddForce(Vector3.up * currentMovementSettings.jumpStrength, ForceMode.VelocityChange);
            currentCoyoteTimer = 0; 
            jumpBufferTimer = 0;
            isGrounded = false;        
        }   
        else
        {
            jumpBufferTimer = (jumpBufferTimer - Time.deltaTime);
        }  
    }

    void JumpInput(InputAction.CallbackContext context)
    {
        jumpBufferTimer = 0.15f;
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
        Vector3 horForward = transform.forward;
        
        horForward.y = 0f;
        horForward.Normalize();

        foreach(ContactPoint cp in contactPoints)
        {
            // If we're hitting the wall at no more than a 45 degree angle...
            if (Vector3.Angle(horForward, -cp.normal) <= 45)
            {
                //Debug.Log("Wall at less than 45 degrees detected");
                bool ledgeAvailable = true;
                
                RaycastHit hit;                

                // If falling too fast, can't climb
                if (vertVelocity.magnitude < -10f)
                {
                    ledgeAvailable = false;
                    //Debug.Log("Falling too fast, ledge unavailable!");
                }

                // If the wall's too tall, can't climb
                if (Physics.Raycast(transform.position + Vector3.up * 3f, Vector3.Scale(-cp.normal, new Vector3 (1f, 0f, 1f)),
                    out hit, 2f, LayerMask.GetMask("Default")))  
                {
                    //Debug.Log("Wall is too tall, ledge unavailable!");
                    //Debug.DrawRay(transform.position + Vector3.up * 1f, -cp.normal);
                    ledgeAvailable = false;
                }

                if (ledgeAvailable)
                {
                    // Start high and keep casting rays until we hit the wall, finding the ledge. If too low, don't clamber
                    //Debug.Log("Ledge is available!");                
                    ledgePos = transform.position + Vector3.up * 1f + Vector3.down * 0.05f;
                    while (!Physics.Raycast(ledgePos, -cp.normal, out hit, 1, LayerMask.GetMask("Default")))
                    {
                        ledgePos += Vector3.down * 0.05f;
                        if (ledgePos.y < transform.position.y + 0.5f)
                            //return;
                            break;
                    }
                    
                    if (playerInput.actions.FindAction("Jump").ReadValue<float>() > 0f && isClimbing == false)
                    {
                        // Cancel all vertical velocity, apply a big upward force once
                        isClimbing = true;
                        //playerRB.velocity = horizVelocity;
                        playerRB.velocity = Vector3.zero;
                        //Debug.Log("Direction force is applied in: " + (currentPos - transform.position));
                        //Vector3 dirToLedge = ledgePos - transform.position;
                        //Vector3 forceDir = Vector3.Max(dirToLedge, dirToLedge.normalized) * 14f; //+ dirToLedge;
                        //playerRB.AddForce(forceDir, ForceMode.VelocityChange);
                        ledgeContact = cp;
                        if (GameMaster.instance.audioManager != null)
                            playerSoundControl.PlayClamberingAudio();
                        Invoke("ClimbingCooldownReset", 1f);
                    }                             
                }
            }
        }

        // If the player is climbing, keep applying upward force. If obstacle is cleared vertically, 
        // apply some forward force and consider them no longer climbing
        if (isClimbing == true)  
        {   
            playerRB.velocity = Vector3.zero;

            Vector3 playerFeetPos = new Vector3 (transform.position.x,
                transform.position.y - (GetComponent<CapsuleCollider>().height / 2), transform.position.z);     
            Vector3 dirToLedge = ledgePos - playerFeetPos + Vector3.up;

            playerRB.AddForce(dirToLedge * 3f, ForceMode.VelocityChange);

            if (playerFeetPos.y > ledgePos.y - 0.1f)
            {
                //Debug.Log("Forward force applied!");
                playerRB.velocity = Vector3.zero;
                Vector3 dirToWall = Vector3.Scale(-ledgeContact.normal, new Vector3 (1f, 0f, 1f));
                playerRB.AddForce(dirToWall.normalized * 1.5f , ForceMode.VelocityChange);
                CancelInvoke("ClimbingCooldownReset");
                ClimbingCooldownReset();                
            }                    
        }            
    }

    void ClimbingCooldownReset()
    {
        isClimbing = false;
        ledgePos = transform.position;
    }  

    bool FindStep(out Vector3 stepUpOffset)  
    {
        stepUpOffset = default(Vector3);

        // Can't step up if player is not moving.
        if (horizVelocity.sqrMagnitude < 0.01f)
        {
            //Debug.Log("Player's not moving");
            return false;
        }

        foreach (ContactPoint cp in contactPoints)
        {
            bool test = ResolveStepUp(out stepUpOffset, cp);
            if (test)
                return test;
        }
        //Debug.Log("No valid step detected");
        return false;
    }

    bool ResolveStepUp(out Vector3 stepUpOffset, ContactPoint stepTestCP)
    {
        stepUpOffset = default(Vector3);
        Collider stepCol = stepTestCP.otherCollider;

        // Check if contact point normal matches that of a short wall (low Y value)
        if (Mathf.Abs(stepTestCP.normal.y) <= 0.01f)
        {   
            //Debug.Log("Contact point does not match that of a wall, step failed.");
            return false;
        }

        // Make sure contact point is not the ground
        if (stepTestCP.point == groundContactPoint.point)
        {
            return false;
        }
            
        
        // Make sure contact point is low enough to be a step
        if (!(stepTestCP.point.y - groundContactPoint.point.y < maxStepHeight))
        {
            //Debug.Log("Contact point is too tall, step failed.");
            return false;
        }
            
        
        // Check to see if there's a place to step up to
        RaycastHit hitInfo;
        float stepHeight = groundContactPoint.point.y + maxStepHeight + 0.0001f;
        Vector3 stepTestInvDir = new Vector3(-stepTestCP.normal.x, 0, -stepTestCP.normal.z).normalized;
        Vector3 origin = new Vector3(stepTestCP.point.x, stepHeight, stepTestCP.point.z) + (stepTestInvDir * stepSearchOvershoot);
        Vector3 direction = Vector3.down;        
        if( !(stepCol.Raycast(new Ray(origin, direction), out hitInfo, maxStepHeight)) )
        {
            //Debug.Log("No place detected to step up to, step failed.");
            return false;
        }

        Vector3 stepUpPoint = new Vector3 (stepTestCP.point.x, hitInfo.point.y + 0.0001f, 
            stepTestCP.point.z) + (stepTestInvDir * stepSearchOvershoot);
        Vector3 stepUpPointOffset = stepUpPoint - new Vector3(stepTestCP.point.x, groundContactPoint.point.y, 
            stepTestCP.point.z);
        stepUpOffset = stepUpPointOffset;
        //Debug.Log("Step up successful!");
        return true;
    }

    void StepUp(Vector3 stepUpOffset)
    {
        transform.position += stepUpOffset;
        playerRB.velocity = prevVelocity;
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
        if (Physics.Raycast(playerCamControl.playerCam.transform.position, playerCamControl.playerCam.transform.forward, out RaycastHit hit, interactRange) && 
            hit.collider.gameObject.GetComponent<InteractableBase>() != null && playerCamControl != null)
        {
            //Debug.Log("Interactable object found, attempting interaction.");
            Debug.DrawLine(playerCamControl.playerCam.transform.position, hit.point);
            currentInteractingObject = hit.collider.gameObject.GetComponent<InteractableBase>();

            //currentInteractingObject.OnInteractEnter(playerInput);

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
                        /* GetComponent<GunFXController>().weaponState = GunFXController.WeaponState.Grab;
                         GetComponent<GunFXController>().NextState();*/

                        float animTime = 0.61f;
                        float animTime_place = 3.5f;

                        playerInventory.Add(currentInteractingObject.GetComponent<CollectInteractable>().itemName);

                        if (currentInteractingObject.name.Contains("Raygun"))
                        {                           
                            GetComponent<GunFXController>().Grab();
                            currentInteractingObject.GetComponent<CollectInteractable>().OnInteractEnter(playerInput, animTime);
                            playerControlState = PlayerState.ControlsDisabled;
                            StartCoroutine(CollectItem("Raygun", animTime));

                        }else if (currentInteractingObject.name.Contains("Journal"))
                        {
                            currentInteractingObject.GetComponent<CollectInteractable>().OnInteractEnter(playerInput, 0);
                            playerControlState = PlayerState.ControlsDisabled;
                            StartCoroutine(CollectItem("Journal", 0));

                        } else if (currentInteractingObject.name.Contains("Toolbox"))
                        {
                            if (playerInventory.Contains("Raygun") && raygunScript.gunUpgradeState != RayCastShootComplete.gunUpgrade.Two)
                            {
                                //GetComponent<GunFXController>().Grab(currentInteractingObject.GetComponent<CollectInteractable>());
                                GetComponent<GunFXController>().PlaceTool();
                                currentInteractingObject.GetComponent<CollectInteractable>().OnInteractEnter(playerInput, animTime_place);
                                playerControlState = PlayerState.ControlsDisabled;
                                StartCoroutine(CollectItem("Toolbox", animTime_place));
                            }
                        }

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

    IEnumerator CollectItem(string item, float delay)
    {
        yield return new WaitForSeconds(delay);

        switch (item)
        {
            case "Raygun":
                CollectRaygun();
                CollectItem();
                break;

            case "Journal":
                CollectJournal();
                CollectItem();
                break;

            case "Toolbox":
                CollectItem();
                break;
        }
     
    }

    void CollectItem()
    {
        playerControlState = PlayerState.MoveAndLook;
    }

    void CollectRaygun()
    {
        SetShootingEnabled(playerInventory.Contains("Raygun"));
        if (playerInventory.Contains("Raygun"))
            GetComponent<GunFXController>().EquipTool();
    }

    void CollectJournal()
    {
        //playerControlState = PlayerState.MoveAndLook;
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

    // Utility Functions below
    bool FindGround(out ContactPoint groundCP, List<ContactPoint> contactPoints)
    {
        groundCP = default(ContactPoint);
        bool found = false;

        foreach (ContactPoint cp in contactPoints)
        {
            // Pointing in some upward direction            
            if (cp.normal.y > slopeWalkingLimit / 90f && (found == false || cp.normal.y > groundCP.normal.y))
            {
                groundCP = cp;
                found = true;                
            }
        }

        return found;
    }    

    /// <summary>
    /// Clamps the Velocity based on the player's current movement settings. Horizontal velocity clamp
    /// only occurs on the ground, and scales based on movement inputs (as joysticks provide incremental input.)
    /// </summary>
    void VelocityClamp()
    {    
        if (isGrounded || currentCoyoteTimer > 0)
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

    // Uses forces fur the purposes of player control
    void SetPlayerFrictionAndDrag()
    {        
        Vector3 movementDirection = new Vector3 (playerInput.actions.FindAction("Movement").ReadValue<Vector2>().x, 0f, playerInput.actions.FindAction("Movement").ReadValue<Vector2>().y);        
        float moveDirDotProd = Vector3.Dot(transform.TransformVector(movementDirection.normalized), horizVelocity.normalized);

        // Use DOT product as sliding scale of force I.e. Between 0 and 1, apply less force, between -1 and 0, apply most force
        // Basically, closer DOT is to 1, the closer the player is to moving in the same direction as their current velocity.
        if (isGrounded || currentCoyoteTimer > 0)
        {
            if (moveDirDotProd >= 0f && moveDirDotProd < 1f)
            {
                playerRB.velocity = Vector3.Lerp(transform.TransformDirection(movementDirection.normalized) * horizVelocity.magnitude, horizVelocity, 
                    Time.deltaTime * 50 / currentMovementSettings.stoppingForce) + vertVelocity;
            }
            else if (moveDirDotProd < 0f)
            {
                playerRB.velocity = Vector3.Lerp(Vector3.zero, horizVelocity, Time.deltaTime * 50 / currentMovementSettings.reverseForce) + vertVelocity;
            }      
        }
        else
        {
            if (movementDirection.magnitude < 0.05f)
            {
                return;
            }

            if (moveDirDotProd >= 0f && moveDirDotProd < 1f)
            {
                playerRB.velocity = Vector3.Lerp(transform.TransformDirection(movementDirection.normalized) * horizVelocity.magnitude, horizVelocity, 
                    Time.deltaTime * 50 / currentMovementSettings.stoppingForce * 5) + vertVelocity;
            }
            else if (moveDirDotProd < 0f)
            {
                playerRB.velocity = Vector3.Lerp(Vector3.zero, horizVelocity, Time.deltaTime * 50 / currentMovementSettings.reverseForce * 5) + vertVelocity;
            }      
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
        // If player is hot
        if (ActiveConditions.Contains(IConditions.ConditionTypes.ConditionHot))
        {
            UpwardForce();
            //ResetSlip();
        }

        // If player is cold
        if (ActiveConditions.Contains(IConditions.ConditionTypes.ConditionCold))
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
        // If player is Hot and NOT Cold
        if (ActiveConditions.Contains(IConditions.ConditionTypes.ConditionHot) && !ActiveConditions.Contains(IConditions.ConditionTypes.ConditionCold))
        {
            currentMovementSettings = hotMovementSettings;
        }
        // If player is cold and NOT hot
        else if (ActiveConditions.Contains(IConditions.ConditionTypes.ConditionCold) && !ActiveConditions.Contains(IConditions.ConditionTypes.ConditionHot))
        {   
            currentMovementSettings = coldMovementSettings;         
        }
        // If player is BOTH Cold and Hot
        else if (ActiveConditions.Contains(IConditions.ConditionTypes.ConditionCold) && ActiveConditions.Contains(IConditions.ConditionTypes.ConditionHot))
        {
            currentMovementSettings = bothMovementSettings;   
        }
        else
        {
            currentMovementSettings = baseMovementSettings;   
        }
    }

    public void UpwardForce()
    {
        if (!isGrounded)
        {
            playerRB.AddForce(-Physics.gravity * antiGravMod * Time.deltaTime * 25f, ForceMode.Acceleration);
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

    private void ToggleGravity(bool gravValueToSet)
    {
        playerRB.useGravity = gravValueToSet;
    }

    private void CoyoteTimeTick()
    {
        if (isGrounded)
        {
            currentCoyoteTimer = coyoteTimer;            
        }
        else
        {
            currentCoyoteTimer = Mathf.Clamp(currentCoyoteTimer - Time.deltaTime, 0, coyoteTimer);
            //Debug.Log("Player left the ground!");
        }
    }
}


/* Legacy Functions
    void GroundedCheck()
    {
        //isGrounded = Physics.CheckSphere(groundChecker.position, 0.01f, -1, QueryTriggerInteraction.Ignore);
        //isGrounded = Physics.SphereCast(groundChecker.position, GetComponent<CapsuleCollider>().radius, Vector3.down, out RaycastHit hit, 1f);
        bool potentialGroundFound = Physics.SphereCast(transform.position, GetComponent<CapsuleCollider>().radius - 0.01f, 
            Vector3.down, out RaycastHit hit, (GetComponent<CapsuleCollider>().height / 2 + 0.01f));

        Debug.Log(Vector3.Dot(hit.normal, Vector3.up));
        if (Vector3.Dot(hit.normal, Vector3.up) > 0.6f)
        {
            groundedHit = hit;
            isGrounded = true;
        }
        else
        {
            groundedHit = emptyRaycast;
            isGrounded = false;
        }
            
        if (isGrounded)
            groundedHit = hit;
        else
            groundedHit = emptyRaycast;
        
        //Debug.DrawRay(groundChecker.position, Vector3.down * GetComponent<CapsuleCollider>().height);
    } 

    void SetPlayerFriction
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