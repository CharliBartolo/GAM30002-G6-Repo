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
    private bool triggerReleased;
    private bool triggerPressed;
   
    // Gun variables 
    public RayCastShootComplete gun;
    public GameObject gun_obj;
    private bool switchingMode;

    // Gun components
    public GameObject BackCrystals;
    public GameObject CrystalCase;
    public GameObject CrystalCase1;
    public GameObject CrystalCase2;
    public GameObject BarrelCrystals;

    // Gun emissive components
    public GameObject[] emissiveLights;
    public GameObject lightning;

    private bool inspectWeapon;
    private bool inspectingWeapon;
    private bool inspectingWeaponComplete;

    public enum WeaponState { Idle, TriggerPressed, TriggerReleased, Inspect, SwitchMode}

    public WeaponState weaponState;

    float startRotation;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        // initialise barrel crystals materials
        //Renderer[] renderers = BarrelCrystals.GetComponentsInChildren<Renderer>();
        // set crystals colour to neutral if not shooting or cannot shoot
/*        foreach (var item in renderers)
        {
            item.sharedMaterial = GameMaster.instance.colourPallete.materials.Crystal;
        }*/

        Renderer[] backCrystals = BackCrystals.GetComponentsInChildren<Renderer>();
        foreach (var item in backCrystals)
        {
            // initialise back crystal material
            if (item.GetComponent<Renderer>() != null)
                item.GetComponent<Renderer>().sharedMaterial = GameMaster.instance.colourPallete.materials.Crystal;
        }
        // initialse crystal case materials
        Renderer case1 = CrystalCase1.GetComponent<Renderer>();
        Renderer case2 = CrystalCase2.GetComponent<Renderer>();
        /*foreach (var item in renderers2)
        {
            item.sharedMaterial = GameObject.Find("ColourPallet").GetComponent<ColourPallet>().Crystal;
        }*/
        case1.sharedMaterial = new Material(GameMaster.instance.colourPallete.materials.Crystal);
        case2.sharedMaterial = new Material(GameMaster.instance.colourPallete.materials.Crystal);
        case1.sharedMaterial.color = Crystal_Cold;
        case2.sharedMaterial.color = Crystal_Hot;
        SetBackCrystal();


        weaponState = WeaponState.Inspect;
        NextState();
        inspectingWeapon = true;
        startRotation = CrystalCase.transform.eulerAngles.z;
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
        // call run gun animation
        AnimateGunTool();
        // call set colour of barrel crystals
        //SetBarrelCrystals();
        // call set colour of back crystal
        SetBackCrystal();
        // call set emissive lights
        SetEmissiveLights();
        // call check for gun mode switch
        CheckForModeSwitch();

        // update cased crystals
        Renderer case1 = CrystalCase1.GetComponent<Renderer>();
        Renderer case2 = CrystalCase2.GetComponent<Renderer>();
        case1.sharedMaterial.SetColor("_SurfaceAlphaColor", Crystal_Cold);
        case2.sharedMaterial.SetColor("_SurfaceAlphaColor", Crystal_Hot);

    }

    // weapon states
    IEnumerator IdleState()
    {
        //Debug.Log("Idle: Enter");
        gun_obj.GetComponent<Animator>().Play("Idle");

        while (weaponState == WeaponState.Idle)
        {
            // do state stuff
            yield return 0;
        }
        //Debug.Log("Idle: Exit");
        NextState();
    }

    IEnumerator TriggerPressedState()
    {
        //Debug.Log("TriggerPressed: Enter");
        gun_obj.GetComponent<Animator>().Play("TriggerPress");

        while (weaponState == WeaponState.TriggerPressed)
        {
            // do state stuff
            //RotateAmmo();
            //CrystalCase.transform.DOLocalRotate(new Vector3(0, 0, CrystalCase.transform.rotation.z + 180), delay, RotateMode.Fast);
            yield return 0;
        }
        //Debug.Log("TriggerPressed: Exit");
        NextState();
    }

    IEnumerator TriggerReleasedState()
    {
        //Debug.Log("TriggerReleased: Enter");
        gun_obj.GetComponent<Animator>().Play("TriggerRelease");

        while (weaponState == WeaponState.TriggerReleased)
        {
            // do state stuff
            if (AnimationComplete("TriggerRelease"))
                weaponState = WeaponState.Idle;

            yield return 0;
        }
        //Debug.Log("TriggerReleased: Exit");
        NextState();
    }

    IEnumerator InspectState()
    {
        //Debug.Log("Inspect: Enter");
        gun_obj.GetComponent<Animator>().Play("InspectTool");

        while (weaponState == WeaponState.Inspect)
        {
            // do state stuff

            if (AnimationComplete())
                weaponState = WeaponState.Idle;
               
            yield return 0;
        }
        //Debug.Log("Inspect: Exit");
        NextState();
    }

    IEnumerator SwitchModeState()
    {
        //Debug.Log("SwitchMode: Enter");
        gun_obj.GetComponent<Animator>().Play("SwitchMode");
        //StartCoroutine(RotateCrystalCase(0.5f));
        
        
        
        float midRotation = startRotation + 360f;
        float t = 0;

        StartCoroutine(RotateCrystalCase(0.5f));

        while (weaponState == WeaponState.SwitchMode)
        {
            // do state stuff

            if (AnimationComplete("SwitchMode"))
            {
                float endRotation = startRotation + 180f;
                CrystalCase.transform.eulerAngles = new Vector3(CrystalCase.transform.eulerAngles.x, CrystalCase.transform.eulerAngles.y, endRotation);
                startRotation = CrystalCase.transform.eulerAngles.z;
                FinishSwitchingMode();
                weaponState = WeaponState.Idle;
            }

            //Debug.Log("Playing switch animation");
            yield return 0;
        }
        //Debug.Log("SwitchMode: Exit");
        NextState();
    }

    void NextState()
    {
        string methodName = weaponState.ToString() + "State";
        System.Reflection.MethodInfo info = GetType().GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        StartCoroutine((IEnumerator)info.Invoke(this, null));
    }

    public virtual bool AnimationComplete()
    {
        return (gun_obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !gun_obj.GetComponent<Animator>().IsInTransition(0));
    }

    public virtual bool AnimationComplete(string animationName)
    {
        return (gun_obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(animationName) && gun_obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !gun_obj.GetComponent<Animator>().IsInTransition(0));
    }

    public void WeaponInspected()
    {
        inspectingWeapon = false;
    }

    //public void CreateLightning

    void AnimateGunTool()
    {
        if(!switchingMode)
        {
            if (gun.TriggerHeld)
            {
                if (weaponState != WeaponState.TriggerPressed && weaponState != WeaponState.Inspect)
                {
                    weaponState = WeaponState.TriggerPressed;
                }
            }
            else
            {
                if (weaponState == WeaponState.TriggerPressed)
                {
                    weaponState = WeaponState.TriggerReleased;
                }
            }
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

/*    // set barrel crystals
    void SetBarrelCrystals()
    {
        Renderer[] renderers = BarrelCrystals.GetComponentsInChildren<Renderer>();

        // set crystals colour to neutral if not shooting or cannot shoot
        if (!gun.TriggerHeld || !gun.CanShoot)
        {
            foreach (var item in renderers)
            {
                item.material.SetColor("_SurfaceAlphaColor", Crystal_Neutral);
            }
        }
        else
        {
            // set crystal colour to hot or cold depending on gun mode
            if (gun.cold)
            {
                foreach (var item in renderers)
                {
                    item.material.SetColor("_SurfaceAlphaColor", Crystal_Cold);
                }
               
            }
            else
            {
                foreach (var item in renderers)
                {
                    item.material.SetColor("_SurfaceAlphaColor", Crystal_Hot);
                }
            }
        }
    }*/

    // set back crystal color
    void SetBackCrystal()
    {
        Renderer[] renderers = BackCrystals.GetComponentsInChildren<Renderer>();

        if (switchingMode)
        {
            foreach (var item in renderers)
            {
                if (item.GetComponent<Renderer>() != null)
                    item.GetComponent<Renderer>().material.SetColor("_SurfaceAlphaColor", Crystal_Neutral);
            }
        }
        else
        {
            if (gun.cold)
            {
                foreach (var item in renderers)
                {
                    if (item.GetComponent<Renderer>() != null)
                        item.GetComponent<Renderer>().material.SetColor("_SurfaceAlphaColor", Crystal_Cold);
                }
            }
            else
            {
                foreach (var item in renderers)
                {
                    if (item.GetComponent<Renderer>() != null)
                        item.GetComponent<Renderer>().material.SetColor("_SurfaceAlphaColor", Crystal_Hot);
                }
            }
        }
    }

    // check for gun mode switch
    public void CheckForModeSwitch()
    {
        // get gun switch bool 
        if(gun.ModeSwitched && !switchingMode)
        {
            switchingMode = true;
            // disable shooting
            DisableShooting();

        }

        // while switching mode 
        if(switchingMode)
        {
            if (weaponState != WeaponState.SwitchMode)
                weaponState = WeaponState.SwitchMode;
        }
    }

    // rotate the ammo case
    IEnumerator RotateCrystalCase(float duration)
    {

        float startRotation = CrystalCase.transform.eulerAngles.z;
        float endRotation = startRotation + 540.0f;
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float zRotation = Mathf.Lerp(startRotation, endRotation, t / duration);
            CrystalCase.transform.eulerAngles = new Vector3(CrystalCase.transform.eulerAngles.x, CrystalCase.transform.eulerAngles.y, zRotation);
            yield return null;
        }
        CrystalCase.transform.eulerAngles = new Vector3(CrystalCase.transform.eulerAngles.x, CrystalCase.transform.eulerAngles.y, endRotation);
    }

    void FinishSwitchingMode()
    {
        switchingMode = false;
        EnableShooting();
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
