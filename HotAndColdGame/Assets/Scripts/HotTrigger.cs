using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotTrigger : MonoBehaviour
{
    [SerializeField] TemperatureStateBase Trigger = null;

    public Transform hotTarget; // the Hot target position
    public Transform origin;
    public Transform collisonPos;
    public GameObject platformObj;
    public bool isBlocked = true;

    [Header("Settings")]
    [Range(1, 10)]
    [SerializeField] float Speed = 5;
    [Range(.5f, 4.0f)]
    [SerializeField] float Delay = 1;

    // Update is called once per frame
    void Update()
    {
        float step = Speed * Time.deltaTime; // step size = speed * frame time
        switch (Trigger.CurrentTempState)
        {
            case ITemperature.tempState.Cold:
                isBlocked = false;
                break;
            case ITemperature.tempState.Hot:
                if (isBlocked)//check if blocked
                {
                    transform.position = Vector3.Lerp(collisonPos.transform.position, origin.transform.position, Mathf.PingPong(Time.time * step, 1.0f));//transform between 2 poiints
                    platformObj.transform.position = Vector3.MoveTowards(platformObj.transform.position, origin.transform.position, step); // moves position a step closer to the target position
                }
                else 
                {
                    platformObj.transform.position = Vector3.MoveTowards(platformObj.transform.position, hotTarget.transform.position, step); // moves position a step closer to the target position
                }                
                break;
            default:
                break;
        }
    }
}
