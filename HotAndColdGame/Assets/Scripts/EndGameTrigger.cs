using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This script now sends the current build index number to the GameMaster 
/// to check if the current scene is the previous scene
/// Last edit: Added GameMaster to script so that build index number is stored
/// By: Jason 16/10/21
/// </summary>


public class EndGameTrigger : MonoBehaviour
{
    public bool isEnabled;
    public DeathEffect screenEffects;
    public Image darknessOverlay;
    public bool travellingBackwards;
    private GameMaster gm;

    // Start is called before the first frame update
    void Start()
    {
        screenEffects = GameObject.Find("UI").GetComponentInChildren<DeathEffect>();
        darknessOverlay = screenEffects.Fade;
        darknessOverlay.GetComponent<Image>().sprite = screenEffects.Overlay_Crystal_Death;
        darknessOverlay.GetComponent<Image>().color = screenEffects.Darkness;
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        //StartCoroutine(StartScene(1));
        StartCoroutine(DarknessFadeIn(0));
        StartCoroutine(DarknessFadeOut(1, 0.25f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerController>()!= null)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            StartCoroutine(LoadNextScene(1));
        }
    }

    IEnumerator LoadNextScene(float delay)
    {
        GameMaster.instance.playerRef.GetComponent<PlayerController>().playerControlState = PlayerController.PlayerState.ControlsDisabled;
        StartCoroutine(DarknessFadeIn(delay));
        yield return new WaitForSeconds(delay);
        if (!travellingBackwards)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            gm.prevScene = SceneManager.GetActiveScene().buildIndex;
        }
        else 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            gm.prevScene = SceneManager.GetActiveScene().buildIndex;
        }
    }

    IEnumerator StartScene(float delay)
    {
        darknessOverlay.GetComponent<Image>().sprite = screenEffects.Overlay_Crystal_Death;
        darknessOverlay.GetComponent<Image>().color = screenEffects.Darkness;
        yield return new WaitForSeconds(delay);
        DarknessFadeOut(delay);
    }

    public IEnumerator DarknessFadeIn(float duration, float delay = 0)
    {
        darknessOverlay.GetComponent<Image>().sprite = screenEffects.Overlay_Crystal_Death;
        darknessOverlay.GetComponent<Image>().color = screenEffects.Darkness;

        yield return new WaitForSeconds(delay);

        darknessOverlay.CrossFadeAlpha(1, duration, false);
        
    }

    public IEnumerator DarknessFadeOut(float duration, float delay = 0)
    {
        darknessOverlay.GetComponent<Image>().sprite = screenEffects.Overlay_Crystal_Death;
        darknessOverlay.GetComponent<Image>().color = screenEffects.Darkness;

        yield return new WaitForSeconds(delay);

        darknessOverlay.CrossFadeAlpha(0, duration, false);

    }
}
