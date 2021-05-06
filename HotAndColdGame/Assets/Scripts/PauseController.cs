using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    /// <summary>
    /// PlayerController requires this.
    /// </summary>
    /// 
    public bool IsPaused; //Pause boolean that other scripts use
    public Text pauseText; //UI element (Text) (Placeholder)

    // Start is called before the first frame update
    void Start()
    {
        IsPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        pauseText.gameObject.SetActive(IsPaused); //Toggles pause text
       
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            // Toggle paused
            IsPaused = !IsPaused;
            Time.timeScale = IsPaused ? 0 : 1; //Actual pausing NOTE: Pauses most things (mailny things that use time.deltatime)
        }
    }

    //Getter for IsPaused boolean
    public bool GetPause()
    {
        return IsPaused;
    }
}
