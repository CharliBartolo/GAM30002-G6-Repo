using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public enum Layer
{
    Master,
    Music
}

public class AudioManager : MonoBehaviour
{
    //Array of sounds
    public AudioMixer audioMixer;
    public Sound_List list;//to see all the sounds
    [SerializeField]
    private Audio_Manager_Group[] groups;
    private List<Sound> sounds = new List<Sound>(); //all sound layers are flattend to here

    public virtual void Awake()
    {
        list.sounds.Clear();

        foreach (Audio_Manager_Group g in groups) //Turn each sound in array to audio scource
        {
            foreach (Sound s in g.sounds) //Turn each sound in array to audio scource
            {
                sounds.Add(s);
                list.sounds.Add(g.name + " - '" + s.name + "'");
            }
        }

        foreach (Sound s in sounds) //Turn each sound in array to audio scource
        {
            if (!s.spawn_on_object)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.outputAudioMixerGroup = audioMixer.FindMatchingGroups(s.layer.ToString())[0];
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

    public virtual void Start() //Play sounds on start (main songs and ambience)
    {

        //FindObjectOfType<AudioManager>().Play("sound"); (this is what to use when calling a sound elsewhere)
    }

    public virtual void Update()
    {
        /* For testing sound volume in inspector
        foreach (Sound s in sounds)
        {
            s.source.volume = s.volume;
        */
    }
    
    // play sound
    public virtual void Play (string name)
    { //Seach for sound in sounds by name, if it matches, play the sound
            Sound s = sounds.Find(sound => sound.name == name);
            if (s!= null)
            {
                if (s.source != null)
                {
                    if (!s.source.isPlaying)
                    {
                        s.source.Play();
                    }
                }
 
            }

    }
    
    // stop sound playing
    public virtual void Stop(string name)
    { //Seach for sound in sounds by name, if it matches, play the sound
            Sound s = sounds.Find(sound => sound.name == name);
            if (s.source != null && s.source.isPlaying)
            {
                s.source.Stop();
            }
    }

    // stop obj playing
    public virtual void Stop(GameObject obj)
    { //Seach for sound in sounds by name, if it matches, stop the sound
        AudioSource source = obj.GetComponent<AudioSource>();
        source.Stop();
    }

    //At the moment objects that play this way can only have 1 sound
    public virtual void Play(GameObject obj)
    { //Seach for sound in sounds by name, if it matches, play the sound
        AudioSource source = obj.GetComponent<AudioSource>();
        source.Play();
    }

    public virtual void Dynamic_Play(Sound s)
    { //Seach for sound in sounds by name, if it matches, play the sound
        if (!s.source.isPlaying)
        {
            s.source.Play();
        }
    }

    public virtual void Spawn(GameObject obj, string name)
    { //Spawn sound onto an object
            Sound s = sounds.Find(sound => sound.name == name);
            s.source = obj.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = audioMixer.FindMatchingGroups(s.layer.ToString())[0];
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = s.space;
        s.source.playOnAwake = false;
    }

    public virtual void Dynamic_Spawn(GameObject obj, List<Sound> sample, string name)
    { //Spawn sound onto an object
        Sound s = sounds.Find(sound => sound.name == name);
        s.source = obj.AddComponent<AudioSource>();
        s.source.clip = s.clip;
        s.source.outputAudioMixerGroup = audioMixer.FindMatchingGroups(s.layer.ToString())[0];
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
        s.source.spatialBlend = s.space;
        s.source.playOnAwake = false;
        sample.Add(s);
    }

    public virtual void SetVolume(string name, float volume)
    { //Set the volume of a sound
            Sound s = sounds.Find(sound => sound.name == name);
            if (s != null)
            {
                if (s.source != null)
                    s.source.volume = volume;
            }            
    }
}
