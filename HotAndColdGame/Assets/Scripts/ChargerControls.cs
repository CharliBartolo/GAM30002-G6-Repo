using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerControls : StateTriggered
{
    public bool delay;
    private bool delayTimer;
    public bool stateChanged;

    protected Color Crystal_Neutral;
    protected Color Crystal_Hot;
    protected Color Crystal_Cold;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();


    }

    // Update is called once per frame
    void Update()
    {
        if (Triggers != null)
            ListenForTrigger();
    }

    public override void ListenForTrigger()
    {
        foreach (var item in Triggers)
        {
            switch (item.CurrentTempState)
            {
                case ITemperature.tempState.Cold:
                    stateChanged = true;


                    break;


                case ITemperature.tempState.Hot:
                    stateChanged = true;


                    break;
            }
        }
    }
}
