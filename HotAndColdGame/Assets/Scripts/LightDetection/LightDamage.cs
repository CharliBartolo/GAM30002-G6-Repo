using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDamage : MonoBehaviour
{
    //Variables & Scripts
    public LightDetection LD; //Credits to Charli.
    public Transform player; //To get Player's position.
    public float DAMAGETIMER = 5.0f; //Timer for how long you can stay in the dark.
    private Vector3 _reset_position; //Position to reset to after timer ends.
    private float _timer = 0.0f; //Timer

    // Start is called before the first frame update
    void Start()
    {
        //Reverts to starting point (Could be changed to checkpoints etc.)
        _reset_position = player.position;
    }

    // Update is called once per frame
    void Update()
    {



        //If not in light.
        if (!LD.isLit)
        {
            _timer += Time.deltaTime; //Add time.
            float _seconds = _timer % 60f; //Seconds (simulates realtime) 

            //If player in dark for too long.
            if (_timer > DAMAGETIMER)
            {
                //Resets timer and position.
                _timer = 0.0f;
                GetComponent<CharacterController>().enabled = false;
                player.position = _reset_position;
                GetComponent<CharacterController>().enabled = true;
            }
        }
        else
        {
            //Resets timer if player returns to safe zone.
            _timer = 0.0f;
        }
    }

}