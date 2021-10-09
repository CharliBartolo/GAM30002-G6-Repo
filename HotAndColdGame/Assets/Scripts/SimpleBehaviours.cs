using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SimpleBehaviours : MonoBehaviour
{
    public bool enabled = true;
    private bool loop = true;
    private bool triggeredEnabled = false;
    private bool triggered = false;
    private bool hold;
    private bool held;
    private int clicks;
    private bool colliding;
    // platform modes
    public enum PlatFormMode { NONE, PINGPONG, ONCE, HOLDOPEN }
    public PlatFormMode platformMode = PlatFormMode.NONE;
    // trigger modes
    public enum TriggerMode { NONE, ONCE, HOLD, TOGGLE }
    public TriggerMode triggerMode = TriggerMode.NONE;

    // start and end positions
    public Vector3 startPos = Vector3.zero;
    public Vector3 endPos = Vector3.zero;

    [Header("Movement")]
    // move speed
    [Range(0, 10f)]
    public float moveSpeed = 2;
    [Range(0, 10f)]
    public float fallSpeed = 2;

    [Header("Wait Time")]
    // wait time
    [Range(0, 10f)]
    public float waitTime;
    private bool waiting;
    private Vector3 targetPos;
    private Vector3 TemptargetPos;

    // raycast variables

    private Collider collider;
    private bool hitDetect;
    private RaycastHit hit;
    public Transform hitTransform;
    public Vector3 hitPosition;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();

        // check if start position exists, if not, get platform position
        if (startPos == Vector3.zero)
            startPos = transform.position;

        // set platform position to start position
        transform.position = startPos;
        Loop();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {

        if (enabled)
        {
            switch (platformMode)
            {
                case PlatFormMode.NONE:
                    // nothing
                    break;

                case PlatFormMode.PINGPONG:
                    PingPong();
                    break;

                case PlatFormMode.ONCE:
                    Once();
                    break;

                case PlatFormMode.HOLDOPEN:
                    HoldOpen();
                    break;
            }
        }
    }

    // platform modes
    #region HOLD OPEN
    public void HoldOpen()
    {
        loop = false;
        switch (triggerMode)
        {

            case TriggerMode.HOLD:
                if (triggered)
                {
                    targetPos = endPos;
                    MovePlatform(moveSpeed);
                }
                else
                {
                    if (Vector3.Distance(transform.position, startPos) > 0.01f)
                    {
                        Vector3 temp = startPos;
                        targetPos = startPos;
                        MovePlatform(fallSpeed);
                        triggered = false;
                    }
                }

                break;
        }
    }
    #endregion

    #region PING PONG
    public void PingPong()
    {
        // set loop
        loop = true;
        switch (triggerMode)
        {
            case TriggerMode.NONE:
                MovePlatform(moveSpeed);
                break;
            case TriggerMode.ONCE:
                if (triggered && !triggeredEnabled)
                {
                    triggeredEnabled = true;
                }
                if (triggeredEnabled)
                    MovePlatform(moveSpeed);
                break;
            case TriggerMode.HOLD:
                if (triggered)
                {
                    MovePlatform(moveSpeed);
                }
                else
                {
                    MovePlatform(0);
                }
                break;
            case TriggerMode.TOGGLE:

                bool started = false;

                if (triggered && !started)
                {
                    started = true;
                }

                if (triggered && started)
                {
                    started = false;
                }
                if (started)
                {
                    MovePlatform(moveSpeed);
                }
                else
                {
                    MovePlatform(0);
                }

                break;
        }
    }
    #endregion

    #region ONCE
    public void Once()
    {
        loop = false;
        switch (triggerMode)
        {
            case TriggerMode.NONE:
                MovePlatform(moveSpeed);
                break;
            case TriggerMode.ONCE:
                if (triggered && !triggeredEnabled)
                {
                    triggeredEnabled = true;
                }
                if (triggeredEnabled)
                    if (transform.position != endPos)
                        MovePlatform(moveSpeed);
                break;

            case TriggerMode.HOLD:
                if (triggered)
                {
                    MovePlatform(moveSpeed);

                }
                else
                {
                    MovePlatform(0);
                }

                break;
        }
    }
    // reset
    public void ResetState(bool activeState)
    {
        Debug.Log("DEATH WATER RESET");
        transform.position = startPos;
        triggered = false;
        gameObject.SetActive(activeState);
    }
    // swap direction
    public void ReturnToStart(float speed)
    {
        targetPos = startPos;
        MovePlatform(speed);
    }
    // stop platform
    private void StopPlatform()
    {
        transform.position = transform.position;
    }
    #endregion
    // move platform
    private void MovePlatform(float speed)
    {

        // wait when reach target
        if (transform.position == targetPos && !waiting)
        {
            waiting = true;
            //StopCoroutine(MoveTo(0));
            StartCoroutine(WaitFor(waitTime));

        }
        // move to position if not waiting
        if (!waiting)
            //StartCoroutine(MoveTo(3));
            MoveToPosition(transform, targetPos, speed);
    }
    // wait for seconds enumerator
    IEnumerator WaitFor(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        // cancel waiting
        waiting = false;
        // loop if set
        if (loop)
            Loop();
    }

    // switch direction if looping
    public void Loop()
    {
        Vector3 angle = targetPos - transform.position;


        if (transform.position == startPos)
        {
            targetPos = endPos;
        }
        else if (transform.position == endPos)
        {
            targetPos = startPos;
        }
    }


    public void MoveToPosition(Transform transform, Vector3 position, float timeToMove)
    {
        if (!hold || !colliding)
            transform.position = Vector3.MoveTowards(transform.position, position, timeToMove * Time.deltaTime);
    }

    public void SetStartPosition()
    {
        startPos = transform.position;
    }
    public void SetEndPosition()
    {
        endPos = transform.position;
    }

    public void Trigger(bool value)
    {
        triggered = value;
        /* if(triggered)
             Debug.Log("Triggered to open: " + this.GetType().Name);*/
    }


    private void OnCollisionEnter(Collision collision)
    {

    }
    private void OnCollisionExit(Collision collision)
    {

    }
}
