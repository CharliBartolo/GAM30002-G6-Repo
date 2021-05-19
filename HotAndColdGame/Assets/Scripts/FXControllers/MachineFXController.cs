using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MachineFXController : FXController
{
    // Machine variables 
    public TemperatureStateBase machine;

    // Machine emissive components
    public GameObject[] emissiveLights;
    public float emissionValue;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PerformFX();
    }

    // perform FX
    public override void PerformFX()
    {
        base.PerformFX();

        SetEmissiveLights();
    }

    // set emmisive lights
    void SetEmissiveLights()
    {
        // set crystals colour to neutral if not shooting or cannot shoot
        switch (machine.CurrentTempState)
        {
            case ITemperature.tempState.Cold:
                foreach (var item in emissiveLights)
                {
                    item.GetComponent<Renderer>().sharedMaterial.color = Crystal_Cold;
                    item.GetComponent<Renderer>().sharedMaterial.SetColor("_EmissiveColor", Crystal_Cold * emissionValue);
                }
                break;
            case ITemperature.tempState.Neutral:
                foreach (var item in emissiveLights)
                {
                    item.GetComponent<Renderer>().sharedMaterial.color = Crystal_Neutral;
                    item.GetComponent<Renderer>().sharedMaterial.SetColor("_EmissiveColor", Crystal_Neutral * 0f);
                }
                break;
            case ITemperature.tempState.Hot:
                foreach (var item in emissiveLights)
                {
                    item.GetComponent<Renderer>().sharedMaterial.color = Crystal_Hot;
                    item.GetComponent<Renderer>().sharedMaterial.SetColor("_EmissiveColor", Crystal_Hot * emissionValue);
                }
                break;
        }
    }
}
