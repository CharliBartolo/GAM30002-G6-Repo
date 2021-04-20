using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureStateBase : MonoBehaviour
{
    public enum TempState {Hot, Cold, Neutral};
    public enum TempStatesAllowed {HotAndCold, OnlyHot, OnlyCold, OnlyNeutral}
    public bool debugEnabled;

    [SerializeField]
    protected TempState currentTempState = TempState.Neutral;
    [SerializeField]
    protected TempStatesAllowed allowedTempStates = TempStatesAllowed.HotAndCold;
    [SerializeField]
    protected float currentTemp = 0;
    public float tempMin = -100;
    public float tempMax = 100;
    public float tempNeutral = 0;

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
        if (debugEnabled)    
            Debug.Log(this.name + " is at temperature: " + CurrentTemperature + " and is in state: " + currentTempState);
    }

    private void TemperatureClamp()
    {
        currentTemp = Mathf.Clamp(currentTemp, tempMin, tempMax);     
    }

    public void ChangeTemperature(float valueToAdd)
    {
        currentTemp = currentTemp + valueToAdd;
        TemperatureClamp();
    }

    public void SetTemperature(float valueToSet)
    {
        currentTemp = valueToSet;
        TemperatureClamp();
    }

    public float CurrentTemperature
    {
        get => currentTemp;
    }

    public TempState CurrentTempState
    {
        get
        {
            return currentTempState;
        }
        set 
        {
            currentTempState = value;
        }
    }
}
