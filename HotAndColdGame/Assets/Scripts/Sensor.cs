using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : TemperatureStateBase
{
    //private TemperatureStateBase SensorRB;

    private void Update()
    {
        /*if (this.CurrentTempState == TempState.Cold) 
        {
            Debug.Log("Hail Yeah");
        }
        switch (this.CurrentTempState) //checks if sensor is Hot or Cold state
        {
            case TemperatureStateBase.TempState.Hot: //if hot
                Debug.Log("Player is overheating");
                Fade.GetComponent<Image>().color = new Color32(255, 0, 0, 100);//red, green, blue, alpha
                Fade.CrossFadeAlpha(1, 3, false); //Fully fade Image with the duration of 3 seconds
                break;
            case TemperatureStateBase.TempState.Cold://if cold
                Debug.Log("Player is freezing");
                Fade.GetComponent<Image>().color = new Color32(0, 200, 255, 100);//red, green, blue, alpha
                Fade.CrossFadeAlpha(1, 3, false); //Fully fade Image with the duration of 3 seconds
                break;
            default:
                //crntTemp.CurrentTempState = TemperatureStateBase.TempState.Neutral;//nothing happens
                break;
        }*/
    }
}