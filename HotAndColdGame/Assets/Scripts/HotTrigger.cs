using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotTrigger : MonoBehaviour
{    
    [SerializeField] TemperatureStateBase Trigger = null;//Trigger onject for moving object    
    public Transform hotTarget;// the Hot target position    
    public Transform origin;//the object's starting position    
    public Transform collisonPos;//the position of collision    
    public GameObject Obj;//Object to be moved    
    public bool isBlocked = true;//to block target if scenario requires it

    [Header("Settings")]    
    [Range(1, 10)]
    [SerializeField] float Speed = 5;//speed at which the object travels

    // Update is called once per frame
    void Update()
    {
        // step size = speed * frame time
        float step = Speed * Time.deltaTime;
        //Checks trigger's state and acts accordingly
        switch (Trigger.CurrentTempState)
        {
            case ITemperature.tempState.Cold:
                //turns of block being blocked
                isBlocked = false;
                break;
            case ITemperature.tempState.Hot:
                //check if blocked
                if (isBlocked)
                {
                    //moves between 2 points
                    transform.position = Vector3.Lerp(collisonPos.transform.position, origin.transform.position, Mathf.PingPong(Time.time * step, 1.0f));                  
                }
                else 
                {
                    //moves position a step closer to the target position
                    Obj.transform.position = Vector3.MoveTowards(Obj.transform.position, hotTarget.transform.position, step); 
                }                
                break;
            default:
                break;
        }
    }
}
