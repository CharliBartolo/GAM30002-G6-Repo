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
    private Material emissiveMaterial;
    public float emissionValue;

    public bool playingSFX;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        emissiveMaterial  = GameMaster.instance.colourPallete.MachineEmissiveLights;
        foreach (var item in this.emissiveLights)
        {
            if (item.GetComponentsInChildren<Renderer>() != null)
            {
                Renderer[] r = item.GetComponentsInChildren<Renderer>();
                foreach (var obj in r)
                {
                    if(obj.GetComponentsInChildren<Renderer>() != null)
                    {
                        obj.GetComponentInChildren<Renderer>().sharedMaterial = emissiveMaterial;
                    }
                }
            }
        }
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
        PlaySound();


    }

    public void PlaySound()
    {
        // have pitch match current temperature
        if (machine.GetComponent<AudioSource>() != null)
            machine.GetComponent<AudioSource>().pitch = (float)Math.Abs((machine.CurrentTemperature) * 0.1);

        if (Math.Abs(machine.CurrentTemperature) > 5)
        {
            if (!playingSFX)
            {
                playingSFX = true;
                GameMaster.instance.audioManager.Play(machine.gameObject);
            }

        }
        else
        {
            playingSFX = false;
            GameMaster.instance.audioManager.Stop(machine.gameObject);
        }
        /*if (playingSFX)
        {
            // stop sound when reach max
            if(Math.Abs(machine.CurrentTemperature) == 100)
            {
                playingSFX = false;
                GameMaster.instance.audioManager.Stop(machine.gameObject);
            }

            
            if (Math.Abs(machine.CurrentTemperature) < 5)
            {
                playingSFX = false;
                GameMaster.instance.audioManager.Stop(machine.gameObject);
            }
        }*/
    }

    public void StartSound()
    {

    }
    public void StopSound()
    {

    }

    // set emmisive lights
    void SetEmissiveLights()
    {
        switch (machine.CurrentTempState)
        {
            case ITemperature.tempState.Cold:
                foreach (var item in this.emissiveLights)
                {
                    if(item.GetComponentsInChildren<Renderer>()!= null)
                    {
                        Renderer[] r = item.GetComponentsInChildren<Renderer>();
                        foreach (var obj in r)
                        {
                            obj.GetComponentInChildren<Renderer>().sharedMaterial.color = Crystal_Cold;
                            obj.GetComponentInChildren<Renderer>().sharedMaterial.SetColor("_EmissiveColor", Crystal_Cold * emissionValue);
                            //item.GetComponent<Renderer>().sharedMaterial.SetFloat("_EmissiveExposureWeight ",emissionValue);
                        }
                    }

                }
                break;
            case ITemperature.tempState.Neutral:
                foreach (var item in this.emissiveLights)
                {
                    if (item.GetComponentsInChildren<Renderer>() != null)
                    {
                        Renderer[] r = item.GetComponentsInChildren<Renderer>();
                        foreach (var obj in r)
                        {
                            obj.GetComponentInChildren<Renderer>().sharedMaterial.color = Crystal_Neutral;
                            obj.GetComponentInChildren<Renderer>().sharedMaterial.SetColor("_EmissiveColor", Crystal_Neutral * emissionValue);
                            //item.GetComponent<Renderer>().sharedMaterial.SetFloat("_EmissiveExposureWeight ", 0f);
                        }
                    }
                }
                break;
            case ITemperature.tempState.Hot:
                foreach (var item in this.emissiveLights)
                {
                    if (item.GetComponentsInChildren<Renderer>() != null)
                    {
                        Renderer[] r = item.GetComponentsInChildren<Renderer>();
                        foreach (var obj in r)
                        {
                            obj.GetComponentInChildren<Renderer>().sharedMaterial.color = Crystal_Hot;
                            obj.GetComponentInChildren<Renderer>().sharedMaterial.SetColor("_EmissiveColor", Crystal_Hot * emissionValue);
                            //item.GetComponent<Renderer>().sharedMaterial.SetFloat("_EmissiveExposureWeight ", emissionValue);
                        }
                    }
                }
                break;
        }
    }
}
