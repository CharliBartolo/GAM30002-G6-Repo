using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CrystalFXController : FXController
{

    public bool effectOnly;
    // variables 
    public float effectRadius = 1;
    // components
    public CrystalBehaviour MainCrystal;
    public Renderer[] CrystalMesh;
    public Light MainCrystaLight;
    public SphereCollider AreaCollider;
    public Transform AreaSphere;
    public VisualEffect vfx;
    public List<GameObject> AffectedObjects;
    public Light Spotight;

    public GameObject[] friends;

    public GameObject CrystalGrowth;

    private int temp;

    public bool warmup;
    public bool ready;

    public void Awake()
    {
        if (warmup)
            ready = false;
    }

    // Start is called before the first frame update
    public override void Start()
    {


        base.Start();

        AffectedObjects = new List<GameObject>();
        MainCrystal = GetComponent<CrystalBehaviour>();
        MainCrystaLight = transform.Find("Area Light").GetComponent<Light>();
        //MainCrystaLight.gameObject.SetActive(false);
        CrystalMesh = transform.Find("Mesh").GetComponentsInChildren<Renderer>();
        //AreaCollider = GetComponent<SphereCollider>();
        AreaSphere = transform.Find("EffectSphere").transform;
        AreaCollider = AreaSphere.GetComponent<SphereCollider>();
        AreaCollider.enabled = false;
        //MainCrystal.GetComponentInChildren<Renderer>().sharedMaterial = new Material(GameMaster.instance.colourPallete.Crystal);
        AreaSphere.gameObject.GetComponent<Renderer>().sharedMaterial = new Material(GameMaster.instance.colourPallete.materials.EffectField);
        vfx = AreaSphere.GetChild(0).GetComponent<VisualEffect>();//= new VisualEffectAsset(GameMaster.instance.colourPallete.VFX.visualEffectAsset);

        foreach (var item in CrystalMesh)
        {
            item.GetComponentInChildren<Renderer>().sharedMaterial = new Material(GameMaster.instance.colourPallete.materials.Crystal);
        }

        temp = 0;

        Invoke(nameof(SetReady), 0.5f);

    }

    // Update is called once per frame
    void Update()
    {
        if (warmup)
        {

        }

        if (ready)
            PerformFX();

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.x, transform.localScale.x);

        /*if(AreaColliderAreaCollider.radius > 0.2f)
            Instantiate(CrystalGrowth,GetPointOnMesh().point, Quaternion.FromToRotation(Vector3.forward, GetPointOnMesh().normal));
        */
    }

    public void SetReady()
    {
        AreaCollider.enabled = true;
        ready = true;
    }

    // perform FX
    public override void PerformFX()
    {
        base.PerformFX();

        if (MainCrystal.CurrentTemperature < -1)
        {
            temp = -1;
        }
        else if (MainCrystal.CurrentTemperature > 1)
        {
            temp = 1;
        }
        else
        {
            temp = 0;
        }

        GrowAreaCollider();

        if (MainCrystal.spreadEffects)
        {
            ColourAreaSphere();
            ColourFriends();
            ColourCrystal();
        }

    }



    public void ColourFriends()
    {
        if (friends.Length > 0)
        {
            foreach (var item in friends)
            {
                if (item != null && item.GetComponent<MeshRenderer>() != null)
                {
                    item.GetComponent<MeshRenderer>().sharedMaterial.color = MainCrystal.GetComponent<MeshRenderer>().sharedMaterial.color;
                }
            }
        }

    }

    public void ColourAreaSphere()
    {
        if (temp == -1)
        {
            vfx.SetVector4("ParticleColour", Crystal_Cold / 8);
            vfx.SetVector4("TrailColour", Crystal_Cold / 8);
            //vfx.SetVector4("TrailColour", new Vector4(Crystal_Cold.r, Crystal_Cold.g, Crystal_Cold.b, 0.1f));
            AreaSphere.GetComponent<Renderer>().sharedMaterial.SetColor("_Emission", Crystal_Cold * 64);
        }
        else if (temp == 1)
        {
            vfx.SetVector4("ParticleColour", Crystal_Hot / 8);
            vfx.SetVector4("TrailColour", Crystal_Hot / 8);
            //vfx.SetVector4("TrailColour", new Vector4(Crystal_Hot.r, Crystal_Hot.g, Crystal_Hot.b, 0.1f));
            AreaSphere.GetComponent<Renderer>().sharedMaterial.SetColor("_Emission", Crystal_Hot * 64);
        }
        else if (temp == 0)
        {
            vfx.SetVector4("ParticleColour", Crystal_Neutral / 8);
            vfx.SetVector4("TrailColour", Crystal_Neutral / 8);
            //vfx.SetVector4("TrailColour", new Vector4(Crystal_Neutral.r, Crystal_Neutral.g, Crystal_Neutral.b, 0.1f));
            AreaSphere.GetComponent<Renderer>().sharedMaterial.SetColor("_Emission", Crystal_Neutral * 64);
        }
    }

    // colour crystial according to state
    public void ColourCrystal()
    {
        if (MainCrystal.CurrentTemperature < -1)
        {
            foreach (var item in CrystalMesh)
            {
                item.sharedMaterial.SetColor("_SurfaceAlphaColor", Crystal_Cold);
            }

        }
        else if (MainCrystal.CurrentTemperature > 1)
        {
            foreach (var item in CrystalMesh)
            {
                item.sharedMaterial.SetColor("_SurfaceAlphaColor", Crystal_Hot);
            }
        }
        else
        {
            foreach (var item in CrystalMesh)
            {
                item.sharedMaterial.SetColor("_SurfaceAlphaColor", Crystal_Neutral / colourIntensity);
            }
        }
    }

    public void GrowAreaCollider()
    {
        AreaCollider.gameObject.transform.localScale = Vector3.one * Math.Abs(CrystalTemp()) * effectRadius / 10f;
    }

    // get back crystal color
    public float CrystalTemp()
    {
        return MainCrystal.CurrentTemperature;
    }

    public int CrystalTempState()
    {
        switch (MainCrystal.CurrentTempState)
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
        if (other)
        {
            // add fx
            AffectedObjects.Add(other.gameObject);
            other.gameObject.AddComponent<LightTexture>();
            other.gameObject.GetComponent<LightTexture>().crystal = gameObject.GetComponent<CrystalBehaviour>();
            other.gameObject.GetComponent<LightTexture>().spotlight = transform.Find("Spot Light");
        }
    }

    public void AddExtraLightTextureComponent(Collider other)
    {
        if (other)
        {
            // add fx
            AffectedObjects.Add(other.gameObject);
            LightTexture comp = other.gameObject.AddComponent<LightTexture>();
            comp.crystal = gameObject.GetComponent<CrystalBehaviour>();
            comp.spotlight = this.transform.Find("Spot Light");
            comp.index = other.gameObject.GetComponents<LightTexture>().Length;
        }
    }
    // remove fx
    public void RemoveLightTextureComponent(Collider other)
    {
        if (other.gameObject.GetComponent<LightTexture>() != null)
        {
            //Remove fx
            LightTexture[] comps = other.gameObject.GetComponents<LightTexture>();

            //Debug.Log("# of Light Texture componenets: " + comps.Length);
            int myIndex = RemoveMyindex(other);
            AffectedObjects.Remove(other.gameObject);
        }
    }
    int RemoveMyindex(Collider other)
    {
        int myIndex = 0;
        LightTexture[] comps = other.gameObject.GetComponents<LightTexture>();

        // get index of 
        foreach (var item in comps)
        {
            if (item.crystal == this.MainCrystal)
            {
                myIndex = item.index;
                item.RemoveHiddenMaterial(myIndex);
                Destroy(item);
            }
            else
            {
                if (item.index > 1)
                    item.index--;
            }
        }

        return myIndex;
    }


    public RaycastHit GetPointOnMesh()
    {
        //float length  = 100.0f;
        float length = AreaCollider.radius;

        float dist = length * 2;

        Vector3 direction = UnityEngine.Random.onUnitSphere;
        Ray ray = new Ray(transform.position + direction * length, -direction);
        RaycastHit hit;

        Physics.Raycast(ray, out hit, dist);
        return hit;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ready && other.transform.parent != this.gameObject && MainCrystal.spreadEffects)
        {
            if (other.gameObject.tag != "DeathWater")
            {
               
                    //Debug.Log("COLLISION: " + other.gameObject.name);
                    if (other.gameObject.GetComponent<MeshRenderer>() != null)
                    {
                        if (other.gameObject.GetComponent<CrystalFXController>() == null)
                        {
                            if (other.gameObject.transform.parent?.GetComponent<CrystalFXController>() == null)
                            {
                                //add texture component
                                AddExtraLightTextureComponent(other);

                                //AddLightTextureComponent(other);
                                // if not have lighttexture, add it
                                /*if (other.gameObject.GetComponent<LightTexture>() == null)
                                {
                                    AddLightTextureComponent(other);
                                }
                                else
                                {
                                    AddExtraLightTextureComponent(other);
                                }*/
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