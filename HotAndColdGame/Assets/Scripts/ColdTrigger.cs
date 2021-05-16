using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdTrigger : MonoBehaviour
{
    [SerializeField] TemperatureStateBase Trigger = null;

    public Transform coldTarget; // the cold target position
    public GameObject platformObj;

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
                platformObj.transform.position = Vector3.MoveTowards(platformObj.transform.position, coldTarget.transform.position, step); // moves position a step closer to the target position
                break;
            default:
                break;
        }
    }
}
