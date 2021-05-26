using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class LightTexture : MonoBehaviour
{
    // public crystal
    public CrystalBehaviour crystal;
    // public Transform lightSource;
    private Transform spotlight;
    // color property
    public Color ColdColor;
    public Color HotColor;
    public Color NeutralColor;
    // hidden material
    public Material HiddenMaterial;
    // hidden material
    public Material[] CurrentMaterial;
    private bool materialSet;

    public float range;
    // Start is called before the first frame update
    void Start()
    {
      

        HiddenMaterial = GameObject.Find("ColourPallet").GetComponent<ColourPallet>()?.HiddenMaterial;
        CurrentMaterial = gameObject.GetComponent<Renderer>().sharedMaterials;
        spotlight = crystal.transform.Find("Spot Light").GetComponent<Light>()?.transform;
        AddHiddenMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateColoursFromPallet();

        if (spotlight!= null)
        {
            SetShaderProperties();
            SetRange(crystal.CurrentTemperature);
        }
    }

    public void UpdateColoursFromPallet()
    {
        if(GameObject.Find("ColourPallet").GetComponent<ColourPallet>()!=null)
        {
            HotColor = GameObject.Find("ColourPallet").GetComponent<ColourPallet>().Positive;
            ColdColor = GameObject.Find("ColourPallet").GetComponent<ColourPallet>().Negative;
            NeutralColor = GameObject.Find("ColourPallet").GetComponent<ColourPallet>().Neutral;
        }
    }
    
    // set spotlight range
    public void SetRange(float range)
    {

        // scale spotlight range to temperature range
        float OldRange = (100 - 0);
        float NewRange = (40 - 0);
        float NewValue = (((range - 0) * NewRange) / OldRange) + 0;



        spotlight.GetComponent<Light>().range = Mathf.Abs(NewValue);
       
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


    // add HiddenMaterial 
    public void AddHiddenMaterial()
    {
        Material[] combo = new Material[CurrentMaterial.Length + 1];
        combo[0] = CurrentMaterial[0];
        combo[combo.Length - 1] = HiddenMaterial;
        gameObject.GetComponent<Renderer>().sharedMaterials = combo;
    }
    public void RemoveHiddenMaterial()
    {
        Material[] combo = new Material[CurrentMaterial.Length];
        combo[0] = CurrentMaterial[0];
        gameObject.GetComponent<Renderer>().sharedMaterials = CurrentMaterial;
    }

    // set material properties
    public void SetMaterialProperties(Color _color)
    {
        Material[] m = GetComponent<Renderer>().sharedMaterials; 
        if(m[1]!=null)
            m[1].color = _color;
    }

    // set shader properties
    private void SetShaderProperties()
    {
        if (spotlight)
        {
            GetComponent<Renderer>().sharedMaterials[1]?.SetFloat("_SpotAngle", spotlight.GetComponent<Light>().spotAngle);
            GetComponent<Renderer>().sharedMaterials[1]?.SetFloat("_Range", spotlight.GetComponent<Light>().range);
            GetComponent<Renderer>().sharedMaterials[1]?.SetVector("_LightPos", spotlight.position);
            GetComponent<Renderer>().sharedMaterials[1]?.SetVector("_LightDir", spotlight.forward);
        }

    }
}
