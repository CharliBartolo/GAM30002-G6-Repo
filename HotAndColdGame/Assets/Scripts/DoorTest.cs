using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTest : TemperatureStateBase
{

    [Header("References")]
    [SerializeField] GameObject Door = null;
    [SerializeField] TemperatureStateBase Trigger = null;

    // Start is called before the first frame update
    protected override void Start()
    {
        if ((Trigger == null) && (GetComponent<TemperatureStateBase>() != null))
        {
            Trigger = GetComponent<TemperatureStateBase>();
        }
        else
        {
            Debug.LogWarning("Missing component. Please add one");
        }
    }

    protected override void FixedUpdate() 
    {
        if (Trigger.CurrentTempState == ITemperature.tempState.Cold)
        {
            Debug.Log("It works");
            Door.gameObject.transform.Translate(-1, 0, 0);
        }
    }
}