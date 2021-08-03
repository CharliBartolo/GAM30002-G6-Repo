using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ColourPalette : MonoBehaviour
{

    [Header("Colour palette")]
    public Color Positive;
    public Color Negative;
    public Color Neutral;

    [System.Serializable]
    public struct MaterialMap
    {
        public Material Crystal;
        public Material EffectField;
        public VisualEffect VFX;
        public Material EmissiveLights;
        public Material HiddenTexture;
        public LineRenderer LightningStrike;
    }

    //public List<MaterialMap> mapObjs;
    [Header("Materials palette")]
    public MaterialMap materials = new MaterialMap();


    private float t = 0;
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

        // randomise colour
        // t += Time.deltaTime;
        // if ((60 % t) > 1)
        // {
        //     Positive = Color.Lerp(RandomColour1(), RandomColour1(), t * Time.deltaTime);
        //     Negative = Color.Lerp(RandomColour2(), RandomColour1(), t * Time.deltaTime);
        //     t = 0;
        // }

        //Positive = Color.Lerp(RandomColour(), RandomColour(),10 * Time.deltaTime);
        //Negative = Color.Lerp(RandomColour(), RandomColour(), 10 * Time.deltaTime);
    }

    Color RandomColour1()
    {
        return Random.ColorHSV(0f, 1f, 0f, 1f, 0.5f, 1f);
    }
    Color RandomColour2()
    {
        return Random.ColorHSV(0f, 1f, 1f, 0.5f, 0.5f, 1f);
    }
    //public Material HiddenMaterial => GetComponent<Renderer>().sharedMaterials[0];
    public Material HiddenMaterial => materials.HiddenTexture;
}
