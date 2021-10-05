using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerTemperature : TemperatureStateBase
{
    public float incomingTempMod = 1f;

    override protected void Start()
    {
        base.Start();        
        powerDownRateInSeconds = 15f;  
    }

    public override void PowerDownToNeutral(float tempCap)
    {
        float min = tempValueRange[0];  //cold -100f
        float max = tempValueRange[2];  //hot 100f
        float mid = tempValueRange[1];  //neutral 0

        //debug for testing purposes
        //Debug.Log("crnt" + currentTemp);

        currentTemp = Mathf.Lerp(currentTemp, mid, Time.deltaTime);

        if (tempValueRange[1] < tempCap)
            currentTemp = Mathf.Clamp(currentTemp, tempValueRange[1], tempCap);
        else
            currentTemp = Mathf.Clamp(currentTemp, tempCap, tempValueRange[1]);       
    }

    public override void ChangeTemperature(float valueToAdd)
    {
        if (canTempChange)
        {
            float prevTemp = currentTemp;

            currentTemp = currentTemp + valueToAdd * incomingTempMod;
            TemperatureClamp();

            TempChanged();
        }
    }


    /// <summary>
    /// Changes the value of the slider accordiing to the temperature variable passed through.
    /// </summary>
    /// <param name="slider"></param>  //UI element
    /// <param name="temperature"></param> //Passed through from the PlayerUI script
    public void ValueChange(Slider slider, string temperature)
    {
        slider.value = -currentTemp;//for slider
    }
}
