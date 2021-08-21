using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Create a sound array object for things
[System.Serializable]
public class Dynamic_Sound
{
    public string name;
    public bool _randomisePitch = false;
    public bool _randomiseStartTime = false;
    public float pitch;
}

public class Play_Dynamic_Sound : MonoBehaviour
{
    public Dynamic_Sound[] _clips;
    private List<Sound> sounds = new List<Sound>();

    //gameObject.SendMessage("Play_Sound");

    void Start()
    {
        for (int i = 0; i < _clips.Length; i++)
        {
           FindObjectOfType<AudioManager>().Dynamic_Spawn(this.gameObject, sounds, _clips[i].name);
        }
    }

    void Play_Sound(string name)
    {
        Sound s = sounds.Find(sound => sound.name == name);
        for (int i = 0; i < _clips.Length; i++)
        {
            if (_clips[i].name == name)
            {
                if (_clips[i]._randomisePitch)
                {
                    float current_pitch = _clips[i].pitch;
                    this.GetComponent<AudioSource>().pitch = Random.Range(current_pitch - 0.05f, current_pitch + 0.05f);
                    // randomise the pitch slightly on spawn 
                }
                if (_clips[i]._randomiseStartTime)
                {
                    this.GetComponent<AudioSource>().time = Random.Range(0, this.GetComponent<AudioSource>().clip.length);
                }
            }
        }
        FindObjectOfType<AudioManager>().Dynamic_Play(s);
    }
}
