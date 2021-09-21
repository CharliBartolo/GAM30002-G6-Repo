using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class PauseController : MonoBehaviour
{
    /// <summary>
    /// PlayerController requires this.
    /// TextField Input should not be 'Interactable' (Can't type decimal into box)
    /// </summary>
    /// 
    public bool IsPaused; //Pause boolean that other scripts use
    public bool Quitting; //For quitting confirmation
    public Text pauseText; //UI element (Text) (Placeholder)
    public PlayerController PC; //Player Controller - Accesses the mouse sensitivity

    public Slider MouseSensitivityXSlider; //Slider option for Mouse Sensitivity X
    public Slider MouseSensitivityYSlider; //Slider option for Mouse Sensitivity Y
    public InputField MouseSensitivityXInput; //Input Field for Mouse Sensitivity X
    public InputField MouseSensitivityYInput; //Input Field for Mouse Sensitivity Y

    public Slider VolumeSlider; //Slider option for Volume
    public InputField VolumeInput; //Input Field for Volume

    public Button QuitButton; //Button for quitiing game
    public GameObject QuitPanel; //Panel for Confirmation buttons
    public Button YesButton; //Yes Confirmation Button
    public Button NoButton; //No Confirmation Button

    public PlayerInput playerInput;
    public PlayerInput menuInput;

    public GameMaster GM;

    //Events
    private UnityEvent _eventVolumeSlider = new UnityEvent();

    private void Awake()
    {
        //Game Master
        if (!GM)
        {
            GM = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        }

        //Player Controller
        if (!PC)
        {
            PC = GameObject.Find("Player").GetComponent<PlayerController>();
        }

        //Player Input
        if (playerInput != null)
        {
            playerInput.actions.FindActionMap("Player").FindAction("Pause").performed +=
            pauseText =>
            {

                if (!IsPaused)
                {
                    Debug.Log("TEST PAUSE");
                    //Quitting = false;
                    IsPaused = true;
                    Time.timeScale = IsPaused ? 0 : 1; //Actual pausing NOTE: Pauses most things (mainly things that use time.deltatime)
                }
            };

            if (IsPaused)
            {
                playerInput.actions.FindActionMap("Menu").FindAction("Pause").performed +=
                pauseText =>
                {
                    if (IsPaused)
                    {
                        Debug.Log("TEST UNPAUSE");
                        //Quitting = false;
                        IsPaused = false;
                        Time.timeScale = IsPaused ? 0 : 1; //Actual pausing NOTE: Pauses most things (mainly things that use time.deltatime)
                    }
                };
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        Quitting = false;
        IsPaused = false;

        MouseSensitivityXSlider.value = GM.CS.XSensitivity;
        MouseSensitivityYSlider.value = GM.CS.YSensitivity;
        VolumeSlider.value = GM.CS.Volume;

        MouseSensitivityXInput.text = GM.CS.XSensitivity.ToString(); 
        MouseSensitivityYInput.text = GM.CS.YSensitivity.ToString();
        VolumeInput.text = GM.CS.Volume.ToString();

        PC.playerMouseLook.mouseSensitivity.x = GM.CS.XSensitivity;
        PC.playerMouseLook.mouseSensitivity.y = GM.CS.YSensitivity;
        PC.GetComponent<Player_Audio_Renamed>().main_volume = GM.CS.Volume;

        //Can't interact with the text fields
        MouseSensitivityXInput.interactable = false;
        MouseSensitivityYInput.interactable = false;
        VolumeInput.interactable = false;

        if(PC != null)
            PC.GetComponent<Player_Audio_Renamed>().main_volume = MouseSensitivityXSlider.value;

        //Listeners
        MouseSensitivityXSlider.onValueChanged.AddListener(delegate { XInputChange(); });
        MouseSensitivityYSlider.onValueChanged.AddListener(delegate { YInputChange(); });
        VolumeSlider.onValueChanged.AddListener(delegate { VolumeChange(); });

        QuitButton.onClick.AddListener(delegate { Quitting = true; });
        YesButton.onClick.AddListener(delegate { Application.Quit(); });
        NoButton.onClick.AddListener(delegate { Quitting = false; });

       

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
        MouseSensitivityXInput.gameObject.SetActive(IsPaused); // Toggles Mouse Sensitivity X Input Field
        MouseSensitivityXInput.gameObject.SetActive(IsPaused); // Toggles Mouse Sensitivity X Input Field
        VolumeSlider.gameObject.SetActive(IsPaused); //Toggles Volume Slider
        VolumeInput.gameObject.SetActive(IsPaused); //Toggles Volume Input Field
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
    
    //Changes Input Field based on Slider
    public void XInputChange()
    {
        MouseSensitivityXInput.text = MouseSensitivityXSlider.value.ToString();
        GM.CS.XSensitivity = MouseSensitivityXSlider.value;
        PC.playerMouseLook.mouseSensitivity.x = GM.CS.XSensitivity;
    }

    public void YInputChange()
    {
        MouseSensitivityYInput.text = MouseSensitivityYSlider.value.ToString();
        GM.CS.YSensitivity = MouseSensitivityYSlider.value;
        PC.playerMouseLook.mouseSensitivity.y = GM.CS.YSensitivity;
    }

    public void VolumeChange()
    {
        VolumeInput.text = VolumeSlider.value.ToString();
        GM.CS.Volume = VolumeSlider.value;
        PC.GetComponent<Player_Audio_Renamed>().main_volume = GM.CS.Volume;
    }
}