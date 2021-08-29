using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathEffect : FXController
{
    [SerializeField] public Image Fade;//for image wanted on vignette    
    [SerializeField] public TemperatureStateBase crntTemp;//get player temperature
    //[SerializeField] private GameMaster gm;//to get last respawn/checkpoint
    //[SerializeField] private GameObject player; //To get Player's position.
    bool isResetting = false;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        // makes sure death effect is invisible until function
        Fade.canvasRenderer.SetAlpha(0.0f); 
        //gm = GameMaster.instance;
        //player = gm.playerRef;
        if(GameMaster.instance.playerRef != null)
        {
            if (GameMaster.instance.playerRef.GetComponent<TemperatureStateBase>() != null)
                crntTemp = GameMaster.instance.playerRef.GetComponent<TemperatureStateBase>();
        }
       
    }

    private void Update() 
    {
        //call fade function
        if(crntTemp!= null)
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
                    //Fade.GetComponent<Image>().color = new Color32(255, 0, 0, 100);//red, green, blue, alpha
                    Fade.GetComponent<Image>().color = Crystal_Hot;//red, green, blue, alpha
                    Fade.CrossFadeAlpha(1, 3, false); //Fully fade Image with the duration of 3 seconds
                    StartCoroutine("ResetPlayer", 3f);                 
                    break;
                case ITemperature.tempState.Cold://if cold
                    //testing purposes
                    //Debug.Log("Player is freezing");
                    //Fade.GetComponent<Image>().color = new Color32(0, 200, 255, 100);//red, green, blue, alpha
                    Fade.GetComponent<Image>().color = Crystal_Cold; ;//red, green, blue, alpha
                    Fade.CrossFadeAlpha(1, 3, false); //Fully fade Image with the duration of 3 seconds
                    StartCoroutine("ResetPlayer", 3f);     
                    break;
                default:
                    //Fade.GetComponent<Image>().color = new Color32(0, 0, 0, 100);
                    //Fade.GetComponent<Image>().color = Crystal_Neutral;
                    Fade.canvasRenderer.SetAlpha(0.0f);
                    break;
            }
        }       
    }

    IEnumerator ResetPlayer(float secondsToWait)
    {
        isResetting = true;
        yield return new WaitForSeconds(secondsToWait); 
        crntTemp.SetTemperature(crntTemp.tempValueRange[1]);
        crntTemp.CurrentTempState = ITemperature.tempState.Neutral;
        GameMaster.instance.playerRef.transform.position = GameMaster.instance.lastCheckPointPos.position;
        //Fade.GetComponent<Image>().color = new Color32(0, 0, 0, 0);//red, green, blue, alpha
        Fade.CrossFadeAlpha(0, 0.5f, false);  
        isResetting = false;      
    }
}
