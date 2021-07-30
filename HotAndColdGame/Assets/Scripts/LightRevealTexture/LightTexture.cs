using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class LightTexture : MonoBehaviour
{
    // public crystal

    public CrystalBehaviour crystal;
    // public Transform lightSource;
    private Transform[] spotlights;
    public Transform spotlight;
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

    public int index = 1;
    // Start is called before the first frame update
    void Start()
    {

        //spotlight = GetNearestLight();
        HiddenMaterial = GameMaster.instance.colourPallete.HiddenMaterial;
        CurrentMaterial = gameObject.GetComponent<Renderer>().sharedMaterials;
        if (crystal != null)
            spotlight = crystal.transform.Find("Spot Light").GetComponent<Light>()?.transform;
        AddHiddenMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        //spotlight = GetNearestLight();
        UpdateColoursFromPallet();


        if (spotlight != null)
        {
            SetShaderProperties();
            //SetRange((crystal.CurrentTemperature * crystal.GetComponent<CrystalFXController>().effectRadius/10)/2);
            SetRange(crystal.CurrentTemperature * crystal.GetComponent<CrystalFXController>().effectRadius * crystal.GetComponent<Transform>().localScale.x / 20f);
        }
    }

    public Transform GetNearestLight()
    {
        Transform closest = null;
        foreach (Transform t in spotlights)
        {
            Vector3 dir = transform.position - t.position;  // object direction relative to light
            float minDist = Mathf.Infinity;
            // object distance from light position
            float dist = dir.magnitude;
            // only update shader parameters if object inside light range and angle
            if (dist <= minDist)
            {
                closest = t;
                minDist = dist;
            }
        }
        return closest;
    }

    public void UpdateColoursFromPallet()
    {
        if (GameMaster.instance.colourPallete != null)
        {
            this.HotColor = GameMaster.instance.colourPallete.Positive;
            this.ColdColor = GameMaster.instance.colourPallete.Negative;
            this.NeutralColor = GameMaster.instance.colourPallete.Neutral;
        }
    }

    // set spotlight range
    public void SetRange(float range)
    {

        // scale spotlight range to temperature range
        //float OldRange = 100;
        //float NewRange = 10;
        //float NewValue = (range * NewRange) / OldRange;

        //spotlight.GetComponent<Light>().range = Mathf.Abs(NewValue);
        spotlight.GetComponent<Light>().range = Mathf.Abs(range);

        //spotlight.GetComponent<Light>().range = crystal.transform.Find("EffectSphere").transform.localScale.x;

        if (range > 0)
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
        //CurrentMaterial = gameObject.GetComponent<Renderer>().sharedMaterials;
        Material[] copy = gameObject.GetComponent<Renderer>().sharedMaterials;
        Material[] combo = new Material[gameObject.GetComponent<Renderer>().sharedMaterials.Length + 1];
        combo[0] = CurrentMaterial[0];
        for (int i = 1; i < copy.Length; i++)
        {
            combo[i] = copy[i];
        }
        combo[combo.Length - 1] = new Material(HiddenMaterial);
        gameObject.GetComponent<Renderer>().sharedMaterials = combo;
    }
    public void RemoveHiddenMaterial(int index)
    {
        Material[] current = gameObject.GetComponent<Renderer>().sharedMaterials;
        Debug.Log("Removing index: " + index);
        Material[] _new = current.Except(new Material[] { current[index] }).ToArray();

        gameObject.GetComponent<Renderer>().sharedMaterials = _new;
    }

    // set material properties
    public void SetMaterialProperties(Color _color)
    {
        Material[] m = GetComponent<Renderer>().sharedMaterials;
        if (m[index] != null)
        {
            //m[index].color = _color;
            m[index].SetVector("_LayerTint", _color);
            m[index].SetVector("_FresnelColorInside", _color);
            m[index].SetVector("_FresnelColorOutside", _color);
            m[index].SetVector("_InnerLightColorOutside", _color);
            m[index].SetVector("_InnerLightColorInside", _color);
        }
       
    }

    // set shader properties
    private void SetShaderProperties()
    {
        if (spotlight)
        {

            //GetComponent<Renderer>().sharedMaterials[1]?.SetFloat("_SpotAngle", spotlight.GetComponent<Light>().spotAngle);
            if (GetComponent<Renderer>().sharedMaterials[index]!=null)
            {
                GetComponent<Renderer>().sharedMaterials[index]?.SetFloat("_Range", spotlight.GetComponent<Light>().range);
                GetComponent<Renderer>().sharedMaterials[index]?.SetVector("_LightPos", spotlight.position);
                GetComponent<Renderer>().sharedMaterials[index]?.SetVector("_LightDir", spotlight.forward);
            }
        }
    }
}
