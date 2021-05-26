using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AudioTest : MonoBehaviour

    //Current volume is not currently set to match temperature value, This will need to be further implemented depending on how we want it to work
{
    //Temprature (placeholder)

    public float current_value = 0;
    public float standard_value = 0;
    public float max_value = 100;
    public float min_value = -100;

    //Song Theme volumes
    float max_volume = 1;
    float min_volume = 0;
    public float main_volume = 1;
    public float ice_volume = 0;
    public float heat_volume = 0;

    //The rate at which the volume changes
    float volume_rate = 0.05f;


    public int collision_count = 0;

    // Update is called once per frame
    void Update()
    {
        //Test playing a sound
        if (Input.GetKeyDown(KeyCode.Q))
        {
            FindObjectOfType<AudioManager>().Play("Lazer");
        }

        //If not hitting crystal trigger, return song to main
        if (collision_count == 0) {
            if (main_volume < max_volume)
            {
                main_volume = Mathf.MoveTowards(main_volume, max_volume, volume_rate * Time.deltaTime);
                ice_volume = Mathf.MoveTowards(ice_volume, min_volume, volume_rate * Time.deltaTime);
                heat_volume = Mathf.MoveTowards(heat_volume, min_volume, volume_rate * Time.deltaTime);
            }

            current_value = Mathf.MoveTowards(current_value, standard_value, 1);

        }

        //move the test cube
        this.transform.Translate(Input.GetAxis("Horizontal") * 0.2f, 0, 0);
        this.transform.Translate(0, Input.GetAxis("Vertical") * 0.2f, 0);

        //Adjust volumes on update
        FindObjectOfType<AudioManager>().SetVolume("Main", main_volume);
        FindObjectOfType<AudioManager>().SetVolume("Ice", ice_volume);
        FindObjectOfType<AudioManager>().SetVolume("Heat", heat_volume);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Blue") || other.gameObject.CompareTag("Red"))
        {
            collision_count++;
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.CompareTag("Red"))
        {
            if (current_value < max_value)
            {
                current_value++;
            }
            if (heat_volume < max_volume)
            {
                //set other volumes to min
                main_volume = Mathf.MoveTowards(main_volume, min_volume, volume_rate * Time.deltaTime);
                ice_volume = Mathf.MoveTowards(ice_volume, min_volume, volume_rate * Time.deltaTime);

                //set heat volume to max
                heat_volume = Mathf.MoveTowards(heat_volume, max_volume, volume_rate * Time.deltaTime);
            }
        }
     
        if (col.gameObject.CompareTag("Blue"))
        {
            if (current_value > min_value)
            {
                current_value--;
            }
            if (ice_volume < max_volume)
            {
                //set other volumes to min
                main_volume = Mathf.MoveTowards(main_volume, min_volume, volume_rate * Time.deltaTime);
                heat_volume = Mathf.MoveTowards(heat_volume, min_volume, volume_rate * Time.deltaTime);

                //set heat volume to max
                ice_volume = Mathf.MoveTowards(ice_volume, max_volume, volume_rate * Time.deltaTime);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Blue") || other.gameObject.CompareTag("Red"))
        {
            collision_count--;
        }
    }

}
