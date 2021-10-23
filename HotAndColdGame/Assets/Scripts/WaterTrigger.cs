using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    public SimpleBehaviours DeathWaterObject;
    public GameObject[] Waterfalls;

    public bool triggered = false;
    public bool activeOnStart;
    public AudioSource soundFX;
    public bool resetOnDeath;
    


    // Start is called before the first frame update
    void Start()
    {
        if(activeOnStart)
        {
          
        }
        else
        {
            if (DeathWaterObject.gameObject.activeSelf == true)
            {
                DeathWaterObject.gameObject.SetActive(false);
                foreach (var item in Waterfalls)
                {
                    item.SetActive(false);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Trigger()
    {
        if (DeathWaterObject.gameObject.activeSelf == false)
        {
            DeathWaterObject.gameObject.SetActive(true);
        }
            
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerController>()!= null)
        {
            if (!triggered)
            {
                Trigger();
            }
        }
    }
}
