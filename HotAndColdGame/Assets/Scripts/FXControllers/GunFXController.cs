using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GunFXController : FXController
{
    // timer variables
    private float swapModeDelay;
    private float triggerDelay;
   

    // Gun variables 
    public RayCastShootComplete gun;
    private bool switchingMode;

    // Gun components
    public GameObject BackCrystal;
    public GameObject CrystalCase;
    public GameObject CrystalCase1;
    public GameObject CrystalCase2;
    public GameObject BarrelCrystals;

    // Gun emissive components
    public GameObject[] emissiveLights;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        // initialise barrel crystals materials
        Renderer[] renderers = BarrelCrystals.GetComponentsInChildren<Renderer>();
        // set crystals colour to neutral if not shooting or cannot shoot
        foreach (var item in renderers)
        {
            item.sharedMaterial = GameObject.Find("ColourPallet").GetComponent<ColourPallet>().Crystal;
        }
        // initialise back crystal material
        BackCrystal.GetComponent<Renderer>().sharedMaterial = GameObject.Find("ColourPallet").GetComponent<ColourPallet>().Crystal;
        // initialse crystal case materials
        Renderer case1 = CrystalCase1.GetComponent<Renderer>();
        Renderer case2 = CrystalCase2.GetComponent<Renderer>();
        /*foreach (var item in renderers2)
        {
            item.sharedMaterial = GameObject.Find("ColourPallet").GetComponent<ColourPallet>().Crystal;
        }*/
        case1.sharedMaterial = new Material(GameObject.Find("ColourPallet").GetComponent<ColourPallet>().Crystal);
        case2.sharedMaterial = new Material(GameObject.Find("ColourPallet").GetComponent<ColourPallet>().Crystal);
        case1.sharedMaterial.color = Crystal_Cold;
        case2.sharedMaterial.color = Crystal_Hot;
        SetBackCrystal();
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
        // call set colour of barrel crystals
        SetBarrelCrystals();
        // call set colour of back crystal
        SetBackCrystal();
        // call set emissive lights
        SetEmissiveLights();
        // call check for gun mode switch
        CheckForModeSwitch();

        // update cased crystals
        Renderer case1 = CrystalCase1.GetComponent<Renderer>();
        if (case1.sharedMaterial.color != Crystal_Cold)
        {
           
            Renderer case2 = CrystalCase2.GetComponent<Renderer>();
            case1.sharedMaterial.color = Crystal_Cold;
            case2.sharedMaterial.color = Crystal_Hot;
        }
       
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
