using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using DG.Tweening;

public class PlayerInteractions : MonoBehaviour
{
    private bool swapTriggered;
    Transform obj;
    public float pushPower = 2.0f;
    public float maxGrapDistance;
    private bool safe;
    [SerializeField] public Vector3 SpwawnPoint;
    [SerializeField] public Vector3 shadowPos;
    [SerializeField] public Vector3 lightPos;
    [SerializeField] public Image DarknessFader;
    private float voidTime;
    private float voidTimer;
    public bool GodMode;

    //WorldController worldController;


    // Start is called before the first frame update
    void Start()
    {
        //worldController = GameObject.Find("WorldController").GetComponent<WorldController>();
        SpwawnPoint = transform.position;
        shadowPos = transform.position;
        lightPos = transform.position;
        DarknessFader.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxGrapDistance))
            {
                //Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * maxGrapDistance, Color.yellow);
                Debug.DrawLine(Camera.main.transform.position, Camera.main.transform.forward * maxGrapDistance);

                if (hit.collider.gameObject.tag == "Box")
                {
                    Debug.Log("BOX AHEAD");
                }

                if (hit.collider.gameObject.tag == "MemoryObject")
                {
                    Debug.Log("FOUND A MEMORY PIECE");
                    hit.collider.gameObject.SetActive(false);
                }

                if (hit.collider.gameObject.GetComponent<IRemoteFunction>() != null)
                {
                    Debug.Log("REMOTE CONTROL USED");
                    hit.collider.gameObject.GetComponent<IRemoteFunction>().RemoteControl();
                }
            }
        }


        // check floor
        RaycastHit groundHit;
        if (Physics.Raycast(transform.position, Vector3.down, out groundHit, 5))
        {
            if (groundHit.collider.gameObject.tag == "LightBeam")
            {
                safe = true;
            }
            else
            {
                safe = false;
            }
        }
        Debug.Log("SAFE: " + safe);
        Debug.Log("VOID TIMER: " + voidTimer);

        if (!GodMode)
        {
            ControlFader();
            /*if (worldController.mode == WorldController.Mode.Shadow)
            {
                ControlFader();
            }
            else
            {
                voidTimer = 0;
                var tempColor = DarknessFader.color;
                tempColor.a = voidTimer;
                DarknessFader.color = tempColor;
            }*/
        }
    }

    private void ControlFader()
    {
        if (safe)
        {
            if (voidTimer > 0)
            {
                voidTimer -= Time.deltaTime;
            }
        }
        else
        {
            if (voidTimer < 1)
            {
                voidTimer += Time.deltaTime;
            }

        }
        var tempColor = DarknessFader.color;
        tempColor.a = voidTimer;
        DarknessFader.color = tempColor;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // push boxes
        Rigidbody body = hit.collider.attachedRigidbody;

        if (body == null || body.isKinematic) { return; }

        if (hit.moveDirection.y < -0.3) { return; }

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        body.AddForce(pushDir * pushPower);
    }

}
