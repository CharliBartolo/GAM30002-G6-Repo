using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolboxFXController : FXController
{
    public GameObject[] positive_emissiive_lights;
    public GameObject[] negative_emissiive_lights;
    public GameObject[] neutral_emissiive_lights;

    private Material emissiveMaterial;

    public float emissionValue = 5;

    public override void PerformFX()
    {
        base.PerformFX();
    }

    public override void Start()
    {
        base.Start();

        emissiveMaterial = GameMaster.instance.colourPallete.materials.EmissiveLights;

        SetEmissiveLights(); 
    }


    public void SetEmissiveLights()
    {
        foreach (var item in positive_emissiive_lights)
        {
            Renderer[] rs = item.GetComponentsInChildren<Renderer>();

            foreach (var r in rs)
            {
                if (r != null)
                {
                    r.sharedMaterial = new Material(emissiveMaterial);
                    r.sharedMaterial.SetColor("_EmissiveColor", Crystal_Hot);
                }
            }
        }

        foreach (var item in negative_emissiive_lights)
        {
            Renderer[] rs = item.GetComponentsInChildren<Renderer>();

            foreach (var r in rs)
            {
                if (r != null)
                {
                    r.sharedMaterial = new Material(emissiveMaterial);
                    r.sharedMaterial.SetColor("_EmissiveColor", Crystal_Cold);
                }
            }
        }

        foreach (var item in neutral_emissiive_lights)
        {
            Renderer[] rs = item.GetComponentsInChildren<Renderer>();

            foreach (var r in rs)
            {
                if (r != null)
                {
                    r.sharedMaterial = new Material(emissiveMaterial);
                    r.sharedMaterial.SetColor("_EmissiveColor", Crystal_Neutral);
                }
            }
        }
    }
}
