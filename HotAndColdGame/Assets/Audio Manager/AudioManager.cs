using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Array of sounds
    public Sound[] sounds; 

    void Awake()
    {
        foreach (Sound s in sounds) //Turn each sound in array to audio scource
        {
            if (!s.spawn_on_object)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.spatialBlend = s.space;
            }
            if (s.spawn_on_object)
            {

            }
        }
    }

    void Start() //Play sounds on start (main songs and ambience)
    {
        Play("Cave");
        Play("Main");
        Play("Ice");
        Play("Heat");
        //FindObjectOfType<AudioManager>().Play("sound"); (this is what to use when calling a sound elsewhere)
    }

    void Update()
    {
        /* For testing sound volume in inspector
        foreach (Sound s in sounds)
        {
            s.source.volume = s.volume;
        */
    }

    public void Play (string name)
    { //Seach for sound in sounds by name, if it matches, play the sound
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (!s.source.isPlaying)
        {
            s.source.Play();
        }
    }

    public void Stop(string name)
    { //Seach for sound in sounds by name, if it matches, play the sound
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s.source.isPlaying)
        {
            s.source.Stop();
        }
    }

    //At the moment objects that play this way can only have 1 sound
    public void Play(GameObject obj)
    { //Seach for sound in sounds by name, if it matches, play the sound
        AudioSource source = obj.GetComponent<AudioSource>();
        source.Play();
    }

    public void Spawn(GameObject obj, string name)
    { //Stop a sound from playing
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source = obj.AddComponent<AudioSource>();
        s.source.clip = s.clip;
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
        s.source.spatialBlend = s.space;
    }

    public void SetVolume(string name, float volume)
    { //Set the volume of a sound
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.volume = volume;
    }
}
