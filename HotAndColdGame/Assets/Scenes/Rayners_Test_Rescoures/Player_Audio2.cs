using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Audio2 : MonoBehaviour
{

    //Current volume is not currently set to match temperature value, This will need to be further implemented depending on how we want it to work

    //Song Theme volumes
    float max_volume = 1;
    float min_volume = 0;
    public float main_volume = 1;
    public float ice_volume = 0;
    public float heat_volume = 0;

    public PlayerController temp;

    //The rate at which the volume changes
    float volume_rate = 0.05f;

    // Update is called once per frame
    void Update()
    {
        //Test playing a sound
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GetComponent<AudioManager>().Play("Lazer");
        }

        //Adjust volumes on update
        GetComponent<AudioManager>().SetVolume("Main", main_volume);
        GetComponent<AudioManager>().SetVolume("Ice", ice_volume);
        GetComponent<AudioManager>().SetVolume("Heat", heat_volume);

        //If not hitting crystal trigger, return song to main

        if (temp.ActiveConditions.Contains(IConditions.ConditionTypes.ConditionHot))
        {
            if (heat_volume < max_volume)
            {
                //set heat volume to max
                heat_volume = Mathf.MoveTowards(heat_volume, max_volume, volume_rate * Time.deltaTime);
            }
            //set other volumes to min
            main_volume = Mathf.MoveTowards(main_volume, min_volume, volume_rate * Time.deltaTime);
            ice_volume = Mathf.MoveTowards(ice_volume, min_volume, volume_rate * Time.deltaTime);
        }

        if (temp.ActiveConditions.Contains(IConditions.ConditionTypes.ConditionCold))
        {
            if (ice_volume < max_volume)
            {
                //set heat volume to max
                ice_volume = Mathf.MoveTowards(ice_volume, max_volume, volume_rate * Time.deltaTime);
            }
            //set other volumes to min
            main_volume = Mathf.MoveTowards(main_volume, min_volume, volume_rate * Time.deltaTime);
            heat_volume = Mathf.MoveTowards(heat_volume, min_volume, volume_rate * Time.deltaTime);
        }

        if (temp.ActiveConditions.Count == 0)
        {
            if (main_volume < max_volume)
            {
                main_volume = Mathf.MoveTowards(main_volume, max_volume, volume_rate * Time.deltaTime);
            }
            ice_volume = Mathf.MoveTowards(ice_volume, min_volume, volume_rate * Time.deltaTime);
            heat_volume = Mathf.MoveTowards(heat_volume, min_volume, volume_rate * Time.deltaTime);
        }

    }
}
