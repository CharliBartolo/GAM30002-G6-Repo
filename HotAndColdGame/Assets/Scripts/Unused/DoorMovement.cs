using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMovement : TemperatureStateBase
{
    [SerializeField] TemperatureStateBase Trigger = null;

    public Transform origin = null; // the start target position
    public Transform coldTarget = null; // the cold target position
    public Transform hotTarget = null; // the cold target position
    public GameObject platformObj;

    public float speed;
    public float delay = 1;
    public bool canMove = true; // a public bool that allows you to toggle this script on and off in the inspector

    private void Update() 
    {       
        if (canMove)
        {
            float step = speed * Time.deltaTime; // step size = speed * frame time
            switch (Trigger.CurrentTempState)
            {
                case ITemperature.tempState.Cold:
                    platformObj.transform.position = Vector3.MoveTowards(platformObj.transform.position, coldTarget.transform.position, step); // moves position a step closer to the target position
                    break;
                case ITemperature.tempState.Hot:
                    platformObj.transform.position = Vector3.MoveTowards(platformObj.transform.position, hotTarget.transform.position, step); // moves position a step closer to the target position
                    break;
                default:
                    break;
            }
        }
    }
}