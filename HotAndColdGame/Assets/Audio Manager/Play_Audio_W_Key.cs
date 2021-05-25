using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play_Audio_W_Key : MonoBehaviour
{
    public string Key;
    public string Clip;

    // Update is called once per frame
    void Start()
    {
        FindObjectOfType<AudioManager>().Spawn(this.gameObject,Clip);
    }

        void Update()
    {
        if (Input.GetKeyDown(Key))
        {
            FindObjectOfType<AudioManager>().Play(this.gameObject);
        }
    }
}
