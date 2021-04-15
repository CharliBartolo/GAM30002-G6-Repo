using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureStateBase : MonoBehaviour
{
    public enum TempState {Hot, Cold, Neutral};
    public enum TempStatesAllowed {HotAndCold, OnlyHot, OnlyCold, OnlyNeutral}

    [SerializeField]
    protected TempState currentTempState = TempState.Neutral;
    [SerializeField]
    protected TempStatesAllowed allowedTempStates = TempStatesAllowed.HotAndCold;
    [SerializeField]
    protected float currentTemp = 0;
    protected float tempMin = -100;
    protected float tempMax = 100;
    protected float tempNeutral = 0;

    protected virtual void Start() 
    {
        
    }

    protected virtual void Update() 
    {
        TemperatureClamp();

        switch (currentTempState)
        {
            case TempState.Neutral:
                // Transition to Hot
                if (currentTemp == tempMax && 
                    (allowedTempStates == TempStatesAllowed.HotAndCold || allowedTempStates == TempStatesAllowed.OnlyHot))
                    {
                        currentTempState = TempState.Hot; 
                    }

                // Transition to Cold                                       
                else if (currentTemp == tempMin && 
                    (allowedTempStates == TempStatesAllowed.HotAndCold || allowedTempStates == TempStatesAllowed.OnlyCold))
                    {
                        currentTempState = TempState.Cold; 
                    }
                                   
                break;
            case TempState.Hot:
                if (currentTemp <= tempNeutral)
                {
                    currentTempState = TempState.Neutral;
                }
                break;
            case TempState.Cold:
                if (currentTemp >= tempNeutral)
                {
                    currentTempState = TempState.Neutral;
                }
                break;
            default:
                currentTempState = TempState.Neutral;
                break;
        }

        PerformTemperatureBehaviour(currentTempState);
          
    }

    protected virtual void PerformTemperatureBehaviour(TempState currentTemperatureState)
    {       
        
    }

    private void TemperatureClamp()
    {
        if (currentTemp < tempMin)
        {
            currentTemp = tempMin;
        }

        if (currentTemp > tempMax)
        {
            currentTemp = tempMax;
        }
    }

    public void ChangeTemperature(float valueToAdd)
    {
        currentTemp = currentTemp + valueToAdd;
    }

    public void SetTemperature(float valueToSet)
    {
        currentTemp = valueToSet;
    }

    public float CurrentTemperature
    {
        get => currentTemp;
    }
}
