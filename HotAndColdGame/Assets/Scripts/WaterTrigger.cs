using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    public SimpleBehaviours DeathWaterObject;

    private bool triggered = false;

    // Start is called before the first frame update
    void Start()
    {
        if(DeathWaterObject.gameObject.activeSelf == true)
            DeathWaterObject.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Trigger()
    {
        DeathWaterObject.gameObject.SetActive(true);
        DeathWaterObject.Trigger(true);
        triggered = true;
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!triggered)
        {
            Trigger();
        }
    }
}
