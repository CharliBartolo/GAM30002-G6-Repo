using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class MachineFXController : FXController
{
    // Machine variables 
    [Header("Machine Components")]
    public TemperatureStateBase machine;
    public GameObject triggeredObj;
   
    [Header("Light Properties")]
    // Machine emissive components
    public GameObject[] emissiveLights;
    private Material emissiveMaterial;
    public float emissionValue;

    [Header("Line Properties")]
    [SerializeField] public Transform LightningHit = null;
    [SerializeField] public LineRenderer Lightning = null;
    [SerializeField] public float LineWidth = 1;
    [SerializeField] [Range(0.1f, 1.0f)] public float Opacity = 1;

    [Header("Sound Properties")]
    public bool playingSFX;


    // Start is called before the first frame update
    private void Awake()
    {
       
        SetAsTrigger();
    }
    public override void Start()
    {
        base.Start();
        emissiveMaterial = GameMaster.instance.colourPallete.materials.EmissiveLights;
        //Lightning = GameMaster.instance.colourPallete.materials.LightningStrike;
        foreach (var item in this.emissiveLights)
        {
            if (item.GetComponentsInChildren<Renderer>() != null)
            {
                Renderer[] r = item.GetComponentsInChildren<Renderer>();
                foreach (var obj in r)
                {
                    if(obj.GetComponentsInChildren<Renderer>() != null)
                    {
                        obj.GetComponentInChildren<Renderer>().sharedMaterial = new Material(emissiveMaterial);
                    }
                }
            }
        }
        Lightning.enabled = true;
        Lightning.GetComponent<DigitalRuby.LightningBolt.LightningBoltScript>().StartObject = transform.Find("Current").gameObject;
        Lightning.GetComponent<DigitalRuby.LightningBolt.LightningBoltScript>().EndObject = LightningHit.gameObject;
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
        UpdateLineProperties();
    }


    void SetAsTrigger()
    {
        // set as state trigger
        if(triggeredObj.GetComponentInChildren<StateTriggered>() != null)
            triggeredObj.GetComponentInChildren<StateTriggered>().Trigger = this.gameObject.GetComponent<TemperatureStateBase>();

        // set as platform trigger
        if (triggeredObj.GetComponentInChildren<PlatformWithTemperature>() != null)
            triggeredObj.GetComponentInChildren<PlatformWithTemperature>().Trigger = this.gameObject.GetComponent<TemperatureStateBase>();

    }

    public void SetLightningPos(Transform target)
    {

        Lightning.GetComponent<DigitalRuby.LightningBolt.LightningBoltScript>().EndObject = target.gameObject;
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
    }

    public void StartSound()
    {

    }
    public void StopSound()
    {

    }

    public void UpdateLineProperties()
    {
        Lightning.startWidth = LineWidth;
        Lightning.endWidth = LineWidth;

        Color col = Lightning.startColor;
        col.a = Opacity;
        Lightning.startColor = col;
        Lightning.endColor = col;
    }
    public void SetLineColour(Color colour)
    {
        Lightning.startColor = colour;
        Lightning.endColor = colour;
    }


    // set emmisive lights
    void SetEmissiveLights()
    {
        switch (machine.CurrentTempState)
        {
            case ITemperature.tempState.Cold:

                if (Lightning.enabled == false)
                {
                    SetLineColour(GameMaster.instance.colourPallete.Negative);
                    Lightning.enabled = true;
                }

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

                if(Lightning.enabled)
                {
                    SetLineColour(GameMaster.instance.colourPallete.Neutral);
                    Lightning.enabled = false;
                }

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

                if (Lightning.enabled == false)
                {
                    SetLineColour(GameMaster.instance.colourPallete.Positive);
                    Lightning.enabled = true;
                }

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
