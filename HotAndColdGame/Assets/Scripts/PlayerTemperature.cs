using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerTemperature : TemperatureStateBase
{
    //Variables
    [SerializeField]
    private float _valueChangeSpeed; //The speed to change the value of the slider

    override protected void Start()
    {
        base.Start();
        _valueChangeSpeed = 10f;  //Set the speed of change
        powerDownRateInSeconds = 15f;  
    }


    /// <summary>
    /// Changes the value of the slider accordiing to the temperature variable passed through.
    /// </summary>
    /// <param name="slider"></param>  //UI element
    /// <param name="temperature"></param> //Passed through from the PlayerUI script
    public void ValueChange(Slider slider, string temperature)
    {
        slider.value = -currentTemp;
        // switch (temperature)
        // {
        //     //Cold
        //     case "Cold":
        //         slider.value += _valueChangeSpeed *  Time.deltaTime;
        //         break;

        //     //Hot
        //     case "Heat":
        //         slider.value -= _valueChangeSpeed * Time.deltaTime;
        //         break;

        //     //Neutral (Possible to clamp the value to exact value)
        //     case "Neutral":
        //         if (slider.value >= slider.maxValue / 2)
        //         {
        //             slider.value -= _valueChangeSpeed * Time.deltaTime;
        //         }
        //         else if (slider.value <= slider.maxValue / 2)
        //         {
        //             slider.value += _valueChangeSpeed * Time.deltaTime;
        //         }
        //         break;
        // }
    }
}
