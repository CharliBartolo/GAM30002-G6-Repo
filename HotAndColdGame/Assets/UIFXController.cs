using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFXController : FXController
{
    public Slider Slider;
    public Image Positive;
    public Image Negative;
    public Image[] Icons;

    public float neutral = 0;

    // Update is called once per frame

    public override void Start()
    {
        base.Start();

        neutral = Slider.value;
    }
    void Update()
    {
        Positive.color = Crystal_Cold;
        Negative.color = Crystal_Hot;

        foreach (var icon in Icons)
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


        }
    }
}
