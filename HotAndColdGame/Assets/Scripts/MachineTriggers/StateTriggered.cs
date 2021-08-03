using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTriggered : MonoBehaviour
{
    [SerializeField] public TemperatureStateBase Trigger = null;

    // Start is called before the first frame update
    public virtual void Start() {}//Lightning = GameMaster.instance.colourPallete.materials.LightningStrike; }

    // Update is called once per frame
    void Update() { }

    // listens for changes in state
    public virtual void ListenForTrigger()
    {
        switch (Trigger.CurrentTempState)
        {
            case ITemperature.tempState.Cold:
                // Cold behaviour
                break;

            case ITemperature.tempState.Hot:
                // Hot behaviour
                break;

            case ITemperature.tempState.Neutral:
                // Neutral behaviour
                break;
        }
    }
}


