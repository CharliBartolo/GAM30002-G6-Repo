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
    public Light MainCrystaLight;
    public SphereCollider AreaCollider;
    public List<GameObject> AffectedObjects;
    public Light Spotight;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        AffectedObjects = new List<GameObject>();
        MainCrystal = GetComponent<CrystalBehaviour>();
        MainCrystaLight = transform.Find("Area Light").GetComponent<Light>();
        AreaCollider = GetComponent<SphereCollider>();
        MainCrystal.GetComponent<Renderer>().sharedMaterial = new Material(GameMaster.instance.colourPallete.Crystal);
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
        ColourCrystal();
        GrowAreaCollider();
    }

    // colour crystial according to state
    public void ColourCrystal()
    {
       //Debug.Log("Temp: " + MainCrystal.CurrentTemperature*1000);

        
        if (MainCrystal.CurrentTemperature < -1)
        {
            MainCrystaLight.color = Crystal_Cold;
            if (Math.Abs(MainCrystal.CurrentTemperature) > 50)
                MainCrystaLight.intensity = Math.Abs(MainCrystal.CurrentTemperature * GameMaster.instance.colourPallete.CrystalEmissionValue);
            else
                MainCrystaLight.intensity = Math.Abs(50 * GameMaster.instance.colourPallete.CrystalEmissionValue);
            MainCrystal.GetComponent<Renderer>().sharedMaterial.SetColor("_EmissiveColor", Crystal_Cold);
            MainCrystal.GetComponent<Renderer>().sharedMaterial.color = Crystal_Cold;

        }
        else if (MainCrystal.CurrentTemperature > 1)
        {
            MainCrystaLight.color = Crystal_Hot;
            if(Math.Abs(MainCrystal.CurrentTemperature)>50)
                MainCrystaLight.intensity = Math.Abs(MainCrystal.CurrentTemperature * GameMaster.instance.colourPallete.CrystalEmissionValue);
            else
                MainCrystaLight.intensity = Math.Abs(50 * GameMaster.instance.colourPallete.CrystalEmissionValue);

            MainCrystal.GetComponent<Renderer>().sharedMaterial.SetColor("_EmissiveColor", Crystal_Hot);
            MainCrystal.GetComponent<Renderer>().sharedMaterial.color = Crystal_Hot;
        }
        else
        {
            MainCrystaLight.color = Crystal_Neutral;
            MainCrystaLight.intensity = Math.Abs(50 * GameMaster.instance.colourPallete.CrystalEmissionValue);
            MainCrystal.GetComponent<Renderer>().sharedMaterial.SetColor("_EmissiveColor", Crystal_Neutral);
            MainCrystal.GetComponent<Renderer>().sharedMaterial.color = Crystal_Neutral;
        }

       /* switch (MainCrystal.CurrentTempState)
        {
            case ITemperature.tempState.Cold:
                //Debug.Log("crystal cold");
               
                //MainCrystal.GetComponent<Renderer>().sharedMaterial.color = Crystal_Cold;
                //MainCrystal.GetComponent<Renderer>().sharedMaterial.SetColor("_EmissiveColor", Crystal_Cold * GameObject.Find("ColourPallet").GetComponent<ColourPallet>().CrystalEmissionValue);
                //MainCrystal.GetComponent<Renderer>().sharedMaterial.SetColor("_EmissiveColor", Crystal_Cold * GameObject.Find("ColourPallet").GetComponent<ColourPallet>().CrystalEmissionValue);

                break;
            case ITemperature.tempState.Neutral:
                //Debug.Log("crystal neutral");
                //MainCrystaLight.color = Crystal_Neutral;
                //MainCrystal.GetComponent<Renderer>().sharedMaterial.color = Crystal_Neutral;
                //MainCrystal.GetComponent<Renderer>().sharedMaterial.SetColor("_EmissiveColor", Crystal_Neutral * 0f);
                //MainCrystal.GetComponent<Renderer>().sharedMaterial.SetColor("_EmissiveColor", Crystal_Neutral * 0f);

                break;
            case ITemperature.tempState.Hot:
                //Debug.Log("crystal hot");
                //MainCrystaLight.color = Crystal_Hot;
                //MainCrystal.GetComponent<Renderer>().sharedMaterial.color = Crystal_Hot;
                //MainCrystal.GetComponent<Renderer>().sharedMaterial.SetColor("_EmissiveColor", Crystal_Hot * GameObject.Find("ColourPallet").GetComponent<ColourPallet>().CrystalEmissionValue);
                //MainCrystal.GetComponent<Renderer>().sharedMaterial.SetColor("_EmissiveColor", Crystal_Hot * GameObject.Find("ColourPallet").GetComponent<ColourPallet>().CrystalEmissionValue);

                break;
        }*/
    }

    public void GrowAreaCollider()
    {
        AreaCollider.radius = Math.Abs(CrystalTemp()) / 150;
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
        if(other != null && AreaCollider.radius > 0.01)
        {
            if(other.gameObject!= this.gameObject)
            {
                if(other.transform.parent != this.gameObject)
                {
                    if(other.GetComponent<CrystalBehaviour>()==null && other.GetComponent<MachineFXController>() == null)
                    {
                        if (other.gameObject != null)
                        {
                            if (other.gameObject.tag != "Player")
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
