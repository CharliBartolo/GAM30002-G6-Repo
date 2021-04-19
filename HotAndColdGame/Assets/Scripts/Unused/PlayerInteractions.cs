using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerInteractions : MonoBehaviour
{
    private bool swapTriggered;
    Transform obj;
    public Camera cam;
    public Transform holding;

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
    private FPSController FPScontroller;
    RaycastHit groundHit;

    public List<int> key_ids = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        FPScontroller = GetComponent<FPSController>();

        SpwawnPoint = transform.position;
        shadowPos = transform.position;
        lightPos = transform.position;
        holding = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (holding != null)
            {
                if(groundHit.collider != null)
                {
                    if (groundHit.collider.gameObject.tag == "Elevator")
                    {
                        holding.parent = groundHit.collider.gameObject.transform;
                        holding = null;
                    }
                    else
                    {
                        holding.parent = null;
                        holding = null;
                    }
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
         
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxGrapDistance))
            {
                //Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * maxGrapDistance, Color.yellow);
                Debug.DrawLine(cam.transform.position, cam.transform.forward * maxGrapDistance);

                if (hit.collider.gameObject.tag == "Box")
                {
                    Debug.Log("BOX AHEAD");
                }

                if (hit.collider.gameObject.tag == "KeyCard")
                {
                    key_ids.Add(hit.collider.gameObject.GetComponent<Key>().id);
                    Destroy(hit.collider.gameObject);
                }

                if (hit.collider.gameObject.GetComponent<IRemoteFunction>() != null)
                {
                    //Debug.Log("REMOTE CONTROL USED");
                    hit.collider.gameObject.GetComponent<IRemoteFunction>().RemoteControl();
                }

                if (holding == null)
                {
                    if (hit.collider.gameObject.tag == "CarryLight")
                    {
                        holding = hit.collider.gameObject.transform;
                        holding.parent = gameObject.transform;
                    }
                }
            }
        }

        // check floor
        
        if (Physics.Raycast(transform.position, Vector3.down, out groundHit, 5))
        {
            if (groundHit.collider.gameObject.tag == "Elevator")
            {
                transform.parent = groundHit.collider.gameObject.transform;

                MovingObject e = groundHit.collider.gameObject.GetComponent<MovingObject>();
                if(e.moving)
                {
                    GetComponent<CharacterController>().enabled = false;
                }
                else
                {
                    GetComponent<CharacterController>().enabled = true;
                }
            }
            else
            {
                if(GetComponent<CharacterController>().enabled == false)
                {
                    GetComponent<CharacterController>().enabled = true;
                }
                if (transform.parent != null)
                {
                    transform.parent = null;
                }
            }
            if (groundHit.collider.gameObject.tag == "LightBeam")
            {
                safe = true;
            }
            else
            {
                safe = false;
            }
        }

        if (!GodMode)
        {
        
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
