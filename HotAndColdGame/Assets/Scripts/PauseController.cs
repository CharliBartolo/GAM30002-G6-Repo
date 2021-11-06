using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is responsible for Pausing the game, and any menus that go into the pause menu.
/// These options include sensitivity, pause text, volume settings, and quitting the game.
/// Last edit: OnVolumeChanged() - Removed functionality from here, moved to AudioManager
/// By: Charli - 25/9/21
/// Last edit: FOV slider - added with slider update and function
/// By: Rayner 1/11/2021
/// </summary>
public class PauseController : MonoBehaviour
{

    public bool IsPaused; //Pause boolean that other scripts use
    public bool Quitting; //For quitting confirmation
    public bool MainMenu; //For returning to mainmenu
    public Text pauseText; //UI element (Text) (Placeholder)
    public PlayerController PC; //Player Controller - Accesses the mouse sensitivity
    public PlayerCameraControl PCC;

    [Header("Mouse Sensitivity")]
    public Slider MouseSensitivityXSlider; //Slider option for Mouse Sensitivity X
    public Slider MouseSensitivityYSlider; //Slider option for Mouse Sensitivity Y
    public InputField MouseSensitivityXInput; //Input Field for Mouse Sensitivity X
    public InputField MouseSensitivityYInput; //Input Field for Mouse Sensitivity Y

    [Header("Volume")]
    public Slider MasterVolumeSlider; //Slider option for Volume
    public Slider MusicVolumeSlider; //Slider option for Volume
    public Slider SFXVolumeSlider; //Slider option for Volume
    public InputField VolumeInput; //Input Field for Volume

    [Header("FOV")]
    public Slider FOVSlider; //Slider option for Field of view

    [Header("Quit")]
    public Button QuitButton; //Button for quitiing game
    public GameObject QuitPanel; //Panel for Confirmation buttons
    public Button YesButton; //Yes Confirmation Button
    public Button NoButton; //No Confirmation Button
    
    [Header("Reload")]  
    public Button ReloadButton;

    //Quit to main menu 
    [Header("Main Menu")]
    public Button MainMenuButton; //Button for returning to mainmenu
    public GameObject MainMenuPanel; //Panel for returning to mainmenu
    public Button MainYesButton; //Yes Confirmation Button
    public Button MainNoButton; //No Confirmation Button

    //public GameMaster.instance GameMaster.instance;

    //Events
    private UnityEvent _eventVolumeSlider = new UnityEvent();

    private void Awake()
    {
        //Game Master
        //if (!GameMaster.instance) //&& GameObject.Find("GameMaster.instance") != null)
        //{
        //    GameMaster.instance = GameMaster.instance.instance;
            //GameMaster.instance = GameObject.Find("GameMaster.instance").GetComponent<GameMaster.instance>();
        //}

        //Player Controller       

        //Player Input
        /*
        if (!PC)
        {
            
            PC.GetComponent<PlayerInput>().actions.FindActionMap("Player").FindAction("Pause").performed +=
            pauseText =>
            {

                if (!IsPaused)
                {
                    Debug.Log("TEST PAUSE");
                    Quitting = false;
                    IsPaused = true;
                    Time.timeScale = IsPaused ? 0 : 1; //Actual pausing NOTE: Pauses most things (mainly things that use time.deltatime)
                }
            };

            if (IsPaused)
            {
                PC.GetComponent<PlayerInput>().actions.FindActionMap("Menu").FindAction("Pause").performed +=
                pauseText =>
                {
                    if (IsPaused)
                    {
                        Debug.Log("TEST UNPAUSE");
                        Quitting = false;
                        IsPaused = false;
                        Time.timeScale = IsPaused ? 0 : 1; //Actual pausing NOTE: Pauses most things (mainly things that use time.deltatime)
                    }
                };
            }           
        }
        */

        //Listeners       
    }

