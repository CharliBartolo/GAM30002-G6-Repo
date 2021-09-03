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
    public GameObject arm_obj;
    public GameObject gun_obj;

    private bool switchingMode;

    // Gun components
    public GameObject BackCrystals;
    public GameObject[] CrystalTubes;
    public GameObject[] TubeCrystals;
    public GameObject CrystalCase;
    public GameObject CrystalCase1;
    public GameObject CrystalCase2;
    public GameObject BarrelCrystals;

    // Gun emissive components
    public GameObject[] emissiveLights;
    public GameObject lightning;

    private bool inspectWeapon;
    public bool inspectingWeapon;
    private bool inspectingWeaponComplete;

    public bool isProp = false;
    
    private Material crystal_mat;
    public enum WeaponState { Idle_Equipped, Idle_Unequipped, TriggerPressed, TriggerReleased, Inspect, SwitchMode, Grab, Place, GrabRetract}

    public WeaponState weaponState;

    float startRotation;
    Quaternion caseRot;

    public bool equipped;
    public int weaponUpgradeState = 0;
    public bool isGrabbing = false;
    public bool isPlacing = false;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        crystal_mat = new Material(GameMaster.instance.colourPallete.materials.Crystal);

        if (TubeCrystals != null)
            SetTubeCrystals();

        // initialse crystal case materials
        

        //SetBackCrystal();

        startRotation = CrystalCase.transform.eulerAngles.z;
        caseRot = CrystalCase.transform.rotation;

        equipped = gameObject.GetComponent<PlayerController>().playerInventory.Contains("Raygun");
        //equipped = false;

        if (equipped)
        {
            if (gun_obj.activeSelf == false)
                gun_obj.SetActive(true);
            gun_obj.SetActive(true);
            weaponState = WeaponState.Inspect;
            NextState();
            inspectingWeapon = true;
        }
        else
        {
            weaponState = WeaponState.Idle_Unequipped;
            NextState();
            if (gun_obj.activeSelf == true)
                gun_obj.SetActive(false);
        }

        //UpdateCasedCrystals();

        //SetWeaponMods(weaponUpgradeState);
        //gun_obj.SetActive(false);

        /*Renderer case1 = CrystalCase1.GetComponent<Renderer>();
        Renderer case2 = CrystalCase2.GetComponent<Renderer>();
        case1.sharedMaterial.SetColor("_SurfaceAlphaColor", Crystal_Cold);
        case2.sharedMaterial.SetColor("_SurfaceAlphaColor", Crystal_Hot);*/
    }

    // Update is called once per frame
    void Update()
    {
        PerformFX();
    }

