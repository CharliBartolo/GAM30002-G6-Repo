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

    public Sprite Overlay_Crystal_Passive;
    public Sprite Overlay_Crystal_Death;
    public Image Overlay_Darkness;
    public Sprite Overlay_Green;
    public Color Darkness = Color.black;

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
                    crntTemp.GetComponent<PlayerController>().playerControlState = PlayerController.PlayerState.ControlsDisabled;
                    Fade.GetComponent<Image>().sprite = Overlay_Crystal_Death;
                    Fade.GetComponent<Image>().color = Crystal_Hot;//red, green, blue, alpha
                    Fade.CrossFadeAlpha(1, 3, false); //Fully fade Image with the duration of 3 seconds
                    StartCoroutine("ResetPlayer", 3f);                 
                    break;
                case ITemperature.tempState.Cold://if cold
                    //testing purposes
                    //Debug.Log("Player is freezing");
                    //Fade.GetComponent<Image>().color = new Color32(0, 200, 255, 100);//red, green, blue, alpha
                    crntTemp.GetComponent<PlayerController>().playerControlState = PlayerController.PlayerState.ControlsDisabled;
                    Fade.GetComponent<Image>().sprite = Overlay_Crystal_Death;
                    Fade.GetComponent<Image>().color = Crystal_Cold; //red, green, blue, alpha
                    Fade.CrossFadeAlpha(1, 3, false); //Fully fade Image with the duration of 3 seconds
                    StartCoroutine("ResetPlayer", 3f);     
                    break;
                default:
                    //Fade.GetComponent<Image>().color = new Color32(0, 0, 0, 100);
                    //Fade.GetComponent<Image>().color = Crystal_Neutral;
                    /*if(crntTemp.CurrentTemperature > 50)
                    {
                        Fade.GetComponent<Image>().sprite = Overlay_Crystal_Passive;
                        Fade.GetComponent<Image>().color = Crystal_Hot;
                        Fade.canvasRenderer.SetAlpha(crntTemp.CurrentTemperature * 0.01f);
                    }
                    else if (crntTemp.CurrentTemperature < 50)
                    {
                        Fade.GetComponent<Image>().sprite = Overlay_Crystal_Passive;
                        Fade.GetComponent<Image>().color = Crystal_Cold;
                        Fade.canvasRenderer.SetAlpha(Mathf.Abs(crntTemp.CurrentTemperature) * 0.01f);
                    }
                    else
                    {
                        Fade.canvasRenderer.SetAlpha(0.0f);
                    }*/
                    Fade.canvasRenderer.SetAlpha(0.0f);
                    break;
            }
        }       
    }

    public void DarknessDeath(float delay)
    {
        if (!isResetting)
        {
            crntTemp.GetComponent<PlayerController>().playerControlState = PlayerController.PlayerState.ControlsDisabled;
            Fade.GetComponent<Image>().sprite = Overlay_Crystal_Death;
            Fade.GetComponent<Image>().color = Darkness;
            Fade.CrossFadeAlpha(1, delay, false);
            StartCoroutine(nameof(ResetPlayer), delay);
        }
    }
    public void GreenDeath(float delay)
    {
        if(!isResetting)
        {
            crntTemp.GetComponent<PlayerController>().playerControlState = PlayerController.PlayerState.ControlsDisabled;
            Fade.GetComponent<Image>().sprite = Overlay_Green;
            Fade.GetComponent<Image>().color = Color.white;
            Fade.CrossFadeAlpha(1, delay, false);
            StartCoroutine(nameof(ResetPlayer), delay);
        }
    }

    IEnumerator ResetPlayer(float secondsToWait)
    {
        isResetting = true;
        yield return new WaitForSeconds(secondsToWait);
        crntTemp.GetComponent<PlayerController>().playerControlState = PlayerController.PlayerState.MoveAndLook;
        crntTemp.SetTemperature(crntTemp.tempValueRange[1]);
        crntTemp.CurrentTempState = ITemperature.tempState.Neutral;
        GameMaster.instance.playerRef.transform.position = GameMaster.instance.lastCheckPointPos.position;
        GameMaster.instance.playerRef.GetComponent<PlayerController>().
            playerMouseLook.ResetMouse(GameMaster.instance.lastCheckPointPos);
        //Fade.GetComponent<Image>().color = new Color32(0, 0, 0, 0);//red, green, blue, alpha
        Fade.CrossFadeAlpha(0, 0.5f, false);  
        isResetting = false;      
    }
}
