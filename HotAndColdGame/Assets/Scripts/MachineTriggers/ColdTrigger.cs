using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdTrigger : StateTriggered
{    
    public Transform coldTarget; //the cold target position    
    public GameObject Obj;//Object to be moved

    [Header("Settings")]    
    [Range(1, 10)]
    [SerializeField] float Speed = 5;//speed at which the object travels

    // Update is called once per frame
    void Update() { ListenForTrigger(); }

    public override void ListenForTrigger()
    {
        // step size = speed * frame time
        float step = Speed * Time.deltaTime;
        //Checks trigger's state and acts accordingly
        switch (Trigger.CurrentTempState)
        {
            case ITemperature.tempState.Cold:
                // moves position a step closer to the target position
                Obj.transform.position = Vector3.MoveTowards(Obj.transform.position, coldTarget.transform.position, step);
                break;
            default:
                break;
        }
    }
}
