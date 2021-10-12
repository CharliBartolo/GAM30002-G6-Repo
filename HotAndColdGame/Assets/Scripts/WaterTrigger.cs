using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    public SimpleBehaviours DeathWaterObject;
<<<<<<< HEAD
    public GameObject[] Waterfalls;

    public bool triggered = false;
    public bool activeOnStart;
    public AudioSource soundFX;
    public bool resetOnDeath;

=======

    private bool triggered = false;
    public bool activeOnStart;
>>>>>>> 7b688233387786860c4dc5b974fab5d75dd2dbe6

    // Start is called before the first frame update
    void Start()
    {
        if(activeOnStart)
        {
          
        }
        else
        {
            if (DeathWaterObject.gameObject.activeSelf == true)
<<<<<<< HEAD
            {
                DeathWaterObject.gameObject.SetActive(false);
                foreach (var item in Waterfalls)
                {
                    item.SetActive(false);
                }
            }
        }
=======
                DeathWaterObject.gameObject.SetActive(false);
        }
      
>>>>>>> 7b688233387786860c4dc5b974fab5d75dd2dbe6
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Trigger()
    {
        if (DeathWaterObject.gameObject.activeSelf == false)
            DeathWaterObject.gameObject.SetActive(true);
<<<<<<< HEAD
        DeathWaterObject.Trigger(true);
        DeathWaterObject.enabled = true;
        triggered = true;

        foreach (var item in Waterfalls)
        {
            item.SetActive(true);
        }

        soundFX.Play();
       
        //Destroy(gameObject);
    }

    public void ResetTrigger()
    {
        triggered = false;

        foreach (var item in Waterfalls)
        {
            item.SetActive(false);
        }
=======
        // DeathWaterObject.Trigger(true);
        DeathWaterObject.enabled = true;
        triggered = true;
        Destroy(gameObject);
>>>>>>> 7b688233387786860c4dc5b974fab5d75dd2dbe6
    }

    private void OnTriggerEnter(Collider other)
    {
<<<<<<< HEAD
        if(other.GetComponent<PlayerController>()!= null)
        {
            if (!triggered)
            {
                Trigger();
            }
=======
        if(!triggered)
        {
            Trigger();
>>>>>>> 7b688233387786860c4dc5b974fab5d75dd2dbe6
        }
    }
}