/*    private void OnEnable()
    {
        EquipTool();
    }*/

    // perform FX


    public void SetWeaponMods(int state)
    {
        //Debug.Log("WEAPON MOD STATE: " + state);
        switch(state)
        {
            case 0:
                EnableMod(0, false);
                EnableMod(1, false);
                CrystalCase.SetActive(false);
                CrystalCase1.SetActive(false);
                CrystalCase2.SetActive(false);
                break;

            case 1:
                EnableMod(0, true);
                EnableMod(1, false);
                CrystalCase.SetActive(true);
                CrystalCase1.SetActive(true);
                CrystalCase2.SetActive(false);

                Material mat = new Material(crystal_mat);
                mat.SetColor("_SurfaceAlphaColor", Crystal_Cold);
                //CrystalCase1.GetComponent<Renderer>().sharedMaterial.SetColor("_SurfaceAlphaColor", Crystal_Cold);
                CrystalCase1.GetComponent<Renderer>().sharedMaterial = mat;
                break;
            case 2:
                EnableMod(0, true);
                EnableMod(1, true);
                CrystalCase.SetActive(true);
                CrystalCase1.SetActive(true);
                CrystalCase2.SetActive(true);
                /*   CrystalCase2.GetComponent<Renderer>().sharedMaterial.SetColor("_SurfaceAlphaColor", Crystal_Hot);
                   CrystalCase1.GetComponent<Renderer>().sharedMaterial.SetColor("_SurfaceAlphaColor", Crystal_Cold);*/
                Material mat1 = new Material(crystal_mat);
                mat1.SetColor("_SurfaceAlphaColor", Crystal_Cold);
                //CrystalCase1.GetComponent<Renderer>().sharedMaterial.SetColor("_SurfaceAlphaColor", Crystal_Cold);
                CrystalCase1.GetComponent<Renderer>().sharedMaterial = mat1;
                Material mat2 = new Material(crystal_mat);
                mat2.SetColor("_SurfaceAlphaColor", Crystal_Hot);
                //CrystalCase1.GetComponent<Renderer>().sharedMaterial.SetColor("_SurfaceAlphaColor", Crystal_Cold);
                CrystalCase2.GetComponent<Renderer>().sharedMaterial = mat2;
                break;

        }
    }

    public void EnableMod(int mod, bool enableState)
    {
        CrystalTubes[mod].SetActive(enableState);
        
    }


    public override void PerformFX()
    {
        base.PerformFX();
       

        // call run gun animation
        AnimateGunTool();
        // call set colour of barrel crystals
        //SetBarrelCrystals();
        // call set colour of back crystal
        //SetBackCrystal();
        if (TubeCrystals != null)
            SetTubeCrystals();
        // call set emissive lights
        //SetEmissiveLights();
        // call check for gun mode switch

        //UpdateCasedCrystals();
        CheckForModeSwitch();


        // update cased crystals
       

    }

    public void InteractCalled()
    {

    }

    public void EquipTool()
    {
        isGrabbing = false;
        gun_obj.SetActive(true);
        arm_obj.SetActive(true);
        equipped = true;
        inspectingWeapon = true;
        weaponState = WeaponState.Inspect;
        NextState();
    }
    public void Grab()
    {
        if(!isGrabbing)
        {
            weaponState = WeaponState.Grab;
            NextState();
            isGrabbing = true;
        }          
    }

    public void PlaceTool()
    {
        if (!isPlacing)
        {
            weaponState = WeaponState.Place;
            NextState();
            isPlacing = true;
        }
    }

    public void UnEquipTool()
    {
        equipped = false;
        gun_obj.SetActive(false);
        arm_obj.SetActive(true);
        inspectingWeapon = false;
        weaponState = WeaponState.Idle_Unequipped;
    }

    public void UpdateCasedCrystals()
    {
        Renderer case1 = CrystalCase1.GetComponent<Renderer>();
        Renderer case2 = CrystalCase2.GetComponent<Renderer>();
        case1.sharedMaterial.color = Crystal_Cold;
        case2.sharedMaterial.color = Crystal_Hot;
        /*foreach (var item in renderers2)
        {
            item.sharedMaterial = GameObject.Find("ColourPallet").GetComponent<ColourPallet>().Crystal;
       *//* }*//*
        case1.sharedMaterial = new Material(GameMaster.instance.colourPallete.materials.Crystal);
        case2.sharedMaterial = new Material(GameMaster.instance.colourPallete.materials.Crystal);

        if (GetComponent<PlayerController>().raygunScript.cold)
        {
            case1.sharedMaterial.color = Crystal_Cold;
            case2.sharedMaterial.color = Crystal_Hot;
        }
        else
        {
            case2.sharedMaterial.color = Crystal_Cold;
            case1.sharedMaterial.color = Crystal_Hot;
        }*/
    }

    // weapon & arm states
    IEnumerator Idle_EquippedState()
    {
        isPlacing = false;
        WeaponInspected();
        //Debug.Log("Idle: Enter");
        arm_obj.GetComponent<Animator>().Play("Idle");
        if (gun.cold)
        {
            GetComponent<ReticleFXController>().ChangeState(ReticleFXController.ReticleState.Negative);
        }
        else
        {
            GetComponent<ReticleFXController>().ChangeState(ReticleFXController.ReticleState.Positive);
        }

        while (weaponState == WeaponState.Idle_Equipped)
        {
            // do state stuff
            yield return 0;
        }
        //Debug.Log("Idle: Exit");
        NextState();
    }

    IEnumerator Idle_UnequippedState()
    {
        isPlacing = false;
        WeaponInspected();
        //Debug.Log("Idle: Enter");
        arm_obj.GetComponent<Animator>().Play("Idle_Unequipped");

        while (weaponState == WeaponState.Idle_Unequipped)
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
        arm_obj.GetComponent<Animator>().Play("TriggerPress");

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
        arm_obj.GetComponent<Animator>().Play("TriggerRelease");

        while (weaponState == WeaponState.TriggerReleased)
        {
            // do state stuff
            if (AnimationComplete("TriggerRelease"))
                weaponState = WeaponState.Idle_Equipped;

            yield return 0;
        }
        //Debug.Log("TriggerReleased: Exit");
        NextState();
    }

    IEnumerator InspectState()
    {
        //Debug.Log("Inspect: Enter");
        arm_obj.GetComponent<Animator>().Play("InspectTool");

        while (weaponState == WeaponState.Inspect)
        {
            // do state stuff

            if (AnimationComplete("InspectTool"))
            {
                WeaponInspected();
                weaponState = WeaponState.Idle_Equipped;
            }
               
               
            yield return 0;
        }
        //Debug.Log("Inspect: Exit");
        NextState();
    }

    IEnumerator GrabState()
    {
        //Debug.Log("Inspect: Enter");
        arm_obj.GetComponent<Animator>().Play("Grab_01");

        while (weaponState == WeaponState.Grab)
        {
            // do state stuff

            if (AnimationComplete("Grab_01"))
            {   
                weaponState = WeaponState.GrabRetract;
            }

            yield return 0;
        }
        //Debug.Log("Inspect: Exit");
        Debug.Log("Weapon state:" + weaponState);
        NextState();
    }

    IEnumerator PlaceState()
    {
        //Debug.Log("Inspect: Enter");
        arm_obj.GetComponent<Animator>().Play("Place_01");

        while (weaponState == WeaponState.Place)
        {
            // do state stuff

            if (AnimationComplete("Place_01"))
            {
                weaponState = WeaponState.GrabRetract;
            }

            yield return 0;
        }
        //Debug.Log("Inspect: Exit");
        isPlacing = false;
        weaponState = WeaponState.GrabRetract;
        NextState();
    }

    IEnumerator GrabRetractState()
    {
        //Debug.Log("Inspect: Enter");
        arm_obj.GetComponent<Animator>().Play("GrabRetract_01");

        while (weaponState == WeaponState.GrabRetract)
        {
            // do state stuff

            if (AnimationComplete("GrabRetract_01"))
            {
                weaponState = WeaponState.Idle_Unequipped;
            }

            yield return 0;
        }
        //Debug.Log("Inspect: Exit");
        NextState();
    }

    IEnumerator SwitchModeState()
    {
        //Debug.Log("SwitchMode: Enter");
        arm_obj.GetComponent<Animator>().Play("SwitchMode");
        //StartCoroutine(RotateCrystalCase(0.5f));
        GetComponent<ReticleFXController>().ChangeState(ReticleFXController.ReticleState.Neutral);

        if (inspectingWeapon)
            WeaponInspected();

       

        StartCoroutine(RotateCrystalCase(0.6f));

        while (weaponState == WeaponState.SwitchMode)
        {
            // do state stuff

            if (AnimationComplete("SwitchMode"))
            {
                //float endRotation = startRotation + 180f;
               
                weaponState = WeaponState.Idle_Equipped;
            }

            //Debug.Log("Playing switch animation");
            yield return 0;
        }
       /* float endRotation = caseRot.z + 180f;
        CrystalCase.transform.eulerAngles = new Vector3(CrystalCase.transform.eulerAngles.x, CrystalCase.transform.eulerAngles.y, endRotation);
        startRotation = endRotation;*/
        FinishSwitchingMode();
        //Debug.Log("SwitchMode: Exit");
        NextState();
    }

    public void NextState()
    {
        string methodName = weaponState.ToString() + "State";
        System.Reflection.MethodInfo info = GetType().GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        StartCoroutine((IEnumerator)info.Invoke(this, null));
    }

    public virtual bool AnimationComplete()
    {
        return (arm_obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !arm_obj.GetComponent<Animator>().IsInTransition(0));
    }

    public virtual bool AnimationComplete(string animationName)
    {
        return (arm_obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(animationName) && arm_obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !arm_obj.GetComponent<Animator>().IsInTransition(0));
    }

    public void WeaponInspected()
    {
        isGrabbing = false;
        inspectingWeapon = false;
    }

    //public void CreateLightning

    void AnimateGunTool()
    {
        if(!switchingMode && equipped)
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
                //item.GetComponent<Renderer>().sharedMaterial.color = Crystal_Neutral;
                item.GetComponent<Renderer>().sharedMaterial.SetColor("_SurfaceAlphaColor", Crystal_Neutral);
            }
        }
        else
        {
            // set crystal colour to hot or cold depending on gun mode
            if (gun.cold)
            {
                foreach (var item in emissiveLights)
                {
                    //item.GetComponent<Renderer>().sharedMaterial.color = Crystal_Cold;
                    item.GetComponent<Renderer>().sharedMaterial.SetColor("_SurfaceAlphaColor", Crystal_Cold);
                }

            }
            else
            {
                foreach (var item in emissiveLights)
                {
                    //item.GetComponent<Renderer>().sharedMaterial.color = Crystal_Hot;
                    item.GetComponent<Renderer>().sharedMaterial.SetColor("_SurfaceAlphaColor", Crystal_Hot);
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
                {
                    Material mat1 = new Material(crystal_mat);
                    mat1.SetColor("_SurfaceAlphaColor", Crystal_Neutral);
                    //item.GetComponent<Renderer>().sharedMaterial.SetColor("_SurfaceAlphaColor", Crystal_Neutral);
                    item.GetComponent<Renderer>().sharedMaterial = mat1;
                }
            }
        }
        else
        {
            if (gun.cold)
            {
                foreach (var item in renderers)
                {
                    if (item.GetComponent<Renderer>() != null)
                    {
                        Material mat1 = new Material(crystal_mat);
                        mat1.SetColor("_SurfaceAlphaColor", Crystal_Cold);
                        //item.GetComponent<Renderer>().sharedMaterial.SetColor("_SurfaceAlphaColor", Crystal_Cold);
                        item.GetComponent<Renderer>().sharedMaterial = mat1;
                    }  
                }
            }
            else
            {
                foreach (var item in renderers)
                {
                    if (item.GetComponent<Renderer>() != null)
                    {
                        Material mat1 = new Material(crystal_mat);
                        mat1.SetColor("_SurfaceAlphaColor", Crystal_Hot);
                        //item.GetComponent<Renderer>().sharedMaterial.SetColor("_SurfaceAlphaColor", Crystal_Hot);
                        item.GetComponent<Renderer>().sharedMaterial = mat1;
                    } 
                }
            }
        }
    }
    void SetTubeCrystals()
    {
        // set crystals colour to neutral if not shooting or cannot shoot
        if (switchingMode)
        {
            foreach (var item in TubeCrystals)
            {
                Material mat1 = new Material(crystal_mat);
                mat1.SetColor("_SurfaceAlphaColor", Crystal_Neutral);
                //item.GetComponent<Renderer>().sharedMaterial.color = Crystal_Neutral;
                //item.GetComponent<Renderer>().sharedMaterial.SetColor("_SurfaceAlphaColor", Crystal_Neutral);
                item.GetComponent<Renderer>().sharedMaterial = mat1;
            }
        }
        else
        {
            // set crystal colour to hot or cold depending on gun mode
            if (gun.cold)
            {
                foreach (var item in TubeCrystals)
                {
                    Material mat1 = new Material(crystal_mat);
                    mat1.SetColor("_SurfaceAlphaColor", Crystal_Cold);
                    //item.GetComponent<Renderer>().sharedMaterial.color = Crystal_Cold;
                    //item.GetComponent<Renderer>().sharedMaterial.SetColor("_SurfaceAlphaColor", Crystal_Cold);
                    item.GetComponent<Renderer>().sharedMaterial = mat1;
                }

            }
            else
            {
                foreach (var item in TubeCrystals)
                {
                    Material mat1 = new Material(crystal_mat);
                    mat1.SetColor("_SurfaceAlphaColor", Crystal_Hot);
                    //item.GetComponent<Renderer>().sharedMaterial.color = Crystal_Hot;
                    //item.GetComponent<Renderer>().sharedMaterial.SetColor("_SurfaceAlphaColor", Crystal_Hot);
                    item.GetComponent<Renderer>().sharedMaterial = mat1;
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
        //Debug.Log("SET STRAIGHT");
        yield return null;
        if(gun.cold)
            CrystalCase.transform.eulerAngles = new Vector3(CrystalCase.transform.eulerAngles.x, CrystalCase.transform.eulerAngles.y, 0);
        else
            CrystalCase.transform.eulerAngles = new Vector3(CrystalCase.transform.eulerAngles.x, CrystalCase.transform.eulerAngles.y, 180);
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
