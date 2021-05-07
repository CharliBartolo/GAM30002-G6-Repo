using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    May want to use code from door movement script 
 */

public class PlatformWithTemperature : TemperatureStateBase
{

    [SerializeField] TemperatureStateBase Trigger = null;
    ITemperature.tempState prevTempState;

    public GameObject origin; // the start target position
    public GameObject coldTarget; // the cold target position
    public GameObject hotTarget; // the cold target position
    
    public float speed; // speed - units per second (gives you control of how fast the object will move in the inspector)
    public float Delay = 1;
    public bool moveObj; // a public bool that allows you to toggle this script on and off in the inspector

    void Start() 
    {
        prevTempState = Trigger.CurrentTempState;
    }

    // Update is called once per frame
    void Update()
    {
        Trigger.CurrentTempState = ITemperature.tempState.Cold;//for testing purposes
        //need to add a delay so that object can reach destination
        if (moveObj == true)
        {
            float step = speed * Time.deltaTime; // step size = speed * frame time
            switch (Trigger.CurrentTempState)
            {
                case ITemperature.tempState.Cold:
                    transform.position = Vector3.MoveTowards(transform.position, coldTarget.transform.position, step); // moves position a step closer to the target position
                    break;
                case ITemperature.tempState.Hot:
                    transform.position = Vector3.MoveTowards(transform.position, hotTarget.transform.position, step); // moves position a step closer to the target position
                    break;
                case ITemperature.tempState.Neutral:
                    transform.position = Vector3.MoveTowards(transform.position, origin.transform.position, step); // moves position a step closer to the target position
                    break;
            }
            //moveObj = true;
        }

        //function below used for moving object to destination
        //transform.position = Vector3.MoveTowards(transform.position, hotTarget.transform.position, speed * Time.deltaTime);//moves to hot position


        /*
        //used for finished version?
        if (Trigger.CurrentTempState != prevTempState) 
        {
            prevTempState = Trigger.CurrentTempState;
            float step = speed * Time.deltaTime; // step size = speed * frame time
            switch (Trigger.CurrentTempState)
            {
                case ITemperature.tempState.Cold:
                    transform.position = Vector3.MoveTowards(transform.position, coldTarget.position, step); // moves position to the coldTarget position
                    break;
                case ITemperature.tempState.Hot:
                    transform.position = Vector3.MoveTowards(transform.position, hotTarget.position, step); // moves position to the hotTarget position
                    break;
                case ITemperature.tempState.Neutral:
                    transform.position = Vector3.MoveTowards(transform.position, origin.position, step); // moves position to the origin position
                    break;
            }
        }
        //Code below used for testing
        if (moveObj == true)
        {
            float step = speed * Time.deltaTime; // step size = speed * frame time
            switch (Trigger.CurrentTempState) 
            {
                case ITemperature.tempState.Cold:
                    transform.position = Vector3.MoveTowards(transform.position, coldTarget.position, step); // moves position a step closer to the target position
                    break;
                case ITemperature.tempState.Hot:
                    transform.position = Vector3.MoveTowards(transform.position, hotTarget.position, step); // moves position a step closer to the target position
                    break;
                case ITemperature.tempState.Neutral:
                    transform.position = Vector3.MoveTowards(transform.position, origin.position, step); // moves position a step closer to the target position
                    Trigger.CurrentTempState = ITemperature.tempState.Cold;
                    break;
            }            
        }*/
    }
}
