using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Renderer))]
public class LightRevealSurface : MonoBehaviour
{

    public Transform lightSource;
    private float range;
    private float angle;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
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