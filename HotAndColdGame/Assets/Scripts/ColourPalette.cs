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

    [Range(0,10)]
    public float colourIntensity;

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

    //[InspectorButton("OnButtonClicked")]
    //public bool saveColoursToPlayerPrefs;

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

    public void SaveColourPrefs(int saveSlot)
    {
        PlayerPrefs.SetString("posCol" + saveSlot.ToString(), ColorUtility.ToHtmlStringRGBA(Positive));
        PlayerPrefs.SetString("negCol" + saveSlot.ToString(), ColorUtility.ToHtmlStringRGBA(Negative));
        PlayerPrefs.SetString("neutralCol" + saveSlot.ToString(), ColorUtility.ToHtmlStringRGBA(Neutral));

        PlayerPrefs.Save();
    }

    public void LoadColourPrefs(int loadSlot)
    {
        ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString("posCol" + loadSlot.ToString()), out Positive);
        ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString("negCol" + loadSlot.ToString()), out Negative);
        ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString("neutralCol" + loadSlot.ToString()), out Neutral);
    }
}
