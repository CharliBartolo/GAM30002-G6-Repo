using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameTrigger : MonoBehaviour
{
    public bool isEnabled;
    public DeathEffect screenEffects;
    public Image darknessOverlay;
    public bool travellingBackwards;

    // Start is called before the first frame update
    void Start()
    {
        screenEffects = GameObject.Find("UI").GetComponentInChildren<DeathEffect>();
        darknessOverlay = screenEffects.Fade;

        //StartCoroutine(StartScene(1));
       DarknessFadeOut(1);
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
        DarknessFadeIn(delay);
        yield return new WaitForSeconds(delay);
        if(!travellingBackwards)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

    }

    IEnumerator StartScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        DarknessFadeOut(delay);
    }

    public void DarknessFadeIn(float duration)
    {
        darknessOverlay.GetComponent<Image>().sprite = screenEffects.Overlay_Crystal_Death;
        darknessOverlay.GetComponent<Image>().color = screenEffects.Darkness;
        darknessOverlay.CrossFadeAlpha(1, duration, false);
        
    }

    public void DarknessFadeOut(float duration)
    {
        darknessOverlay.GetComponent<Image>().sprite = screenEffects.Overlay_Crystal_Death;
        darknessOverlay.GetComponent<Image>().color = screenEffects.Darkness;
        darknessOverlay.CrossFadeAlpha(0, duration, false);

    }
}
