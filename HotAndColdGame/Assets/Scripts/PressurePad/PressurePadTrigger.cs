using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePadTrigger : MonoBehaviour
{
    public PressurePadController pressurePad;
    protected bool triggered;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(Triggered)
        {
            Trigger();
        }
    }

    public virtual void Trigger()
    {
        //Debug.Log("TRIGGERED");
        if(!triggered)
        {
            triggered = true;
        }
    }

    public bool Triggered => pressurePad.PressedDown;

}
