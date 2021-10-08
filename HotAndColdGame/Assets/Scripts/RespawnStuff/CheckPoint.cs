using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for managing checkpoint objects. Functionality includes lighting campfires,
/// putting campfires out, and updating Player spawn position when triggers are entered.
/// Last edit: Adding Class Summary
/// By: Charli - 8/10/21
/// </summary>
public class CheckPoint : MonoBehaviour
{
    [SerializeField] private GameMaster gm;//reference game master script
    [SerializeField] public Transform spawnPos;
    [SerializeField] public float campfireTemp = 30.0f;
    [SerializeField] private bool isActiveInChallenger = true;

    private bool triggered;

    public AudioClip activationSound;
    public AudioClip passiveSound;

    // Start is called before the first frame update
    void Start()
    {
        PutOutCampfires();
        //get game master last position
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();

        // Disable campfire and checkpoint if not supposed to be active in challenger difficulty, and challenger is enabled
        if (!isActiveInChallenger && gm.difficultyNum == 1)
        {
            gameObject.SetActive(false);
        }

        if (spawnPos == null)
        {
            spawnPos = transform;
        }
    }

    public void LightCampfire()
    {
        CrystalBehaviour campfire = GetComponentInChildren<CrystalBehaviour>();
        PlayActivationSound();
        PlayPassiveSound();

        if (campfire != null)
        {
            
            campfire.isPermanentlyPowered = true;
            campfire.transform.Find("EffectLight").GetComponent<Light>().enabled = true;
            campfire.SetTemperature(campfireTemp);
            this.triggered = true;
        }
    }

    public void PlayActivationSound()
    {
        //GetComponent<AudioSource>().clip = activationSound;
        GetComponent<AudioSource>().PlayOneShot(activationSound);
    }

    public void PlayPassiveSound()
    {
        GetComponent<AudioSource>().clip = passiveSound;
        GetComponent<AudioSource>().loop = true;
        GetComponent<AudioSource>().Play();
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
                GetComponent<AudioSource>().loop = false;
                GetComponent<AudioSource>().Stop();
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
