using UnityEngine.Audio;
using System;
using UnityEngine;

public class PlayerAudioManager : AudioManager
{

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start() //Play sounds on start (main songs and ambience)
    {
        base.Start();
        Play("Cave");
        Play("Main");
        Play("Ice");
        Play("Heat");
        //FindObjectOfType<AudioManager>().Play("sound"); (this is what to use when calling a sound elsewhere)
    }

    public override void Update()
    {
       
    }

    public override void Play (string name)
    {
        base.Play(name);
    }

    public override void Stop(string name)
    { //Seach for sound in sounds by name, if it matches, play the sound
        base.Stop(name);
    }

    //At the moment objects that play this way can only have 1 sound
    public override void Play(GameObject obj)
    { //Seach for sound in sounds by name, if it matches, play the sound
        base.Play(obj);
    }

    public override void Spawn(GameObject obj, string name)
    { //Stop a sound from playing
        base.Spawn(obj, name);
    }

    public override void SetVolume(string name, float volume)
    { //Set the volume of a sound
        base.SetVolume(name, volume);
    }
}
