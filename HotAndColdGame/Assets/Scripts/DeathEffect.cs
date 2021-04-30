using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathEffect : MonoBehaviour
{
    public Image Fade;

    public TemperatureStateBase crntTemp;

    // Start is called before the first frame update
    void Start()
    {
        Fade.canvasRenderer.SetAlpha(0.0f);// makes sure death effect is invisible until function

        
    }

    private void Update() {
        DeathFade();//call fade function
    }

    // DeathEffect is called once per frame
    void DeathFade()
    {
        //crntTemp.CurrentTempState = temp.CurrentTempState;//get current temp of player

        switch (crntTemp.CurrentTempState) //checks if player is in Hot or Cold state
        {
            case ITemperature.tempState.Hot: //if hot
                Debug.Log("Player is overheating");
                Fade.GetComponent<Image>().color = new Color32(255, 0, 0, 100);//red, green, blue, alpha
                Fade.CrossFadeAlpha(1, 3, false); //Fully fade Image with the duration of 3 seconds
                break;
            case ITemperature.tempState.Cold://if cold
                Debug.Log("Player is freezing");
                Fade.GetComponent<Image>().color = new Color32(0, 200, 255, 100);//red, green, blue, alpha
                Fade.CrossFadeAlpha(1, 3, false); //Fully fade Image with the duration of 3 seconds
                break;
            default:
                //crntTemp.CurrentTempState = TemperatureStateBase.TempState.Neutral;//nothing happens
                break;
        }
    }
}
