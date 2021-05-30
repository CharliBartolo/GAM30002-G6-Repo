using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathEffect : MonoBehaviour
{
    [SerializeField] public Image Fade;//for image wanted on vignette    
    [SerializeField] public TemperatureStateBase crntTemp;//get player temperature
    [SerializeField] private GameMaster gm;//to get last respawn/checkpoint
    [SerializeField] private Transform player; //To get Player's position.
    bool isResetting = false;

    // Start is called before the first frame update
    void Start()
    {
        // makes sure death effect is invisible until function
        Fade.canvasRenderer.SetAlpha(0.0f);   
    }

    private void Update() 
    {
        //call fade function
        DeathFade();
    }

    // DeathEffect is called once per frame
    void DeathFade()
    {        
        //tweak fade effect
        if (!isResetting)
        {
            switch (crntTemp.CurrentTempState) //checks if player is in Hot or Cold state
            {
                case ITemperature.tempState.Hot: //if hot
                    //testing purposes
                    //Debug.Log("Player is overheating");
                    Fade.GetComponent<Image>().color = new Color32(255, 0, 0, 100);//red, green, blue, alpha
                    Fade.CrossFadeAlpha(1, 3, false); //Fully fade Image with the duration of 3 seconds
                    StartCoroutine("ResetPlayer", 3f);                 
                    break;
                case ITemperature.tempState.Cold://if cold
                    //testing purposes
                    //Debug.Log("Player is freezing");
                    Fade.GetComponent<Image>().color = new Color32(0, 200, 255, 100);//red, green, blue, alpha
                    Fade.CrossFadeAlpha(1, 3, false); //Fully fade Image with the duration of 3 seconds
                    StartCoroutine("ResetPlayer", 3f);     
                    break;
                default:
                    Fade.GetComponent<Image>().color = new Color32(0, 0, 0, 100);
                    break;
            }
        }       
    }

    IEnumerator ResetPlayer(float secondsToWait)
    {
        isResetting = true;
        yield return new WaitForSeconds(secondsToWait); 
        crntTemp.SetTemperature(crntTemp.tempValueRange[1]);
        player.transform.position = gm.lastCheckPointPos.position;
        Fade.CrossFadeAlpha(0, 0.1f, false);  
        isResetting = false;      
    }
}
