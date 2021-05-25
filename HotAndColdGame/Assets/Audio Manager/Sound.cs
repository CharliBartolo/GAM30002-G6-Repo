using UnityEngine.Audio;
using UnityEngine;

//Sound class used in Audio Manager

[System.Serializable]
public class Sound
{
    //Name and sample
    public string name;
    public AudioClip clip;

    //Sliders to set volume and pitch
    [Range(0f,1f)]
    public float volume;
    [Range(.1f,3f)]
    public float pitch;
    [Range(0f, 1f)] //2D - 3D
    public float space;

    public bool loop;
    public bool spawn_on_object;

    [HideInInspector]
    public AudioSource source;
}
