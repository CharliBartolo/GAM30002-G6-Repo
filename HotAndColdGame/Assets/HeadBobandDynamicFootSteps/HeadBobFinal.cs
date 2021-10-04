using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobFinal : MonoBehaviour
{
    [SerializeField] private bool canUseHeadbob;

    [SerializeField] private float runBobSpeed = 14f;
    [SerializeField] private float runBobAmount = 0.05f;
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float walkBobAmount = 0.05f;
    private float defaultYpos = 0;
    private float timer;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private Rigidbody playerRigidbody;

    // Start is called before the first frame update
    void Awake()
    {
        defaultYpos = playerCamera.transform.localPosition.y;
    }

    // Update is called once per frame
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
            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                defaultYpos + Mathf.Sin(timer) * (walkBobAmount));
        }
    }
}
