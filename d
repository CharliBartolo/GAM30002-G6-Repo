5ee06e095 (Charli            2021-04-15 12:43:35 +1000   1) using System.Collections;
5ee06e095 (Charli            2021-04-15 12:43:35 +1000   2) using System.Collections.Generic;
5ee06e095 (Charli            2021-04-15 12:43:35 +1000   3) using UnityEngine;
5ee06e095 (Charli            2021-04-15 12:43:35 +1000   4) using UnityEngine.InputSystem;
5ee06e095 (Charli            2021-04-15 12:43:35 +1000   5) 
3442de4df (Charli            2021-05-13 19:29:02 +1000   6) public class PlayerController : MonoBehaviour, IConditions
5ee06e095 (Charli            2021-04-15 12:43:35 +1000   7) {
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000   8)     [HideInInspector]
fb7fca17c (Charli            2021-08-02 18:12:13 +1000   9)     public enum PlayerState {ControlsDisabled, MoveAndLook, MoveOnly}   
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  10) 
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  11)     [System.Serializable]
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  12)     public struct playerMovementSettings
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  13)     {
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  14)         public float movementSpeed;
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  15)         public float speedCapSmoothFactor;
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  16)         public float jumpStrength;
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  17)         public float airSpeed; 
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  18)         public Vector2 velocityCap;
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  19)         public float antiGravMod; 
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  20) 
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  21)         public playerMovementSettings(float movementSpeed, float speedCapSmoothFactor, float jumpStrength, float airSpeed, Vector2 velocityCap, float antiGravMod)
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  22)         {
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  23)             this.movementSpeed = movementSpeed;
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  24)             this.speedCapSmoothFactor = speedCapSmoothFactor;
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  25)             this.jumpStrength = jumpStrength;
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  26)             this.airSpeed = airSpeed;
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  27)             this.velocityCap = velocityCap;
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  28)             this.antiGravMod = antiGravMod;
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  29)         }
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  30)     }  
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  31) 
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  32)     [Header("Player Control Settings")]
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  33)     public playerMovementSettings currentMovementSettings = new playerMovementSettings(20f, 15f, 10f, 0.02f, new Vector2 (8f, 20f), 1f);    
5ee06e095 (Charli            2021-04-15 12:43:35 +1000  34)     public float interactRange = 2f;
489f2f6ed (Charli            2021-08-05 18:12:00 +1000  35)     public float timeBetweenFootsteps = 1f;
489f2f6ed (Charli            2021-08-05 18:12:00 +1000  36)     private float currentTimeBetweenFootsteps = 0f;
489f2f6ed (Charli            2021-08-05 18:12:00 +1000  37)     public float timeBeforeFrictionReturns = 0.2f;
489f2f6ed (Charli            2021-08-05 18:12:00 +1000  38)     private float currentTimeBeforeFrictionReturns = 0f;
882b79a15 (Charli            2021-07-29 01:53:37 +1000  39)     private Vector3 horizVelocity;
882b79a15 (Charli            2021-07-29 01:53:37 +1000  40)     private Vector3 vertVelocity;
489f2f6ed (Charli            2021-08-05 18:12:00 +1000  41)     public float[] playerFriction = new float[2] {1f, 1f};
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  42)     
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  43)     // Mouse Control Settings   
489f2f6ed (Charli            2021-08-05 18:12:00 +1000  44)     public Vector2 mouseSensitivity = new Vector2 (1, 1);
489f2f6ed (Charli            2021-08-05 18:12:00 +1000  45)     public Vector2 mouseSmoothing = new Vector2 (2,2);        
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  46)     const float MIN_X = 0.0f;
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  47)     const float MAX_X = 360.0f;
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  48)     const float MIN_Y = -90.0f;
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  49)     const float MAX_Y = 90.0f;    
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  50)     private Vector2 _mouseAbsolute;
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  51)     private Vector2 _mouseSmooth;  
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000  52) 
243980762 (Charli            2021-07-28 18:47:53 +1000  53)     [Header("Player Condition Control Settings")]
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  54)     public playerMovementSettings baseMovementSettings = new playerMovementSettings(20f, 15f, 10f, 0.02f, new Vector2 (8f, 20f), 1f);
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  55)     public playerMovementSettings hotMovementSettings = new playerMovementSettings(20f, 3f, 14f, 0.15f, new Vector2 (6f, 10f), 1f);
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  56)     public playerMovementSettings coldMovementSettings = new playerMovementSettings(24f, 15f, 10f, 0.02f, new Vector2 (16f, 20f), 1f);    
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  57) 
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000  58)     [Header("Player State Settings")]
05e113859 (Charli            2021-04-22 01:18:28 +1000  59)     public bool isGravityEnabled = true;
7e2cf2335 (Charli            2021-04-27 16:20:26 +1000  60)     public bool isGunEnabled = true;
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000  61)     [SerializeField] private bool isGrounded;
882b79a15 (Charli            2021-07-29 01:53:37 +1000  62)     private bool isClimbing = false;
1ee942bac (Charli            2021-07-22 17:38:14 +1000  63)     private RaycastHit groundedHit;
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000  64)     public PlayerState playerControlState = PlayerState.MoveAndLook;
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000  65)     public List<string> playerInventory;
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000  66)     [SerializeField] private List<IConditions.ConditionTypes> _activeConditions;
243980762 (Charli            2021-07-28 18:47:53 +1000  67)     private bool isConditionChanging = false;    
5ee06e095 (Charli            2021-04-15 12:43:35 +1000  68) 
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000  69)     [Header("References")]    
5ee06e095 (Charli            2021-04-15 12:43:35 +1000  70)     public Camera playerCam;
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000  71)     public RayCastShootComplete raygunScript;
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000  72)     public PlayerInput playerInput;
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000  73)     private Rigidbody playerRB;
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000  74)     public Transform groundChecker; 
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000  75)     private TemperatureStateBase playerTemp;   
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000  76)     private InteractableBase currentInteractingObject;
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000  77)     public PauseController PC;
489f2f6ed (Charli            2021-08-05 18:12:00 +1000  78)     public AudioManager audioManager;
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000  79)     public PhysicMaterial icyPhysicMaterial; //Physics mat for slippery effect
92bf1f6eb (CHARLI BARTOLO    2021-05-26 15:59:29 +1000  80)     public PhysicMaterial regularPhysicMaterial;
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000  81) 
5ee06e095 (Charli            2021-04-15 12:43:35 +1000  82)     private void Awake() 
5ee06e095 (Charli            2021-04-15 12:43:35 +1000  83)     {   
489f2f6ed (Charli            2021-08-05 18:12:00 +1000  84)         //playerFriction[0] = regularPhysicMaterial.staticFriction;
489f2f6ed (Charli            2021-08-05 18:12:00 +1000  85)         //playerFriction[1] = regularPhysicMaterial.dynamicFriction;
ff3cb9f03 (Charli            2021-04-26 18:42:30 +1000  86)         playerRB = GetComponent<Rigidbody>();
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000  87)         playerTemp = GetComponent<TemperatureStateBase>();
9b6619a4a (Charli            2021-05-25 02:30:04 +1000  88)         _activeConditions = new List<IConditions.ConditionTypes>();
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  89)         playerInventory = new List<string>();         
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  90)         
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  91)         // baseMovementSpeed = movementSpeed;            
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  92)         // baseJumpStrength = jumpStrength;
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  93)         // baseAirSpeed = airSpeed;
fb7fca17c (Charli            2021-08-02 18:12:13 +1000  94)         // baseVelocityCap = velocityCap;               
5a0665752 (Charli            2021-05-11 17:59:55 +1000  95) 
5a0665752 (Charli            2021-05-11 17:59:55 +1000  96)         playerInput.actions.FindAction("Interact").performed += context => Interact(context);
5a0665752 (Charli            2021-05-11 17:59:55 +1000  97)         playerInput.actions.FindAction("Interact").canceled += ExitInteract;
5a0665752 (Charli            2021-05-11 17:59:55 +1000  98)         playerInput.actions.FindAction("Jump").performed += Jump;
5a0665752 (Charli            2021-05-11 17:59:55 +1000  99)         playerInput.actions.FindAction("Shoot").performed += raygunScript.FireBeam;
5a0665752 (Charli            2021-05-11 17:59:55 +1000 100)         playerInput.actions.FindAction("Shoot").canceled += raygunScript.FireBeam;
5a0665752 (Charli            2021-05-11 17:59:55 +1000 101)         playerInput.actions.FindAction("Swap Beam").performed += raygunScript.SwapBeam;
98b904775 (Charli            2021-04-21 01:34:21 +1000 102) 
5a0665752 (Charli            2021-05-11 17:59:55 +1000 103)         playerInput.ActivateInput();      
7e2cf2335 (Charli            2021-04-27 16:20:26 +1000 104) 
000000000 (Not Committed Yet 2021-08-08 14:26:27 +1000 105)         if (isGunEnabled)
000000000 (Not Committed Yet 2021-08-08 14:26:27 +1000 106)             playerInventory.Add("Raygun");
1bdc9eb80 (Charli            2021-05-13 20:06:15 +1000 107)         LockCursor();
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 108)     }
c30dc5d05 (Charli            2021-07-21 17:28:07 +1000 109) 
cffdb7c5d (Matt Smith        2021-05-31 22:16:44 +1000 110)     private void Start()
cffdb7c5d (Matt Smith        2021-05-31 22:16:44 +1000 111)     {
cffdb7c5d (Matt Smith        2021-05-31 22:16:44 +1000 112)         SetShootingEnabled(playerInventory.Contains("Raygun"));
cffdb7c5d (Matt Smith        2021-05-31 22:16:44 +1000 113)     }
c30dc5d05 (Charli            2021-07-21 17:28:07 +1000 114) 
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 115)     private void Update() 
f1ddf4483 (charadey_kong     2021-05-06 16:59:15 +1000 116)     {
c7fd2e40c (Charli            2021-07-22 16:00:33 +1000 117)         //Debug.Log(playerRB.velocity.magnitude);
c30dc5d05 (Charli            2021-07-21 17:28:07 +1000 118) 
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000 119)         if (PC != null)
f1ddf4483 (charadey_kong     2021-05-06 16:59:15 +1000 120)         {
91c469d50 (Rayner Brandon    2021-05-10 19:55:47 +1000 121)             if (PC.GetPause())
91c469d50 (Rayner Brandon    2021-05-10 19:55:47 +1000 122)             {
91c469d50 (Rayner Brandon    2021-05-10 19:55:47 +1000 123)                 playerControlState = PlayerState.ControlsDisabled;
91c469d50 (Rayner Brandon    2021-05-10 19:55:47 +1000 124)             }
91c469d50 (Rayner Brandon    2021-05-10 19:55:47 +1000 125)             else
91c469d50 (Rayner Brandon    2021-05-10 19:55:47 +1000 126)             {
91c469d50 (Rayner Brandon    2021-05-10 19:55:47 +1000 127)                 playerControlState = PlayerState.MoveAndLook;
91c469d50 (Rayner Brandon    2021-05-10 19:55:47 +1000 128)             }
f1ddf4483 (charadey_kong     2021-05-06 16:59:15 +1000 129)         }
f1ddf4483 (charadey_kong     2021-05-06 16:59:15 +1000 130)         
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 131)         switch (playerControlState)
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 132)         {
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 133)             case (PlayerState.ControlsDisabled):
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000 134)                 if (playerInput.currentActionMap == playerInput.actions.FindActionMap("Player"))
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000 135)                     playerInput.currentActionMap = playerInput.actions.FindActionMap("Menu");
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 136)                 break;
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 137)             case (PlayerState.MoveAndLook):
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000 138)                 if (playerInput.currentActionMap == playerInput.actions.FindActionMap("Menu"))
c7fd2e40c (Charli            2021-07-22 16:00:33 +1000 139)                     playerInput.currentActionMap = playerInput.actions.FindActionMap("Player");                
c7fd2e40c (Charli            2021-07-22 16:00:33 +1000 140)                 MouseLook(playerInput.actions.FindAction("Look").ReadValue<Vector2>());                
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 141)                 break;
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 142)             case (PlayerState.MoveOnly):
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000 143)                 if (playerInput.currentActionMap == playerInput.actions.FindActionMap("Menu"))
c7fd2e40c (Charli            2021-07-22 16:00:33 +1000 144)                     playerInput.currentActionMap = playerInput.actions.FindActionMap("Player");                
05e113859 (Charli            2021-04-22 01:18:28 +1000 145)                 //ResetMouse();
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 146)                 break;
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 147)             default:
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000 148)                 if (playerInput.currentActionMap == playerInput.actions.FindActionMap("Menu"))
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000 149)                     playerInput.currentActionMap = playerInput.actions.FindActionMap("Player");
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 150)                 playerControlState = PlayerState.MoveAndLook;
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 151)                 break;
c7fd2e40c (Charli            2021-07-22 16:00:33 +1000 152)         }       
c7fd2e40c (Charli            2021-07-22 16:00:33 +1000 153) 
243980762 (Charli            2021-07-28 18:47:53 +1000 154)         // TODO: Add distance + rotation restriction on interacting, so can't keep interacting if too far / not looking at it  
c7fd2e40c (Charli            2021-07-22 16:00:33 +1000 155)         if (currentInteractingObject != null)
c7fd2e40c (Charli            2021-07-22 16:00:33 +1000 156)         {
c7fd2e40c (Charli            2021-07-22 16:00:33 +1000 157)             currentInteractingObject.OnInteracting();
c7fd2e40c (Charli            2021-07-22 16:00:33 +1000 158)         }
c7fd2e40c (Charli            2021-07-22 16:00:33 +1000 159)     }
c7fd2e40c (Charli            2021-07-22 16:00:33 +1000 160) 
c7fd2e40c (Charli            2021-07-22 16:00:33 +1000 161)     private void FixedUpdate() 
243980762 (Charli            2021-07-28 18:47:53 +1000 162)     {        
243980762 (Charli            2021-07-28 18:47:53 +1000 163)         ExecuteConditions();
243980762 (Charli            2021-07-28 18:47:53 +1000 164)         RemoveConditionsIfReturningToNeutral();       
243980762 (Charli            2021-07-28 18:47:53 +1000 165) 
c7fd2e40c (Charli            2021-07-22 16:00:33 +1000 166)         switch (playerControlState)
c7fd2e40c (Charli            2021-07-22 16:00:33 +1000 167)         {
c7fd2e40c (Charli            2021-07-22 16:00:33 +1000 168)             case (PlayerState.ControlsDisabled):                
c7fd2e40c (Charli            2021-07-22 16:00:33 +1000 169)                 break;
c7fd2e40c (Charli            2021-07-22 16:00:33 +1000 170)             case (PlayerState.MoveAndLook):                
c7fd2e40c (Charli            2021-07-22 16:00:33 +1000 171)                 GroundedCheck();                
c7fd2e40c (Charli            2021-07-22 16:00:33 +1000 172)                 MovePlayer(playerInput.actions.FindAction("Movement").ReadValue<Vector2>());
c7fd2e40c (Charli            2021-07-22 16:00:33 +1000 173)                 break;
c7fd2e40c (Charli            2021-07-22 16:00:33 +1000 174)             case (PlayerState.MoveOnly):                
c7fd2e40c (Charli            2021-07-22 16:00:33 +1000 175)                 GroundedCheck();
c7fd2e40c (Charli            2021-07-22 16:00:33 +1000 176)                 MovePlayer(playerInput.actions.FindAction("Movement").ReadValue<Vector2>());
c7fd2e40c (Charli            2021-07-22 16:00:33 +1000 177)                 //ResetMouse();
c7fd2e40c (Charli            2021-07-22 16:00:33 +1000 178)                 break;
c7fd2e40c (Charli            2021-07-22 16:00:33 +1000 179)             default:               
c7fd2e40c (Charli            2021-07-22 16:00:33 +1000 180)                 break;
f1ddf4483 (charadey_kong     2021-05-06 16:59:15 +1000 181)         }
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 182) 
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 183)         if (audioManager != null)
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 184)             PlayFootstepSound();
882b79a15 (Charli            2021-07-29 01:53:37 +1000 185)         VelocityClamp();
882b79a15 (Charli            2021-07-29 01:53:37 +1000 186)         SetPlayerFriction();
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 187)     }
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 188) 
c7fd2e40c (Charli            2021-07-22 16:00:33 +1000 189)     // Input functions 
7e2cf2335 (Charli            2021-04-27 16:20:26 +1000 190) 
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 191)     void MovePlayer(Vector2 stickMovementVector)
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 192)     {
c30dc5d05 (Charli            2021-07-21 17:28:07 +1000 193)         // Translate 2d analog movement to 3d vector movement            
243980762 (Charli            2021-07-28 18:47:53 +1000 194)         Vector3 movementVector = new Vector3 (stickMovementVector.x, 0f, stickMovementVector.y);   
ad6ad85ed (Charli            2021-05-12 18:18:48 +1000 195)         movementVector = transform.TransformDirection(movementVector);
c30dc5d05 (Charli            2021-07-21 17:28:07 +1000 196) 
1ee942bac (Charli            2021-07-22 17:38:14 +1000 197)         // Reflect along surface if grounded?
1ee942bac (Charli            2021-07-22 17:38:14 +1000 198)         if (isGrounded)
1ee942bac (Charli            2021-07-22 17:38:14 +1000 199)         {
1ee942bac (Charli            2021-07-22 17:38:14 +1000 200)             movementVector = Vector3.ProjectOnPlane(movementVector, groundedHit.normal);
43e2e4fce (Charli            2021-07-30 17:14:46 +1000 201)             Debug.DrawRay(transform.position, movementVector * 100);
1ee942bac (Charli            2021-07-22 17:38:14 +1000 202)         }
243980762 (Charli            2021-07-28 18:47:53 +1000 203)         else
882b79a15 (Charli            2021-07-29 01:53:37 +1000 204)         {            
fb7fca17c (Charli            2021-08-02 18:12:13 +1000 205)             //movementVector *= airSpeed;
fb7fca17c (Charli            2021-08-02 18:12:13 +1000 206)             movementVector *= currentMovementSettings.airSpeed;
26bc0d6a2 (Charli            2021-04-30 03:05:43 +1000 207)         }
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000 208) 
243980762 (Charli            2021-07-28 18:47:53 +1000 209)          // If movement vector greater than one, reduce magnitude to one, otherwise leave untouched (in case of analog stick input)
243980762 (Charli            2021-07-28 18:47:53 +1000 210)         if (movementVector.magnitude > 1f)
243980762 (Charli            2021-07-28 18:47:53 +1000 211)         {
243980762 (Charli            2021-07-28 18:47:53 +1000 212)             movementVector = movementVector.normalized;
243980762 (Charli            2021-07-28 18:47:53 +1000 213)         } 
30451c4f3 (Matt Smith        2021-07-28 13:02:27 +1000 214) 
fb7fca17c (Charli            2021-08-02 18:12:13 +1000 215)         //playerRB.AddForce(movementVector * movementSpeed, ForceMode.Acceleration);
fb7fca17c (Charli            2021-08-02 18:12:13 +1000 216)         playerRB.AddForce(movementVector * currentMovementSettings.movementSpeed, ForceMode.Acceleration);
05e113859 (Charli            2021-04-22 01:18:28 +1000 217)     }
05e113859 (Charli            2021-04-22 01:18:28 +1000 218) 
ff3cb9f03 (Charli            2021-04-26 18:42:30 +1000 219)     void Jump(InputAction.CallbackContext context)
05e113859 (Charli            2021-04-22 01:18:28 +1000 220)     {
ff3cb9f03 (Charli            2021-04-26 18:42:30 +1000 221)         if (isGrounded)
ff3cb9f03 (Charli            2021-04-26 18:42:30 +1000 222)         {
ff3cb9f03 (Charli            2021-04-26 18:42:30 +1000 223)             //Debug.Log("Jump attempted");
fb7fca17c (Charli            2021-08-02 18:12:13 +1000 224)             //playerRB.AddForce(Vector3.up * jumpStrength, ForceMode.VelocityChange);
fb7fca17c (Charli            2021-08-02 18:12:13 +1000 225)             playerRB.AddForce(Vector3.up * currentMovementSettings.jumpStrength, ForceMode.VelocityChange);
ff3cb9f03 (Charli            2021-04-26 18:42:30 +1000 226)         }
ff3cb9f03 (Charli            2021-04-26 18:42:30 +1000 227)     }
882b79a15 (Charli            2021-07-29 01:53:37 +1000 228)     
882b79a15 (Charli            2021-07-29 01:53:37 +1000 229)     private void OnCollisionStay(Collision other) 
882b79a15 (Charli            2021-07-29 01:53:37 +1000 230)     {
882b79a15 (Charli            2021-07-29 01:53:37 +1000 231)         //Debug.Log("Collision is happening");
882b79a15 (Charli            2021-07-29 01:53:37 +1000 232)         Vector3 normal = other.GetContact(0).normal;
882b79a15 (Charli            2021-07-29 01:53:37 +1000 233)         Vector3 horForward = playerCam.transform.forward;
882b79a15 (Charli            2021-07-29 01:53:37 +1000 234)         horForward.y = 0f;
882b79a15 (Charli            2021-07-29 01:53:37 +1000 235)         horForward.Normalize();
882b79a15 (Charli            2021-07-29 01:53:37 +1000 236) 
882b79a15 (Charli            2021-07-29 01:53:37 +1000 237)         // If we're hitting the wall at no more than a 45 degree angle...
882b79a15 (Charli            2021-07-29 01:53:37 +1000 238)         if (Vector3.Angle(horForward, -normal) <= 45)
882b79a15 (Charli            2021-07-29 01:53:37 +1000 239)         {
882b79a15 (Charli            2021-07-29 01:53:37 +1000 240)             bool ledgeAvailable = true;
882b79a15 (Charli            2021-07-29 01:53:37 +1000 241)             
882b79a15 (Charli            2021-07-29 01:53:37 +1000 242)             RaycastHit hit;
882b79a15 (Charli            2021-07-29 01:53:37 +1000 243)             // If the wall's too tall, don't climb
882b79a15 (Charli            2021-07-29 01:53:37 +1000 244)             if (Physics.Raycast(playerCam.transform.position + Vector3.up * 0.5f, -normal, out hit, 1, LayerMask.GetMask("Default")))
882b79a15 (Charli            2021-07-29 01:53:37 +1000 245)             {
882b79a15 (Charli            2021-07-29 01:53:37 +1000 246)                 ledgeAvailable = false;
882b79a15 (Charli            2021-07-29 01:53:37 +1000 247)             }
05e113859 (Charli            2021-04-22 01:18:28 +1000 248) 
882b79a15 (Charli            2021-07-29 01:53:37 +1000 249)             if (ledgeAvailable)
882b79a15 (Charli            2021-07-29 01:53:37 +1000 250)             {
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 251)                 //Debug.Log("Ledge is available!");                
882b79a15 (Charli            2021-07-29 01:53:37 +1000 252)                 Vector3 currentPos = playerCam.transform.position + Vector3.up * 0.5f + Vector3.down * 0.05f;
882b79a15 (Charli            2021-07-29 01:53:37 +1000 253)                 while (!Physics.Raycast(currentPos, -normal, out hit, 1, LayerMask.GetMask("Default")))
882b79a15 (Charli            2021-07-29 01:53:37 +1000 254)                 {
882b79a15 (Charli            2021-07-29 01:53:37 +1000 255)                     currentPos += Vector3.down * 0.05f;
882b79a15 (Charli            2021-07-29 01:53:37 +1000 256)                     if (currentPos.y < playerCam.transform.position.y - 2f)
882b79a15 (Charli            2021-07-29 01:53:37 +1000 257)                         break;
882b79a15 (Charli            2021-07-29 01:53:37 +1000 258)                 }
882b79a15 (Charli            2021-07-29 01:53:37 +1000 259) 
882b79a15 (Charli            2021-07-29 01:53:37 +1000 260)                 
882b79a15 (Charli            2021-07-29 01:53:37 +1000 261)                 if (playerInput.actions.FindAction("Jump").ReadValue<float>() > 0f && isClimbing == false)
882b79a15 (Charli            2021-07-29 01:53:37 +1000 262)                 {
882b79a15 (Charli            2021-07-29 01:53:37 +1000 263)                     //isClimbing = true;
882b79a15 (Charli            2021-07-29 01:53:37 +1000 264)                     //Debug.Log("Direction force is applied in: " + (currentPos - transform.position));
882b79a15 (Charli            2021-07-29 01:53:37 +1000 265)                     playerRB.AddForce(Vector3.up * 30, ForceMode.Impulse);
882b79a15 (Charli            2021-07-29 01:53:37 +1000 266)                 }
882b79a15 (Charli            2021-07-29 01:53:37 +1000 267)                 
882b79a15 (Charli            2021-07-29 01:53:37 +1000 268)             }
882b79a15 (Charli            2021-07-29 01:53:37 +1000 269)         }
882b79a15 (Charli            2021-07-29 01:53:37 +1000 270) 
882b79a15 (Charli            2021-07-29 01:53:37 +1000 271) 
882b79a15 (Charli            2021-07-29 01:53:37 +1000 272)     }
882b79a15 (Charli            2021-07-29 01:53:37 +1000 273)     
7e2cf2335 (Charli            2021-04-27 16:20:26 +1000 274)     void SetShootingEnabled(bool setToEnable)
ff3cb9f03 (Charli            2021-04-26 18:42:30 +1000 275)     {
7e2cf2335 (Charli            2021-04-27 16:20:26 +1000 276)         if (setToEnable)
7e2cf2335 (Charli            2021-04-27 16:20:26 +1000 277)         {
5a0665752 (Charli            2021-05-11 17:59:55 +1000 278)             playerInput.actions.FindAction("Shoot").Enable();
5a0665752 (Charli            2021-05-11 17:59:55 +1000 279)             playerInput.actions.FindAction("Swap Beam").Enable();
e4ee788d6 (Matt Smith        2021-05-31 21:46:13 +1000 280)             //raygunScript.gameObject.SetActive(true);
7e2cf2335 (Charli            2021-04-27 16:20:26 +1000 281)         }
7e2cf2335 (Charli            2021-04-27 16:20:26 +1000 282)         else
7e2cf2335 (Charli            2021-04-27 16:20:26 +1000 283)         {
5a0665752 (Charli            2021-05-11 17:59:55 +1000 284)             playerInput.actions.FindAction("Shoot").Disable();
5a0665752 (Charli            2021-05-11 17:59:55 +1000 285)             playerInput.actions.FindAction("Swap Beam").Disable();
e4ee788d6 (Matt Smith        2021-05-31 21:46:13 +1000 286)             //raygunScript.gameObject.SetActive(false);
7e2cf2335 (Charli            2021-04-27 16:20:26 +1000 287)         }
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 288)     }
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 289) 
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 290)     public void Interact(InputAction.CallbackContext context)
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 291)     {
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 292)         if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward * interactRange, out RaycastHit hit) && 
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 293)             hit.collider.gameObject.GetComponent<InteractableBase>() != null)
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 294)         {
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 295)             //Debug.Log("Interactable object found, attempting interaction.");
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 296)             currentInteractingObject = hit.collider.gameObject.GetComponent<InteractableBase>();
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 297) 
5a0665752 (Charli            2021-05-11 17:59:55 +1000 298)             currentInteractingObject.OnInteractEnter(playerInput);
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 299) 
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 300)             switch (currentInteractingObject.pInteractionType)
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 301)             {
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 302)                 case InteractableBase.InteractionType.Carry:
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 303)                     break;
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 304)                 case InteractableBase.InteractionType.RotateOnly:
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 305)                     playerControlState = PlayerState.MoveOnly;
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 306)                     break;
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 307)                 // Use object, trigger exit interaction, and remove object from script.
34aa422be (Charli            2021-04-20 18:00:58 +1000 308)                 case InteractableBase.InteractionType.Use:
34aa422be (Charli            2021-04-20 18:00:58 +1000 309)                     if (currentInteractingObject.GetComponent<CollectInteractable>() != null) 
34aa422be (Charli            2021-04-20 18:00:58 +1000 310)                     {
7e2cf2335 (Charli            2021-04-27 16:20:26 +1000 311)                         playerInventory.Add(currentInteractingObject.GetComponent<CollectInteractable>().itemName);
80b6c33ea (Matt Smith        2021-08-07 15:58:48 +1000 312)                         SetShootingEnabled(playerInventory.Contains("Raygun"));
80b6c33ea (Matt Smith        2021-08-07 15:58:48 +1000 313)                         if (playerInventory.Contains("Raygun"))
80b6c33ea (Matt Smith        2021-08-07 15:58:48 +1000 314)                             GetComponent<GunFXController>().EquipTool();
34aa422be (Charli            2021-04-20 18:00:58 +1000 315)                     }                   
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 316)                     ExitInteract(context);
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 317)                     break;
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 318)                 default:
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 319)                     break;
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 320)             }
34aa422be (Charli            2021-04-20 18:00:58 +1000 321)             //currentInteractingObject.OnInteractEnter(controls);
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 322)         }
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 323)         else
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 324)         {
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 325)             //Debug.Log("Interaction failed, as no object was found capable of being interacted with.");
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 326)         }
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 327)     }
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 328) 
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 329)     private void ExitInteract(InputAction.CallbackContext context)
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 330)     {
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 331)         if (currentInteractingObject != null)
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 332)         {
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 333)             if (currentInteractingObject.pInteractionType == InteractableBase.InteractionType.RotateOnly)
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 334)             {
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 335)                 playerControlState = PlayerState.MoveAndLook;
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 336)             }
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 337) 
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 338)             currentInteractingObject.OnInteractExit();
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 339)             currentInteractingObject = null;
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 340)         }
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 341)     }
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 342) 
7e2cf2335 (Charli            2021-04-27 16:20:26 +1000 343)     // Mouse Functions below
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 344)     Vector2 MouseSmooth(Vector2 deltaParam)
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 345)     {
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 346)         Vector2 mouseDelta = deltaParam;
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 347)         mouseDelta = Vector2.Scale(mouseDelta, new Vector2 (mouseSensitivity.x * mouseSmoothing.x / 10f, mouseSensitivity.y * mouseSmoothing.y / 10f));
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 348) 
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 349)         // Interpolate mouse movement over time to smooth delta
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 350)         _mouseSmooth.x = Mathf.Lerp(_mouseSmooth.x, mouseDelta.x, 1f / mouseSmoothing.x);
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 351)         _mouseSmooth.y = Mathf.Lerp(_mouseSmooth.y, mouseDelta.y, 1f / mouseSmoothing.y);
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 352) 
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 353)         return _mouseSmooth;
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 354)     }
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 355) 
7e2cf2335 (Charli            2021-04-27 16:20:26 +1000 356)     void ResetMouse()
7e2cf2335 (Charli            2021-04-27 16:20:26 +1000 357)     {
7e2cf2335 (Charli            2021-04-27 16:20:26 +1000 358)         //_mouseAbsolute = Vector2.zero;
7e2cf2335 (Charli            2021-04-27 16:20:26 +1000 359)         _mouseSmooth = Vector2.zero;
7e2cf2335 (Charli            2021-04-27 16:20:26 +1000 360)     }
7e2cf2335 (Charli            2021-04-27 16:20:26 +1000 361) 
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 362)     private void MouseClamp()
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 363)     {
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 364)          // Manages and clamps X axis rotation        
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 365)         if (_mouseAbsolute.x < MIN_X)
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 366)             _mouseAbsolute.x += MAX_X;
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 367)         else if (_mouseAbsolute.x > MAX_X)
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 368)             _mouseAbsolute.x -= MAX_X;
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 369) 
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 370)         // Manages and clamps Y axis rotation
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 371)         if (_mouseAbsolute.y < MIN_Y)
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 372)             _mouseAbsolute.y = MIN_Y;
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 373)         else if (_mouseAbsolute.y > MAX_Y)
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 374)             _mouseAbsolute.y = MAX_Y;
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 375)     }
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 376) 
7e2cf2335 (Charli            2021-04-27 16:20:26 +1000 377)     void MouseLook(Vector2 deltaParam)
7e2cf2335 (Charli            2021-04-27 16:20:26 +1000 378)     {
7e2cf2335 (Charli            2021-04-27 16:20:26 +1000 379)         _mouseAbsolute += MouseSmooth(deltaParam);
f1ddf4483 (charadey_kong     2021-05-06 16:59:15 +1000 380)         MouseClamp();
f1ddf4483 (charadey_kong     2021-05-06 16:59:15 +1000 381) 
7e2cf2335 (Charli            2021-04-27 16:20:26 +1000 382)         transform.rotation = Quaternion.Euler(0f, _mouseAbsolute.x, 0f);
f1ddf4483 (charadey_kong     2021-05-06 16:59:15 +1000 383)         playerCam.transform.rotation = Quaternion.Euler(-_mouseAbsolute.y, transform.eulerAngles.y, transform.eulerAngles.z);      
7e2cf2335 (Charli            2021-04-27 16:20:26 +1000 384)     }
7e2cf2335 (Charli            2021-04-27 16:20:26 +1000 385) 
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 386)     // Sound Functions Below
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 387) 
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 388)     void PlayFootstepSound()
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 389)     {
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 390)         //Debug.Log(currentTimeBetweenFootsteps);
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 391)         if (horizVelocity.magnitude > 0f && isGrounded)
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 392)         {
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 393)             currentTimeBetweenFootsteps -= 5f * horizVelocity.magnitude * Time.deltaTime;
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 394)         }   
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 395)         else
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 396)         {
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 397)             currentTimeBetweenFootsteps = 0.01f;
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 398)         }
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 399) 
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 400)         if (currentTimeBetweenFootsteps <= 0f)
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 401)         {
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 402)             audioManager.Play("Footstep");
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 403)             
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 404)             currentTimeBetweenFootsteps = timeBetweenFootsteps;
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 405)         }
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 406)     }
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 407) 
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 408) 
7e2cf2335 (Charli            2021-04-27 16:20:26 +1000 409)     // Utility Functions below
7e2cf2335 (Charli            2021-04-27 16:20:26 +1000 410)     void GroundedCheck()
7e2cf2335 (Charli            2021-04-27 16:20:26 +1000 411)     {
4dae9cb3a (Charli            2021-04-29 21:11:08 +1000 412)         //isGrounded = Physics.CheckSphere(groundChecker.position, 0.01f, -1, QueryTriggerInteraction.Ignore);
e5d3db8e8 (Charli            2021-05-30 20:27:55 +1000 413)         //isGrounded = Physics.SphereCast(groundChecker.position, GetComponent<CapsuleCollider>().radius, Vector3.down, out RaycastHit hit, 1f);
e5d3db8e8 (Charli            2021-05-30 20:27:55 +1000 414)         isGrounded = Physics.SphereCast(transform.position, GetComponent<CapsuleCollider>().radius - 0.01f, 
e5d3db8e8 (Charli            2021-05-30 20:27:55 +1000 415)             Vector3.down, out RaycastHit hit, (GetComponent<CapsuleCollider>().height / 2 + 0.01f));
1ee942bac (Charli            2021-07-22 17:38:14 +1000 416) 
1ee942bac (Charli            2021-07-22 17:38:14 +1000 417)         if (isGrounded)
1ee942bac (Charli            2021-07-22 17:38:14 +1000 418)             groundedHit = hit;
1ee942bac (Charli            2021-07-22 17:38:14 +1000 419)         else
1ee942bac (Charli            2021-07-22 17:38:14 +1000 420)             groundedHit = new RaycastHit();
e5d3db8e8 (Charli            2021-05-30 20:27:55 +1000 421)         //Debug.DrawRay(groundChecker.position, Vector3.down * GetComponent<CapsuleCollider>().height);
7e2cf2335 (Charli            2021-04-27 16:20:26 +1000 422)     } 
7e2cf2335 (Charli            2021-04-27 16:20:26 +1000 423) 
882b79a15 (Charli            2021-07-29 01:53:37 +1000 424)     void VelocityClamp()
26bc0d6a2 (Charli            2021-04-30 03:05:43 +1000 425)     {
882b79a15 (Charli            2021-07-29 01:53:37 +1000 426)         horizVelocity = Vector3.Scale(playerRB.velocity, new Vector3 (1f, 0f, 1f));
fb7fca17c (Charli            2021-08-02 18:12:13 +1000 427)         vertVelocity = Vector3.Scale(playerRB.velocity, new Vector3 (0f, 1f, 0f));        
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000 428)         
fb7fca17c (Charli            2021-08-02 18:12:13 +1000 429)         if (horizVelocity.magnitude > currentMovementSettings.velocityCap.x && currentMovementSettings.velocityCap.x > 0f)
882b79a15 (Charli            2021-07-29 01:53:37 +1000 430)         { 
882b79a15 (Charli            2021-07-29 01:53:37 +1000 431)             // First Exp value = smoothing factor, higher values = no smooth, lower = more smooth 
fb7fca17c (Charli            2021-08-02 18:12:13 +1000 432)             horizVelocity = Vector3.Lerp(horizVelocity, horizVelocity.normalized * currentMovementSettings.velocityCap.x,
fb7fca17c (Charli            2021-08-02 18:12:13 +1000 433)                 1 - Mathf.Exp(-currentMovementSettings.speedCapSmoothFactor * Time.deltaTime));                         
882b79a15 (Charli            2021-07-29 01:53:37 +1000 434)         }
882b79a15 (Charli            2021-07-29 01:53:37 +1000 435) 
fb7fca17c (Charli            2021-08-02 18:12:13 +1000 436)         if (vertVelocity.magnitude > currentMovementSettings.velocityCap.y && currentMovementSettings.velocityCap.y > 0f)
882b79a15 (Charli            2021-07-29 01:53:37 +1000 437)         { 
c30dc5d05 (Charli            2021-07-21 17:28:07 +1000 438)             // First Exp value = smoothing factor, higher values = no smooth, lower = more smooth 
fb7fca17c (Charli            2021-08-02 18:12:13 +1000 439)             vertVelocity = Vector3.Lerp(vertVelocity, vertVelocity.normalized * currentMovementSettings.velocityCap.y, 
fb7fca17c (Charli            2021-08-02 18:12:13 +1000 440)                 1 - Mathf.Exp(-currentMovementSettings.speedCapSmoothFactor * Time.deltaTime));            
882b79a15 (Charli            2021-07-29 01:53:37 +1000 441)         }
882b79a15 (Charli            2021-07-29 01:53:37 +1000 442) 
882b79a15 (Charli            2021-07-29 01:53:37 +1000 443)         playerRB.velocity = horizVelocity + vertVelocity;
882b79a15 (Charli            2021-07-29 01:53:37 +1000 444)     }
882b79a15 (Charli            2021-07-29 01:53:37 +1000 445) 
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 446)     // TODO: Use forces instead of friction for player control
882b79a15 (Charli            2021-07-29 01:53:37 +1000 447)     void SetPlayerFriction()
882b79a15 (Charli            2021-07-29 01:53:37 +1000 448)     {
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 449)         // Player should have no friction when either airborne or providing movement input.
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 450)         // Should probably enable friction if trying to move opposite current direction?
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 451)         if (!isGrounded || playerInput.actions.FindAction("Movement").ReadValue<Vector2>().magnitude > 0)
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 452)         {
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 453)             //Debug.Log("Friction disabled");
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 454)             regularPhysicMaterial.staticFriction = 0f;
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 455)             regularPhysicMaterial.dynamicFriction = 0f;
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 456)             currentTimeBeforeFrictionReturns = timeBeforeFrictionReturns;
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 457)         }
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 458)         else
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 459)         {
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 460)             currentTimeBeforeFrictionReturns -= Time.deltaTime;
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 461) 
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 462)             if (currentTimeBeforeFrictionReturns <= 0f)
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 463)             {
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 464)                 //Debug.Log("Friction enabled");
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 465)                 regularPhysicMaterial.staticFriction = playerFriction[0];
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 466)                 regularPhysicMaterial.dynamicFriction = playerFriction[1];
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 467)             }
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 468)         }
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 469)         /*
882b79a15 (Charli            2021-07-29 01:53:37 +1000 470)         if (isGrounded)
882b79a15 (Charli            2021-07-29 01:53:37 +1000 471)         {
882b79a15 (Charli            2021-07-29 01:53:37 +1000 472)             if (regularPhysicMaterial.staticFriction < playerFriction[0] || regularPhysicMaterial.dynamicFriction < playerFriction[1])
882b79a15 (Charli            2021-07-29 01:53:37 +1000 473)             {
882b79a15 (Charli            2021-07-29 01:53:37 +1000 474)                 regularPhysicMaterial.staticFriction = Mathf.Clamp(regularPhysicMaterial.staticFriction +
882b79a15 (Charli            2021-07-29 01:53:37 +1000 475)                     Mathf.Lerp(0f, playerFriction[0], Time.deltaTime), 0, playerFriction[0]);
882b79a15 (Charli            2021-07-29 01:53:37 +1000 476)                 regularPhysicMaterial.dynamicFriction = Mathf.Clamp(regularPhysicMaterial.dynamicFriction +
882b79a15 (Charli            2021-07-29 01:53:37 +1000 477)                     Mathf.Lerp(0f, playerFriction[1], Time.deltaTime), 0, playerFriction[1]);
882b79a15 (Charli            2021-07-29 01:53:37 +1000 478)             }            
882b79a15 (Charli            2021-07-29 01:53:37 +1000 479)         }
882b79a15 (Charli            2021-07-29 01:53:37 +1000 480)         else
882b79a15 (Charli            2021-07-29 01:53:37 +1000 481)         {
882b79a15 (Charli            2021-07-29 01:53:37 +1000 482)             regularPhysicMaterial.staticFriction = 0f;
882b79a15 (Charli            2021-07-29 01:53:37 +1000 483)             regularPhysicMaterial.dynamicFriction = 0f;
26bc0d6a2 (Charli            2021-04-30 03:05:43 +1000 484)         }
489f2f6ed (Charli            2021-08-05 18:12:00 +1000 485)         */
26bc0d6a2 (Charli            2021-04-30 03:05:43 +1000 486)     }
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000 487)  
05e113859 (Charli            2021-04-22 01:18:28 +1000 488)     private void LockCursor()
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 489)     {
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 490)         Cursor.lockState = CursorLockMode.Locked;
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 491)         Cursor.visible = false;
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 492)     }
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 493) 
05e113859 (Charli            2021-04-22 01:18:28 +1000 494)     private void UnlockCursor()
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 495)     {
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 496)         Cursor.lockState = CursorLockMode.None;
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 497)         Cursor.visible = true;
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 498)     }
34aa422be (Charli            2021-04-20 18:00:58 +1000 499) 
34aa422be (Charli            2021-04-20 18:00:58 +1000 500)     void OnGUI()
34aa422be (Charli            2021-04-20 18:00:58 +1000 501)     {
34aa422be (Charli            2021-04-20 18:00:58 +1000 502)         GUILayout.BeginArea(new Rect(10f, 10f, Screen.width, Screen.height));
34aa422be (Charli            2021-04-20 18:00:58 +1000 503)         string stringToShow = "Player Inventory: ";
34aa422be (Charli            2021-04-20 18:00:58 +1000 504) 
34aa422be (Charli            2021-04-20 18:00:58 +1000 505)         if (playerInventory.Count > 0)
34aa422be (Charli            2021-04-20 18:00:58 +1000 506)         {            
34aa422be (Charli            2021-04-20 18:00:58 +1000 507)             for (int i = 0; i < playerInventory.Count - 1; i++)
34aa422be (Charli            2021-04-20 18:00:58 +1000 508)             {
34aa422be (Charli            2021-04-20 18:00:58 +1000 509)                 stringToShow += playerInventory[i] + ", ";
34aa422be (Charli            2021-04-20 18:00:58 +1000 510)             }
34aa422be (Charli            2021-04-20 18:00:58 +1000 511)             stringToShow += playerInventory[playerInventory.Count - 1];
34aa422be (Charli            2021-04-20 18:00:58 +1000 512)         }
34aa422be (Charli            2021-04-20 18:00:58 +1000 513)         else
34aa422be (Charli            2021-04-20 18:00:58 +1000 514)         {
34aa422be (Charli            2021-04-20 18:00:58 +1000 515)             stringToShow += "None";
34aa422be (Charli            2021-04-20 18:00:58 +1000 516)         }
34aa422be (Charli            2021-04-20 18:00:58 +1000 517)         
34aa422be (Charli            2021-04-20 18:00:58 +1000 518)         GUILayout.Label(stringToShow);
34aa422be (Charli            2021-04-20 18:00:58 +1000 519) 
34aa422be (Charli            2021-04-20 18:00:58 +1000 520)         GUILayout.EndArea();
34aa422be (Charli            2021-04-20 18:00:58 +1000 521)     }
3442de4df (Charli            2021-05-13 19:29:02 +1000 522) 
07f586e08 (charadey_kong     2021-05-24 17:22:33 +1000 523)     public void AddCondition(IConditions.ConditionTypes nameOfCondition)
07f586e08 (charadey_kong     2021-05-24 17:22:33 +1000 524)     {
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000 525)         if (!ActiveConditions.Contains(nameOfCondition))
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000 526)         {
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000 527)             _activeConditions.Add(nameOfCondition);
243980762 (Charli            2021-07-28 18:47:53 +1000 528)             isConditionChanging = true;
243980762 (Charli            2021-07-28 18:47:53 +1000 529)         }        
07f586e08 (charadey_kong     2021-05-24 17:22:33 +1000 530)     }
07f586e08 (charadey_kong     2021-05-24 17:22:33 +1000 531) 
07f586e08 (charadey_kong     2021-05-24 17:22:33 +1000 532)     public void RemoveCondition(IConditions.ConditionTypes nameOfCondition)
07f586e08 (charadey_kong     2021-05-24 17:22:33 +1000 533)     {
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000 534)         if (ActiveConditions.Contains(nameOfCondition))
243980762 (Charli            2021-07-28 18:47:53 +1000 535)         {
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000 536)             _activeConditions.Remove(nameOfCondition);
243980762 (Charli            2021-07-28 18:47:53 +1000 537)             isConditionChanging = true;
243980762 (Charli            2021-07-28 18:47:53 +1000 538)         }       
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000 539)     }
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000 540) 
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000 541)     // In future, could edit list to store separate condition timers for each. For now, using isReturningToNeutral works.
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000 542)     private void RemoveConditionsIfReturningToNeutral()
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000 543)     {
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000 544)         if (playerTemp.isReturningToNeutral)
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000 545)         {
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000 546)             RemoveCondition(IConditions.ConditionTypes.ConditionHot);
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000 547)             RemoveCondition(IConditions.ConditionTypes.ConditionCold);
f0ca20d3c (Charli            2021-05-26 02:21:13 +1000 548)         }
07f586e08 (charadey_kong     2021-05-24 17:22:33 +1000 549)     }
07f586e08 (charadey_kong     2021-05-24 17:22:33 +1000 550) 
07f586e08 (charadey_kong     2021-05-24 17:22:33 +1000 551)     public List<IConditions.ConditionTypes> ActiveConditions
07f586e08 (charadey_kong     2021-05-24 17:22:33 +1000 552)     {
07f586e08 (charadey_kong     2021-05-24 17:22:33 +1000 553)         get => _activeConditions;
243980762 (Charli            2021-07-28 18:47:53 +1000 554)         set
243980762 (Charli            2021-07-28 18:47:53 +1000 555)         {
243980762 (Charli            2021-07-28 18:47:53 +1000 556)             _activeConditions = value;
243980762 (Charli            2021-07-28 18:47:53 +1000 557)             isConditionChanging = true;
243980762 (Charli            2021-07-28 18:47:53 +1000 558)         }        
07f586e08 (charadey_kong     2021-05-24 17:22:33 +1000 559)     }
07f586e08 (charadey_kong     2021-05-24 17:22:33 +1000 560) 
07f586e08 (charadey_kong     2021-05-24 17:22:33 +1000 561)     public void ExecuteConditions()
07f586e08 (charadey_kong     2021-05-24 17:22:33 +1000 562)     {
92bf1f6eb (CHARLI BARTOLO    2021-05-26 15:59:29 +1000 563)         // If player is hot and NOT cold
92bf1f6eb (CHARLI BARTOLO    2021-05-26 15:59:29 +1000 564)         if (ActiveConditions.Contains(IConditions.ConditionTypes.ConditionHot) && !ActiveConditions.Contains(IConditions.ConditionTypes.ConditionCold))
92bf1f6eb (CHARLI BARTOLO    2021-05-26 15:59:29 +1000 565)         {
92bf1f6eb (CHARLI BARTOLO    2021-05-26 15:59:29 +1000 566)             UpwardForce();
243980762 (Charli            2021-07-28 18:47:53 +1000 567)             //ResetSlip();
92bf1f6eb (CHARLI BARTOLO    2021-05-26 15:59:29 +1000 568)         }
92bf1f6eb (CHARLI BARTOLO    2021-05-26 15:59:29 +1000 569) 
92bf1f6eb (CHARLI BARTOLO    2021-05-26 15:59:29 +1000 570)         // If player is cold and NOT hot
92bf1f6eb (CHARLI BARTOLO    2021-05-26 15:59:29 +1000 571)         else if (ActiveConditions.Contains(IConditions.ConditionTypes.ConditionCold) && !ActiveConditions.Contains(IConditions.ConditionTypes.ConditionHot))
92bf1f6eb (CHARLI BARTOLO    2021-05-26 15:59:29 +1000 572)         {
7456e427b (Charli            2021-07-17 13:26:00 +1000 573)             /*
d87464e36 (102177443         2021-05-26 16:34:53 +1000 574)             // If player is making any movement inputs, remove friction, otherwise re-enable it.
d87464e36 (102177443         2021-05-26 16:34:53 +1000 575)             if (playerInput.actions.FindAction("Movement").ReadValue<Vector2>().magnitude > 0f)
d87464e36 (102177443         2021-05-26 16:34:53 +1000 576)                 IcySlip();
d87464e36 (102177443         2021-05-26 16:34:53 +1000 577)             else
d87464e36 (102177443         2021-05-26 16:34:53 +1000 578)                 ResetSlip();
7456e427b (Charli            2021-07-17 13:26:00 +1000 579)             */
92bf1f6eb (CHARLI BARTOLO    2021-05-26 15:59:29 +1000 580)         }
243980762 (Charli            2021-07-28 18:47:53 +1000 581)         
243980762 (Charli            2021-07-28 18:47:53 +1000 582)         //if (!_prevConditions.Equals(_activeConditions))
243980762 (Charli            2021-07-28 18:47:53 +1000 583)         if (isConditionChanging)
92bf1f6eb (CHARLI BARTOLO    2021-05-26 15:59:29 +1000 584)         {
243980762 (Charli            2021-07-28 18:47:53 +1000 585)             Debug.Log("Conditions have changed!");
243980762 (Charli            2021-07-28 18:47:53 +1000 586)             UpdateControlSettingsBasedOnConditions();
243980762 (Charli            2021-07-28 18:47:53 +1000 587)             isConditionChanging = false;
243980762 (Charli            2021-07-28 18:47:53 +1000 588)         }    
243980762 (Charli            2021-07-28 18:47:53 +1000 589)     }
92bf1f6eb (CHARLI BARTOLO    2021-05-26 15:59:29 +1000 590) 
243980762 (Charli            2021-07-28 18:47:53 +1000 591)     public void UpdateControlSettingsBasedOnConditions()
243980762 (Charli            2021-07-28 18:47:53 +1000 592)     {
243980762 (Charli            2021-07-28 18:47:53 +1000 593)         if (ActiveConditions.Contains(IConditions.ConditionTypes.ConditionHot) && !ActiveConditions.Contains(IConditions.ConditionTypes.ConditionCold))
07f586e08 (charadey_kong     2021-05-24 17:22:33 +1000 594)         {
fb7fca17c (Charli            2021-08-02 18:12:13 +1000 595)             currentMovementSettings = hotMovementSettings;
fb7fca17c (Charli            2021-08-02 18:12:13 +1000 596)             // movementSpeed = baseMovementSpeed * hotMoveSpeedMod;            
fb7fca17c (Charli            2021-08-02 18:12:13 +1000 597)             // jumpStrength = baseJumpStrength * hotJumpStrengthMod;
fb7fca17c (Charli            2021-08-02 18:12:13 +1000 598)             // airSpeed = baseAirSpeed * hotAirSpeedMod;  
fb7fca17c (Charli            2021-08-02 18:12:13 +1000 599)             // velocityCap = baseVelocityCap * hotVelocityCapMod;    
243980762 (Charli            2021-07-28 18:47:53 +1000 600)         }
243980762 (Charli            2021-07-28 18:47:53 +1000 601)         // If player is cold and NOT hot
243980762 (Charli            2021-07-28 18:47:53 +1000 602)         else if (ActiveConditions.Contains(IConditions.ConditionTypes.ConditionCold) && !ActiveConditions.Contains(IConditions.ConditionTypes.ConditionHot))
243980762 (Charli            2021-07-28 18:47:53 +1000 603)         {   
fb7fca17c (Charli            2021-08-02 18:12:13 +1000 604)             currentMovementSettings = coldMovementSettings;
fb7fca17c (Charli            2021-08-02 18:12:13 +1000 605)             // movementSpeed = baseMovementSpeed * coldMoveSpeedMod;            
fb7fca17c (Charli            2021-08-02 18:12:13 +1000 606)             // jumpStrength = baseJumpStrength * coldJumpStrengthMod;
fb7fca17c (Charli            2021-08-02 18:12:13 +1000 607)             // airSpeed = baseAirSpeed * coldAirSpeedMod; 
fb7fca17c (Charli            2021-08-02 18:12:13 +1000 608)             // velocityCap = baseVelocityCap * coldVelocityCapMod;          
243980762 (Charli            2021-07-28 18:47:53 +1000 609)         }
243980762 (Charli            2021-07-28 18:47:53 +1000 610)         else
243980762 (Charli            2021-07-28 18:47:53 +1000 611)         {
fb7fca17c (Charli            2021-08-02 18:12:13 +1000 612)             currentMovementSettings = baseMovementSettings;
fb7fca17c (Charli            2021-08-02 18:12:13 +1000 613)             // movementSpeed = baseMovementSpeed;            
fb7fca17c (Charli            2021-08-02 18:12:13 +1000 614)             // jumpStrength = baseJumpStrength;
fb7fca17c (Charli            2021-08-02 18:12:13 +1000 615)             // airSpeed = baseAirSpeed;  
fb7fca17c (Charli            2021-08-02 18:12:13 +1000 616)             // velocityCap = baseVelocityCap;    
243980762 (Charli            2021-07-28 18:47:53 +1000 617)         }
07f586e08 (charadey_kong     2021-05-24 17:22:33 +1000 618)     }
07f586e08 (charadey_kong     2021-05-24 17:22:33 +1000 619) 
07f586e08 (charadey_kong     2021-05-24 17:22:33 +1000 620)     public void UpwardForce()
07f586e08 (charadey_kong     2021-05-24 17:22:33 +1000 621)     {
07f586e08 (charadey_kong     2021-05-24 17:22:33 +1000 622)         if (GetComponent<Rigidbody>() != null)
07f586e08 (charadey_kong     2021-05-24 17:22:33 +1000 623)         {
fb7fca17c (Charli            2021-08-02 18:12:13 +1000 624)             GetComponent<Rigidbody>().AddForce(-Physics.gravity * currentMovementSettings.antiGravMod * Time.deltaTime * 25f, ForceMode.Acceleration);
07f586e08 (charadey_kong     2021-05-24 17:22:33 +1000 625)         }
07f586e08 (charadey_kong     2021-05-24 17:22:33 +1000 626)     }
07f586e08 (charadey_kong     2021-05-24 17:22:33 +1000 627) 
07f586e08 (charadey_kong     2021-05-24 17:22:33 +1000 628)     public void IcySlip()
3442de4df (Charli            2021-05-13 19:29:02 +1000 629)     {
07f586e08 (charadey_kong     2021-05-24 17:22:33 +1000 630)         if (GetComponent<Collider>() != null)
07f586e08 (charadey_kong     2021-05-24 17:22:33 +1000 631)         {
92bf1f6eb (CHARLI BARTOLO    2021-05-26 15:59:29 +1000 632)             GetComponent<Collider>().material = icyPhysicMaterial;            
92bf1f6eb (CHARLI BARTOLO    2021-05-26 15:59:29 +1000 633)         }
92bf1f6eb (CHARLI BARTOLO    2021-05-26 15:59:29 +1000 634)     }
92bf1f6eb (CHARLI BARTOLO    2021-05-26 15:59:29 +1000 635) 
92bf1f6eb (CHARLI BARTOLO    2021-05-26 15:59:29 +1000 636)     public void ResetSlip()
92bf1f6eb (CHARLI BARTOLO    2021-05-26 15:59:29 +1000 637)     {
92bf1f6eb (CHARLI BARTOLO    2021-05-26 15:59:29 +1000 638)         if (GetComponent<Collider>() != null)
92bf1f6eb (CHARLI BARTOLO    2021-05-26 15:59:29 +1000 639)         {
92bf1f6eb (CHARLI BARTOLO    2021-05-26 15:59:29 +1000 640)             GetComponent<Collider>().material = regularPhysicMaterial;
07f586e08 (charadey_kong     2021-05-24 17:22:33 +1000 641)         }
3442de4df (Charli            2021-05-13 19:29:02 +1000 642)     }
5ee06e095 (Charli            2021-04-15 12:43:35 +1000 643) }
07f586e08 (charadey_kong     2021-05-24 17:22:33 +1000 644) 
