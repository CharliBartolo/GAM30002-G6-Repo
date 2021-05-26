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
    public List<GameObject> AffectedObjects;
    public Light Spotight;


    // Start is called before the first frame update
    void Start()
    {
        AffectedObjects = new List<GameObject>();
        MainCrystal = GetComponent<CrystalBehaviour>();
        AreaCollider = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {

        AddAffectedObjects();
        RemoveAffectedObjects();

        PerformFX();

    }

    public void AddAffectedObjects()
    {
        foreach (var item in GetComponent<CrystalBehaviour>().objectsInTempArea.Keys)
        {
            if(!AffectedObjects.Contains(item))
            {
                AffectedObjects.Add(item);
            }
        }
    }
    public void RemoveAffectedObjects()
    {
        foreach (var item in GetComponent<CrystalBehaviour>().objectsInTempArea.Keys)
        {
            if (!AffectedObjects.Contains(item))
            {
                AffectedObjects.Remove(item);
            }
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if (!AffectedObjects.Contains(other.gameObject))
        {
            // add fx
            AffectedObjects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (AffectedObjects.Contains(other.gameObject))
        {
            // Remove fx
            //other.GetComponent<Collider>().material = AffectedObjects[other.gameObject];

            AffectedObjects.Remove(other.gameObject);
        }
    }
}
