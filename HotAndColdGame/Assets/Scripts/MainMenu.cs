using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameMaster GM;

    public Slider MenuMouseSensitivityXSlider; //Slider option for Mouse Sensitivity X
    public Slider MenuMouseSensitivityYSlider; //Slider option for Mouse Sensitivity Y
    public InputField MenuMouseSensitivityXInput; //Input Field for Mouse Sensitivity X
    public InputField MenuMouseSensitivityYInput; //Input Field for Mouse Sensitivity Y

    public Slider MenuVolumeSlider; //Slider option for Volume
    public InputField MenuVolumeInput; //Input Field for Volume

    public struct ControlSettings
    {       
        public float XSensitivity;
        public float YSensitivity;
        public float Volume;

        public ControlSettings(float X, float Y, float volume)
        {
            XSensitivity = X;
            YSensitivity = Y;
            Volume = volume;
        }
    }

    private void Awake()
    {
        //Game Master
        if (!GM)
        {
            GM = GameObject.Find("GameMaster").GetComponent<GameMaster>();
            Debug.Log("THIS IS A MAIN MENU SCRIPT: " + this);
            Debug.Log("THIS IS A GAMEMASTER SCRIPT: " + GM);
        }    
    }

    // Start is called before the first frame update
    private void Start()
    {
        MenuMouseSensitivityXSlider.value = GM.CS.XSensitivity;
        MenuMouseSensitivityYSlider.value = GM.CS.YSensitivity;
        MenuVolumeSlider.value = GM.CS.Volume;

        MenuMouseSensitivityXInput.text = GM.CS.XSensitivity.ToString();
        MenuMouseSensitivityYInput.text = GM.CS.YSensitivity.ToString();
        MenuVolumeInput.text = GM.CS.Volume.ToString();

        //Can't interact with the text fields
        MenuMouseSensitivityXInput.interactable = false;
        MenuMouseSensitivityYInput.interactable = false;
        MenuVolumeInput.interactable = false;

        //Listeners
        MenuMouseSensitivityXSlider.onValueChanged.AddListener(delegate { XInputChange(); });
        MenuMouseSensitivityYSlider.onValueChanged.AddListener(delegate { YInputChange(); });
        MenuVolumeSlider.onValueChanged.AddListener(delegate { VolumeChange(); });

        GameObject.Find("Controls Menus").SetActive(false);
        GameObject.Find("Sound Menus").SetActive(false);
    }

    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    //Changes Input Field based on Slider
    public void XInputChange()
    {
        MenuMouseSensitivityXInput.text = MenuMouseSensitivityXSlider.value.ToString();
        GM.CS.XSensitivity = MenuMouseSensitivityXSlider.value;
    }

    public void YInputChange()
    {
        MenuMouseSensitivityYInput.text = MenuMouseSensitivityYSlider.value.ToString();
        GM.CS.YSensitivity = MenuMouseSensitivityYSlider.value;
    }

    public void VolumeChange()
    {
        MenuVolumeInput.text = MenuVolumeSlider.value.ToString();
        GM.CS.Volume = MenuVolumeSlider.value;
    }
}
