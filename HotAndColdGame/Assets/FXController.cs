using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FXController : MonoBehaviour
{
    // FX variables
    public float delay;
    private float triggerDelay;
    private float swapDelay;
    private float delayTimer;

    // Gun variables 
    public RayCastShootComplete gun;
    private bool switchingMode;

    // Gun components
    public GameObject BackCrystal;
    public GameObject CrystalCase;
    public GameObject BarrelCrystals;
    public Animator Anim;

    // Gun emissive components
    public GameObject[] emissiveLights;

    // Gun crystal colours
    public Color Crystal_Neutral;
    public Color Crystal_Hot;
    public Color Crystal_Cold;

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();   
    }

    // Update is called once per frame
    void Update()
    {
        // call set colour of barrel crystals
        SetBarrelCrystals();
        // call set colour of back crystal
        SetBackCrystal();
        // call set emissive lights
        SetEmissiveLights();
        // call check for gun mode switch
        CheckForModeSwitch();
      

    }

    // set emmisive lights
    void SetEmissiveLights()
    {
        // set crystals colour to neutral if not shooting or cannot shoot
        if (!gun.TriggerHeld || !gun.CanShoot)
        {
            foreach (var item in emissiveLights)
            {
                item.GetComponent<Renderer>().sharedMaterial.color = Crystal_Neutral;
            }
        }
        else
        {
            // set crystal colour to hot or cold depending on gun mode
            if (gun.cold)
            {
                foreach (var item in emissiveLights)
                {
                    item.GetComponent<Renderer>().sharedMaterial.color = Crystal_Cold;
                }

            }
            else
            {
                foreach (var item in emissiveLights)
                {
                    item.GetComponent<Renderer>().sharedMaterial.color = Crystal_Hot;
                }
            }
        }
    }

    // set barrel crystals
    void SetBarrelCrystals()
    {
        Renderer[] renderers = BarrelCrystals.GetComponentsInChildren<Renderer>();

        // set crystals colour to neutral if not shooting or cannot shoot
        if (!gun.TriggerHeld || !gun.CanShoot)
        {
            foreach (var item in renderers)
            {
                item.material.color = Crystal_Neutral;
            }
        }
        else
        {
            // set crystal colour to hot or cold depending on gun mode
            if (gun.cold)
            {
                foreach (var item in renderers)
                {
                    item.material.color = Crystal_Cold;
                }
               
            }
            else
            {
                foreach (var item in renderers)
                {
                    item.material.color = Crystal_Hot;
                }
            }
        }
    }

    // set back crystal color
    void SetBackCrystal()
    {
        if(switchingMode)
        {
            BackCrystal.GetComponentInChildren<Renderer>().material.color = Crystal_Neutral;
        }
        else
        {
            if (gun.cold)
            {
                BackCrystal.GetComponentInChildren<Renderer>().material.color = Crystal_Cold;
            }
            else
            {
                BackCrystal.GetComponentInChildren<Renderer>().material.color = Crystal_Hot;
            }
        }
    }

    // check for gun mode switch
    public void CheckForModeSwitch()
    {
        // get gun switch bool 
        if(gun.ModeSwitched)
        {
            switchingMode = true;
            // disable shooting
            DisableShooting();
        }

        // while switching mode 
        if(switchingMode)
        {
            // perform ammo rotation
            RotateAmmo();
            
            // count through delay time and reenable shooting, and set switching to off
            delayTimer += Time.deltaTime;
            if (delayTimer > delay)
            {
                EnableShooting();
                switchingMode = false;
                delayTimer = 0;
            }
        }
    }

    // rotate the ammo case
    void RotateAmmo()
    {
        if (gun.cold)
        {
            CrystalCase.transform.DOLocalRotate(new Vector3(0, 0, CrystalCase.transform.rotation.z + 0), delay, RotateMode.Fast);
        }
        else
        {
            CrystalCase.transform.DOLocalRotate(new Vector3(0, 0, CrystalCase.transform.rotation.z + 180), delay, RotateMode.Fast);

        }
    }
    // enable shooting
    void EnableShooting()
    {
        gun.CanShoot = true;
    }
    // disable shooting
    void DisableShooting()
    {
        gun.CanShoot = false;
    }


   
}
