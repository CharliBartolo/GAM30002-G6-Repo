using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for managing Player Camera. Functionality includes adjusting general FOV,
/// adjusting FOV based on speed, and 'headbobbing' while moving.
/// Last edit: Adding Class Summary
/// By: Charli - 8/10/21
/// </summary>
public class PlayerCameraControl : MonoBehaviour
{
    [Header("FOV Settings")]
    public Camera playerCam;
    public float baseFOV = 67;  // The player's starting FOV
    public float currentFOV = 67;   // The player's FOV at this point in time
    private float deltaFOV; // The change in FOV from the last frame
    public float fovCap = 72;   // The max possible FOV from this script

    private float fovTarget;    // The FOV value that currentFOV approaches over time.

    public float velocityFloor = 8f;    // The minimum value for which FOV changes occur. Velocity below this = no change.
    public float velocityCap = 12f;     // The value at which the FOV will cap at max. Velocity higher than this = no change.

    [Header("Headbob Settings")]
    [SerializeField] private bool canUseHeadbob;

    [SerializeField] private float runBobSpeed = 14f;
    [SerializeField] private float runBobAmount = 0.05f;
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float walkBobAmount = 0.05f;
    private float defaultYpos = 0;
    private float timer;

    [Header("Endgame Camera Pan Settings")]
    //Looking at the target object
    [SerializeField]
    [Tooltip("If true, the must look in a certain direction to activate the Tutorial Prompt. (Mainly for debug)")]
    private bool needsVision;

    [SerializeField]
    [Tooltip("The minimum angle from the expected direction. -1 is behind, 0 is perpendicular and 1 is same direction")]
    [Range(-1, 1)]
    private float visionWindow;

    [SerializeField]
    [Tooltip("The target object that will trigger camera pan")]
    private GameObject targetObject;

    [SerializeField]
    [Tooltip("The area where the player can trigger the camera pan")]
    private Collider endgameTrigger;

    //Booleans
    [SerializeField]
    [Tooltip("Boolean that tells if camera is ready to pan")]
    private bool isReadyForPan;

    [SerializeField]
    [Tooltip("Boolean that tells if the game has reached its final sequence")]
    private bool isGameEnd;

    //Coroutines
    private Coroutine preRotationCoroutine;
    private Coroutine endGameRotationCoroutine;

    //Pre Rotation
    [Header("Pre Rotation Settings")]
    [SerializeField]
    [Tooltip("How long the camera will take to pan")]
    private float preRotationSpeed;

    //EndGame Rotation
    [Header("End Game Rotation Settings")]

    [SerializeField]
    [Tooltip("How far the camera will rotate upwards for end game pan")]
    private float endGamePanRotation;

    [SerializeField]
    [Tooltip("How long the camera will take to pan for end game pan")]
    private float endGameRotationSpeed;

    [SerializeField]
    [Tooltip("The delay for the fade into black")]
    private float endGameFadeDelay;



    private void Awake()
    {
        playerCam = GetComponentInChildren<Camera>();

        defaultYpos = playerCam.transform.localPosition.y;
    }

    // Start is called before the first frame update
    void Start()
    {
        baseFOV = playerCam.fieldOfView;
        currentFOV = baseFOV;

    }

