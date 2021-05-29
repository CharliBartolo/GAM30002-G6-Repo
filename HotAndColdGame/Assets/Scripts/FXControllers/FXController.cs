using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FXController : MonoBehaviour
{
    // FX variables
    public float delay;
    protected float delayTimer;

    // Components
    public Animator Anim;

    // Gun crystal colours
    public Color Crystal_Neutral;
    public Color Crystal_Hot;
    public Color Crystal_Cold;

    // Start is called before the first frame update
    public virtual void Start()
    {
        Crystal_Hot = GameObject.Find("ColourPallet").GetComponent<ColourPallet>().Positive;
        Crystal_Cold = GameObject.Find("ColourPallet").GetComponent<ColourPallet>().Negative;
        Crystal_Neutral = GameObject.Find("ColourPallet").GetComponent<ColourPallet>().Neutral;

        Anim = GetComponent<Animator>();   
    }

    // Update is called once per frame
    void Update()
    {
        // call perform FX
        PerformFX();

    }

    // perform FX
    public virtual void PerformFX()
    {
        Crystal_Hot = GameObject.Find("ColourPallet").GetComponent<ColourPallet>().Positive;
        Crystal_Cold = GameObject.Find("ColourPallet").GetComponent<ColourPallet>().Negative;
        Crystal_Neutral = GameObject.Find("ColourPallet").GetComponent<ColourPallet>().Neutral;
    }


   
}
