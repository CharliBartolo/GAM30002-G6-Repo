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

    public Slider MenuMainVolumeSlider; //Slider option for Volume
    public Slider MenuMusicVolumeSlider; //Slider option for Volume
    public Slider MenuSFXVolumeSlider; //Slider option for Volume
    public InputField MenuMainVolumeInput; //Input Field for Volume
    public InputField MenuMusicVolumeInput; //Input Field for Volume
    public InputField MenuSFXVolumeInput; //Input Field for Volume

    //public Button QuitButton;

    public Button QuitButton; //Button for quitiing game
    public GameObject QuitPanel; //Panel for Confirmation buttons
    public Button YesButton; //Yes Confirmation Button
    public Button NoButton; //No Confirmation Button

    public bool Quitting; //For quitting confirmation
    public struct ControlSettings
    {       
        public float XSensitivity;
        public float YSensitivity;
        public float MainVolume;
        public float MusicVolume;
        public float SFXVolume;

        public ControlSettings(float X, float Y, float mainVolume, float musicVolume, float sfxVolume)
        {
            XSensitivity = X;
            YSensitivity = Y;
            MainVolume = mainVolume;
            MusicVolume = musicVolume;
            SFXVolume = sfxVolume;
        }
    }

    private void Awake()
    {
        //Listeners
        MenuMouseSensitivityXSlider.onValueChanged.AddListener(delegate { XInputChange(); });
        MenuMouseSensitivityYSlider.onValueChanged.AddListener(delegate { YInputChange(); });
        MenuMainVolumeSlider.onValueChanged.AddListener(delegate { MainVolumeChange(); });
        MenuMusicVolumeSlider.onValueChanged.AddListener(delegate { MusicVolumeChange(); });
        MenuSFXVolumeSlider.onValueChanged.AddListener(delegate { SFXVolumeChange(); });
        //QuitButton.onClick.AddListener(delegate { Application.Quit(); });

        //Quit
        QuitButton.onClick.AddListener(delegate { Quitting = true; });
        YesButton.onClick.AddListener(delegate { Application.Quit(); });
        NoButton.onClick.AddListener(delegate { Quitting = false; });
    }

    // Start is called before the first frame update
    private void Start()
    {
        //Game Master
        if (!GM)
        {
            GM = GameObject.Find("GameMaster").GetComponent<GameMaster>();
            Debug.Log("THIS IS A MAIN MENU SCRIPT: " + this);
            Debug.Log("THIS IS A GAMEMASTER SCRIPT: " + GM);
        }

        if (PlayerPrefs.HasKey("MusicVol"))
            MenuMusicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVol");
        
        if (PlayerPrefs.HasKey("MasterVol"))
            MenuMainVolumeSlider.value = PlayerPrefs.GetFloat("MasterVol");

        if (PlayerPrefs.HasKey("SFXVol"))
            MenuSFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVol");
        
        MenuMouseSensitivityXSlider.value = GM.CS.XSensitivity;
        MenuMouseSensitivityYSlider.value = GM.CS.YSensitivity;
        MenuMainVolumeSlider.value = GM.CS.MainVolume;
        MenuMusicVolumeSlider.value = GM.CS.MusicVolume;
        MenuSFXVolumeSlider.value = GM.CS.SFXVolume;

        MenuMouseSensitivityXInput.text = GM.CS.XSensitivity.ToString();
        MenuMouseSensitivityYInput.text = GM.CS.YSensitivity.ToString();
        MenuMainVolumeInput.text = GM.CS.MainVolume.ToString();
        MenuMusicVolumeInput.text = GM.CS.MusicVolume.ToString();
        MenuSFXVolumeInput.text = GM.CS.SFXVolume.ToString();

        //Can't interact with the text fields
        MenuMouseSensitivityXInput.interactable = false;
        MenuMouseSensitivityYInput.interactable = false;
        MenuMainVolumeInput.interactable = false;
        MenuMusicVolumeInput.interactable = false;
        MenuSFXVolumeInput.interactable = false;
    }

    /*
    private void Update()
    {
        //Quit
        QuitPanel.gameObject.SetActive(Quitting); //Toggles panel for confirmation
        YesButton.gameObject.SetActive(Quitting); //Toggles button for confirmation
        NoButton.gameObject.SetActive(Quitting); // Toggles button for confirmation
    }
    */

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

    public void MainVolumeChange()
    {
        MenuMainVolumeInput.text = MenuMainVolumeSlider.value.ToString();
        GM.CS.MainVolume = MenuMainVolumeSlider.value;
    }

    public void MusicVolumeChange()
    {
        MenuMusicVolumeInput.text = MenuMusicVolumeSlider.value.ToString();
        GM.CS.MusicVolume = MenuMusicVolumeSlider.value;
    }

    public void SFXVolumeChange()
    {
        MenuSFXVolumeInput.text = MenuSFXVolumeSlider.value.ToString();
        GM.CS.SFXVolume = MenuSFXVolumeSlider.value;
    }
    
}