    private void FixedUpdate()
    {
        if (this.isReadyForPan)
        {
            this.gameObject.GetComponent<PlayerController>().playerControlState = PlayerController.PlayerState.ControlsDisabled;
            if (needsVision)
            {
                var direction = Vector3.Normalize(this.playerCam.transform.TransformDirection(Vector3.forward));    //Players looking direction
                var expectedDirection = Vector3.Normalize(this.targetObject.transform.position - this.playerCam.transform.position);  //Vector from startTrigger to player
                var dotProduct = Vector3.Dot(expectedDirection, direction);   //Dot product of above vectors

                if (dotProduct >= -this.visionWindow)
                {

                    if (!isGameEnd)
                    {
                        PreRotate();
                    }
                    else
                    {
                        EndGameRotate();
                    }

                }
            }
            else
            {
                if (!isGameEnd)
                {
                    PreRotate();
                }
                else 
                {
                    EndGameRotate();
                }
            }
        }

        //DEBUG
        if (Debug.isDebugBuild)
        {
            Debug.DrawRay(this.targetObject.transform.position, this.playerCam.transform.position, Color.red);
            Debug.DrawRay(this.playerCam.transform.position, playerCam.transform.forward, Color.green);
            //Debug.Log(Player.GetComponent<PlayerInput>().currentControlScheme);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == this.endgameTrigger)
        {
            this.isReadyForPan = true;
        }
    }

    public void UpdateFOVBasedOnSpeed(float playerSpeed)
    {
        UpdateTargetFOV(playerSpeed);
        currentFOV = Mathf.SmoothDamp(currentFOV, fovTarget, ref deltaFOV, 0.3f);
        playerCam.fieldOfView = currentFOV;
    }

    public void UpdateTargetFOV(float playerSpeed)
    {
        if (playerSpeed > velocityFloor)
        {
            float velocityProportion = Mathf.Clamp01((playerSpeed - velocityFloor) / velocityCap);           
            fovTarget = Mathf.Lerp(baseFOV, fovCap, velocityProportion);
        }
        else
            fovTarget = baseFOV;
    }

    public void UpdateHeadbob(Vector3 horizVelocity, bool Grounded)
    {
        if (canUseHeadbob)
        {
            HandleHeadbob(horizVelocity ,Grounded);
        }
    }

    private void HandleHeadbob(Vector3 horizVelocity, bool Grounded)
    {
        if(horizVelocity.magnitude > 0.1f && Grounded)
        {
            timer += Time.deltaTime * (walkBobSpeed);
            playerCam.transform.localPosition = new Vector3(
                playerCam.transform.localPosition.x,
                defaultYpos + Mathf.Sin(timer) * (walkBobAmount));
        }
        else
        {
            playerCam.transform.localPosition = Vector3.Lerp(
                new Vector3 (playerCam.transform.localPosition.x, defaultYpos), 
                playerCam.transform.localPosition, 48f * Time.deltaTime);
        }
    }

    public void PreRotate()
    {
        if (this.preRotationCoroutine == null)
        {
            this.preRotationCoroutine = StartCoroutine(PreRotationPan());        
        } 
    }

    public void EndGameRotate()
    {
        if (this.endGameRotationCoroutine == null)
        {
            this.endGameRotationCoroutine = StartCoroutine(EndgameCameraPan());
        }
    }

    //EndGame Camera Pan Addition
    IEnumerator PreRotationPan()
    {
        Quaternion lookRotation = Quaternion.LookRotation(this.targetObject.transform.position - this.transform.position);
        float t = 0.0f;

        while (t < 1.0f)
        {
            if (this.transform.rotation == Quaternion.Slerp(this.transform.rotation, lookRotation, 1))
            {
                t += 1.0f; 
            }

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRotation, t);
            t += Time.deltaTime * preRotationSpeed;

            yield return null;
        }

        this.isGameEnd = true;
    }

    IEnumerator EndgameCameraPan()
    {
        Vector3 targetVector = this.targetObject.transform.position + new Vector3(0, this.endGamePanRotation, 0);
        Debug.Log(targetObject.transform.position);
        Debug.Log(targetVector);
        Quaternion lookRotation = Quaternion.LookRotation(targetVector - this.transform.position);
        float t = 0.0f;

        while (t < 1.0f)
        {
            if (this.transform.rotation == Quaternion.Slerp(this.transform.rotation, lookRotation, 1))
            {
                t += 1.0f;
            }

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRotation, t);
            t += Time.deltaTime * endGameRotationSpeed;

            yield return null;
        }

        StartCoroutine(gameObject.GetComponent<EndGameTrigger>().LoadNextScene(endGameFadeDelay));
    }
}
