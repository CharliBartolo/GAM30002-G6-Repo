using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
/// <summary>
/// Responsible for managing Player Camera. Functionality includes adjusting general FOV,
/// adjusting FOV based on speed, and 'headbobbing' while moving.
/// Last edit: Adding Class Summary
/// By: Charli - 8/10/21
/// </summary>
=======
>>>>>>> 7b688233387786860c4dc5b974fab5d75dd2dbe6
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
<<<<<<< HEAD
        else
        {
            playerCam.transform.localPosition = Vector3.Lerp(
                new Vector3 (playerCam.transform.localPosition.x, defaultYpos), 
                playerCam.transform.localPosition, 48f * Time.deltaTime);
        }
=======
>>>>>>> 7b688233387786860c4dc5b974fab5d75dd2dbe6
    }
}
