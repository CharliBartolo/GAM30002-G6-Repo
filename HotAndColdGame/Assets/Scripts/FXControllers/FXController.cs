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
        Crystal_Hot = GameMaster.instance.colourPallete.Positive * 2;
        Crystal_Cold = GameMaster.instance.colourPallete.Negative * 2;
        Crystal_Neutral = GameMaster.instance.colourPallete.Neutral;

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
        Crystal_Hot = GameMaster.instance.colourPallete.Positive * 2;
        Crystal_Cold = GameMaster.instance.colourPallete.Negative * 2;
        Crystal_Neutral = GameMaster.instance.colourPallete.Neutral;
    }


   
}
