using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFXController : FXController
{
    public bool isTemperatureHidden = false;
    public Slider Slider;
    public Image Positive;
    public Image Negative;
    //public Image[] Icons;
    public Image Icon_CrystalBig;
    public Image Icon_CrystalSmall;

    public float neutral = 0;

    public PlayerController player;

    // Update is called once per frame

    public override void Start()
    {
        base.Start();

        Crystal_Cold /= colourIntensity;
        Crystal_Hot /= colourIntensity;
        Crystal_Neutral /= colourIntensity;

        Positive.color = Crystal_Cold;
        Negative.color = Crystal_Hot;

        neutral = Slider.value;
        
        if (GameObject.Find("Player") != null)
            player = GameObject.Find("Player").GetComponent<PlayerController>();

    }
    void Update()
    {
        if (isTemperatureHidden)
        {
            Slider.gameObject.SetActive(false);
            return;
        }

        Slider.gameObject.SetActive(true);

        if(player != null)
            UpdatePlayerTempUI();
    }

    public void UpdatePlayerTempUI()
    {
        List<IConditions.ConditionTypes> activeConditions = player.ActiveConditions;

        if (player.ActiveConditions.Count == 2)
        {
            IConditions.ConditionTypes cond = player.ActiveConditions[1];

            if (cond == IConditions.ConditionTypes.ConditionCold)
            {
                Icon_CrystalSmall.color = Crystal_Cold;
            }
            else
            {
                Icon_CrystalSmall.color = Crystal_Hot;
            }
        }
        else if (player.ActiveConditions.Count == 1)
        {
            IConditions.ConditionTypes cond = player.ActiveConditions[0];

            if(cond == IConditions.ConditionTypes.ConditionCold)
            {
                Icon_CrystalBig.color = Crystal_Cold;
                Icon_CrystalSmall.color = Crystal_Cold;
            }
            else
            {
                Icon_CrystalBig.color = Crystal_Hot;
                Icon_CrystalSmall.color = Crystal_Hot;
            }
        }
        else
        {
            Icon_CrystalBig.color = Crystal_Neutral;
            Icon_CrystalSmall.color = Crystal_Neutral;
        }

      /*  foreach (var icon in Icons)
        {
            if (Slider.GetComponent<Slider>().value < (neutral - 0.5f))
            {
                icon.color = Crystal_Hot;
            }
            else if (Slider.GetComponent<Slider>().value > (neutral + 0.5f))
            {
                icon.color = Crystal_Cold;
            }
            else
            {
                icon.color = Crystal_Neutral;
            }
        }*/
    }
}
