using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play_Static_Sound : MonoBehaviour
{
    public string Clip;
    public bool _randomiseStartTime = false;
    public bool _randomisePitch = false;
    [Range(0.01f, 0.5f)]
    public float pitch_range;

    // random start timer.

    // Update is called once per frame
    void Start()
    {
        FindObjectOfType<AudioManager>().Spawn(this.gameObject,Clip);

        this.GetComponent<AudioSource>().loop = true;

        if (_randomisePitch)
        {
            float current_pitch = this.GetComponent<AudioSource>().pitch;
            this.GetComponent<AudioSource>().pitch = Random.Range(current_pitch - pitch_range, current_pitch + pitch_range);
            // randomise the pitch slightly on spawn 
        }
        if (_randomiseStartTime)
        {
            this.GetComponent<AudioSource>().time = Random.Range(0, this.GetComponent<AudioSource>().clip.length);
        }

        FindObjectOfType<AudioManager>().Play(this.gameObject);
    }
}
