using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourPallet : MonoBehaviour
{

    [Header("Colours")]
    public Color Positive;
    public Color Negative;
    public Color Neutral;
    [Header("HiddenTexture")]
    public Material HiddenTexture;


    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public Material HiddenMaterial => GetComponent<Renderer>().sharedMaterials[0];
    public Material HiddenMaterial => HiddenTexture;
}
