using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CrystalFXController : FXController
{
    // variables 


    // components
    public CrystalBehaviour MainCrystal;
    public SphereCollider AreaCollider;
    public GameObject[] AffectedObjects;
    public Light Spotight;

    //emissive components
    public GameObject[] Lights;


    // Start is called before the first frame update
    void Start()
    {
        MainCrystal = GetComponent<CrystalBehaviour>();
        AreaCollider = GetComponent<SphereCollider>();
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

        GrowAreaCollider();
    }

    public void GrowAreaCollider()
    {
        AreaCollider.radius = Math.Abs(CrystalTemp()) / 100;
    }

    // get back crystal color
    public float CrystalTemp()
    {
        return MainCrystal.CurrentTemperature;
    }

    public int CrystalTempState()
    {
        switch(MainCrystal.CurrentTempState)
        {
            case ITemperature.tempState.Cold:
                return -1;
            case ITemperature.tempState.Neutral:
                return 0;
            case ITemperature.tempState.Hot:
                return 1;
            default:
                return 0;
        }
    }

    // check for gun mode switch
    public void CheckForTempChange()
    {

    }
   
}
