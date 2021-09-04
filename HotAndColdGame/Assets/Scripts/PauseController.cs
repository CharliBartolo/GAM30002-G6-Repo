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
    public bool isMenuEnabled = false;
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

    //Events
    private UnityEvent _eventVolumeSlider = new UnityEvent();
    // Start is called before the first frame update
    void Start()
    {
        Quitting = false;
        IsPaused = false;

        if (isMenuEnabled)
        {
            /* MouseSensitivityXSlider.minValue = 0;
             MouseSensitivityXSlider.maxValue = 1;

             MouseSensitivityYSlider.minValue = 0;
             MouseSensitivityYSlider.maxValue = 1;*/

            MouseSensitivityXInput.text = PC.mouseSensitivity.x.ToString();
            MouseSensitivityYInput.text = PC.mouseSensitivity.y.ToString();

            //Can't interact with the text fields
            MouseSensitivityXInput.interactable = false;
            MouseSensitivityYInput.interactable = false;

            MouseSensitivityXInput.gameObject.SetActive(false);
            MouseSensitivityYInput.gameObject.SetActive(false);
            VolumeInput.gameObject.SetActive(false);

            //VolumeSlider

           

        }

        PC.GetComponent<Player_Audio_Renamed>().main_volume = MouseSensitivityXSlider.value;

        //Listeners
        MouseSensitivityXSlider.onValueChanged.AddListener(delegate { XInputChange(); });
        MouseSensitivityYSlider.onValueChanged.AddListener(delegate { YInputChange(); });
        //MouseSensitivityXInput.onValueChanged.AddListener(delegate { XSliderChange(); });
        //MouseSensitivityYInput.onValueChanged.AddListener(delegate { YSliderChange(); });

        QuitButton.onClick.AddListener(delegate { Quitting = true; });
        YesButton.onClick.AddListener(delegate { Application.Quit(); });
        NoButton.onClick.AddListener(delegate { Quitting = false; });

        VolumeSlider.onValueChanged.AddListener(delegate { VolumeChange(); });

    }

    // log values
    public void DebugValues()
    {
        Debug.Log("Current X sensitivity: " + PC.mouseSensitivity.x.ToString());
        Debug.Log("Current Y sensitivity: " + PC.mouseSensitivity.y.ToString());
        Debug.Log("Current main volume: " + PC.GetComponent<Player_Audio_Renamed>().main_volume.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        // log valuess
        //DebugValues();

        pauseText.gameObject.SetActive(IsPaused); //Toggles pause text
        //isMenuEnabled = IsPaused; //Toggles pause text

        if (isMenuEnabled)
        {
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

            //Keeps the value the same for both slider and input field




        }

        playerInput.actions.FindAction("Pause").performed +=
            pauseText =>
            {
                Debug.Log("TEST Pause");
                //Toggle paused
                Quitting = false;
                IsPaused = !IsPaused;

                /*
                if (IsPaused)
                {
                    PC.GetComponent<Player_Audio_Renamed>().main_volume = 0f;
                }
                else
                {
                    VolumeChange();
                }
                */

                Time.timeScale = IsPaused ? 0 : 1; //Actual pausing NOTE: Pauses most things (mainly things that use time.deltatime)
            };

        if(IsPaused)
        {
            playerInput.actions.FindActionMap("Menu").FindAction("Pause").performed +=
            pauseText =>
            {
                Debug.Log("TEST MENU PAUSE");
                //Toggle paused
                Quitting = false;
                IsPaused = !IsPaused;


                Time.timeScale = IsPaused ? 0 : 1; //Actual pausing NOTE: Pauses most things (mainly things that use time.deltatime)
            };
        }
    }

    //Getter for IsPaused boolean
    public bool GetPause()
    {
        return IsPaused;
    }

    //Commented out because can't change value properly
    /* 
    //Changes Slider based on Input Field
    public void XSliderChange()
    {
        float value = float.Parse(MouseSensitivityXInput.text);
        if (value > 1.0f)
        {
            value = 1.0f;
        }

        if (value < 0.0f)
        {
            value = 0.1f;
        }
        MouseSensitivityXSlider.value = value;
        MouseSensitivityXInput.text = value.ToString();
        PC.mouseSensitivity.x = MouseSensitivityXSlider.value;

    }
    public void YSliderChange()
    {
        float value = float.Parse(MouseSensitivityYInput.text);
        if (value > 1.0f)
        {
            value = 1.0f;
        }

        if (value < 0.0f)
        {
            value = 0.0f;
        }
        MouseSensitivityYSlider.value = value;
        MouseSensitivityYInput.text = value.ToString();
        PC.mouseSensitivity.y = MouseSensitivityYSlider.value;
    }
    */

    public void VolumeChange()
    {
        VolumeInput.text = VolumeSlider.value.ToString();
        PC.GetComponent<Player_Audio_Renamed>().main_volume = VolumeSlider.value;


    }
    //Changes Input Field based on Slider
    public void XInputChange()
    {
        MouseSensitivityXInput.text = MouseSensitivityXSlider.value.ToString();
        PC.mouseSensitivity.x = MouseSensitivityXSlider.value;
    }
    public void YInputChange()
    {
        MouseSensitivityYInput.text = MouseSensitivityYSlider.value.ToString();
        PC.mouseSensitivity.y = MouseSensitivityYSlider.value;
    }
}