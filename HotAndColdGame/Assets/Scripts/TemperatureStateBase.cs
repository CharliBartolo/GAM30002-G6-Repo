using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureStateBase : MonoBehaviour, ITemperature
{
    //public enum tempState {Hot, Cold, Neutral};
    //public enum tempStatesAllowed {HotAndCold, OnlyHot, OnlyCold, OnlyNeutral}
    public bool debugEnabled;

    [SerializeField]
    protected ITemperature.tempState currenttempState = ITemperature.tempState.Neutral;
    [SerializeField]
    protected ITemperature.tempStatesAllowed allowedtempStates = ITemperature.tempStatesAllowed.HotAndCold;
    [SerializeField]
    protected float currentTemp = 0;    
    protected float powerDownRateInSeconds = 30f;

    public bool isPermanentlyPowered = false;
    public float[] tempValueRange = new float[3] {-100f, 0f, 100f};
    //public float tempMin = -100;
    //public float tempMax = 100;
    //public float tempNeutral = 0;

    public bool isReturningToNeutral = true;

    private float countdownBeforeReturnToNeutral;
    public float startingCountdownBeforeReturnToNeutral = 2f;

    protected virtual void Start() 
    {
        countdownBeforeReturnToNeutral = startingCountdownBeforeReturnToNeutral;
    }    

    protected virtual void FixedUpdate() 
    {
        TemperatureClamp();        

        switch (currenttempState)
        {
            case ITemperature.tempState.Neutral:
                // Transition to Hot
                if (currentTemp == tempValueRange[2] && 
                    (allowedtempStates == ITemperature.tempStatesAllowed.HotAndCold || allowedtempStates == ITemperature.tempStatesAllowed.OnlyHot))
                    {
                        currenttempState = ITemperature.tempState.Hot; 
                    }

                // Transition to Cold                                       
                else if (currentTemp == tempValueRange[0] && 
                    (allowedtempStates == ITemperature.tempStatesAllowed.HotAndCold || allowedtempStates == ITemperature.tempStatesAllowed.OnlyCold))
                    {
                        currenttempState = ITemperature.tempState.Cold; 
                    }
                                   
                break;
            case ITemperature.tempState.Hot:
                if (currentTemp <= tempValueRange[1])
                {
                    currenttempState = ITemperature.tempState.Neutral;
                }
                break;
            case ITemperature.tempState.Cold:
                if (currentTemp >= tempValueRange[1])
                {
                    currenttempState = ITemperature.tempState.Neutral;
                }
                break;
            default:
                currenttempState = ITemperature.tempState.Neutral;
                break;
        }

        if (!isPermanentlyPowered && isReturningToNeutral)
        {
            if (currentTemp > 0)
            {
                PowerDownToNeutral(tempValueRange[2]);
            }
            else if (currentTemp < 0)
            {
                PowerDownToNeutral(tempValueRange[0]);
            }
        }  

        CheckIfTempChanged(currentTemp, currentTemp);
         
        PerformTemperatureBehaviour(currenttempState);              
    }

    public virtual void PerformTemperatureBehaviour(ITemperature.tempState currentTemperatureState)
    {   
        if (debugEnabled)    
            Debug.Log(this.name + " is at temperature: " + CurrentTemperature + " and is in state: " + currenttempState);
    }

    public void TemperatureClamp()
    {
        currentTemp = Mathf.Clamp(currentTemp, tempValueRange[0], tempValueRange[2]);     
    }

    public void ChangeTemperature(float valueToAdd)
    {
        float prevTemp = currentTemp;

        currentTemp = currentTemp + valueToAdd;
        TemperatureClamp();

        CheckIfTempChanged(prevTemp, currentTemp);
    }

    public void SetTemperature(float valueToSet)
    {
        float prevTemp = currentTemp;

        currentTemp = valueToSet;
        TemperatureClamp();

        CheckIfTempChanged(prevTemp, currentTemp);
    }

    public virtual void PowerDownToNeutral(float tempCap)
    {
        // If temperature is not zero, begin approaching neutral by ticking down current temperature until it hits neutral.
        float tempPercent = currentTemp / (tempCap - tempValueRange[1]);   // Ranges from 0 - 1
        //Debug.Log(tempPercent);
        // Rearrange equation for currentTemp while adjusting hot percent
        currentTemp = (tempPercent - (1 / powerDownRateInSeconds * Time.deltaTime)) * (tempCap - tempValueRange[1]);

        if (tempValueRange[1] < tempCap) 
            currentTemp = Mathf.Clamp(currentTemp, tempValueRange[1], tempCap);
        else
            currentTemp = Mathf.Clamp(currentTemp, tempCap, tempValueRange[1]);    
    }

    public void CheckIfTempChanged(float prevTemp, float currentTemp)
    {
        if (prevTemp != currentTemp)
        {
            if (isReturningToNeutral)
            {
                isReturningToNeutral = false;
                countdownBeforeReturnToNeutral = startingCountdownBeforeReturnToNeutral;                
            }
        }
        else 
        {
            countdownBeforeReturnToNeutral -= Time.deltaTime;
        }

        if (countdownBeforeReturnToNeutral <= 0f)
        {
            isReturningToNeutral = true;
        }
        else
        {
            isReturningToNeutral = false;
        }
    }

    public float CurrentTemperature
    {
        get => currentTemp;
    }

    public ITemperature.tempState CurrentTempState
    {
        get
        {
            return currenttempState;
        }
        set 
        {
            currenttempState = value;
        }
    }

    public ITemperature.tempStatesAllowed TempStatesAllowed
    {
        get => allowedtempStates;
        set => allowedtempStates = value;
    }

    public float[] TempValueRange
    {
        get => tempValueRange;
        set => tempValueRange = value;
    } 
}
