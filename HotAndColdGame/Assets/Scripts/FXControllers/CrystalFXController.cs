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

    // add fx
    public void AddLightTextureComponent(Collider other)
    {
        if(other)
        {
            // add fx
            AffectedObjects.Add(other.gameObject);
            other.gameObject.AddComponent<LightTexture>();
            other.gameObject.GetComponent<LightTexture>().crystal = gameObject.GetComponent<CrystalBehaviour>();
            other.gameObject.GetComponent<LightTexture>().spotlight = transform.Find("Spot Light");
        }
      
    }

    // remove fx
    public void RemoveLightTextureComponent(Collider other)
    {
        if (other.gameObject.GetComponent<LightTexture>() != null)
        {
            //Remove fx
            //other.GetComponent<Collider>().material = AffectedObjects[other.gameObject];
            other.gameObject.GetComponent<LightTexture>().RemoveHiddenMaterial();
            Destroy(other.gameObject.GetComponent<LightTexture>());
            AffectedObjects.Remove(other.gameObject);
        }
    }

    // detect objects 
    private void OnTriggerEnter(Collider other)
    {
        if(other != null)
        {
            if(other.gameObject != null)
            {
                if (other.gameObject.tag != "Player" )
                {
                    if (other.transform.parent != null)
                    {
                        if (other.transform.parent.gameObject.tag != "Player")
                        {
                            if (other.gameObject.GetComponent<MeshRenderer>() != null)
                            {
                                // if not have lighttexture, add it
                                if (other.gameObject.GetComponent<LightTexture>() == null)
                                {
                                    AddLightTextureComponent(other);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // remove fx when out of range
        if (AffectedObjects.Contains(other.gameObject))
        {
            RemoveLightTextureComponent(other);
        }
    }
}
