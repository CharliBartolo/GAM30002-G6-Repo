using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraControl : MonoBehaviour
{
    public Camera playerCam;
    public float baseFOV = 67;  // The player's starting FOV
    public float currentFOV = 67;   // The player's FOV at this point in time
    private float deltaFOV; // The change in FOV from the last frame
    public float fovCap = 72;   // The max possible FOV from this script

    private float fovTarget;    // The FOV value that currentFOV approaches over time.

    public float velocityFloor = 8f;    // The minimum value for which FOV changes occur. Velocity below this = no change.
    public float velocityCap = 12f;     // The value at which the FOV will cap at max. Velocity higher than this = no change.

    private void Awake()
    {
        playerCam = GetComponentInChildren<Camera>();
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
}