    // Start is called before the first frame update
    void Start()
    {
        PC = GameMaster.instance.playerRef.GetComponent<PlayerController>();
        PCC = GameMaster.instance.playerRef.GetComponent<PlayerCameraControl>();
            //PC = GameObject.Find("Player").GetComponent<PlayerController>();
        

        //Volume component
        if (PC != null)
        {
            //Options
            PC.GetComponent<Player_Audio_Renamed>().main_volume = MouseSensitivityXSlider.value;
            MouseSensitivityXSlider.onValueChanged.AddListener(delegate { XInputChange(); });
            MouseSensitivityYSlider.onValueChanged.AddListener(delegate { YInputChange(); });
            MasterVolumeSlider.onValueChanged.AddListener(delegate { GameMaster.instance.audioManager.SetMasterVolume(MasterVolumeSlider.value); });
            MusicVolumeSlider.onValueChanged.AddListener(delegate { GameMaster.instance.audioManager.SetMusicVolume(MusicVolumeSlider.value); });
            SFXVolumeSlider.onValueChanged.AddListener(delegate { GameMaster.instance.audioManager.SetSFXVolume(SFXVolumeSlider.value); });
            FOVSlider.onValueChanged.AddListener(delegate { FOVChange(); });

            //Quit
            QuitButton.onClick.AddListener(delegate { Quitting = true; });
            YesButton.onClick.AddListener(delegate { Application.Quit(); });
            NoButton.onClick.AddListener(delegate { Quitting = false; });

            //Reload
            ReloadButton.onClick.AddListener(delegate {GameMaster.instance.GetComponent<Reload>().OnButtonPress(); });

            //MainMenu
            MainMenuButton.onClick.AddListener(delegate { MainMenu = true; });
            MainYesButton.onClick.AddListener(delegate { Time.timeScale = 1; SceneManager.LoadScene(0); });
            MainNoButton.onClick.AddListener(delegate { MainMenu = false; });
        }

        if (PC != null)
        {
            PC.GetComponent<PlayerInput>().actions.FindActionMap("Player").FindAction("Pause").performed += OnPause;
            PC.GetComponent<PlayerInput>().actions.FindActionMap("Menu").FindAction("Pause").performed += OnPause;
        }

        Quitting = false;
        IsPaused = false;

        MouseSensitivityXSlider.value = GameMaster.instance.CS.XSensitivity;
        MouseSensitivityYSlider.value = GameMaster.instance.CS.YSensitivity;

        if (PlayerPrefs.HasKey("FOV"))
            FOVSlider.value = PlayerPrefs.GetFloat("FOV");

        if (PlayerPrefs.HasKey("MusicVol"))
            MusicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVol");
        
        if (PlayerPrefs.HasKey("MasterVol"))
            MasterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVol");

        if (PlayerPrefs.HasKey("SFXVol"))
            SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVol");

        VolumeInput.text = GameMaster.instance.CS.Volume.ToString();

        PC.playerMouseLook.mouseSensitivity.x = GameMaster.instance.CS.XSensitivity;
        PC.playerMouseLook.mouseSensitivity.y = GameMaster.instance.CS.YSensitivity;
        PC.GetComponent<Player_Audio_Renamed>().main_volume = GameMaster.instance.CS.Volume;

        //Can't interact with the text fields
        //MouseSensitivityXInput.interactable = false;
        //MouseSensitivityYInput.interactable = false;
        VolumeInput.interactable = false;            
    }

