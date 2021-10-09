using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    public SimpleBehaviours DeathWaterObject;

    private bool triggered = false;
    public bool activeOnStart;
    public AudioSource soundFX;


    // Start is called before the first frame update
    void Start()
    {
        if(activeOnStart)
        {
          
        }
        else
        {
            if (DeathWaterObject.gameObject.activeSelf == true)
                DeathWaterObject.gameObject.SetActive(false);
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Trigger()
    {
        if (DeathWaterObject.gameObject.activeSelf == false)
            DeathWaterObject.gameObject.SetActive(true);
        DeathWaterObject.Trigger(true);
        DeathWaterObject.enabled = true;
        triggered = true;
        soundFX.Play();
        //Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!triggered)
        {
            Trigger();
        }
    }
}
