using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

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
    public Text pauseText; //UI element (Text) (Placeholder)
    public PlayerController PC; //Player Controller - Accesses the mouse sensitivity
    public PlayerCameraControl PCC;

    public Slider MouseSensitivityXSlider; //Slider option for Mouse Sensitivity X
    public Slider MouseSensitivityYSlider; //Slider option for Mouse Sensitivity Y
    public InputField MouseSensitivityXInput; //Input Field for Mouse Sensitivity X
    public InputField MouseSensitivityYInput; //Input Field for Mouse Sensitivity Y

    public Slider MasterVolumeSlider; //Slider option for Volume
    public Slider MusicVolumeSlider; //Slider option for Volume
    public Slider SFXVolumeSlider; //Slider option for Volume

    public Slider FOVSlider; //Slider option for Field of view

    public InputField VolumeInput; //Input Field for Volume


    public Button QuitButton; //Button for quitiing game
    public GameObject QuitPanel; //Panel for Confirmation buttons
    public Button YesButton; //Yes Confirmation Button
    public Button NoButton; //No Confirmation Button

    public GameMaster GM;

    //Events
    private UnityEvent _eventVolumeSlider = new UnityEvent();

    private void Awake()
    {
        //Game Master
        if (!GM && GameObject.Find("GameMaster") != null)
        {
            GM = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        }

        //Player Controller
        if (!PC && GameObject.Find("Player") != null)
        {
            PC = GameObject.Find("Player").GetComponent<PlayerController>();
        }

        //Volume component
        if (PC != null)
        {
            PC.GetComponent<Player_Audio_Renamed>().main_volume = MouseSensitivityXSlider.value;
            MouseSensitivityXSlider.onValueChanged.AddListener(delegate { XInputChange(); });
            MouseSensitivityYSlider.onValueChanged.AddListener(delegate { YInputChange(); });
            MasterVolumeSlider.onValueChanged.AddListener(delegate { GM.audioManager.SetMasterVolume(MasterVolumeSlider.value); });
            MusicVolumeSlider.onValueChanged.AddListener(delegate { GM.audioManager.SetMusicVolume(MusicVolumeSlider.value); });
            SFXVolumeSlider.onValueChanged.AddListener(delegate { GM.audioManager.SetSFXVolume(SFXVolumeSlider.value); });
            FOVSlider.onValueChanged.AddListener(delegate { FOVChange(); });

            QuitButton.onClick.AddListener(delegate { Quitting = true; });
            YesButton.onClick.AddListener(delegate { Application.Quit(); });
            NoButton.onClick.AddListener(delegate { Quitting = false; });
        }
          

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
      
        
        if (PC != null)
        {
            PC.GetComponent<PlayerInput>().actions.FindActionMap("Player").FindAction("Pause").performed += OnPause;
            PC.GetComponent<PlayerInput>().actions.FindActionMap("Menu").FindAction("Pause").performed += OnPause;
        }
       
    }

    // Start is called before the first frame update
    void Start()
    {
        Quitting = false;
        IsPaused = false;

        MouseSensitivityXSlider.value = GM.CS.XSensitivity;
        MouseSensitivityYSlider.value = GM.CS.YSensitivity;

        if (PlayerPrefs.HasKey("MusicVol"))
            MusicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVol");
        
        if (PlayerPrefs.HasKey("MasterVol"))
            MasterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVol");

        if (PlayerPrefs.HasKey("SFXVol"))
            SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVol");

        //MouseSensitivityXInput.text = GM.CS.XSensitivity.ToString(); 
        //MouseSensitivityYInput.text = GM.CS.YSensitivity.ToString();
        VolumeInput.text = GM.CS.Volume.ToString();

        PC.playerMouseLook.mouseSensitivity.x = GM.CS.XSensitivity;
        PC.playerMouseLook.mouseSensitivity.y = GM.CS.YSensitivity;
        PC.GetComponent<Player_Audio_Renamed>().main_volume = GM.CS.Volume;

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
        QuitButton.gameObject.SetActive(IsPaused); // TToggles the Quit Button
        QuitPanel.gameObject.gameObject.SetActive(Quitting); //Toggles panel for confirmation
        YesButton.gameObject.gameObject.SetActive(Quitting); //Toggles button for confirmation
        NoButton.gameObject.SetActive(Quitting); // Toggles button for confirmation 

        
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
        GM.CS.XSensitivity = MouseSensitivityXSlider.value;
        PC.playerMouseLook.mouseSensitivity.x = GM.CS.XSensitivity;
    }

    public void YInputChange()
    {
        //MouseSensitivityYInput.text = MouseSensitivityYSlider.value.ToString();
        GM.CS.YSensitivity = MouseSensitivityYSlider.value;
        PC.playerMouseLook.mouseSensitivity.y = GM.CS.YSensitivity;
    }

    public void FOVChange(){
       PCC.baseFOV = FOVSlider.value;
       PCC.UpdateFOVonPaused();
    }

    public void VolumeChange()
    {
        //VolumeInput.text = VolumeSlider.value.ToString();
        //if (GM.audioManager.GetMixerVolume("MusicVol", out float newVol))
        //{
        //    VolumeInput.text = newVol.ToString();
        //}
        
        //GM.CS.Volume = VolumeSlider.value;
        //PC.GetComponent<Player_Audio_Renamed>().main_volume = GM.CS.Volume;
    }
}