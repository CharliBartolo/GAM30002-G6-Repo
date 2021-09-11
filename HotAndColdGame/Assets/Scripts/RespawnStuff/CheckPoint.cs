using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private GameMaster gm;//reference game master script
    [SerializeField] private Transform spawnPos;
    [SerializeField] public float campfireTemp = 30.0f;

    private bool triggered;

    // Start is called before the first frame update
    void Start()
    {
        PutOutCampfires();
        //get game master last position
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();

        if (spawnPos == null)
        {
            spawnPos = transform;
        }
    }

    public void LightCampfire()
    {
        CrystalBehaviour campfire = GetComponentInChildren<CrystalBehaviour>();

        if(campfire != null)
        {
            campfire.isPermanentlyPowered = true;
            campfire.transform.Find("EffectLight").GetComponent<Light>().enabled = true;
            campfire.SetTemperature(campfireTemp);
            this.triggered = true;
        }
    }

    public void PutOutCampfires()
    {
        GameObject[] campfires = GameObject.FindGameObjectsWithTag("Campfire");

        if(campfires != null && campfires.Length >0)
        {
            foreach (var item in campfires)
            {
                item.transform.Find("EffectAreaOnly").transform.Find("EffectLight").GetComponent<Light>().enabled = false;
                item.GetComponentInParent<CheckPoint>().triggered = false;
                item.GetComponentInChildren<CrystalBehaviour>().SetTemperature(0);
                item.GetComponentInChildren<CrystalBehaviour>().isPermanentlyPowered = false;
            }
        }
    }

    //Sets new checkpoint
    void OnTriggerEnter(Collider other)
    {
        //check if player enters
        if (other.CompareTag("Player") && !triggered) 
        {
            gm.lastCheckPointPos = spawnPos;

            PutOutCampfires();
            LightCampfire();

        }
    }
}