    // log values
    public void DebugValues()
    {
        Debug.Log("Current X sensitivity: " + PC.playerMouseLook.mouseSensitivity.x.ToString());
        Debug.Log("Current Y sensitivity: " + PC.playerMouseLook.mouseSensitivity.y.ToString());
        Debug.Log("Current main volume: " + PC.GetComponent<Player_Audio_Renamed>().main_volume.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        // log valuess
        //DebugValues();

        pauseText.gameObject.SetActive(IsPaused); //Toggles pause text
        //isMenuEnabled = IsPaused; //Toggles pause text
        
        MouseSensitivityXSlider.gameObject.SetActive(IsPaused); //Toggles Mouse Sensivity X Slider
        MouseSensitivityYSlider.gameObject.SetActive(IsPaused); //Toggles Mouse Sensivity Y Slider
        //MouseSensitivityXInput.gameObject.SetActive(IsPaused); // Toggles Mouse Sensitivity X Input Field
        //MouseSensitivityXInput.gameObject.SetActive(IsPaused); // Toggles Mouse Sensitivity X Input Field
        MasterVolumeSlider.gameObject.SetActive(IsPaused); //Toggles Volume Slider
        MusicVolumeSlider.gameObject.SetActive(IsPaused); //Toggles Volume Slider
        SFXVolumeSlider.gameObject.SetActive(IsPaused); //Toggles Volume Slider
        FOVSlider.gameObject.SetActive(IsPaused); //Toggles Volume Slider
        //VolumeInput.gameObject.SetActive(IsPaused); //Toggles Volume Input Field
        QuitButton.gameObject.SetActive(IsPaused); // Toggles the Quit Button
        QuitPanel.gameObject.SetActive(Quitting); //Toggles panel for confirmation
        YesButton.gameObject.SetActive(Quitting); //Toggles button for confirmation
        NoButton.gameObject.SetActive(Quitting); // Toggles button for confirmation
        
        //Main Menu
        MainMenuButton.gameObject.SetActive(IsPaused); // Toggles the Main Menu Button
        MainMenuPanel.gameObject.SetActive(MainMenu); //Toggles panel for confirmation
        MainYesButton.gameObject.SetActive(MainMenu); //Toggles button for confirmation
        MainNoButton.gameObject.SetActive(MainMenu); // Toggles button for confirmation 


    }

    //Getter for IsPaused boolean
    public bool GetPause()
    {
        return IsPaused;
    }

    void OnPause(InputAction.CallbackContext context)
    {
        // This handles errors for delegates calling Players that don't exist. Not the best way to deal with it.
        if (PC == null)
        {
            //PC.GetComponent<PlayerInput>().actions.FindActionMap("Player").FindAction("Pause").performed -= OnPause;
            //PC.GetComponent<PlayerInput>().actions.FindActionMap("Menu").FindAction("Pause").performed -= OnPause;
            return;
        }

        if (PC.GetComponent<PlayerInput>().currentActionMap == PC.GetComponent<PlayerInput>().actions.FindActionMap("Menu")
            && !IsPaused)
        {
            return;
        }        
        else
        {
            Debug.Log("TEST PAUSE");
            Quitting = false;
            IsPaused = !IsPaused;
        }
        
        Time.timeScale = IsPaused ? 0 : 1; //Actual pausing NOTE: Pauses most things (mainly things that use time.deltatime)
    }

    //Changes Input Field based on Slider
    public void XInputChange()
    {
        //MouseSensitivityXInput.text = MouseSensitivityXSlider.value.ToString();
        GameMaster.instance.CS.XSensitivity = MouseSensitivityXSlider.value;
        PC.playerMouseLook.mouseSensitivity.x = GameMaster.instance.CS.XSensitivity;
    }

    public void YInputChange()
    {
        //MouseSensitivityYInput.text = MouseSensitivityYSlider.value.ToString();
        GameMaster.instance.CS.YSensitivity = MouseSensitivityYSlider.value;
        PC.playerMouseLook.mouseSensitivity.y = GameMaster.instance.CS.YSensitivity;
    }

    public void FOVChange(){
       PCC.baseFOV = FOVSlider.value;
       PCC.fovCap = FOVSlider.value + 10f;
       PCC.UpdateFOVonPaused();
       PlayerPrefs.SetFloat("FOV", PCC.baseFOV);
    }

    public void VolumeChange()
    {
        //VolumeInput.text = VolumeSlider.value.ToString();
        //if (GameMaster.instance.audioManager.GetMixerVolume("MusicVol", out float newVol))
        //{
        //    VolumeInput.text = newVol.ToString();
        //}
        
        //GameMaster.instance.CS.Volume = VolumeSlider.value;
        //PC.GetComponent<Player_Audio_Renamed>().main_volume = GameMaster.instance.CS.Volume;
    }

    
}