using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePadController : MonoBehaviour
{
    public Collider Operator;
    /*
    public bool OnTrigger;
    public float StartPos;
    public float EndPos;
    public float Distance;
    public float Speed;
    private float _rate;
    public float Timer = 0.0f;
    */
    // Start is called before the first frame update
    void Start()
    {
        /*
        StartPos = transform.position.y;
        Distance = -5.0f;
        Speed = 1.0f;
        _rate = 1.0f / Mathf.Abs(Distance) * Speed;
        Timer = 0.0f;
        */
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (OnTrigger)
        {
            if (Timer < 1.0f)
            {
                Debug.Log(Timer);
                Timer += Time.deltaTime * _rate;
                transform.position = new Vector3(transform.position.x, Mathf.Lerp(StartPos, EndPos, Timer), transform.position.z);
            }
        }
        */
        MoveDown(transform, 5f, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other == Operator)
        {
            Debug.Log("CollisionEnter Successful");
            

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other == Operator)
        {
            Debug.Log("CollisionExit Successful");

        }

    }
    
    private void MoveDown(Transform thisTransform, float distance, float speed)
    {
        float startPos = thisTransform.position.y;
        float endPos = startPos - distance;
        float rate = 1.0f / Mathf.Abs(distance) * speed;
        float t = 0.0f;
        while (t < 1.0f)
        {
            Debug.Log(t);
            t += Time.deltaTime * rate;
            thisTransform.position = new Vector3(thisTransform.position.x, Mathf.Lerp(startPos, endPos, t), thisTransform.position.z);
        }
    }
    
}
