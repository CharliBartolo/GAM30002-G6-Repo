using System.Collections;
using System.Collections.Generic;
//using System.Linq;
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
    public List<Material> currentMaterials;
    private bool materialSet;
    private Renderer rendererComponent;

    public float range = 0;

    public int index = 1;

    // Start is called before the first frame update
    void Start()
    {
        rendererComponent = GetComponent<Renderer>();
        //spotlight = GetNearestLight();
        HiddenMaterial = GameMaster.instance.colourPallete.HiddenMaterial;

        currentMaterials = new List <Material>();
        foreach (Material mat in rendererComponent.sharedMaterials)
        {
            currentMaterials.Add(mat);
        }
        
        if (crystal != null)
            spotlight = crystal.transform.Find("Spot Light").GetComponent<Light>()?.transform;
        AddHiddenMaterial();

    }

    // Update is called once per frame
    void LateUpdate()
    {
        //spotlight = GetNearestLight();
        UpdateColoursFromPallet();


        if (spotlight != null && crystal != null)
        {
            SetShaderProperties();
            SetRange(crystal.CurrentTemperature);
            if (range != 0)
                SetMaterial();
        }
    }

    /*public Transform GetNearestLight()
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
    }*/

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
        //if(crystal.CurrentTempState != ITemperature.tempState.Neutral)
        //{
        spotlight.GetComponent<Light>().range = Mathf.Abs(range) * crystal.GetComponent<CrystalFXController>().effectRadius * crystal.GetComponent<Transform>().localScale.x / 20f;
        this.range = range;
        //}
    }

    public void SetMaterial()
    {
        //Debug.Log("Range: " + range);
        if (spotlight.GetComponent<Light>().range != range)
        {
            if (range > 1)
            {
                //Debug.Log("RUNNING HOT");
                SetMaterialProperties(HotColor);
            }
            else if (range < -1)
            {
                //Debug.Log("RUNNING COLD " + " - Range: " + range);
                SetMaterialProperties(ColdColor);
            }
            else
            {
                //Debug.Log("RUNNING NEUTRAL");
                SetMaterialProperties(NeutralColor);
            }
        }
    }

    // add HiddenMaterial 
    public void AddHiddenMaterial()
    {
        //CurrentMaterial = gameObject.GetComponent<Renderer>().sharedMaterials;
        //Material[] copy = gameObject.GetComponent<Renderer>().sharedMaterials;
        //Material[] combo = new Material[gameObject.GetComponent<Renderer>().sharedMaterials.Length + 1];
        //combo[0] = currentMaterials[0];
        //for (int i = 1; i < copy.Length; i++)
        //{
        //    combo[i] = copy[i];
        //}
        //combo[combo.Length - 1] = new Material(HiddenMaterial);
        currentMaterials.Clear();
        foreach (Material mat in rendererComponent.sharedMaterials)
        {
            currentMaterials.Add(mat);
        }        

        currentMaterials.Insert(currentMaterials.Count, new Material (HiddenMaterial));
        rendererComponent.sharedMaterials = currentMaterials.ToArray();
        //gameObject.GetComponent<Renderer>().sharedMaterials = combo;
    }

    // remove HiddenMaterial
    public void RemoveHiddenMaterial(int index)
    {
        //Material[] currentMats = gameObject.GetComponent<Renderer>().sharedMaterials;
        //Debug.Log("Removing index: " + index + " on: " + gameObject.name);
        /*    if(index > gameObject.GetComponent<Renderer>().sharedMaterials.Length)
            {
                Debug.Log("Index out of range, reducing index: " + index);
                RemoveHiddenMaterial(index - 1);
            }*/
        if (index < currentMaterials.Count)        
            currentMaterials.RemoveAt(index);
        //Material[] _new = currentMats.Except(new Material[] { currentMats[index] }).ToArray();

        //gameObject.GetComponent<Renderer>().sharedMaterials = _new;
        rendererComponent.sharedMaterials = currentMaterials.ToArray();
    }

    // set material properties
    public void SetMaterialProperties(Color _color)
    {
        // Problem line
        //if (m != null && m[index] != null)        
        
        if (index < rendererComponent.sharedMaterials.Length)
        {
            Material[] materials = rendererComponent.sharedMaterials;
            //m[index].color = _color;
            materials[index].SetVector("_LayerTint", _color);
            materials[index].SetVector("_FresnelColorInside", _color);
            materials[index].SetVector("_FresnelColorOutside", _color);
            materials[index].SetVector("_InnerLightColorOutside", _color);
            materials[index].SetVector("_InnerLightColorInside", _color);
        }
            
        
    }

    // set shader properties
    private void SetShaderProperties()
    {
        if (spotlight)
        {
            //GetComponent<Renderer>().sharedMaterials[1]?.SetFloat("_SpotAngle", spotlight.GetComponent<Light>().spotAngle);
            if (rendererComponent.sharedMaterials.Length > index && rendererComponent.sharedMaterials[index] != null)
            {
                rendererComponent.sharedMaterials[index]?.SetFloat("_Range", spotlight.GetComponent<Light>().range);
                rendererComponent.sharedMaterials[index]?.SetVector("_LightPos", spotlight.position);
                rendererComponent.sharedMaterials[index]?.SetVector("_LightDir", spotlight.forward);
            }
        }
    }
}
