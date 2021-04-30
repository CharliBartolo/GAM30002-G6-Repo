using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITemperature
{
    enum tempState {Hot, Cold, Neutral};
    enum tempStatesAllowed {HotAndCold, OnlyHot, OnlyCold, OnlyNeutral}
    public void PerformTemperatureBehaviour(tempState currentTemperatureState);

    void TemperatureClamp();
    void ChangeTemperature(float valueToAdd);
    void SetTemperature(float valueToSet);
    void PowerDownToNeutral(float tempCap);

    float CurrentTemperature {get;}

    tempState CurrentTempState{ get; set;}

    tempStatesAllowed TempStatesAllowed{ get; set;}

    // Values for x, y, z translate to tempMin, tempNeutral and tempMax
    float[] TempValueRange { get; set;}
}
