using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Renderer))]
public class LightTexture : MonoBehaviour
{
    // public crystal
    public CrystalBehaviour crystal;
    // public Transform lightSource;
    public Transform[] spotlights;
    // color property

    public Color ColdColor;
    public Color HotColor;
    public Color NeutralColor;

    public float range;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SetShaderProperties();
        SetRange(crystal.CurrentTemperature);
    }
    
    // set spotlight range
    public void SetRange(float range)
    {

        // scale spotlight range to temperature range
        float OldRange = (100 - 0);
        float NewRange = (40 - 0);
        float NewValue = (((range - 0) * NewRange) / OldRange) + 0;

        foreach (var item in spotlights)
        {
            item.GetComponent<Light>().range = Mathf.Abs(NewValue);
        }
       
        if(range > 0)
        {
            SetMaterialProperties(HotColor);
        }
        else if (range < 0)
        {
            SetMaterialProperties(ColdColor);
        }
        else
        {
            SetMaterialProperties(NeutralColor);
        }


    }

    // set material properties
    public void SetMaterialProperties(Color _color)
    {
        Material[] m = GetComponent<Renderer>().sharedMaterials; 
        m[0].color = _color;
    }

    // set shader properties
    private void SetShaderProperties()
    {
        foreach (var lightSource in spotlights)
        {
            if (lightSource)
            {
                GetComponent<Renderer>().sharedMaterial.SetFloat("_SpotAngle", lightSource.GetComponent<Light>().spotAngle);
                GetComponent<Renderer>().sharedMaterial.SetFloat("_Range", lightSource.GetComponent<Light>().range);
                GetComponent<Renderer>().sharedMaterial.SetVector("_LightPos", lightSource.position);
                GetComponent<Renderer>().sharedMaterial.SetVector("_LightDir", lightSource.forward);
            }
        }
    }
}
