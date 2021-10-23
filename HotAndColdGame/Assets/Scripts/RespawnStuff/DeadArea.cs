using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is respondible to killing the player and resets the triggers that activate the object that this script is added onto.
/// Last edit: Added TriggerRest() so that when player death caused by hot/cold or player enters the object the script is added onto can now reset trigger  
/// By: Jason 23/10/21
/// </summary>

public class DeadArea : MonoBehaviour
{
    [SerializeField] private GameMaster gm;//to get last respawn/checkpoint
    [SerializeField] private Transform player; //To get Player's position.
    [SerializeField] private WaterTrigger trigger; //To get Trigger.

    // FX prefab(s)
    [SerializeField] public GameObject splashFX; //To get Player's position.
    [SerializeField] public AudioClip deathSound; //To get Player's position.

    public enum AreaType { Green, Darkness}

    public AreaType Type = AreaType.Green;


    public bool resetOnDeath;


    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        player = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        if (player.GetComponent<PlayerTemperature>().CurrentTemperature == 100f || player.GetComponent<PlayerTemperature>().CurrentTemperature == -100f) 
        {
            TriggerReset();
        }
    }

    //Sets new checkpoint
    void OnTriggerEnter(Collider other)
    {
        /*if (other.GetComponent<PlayerController>() != null)
            player.transform.position = gm.lastCheckPointPos.position;*/

        if (other.GetComponent<PlayerController>() != null)
        {
            if (Type == AreaType.Green)
            {
                GameObject.Find("UI").GetComponentInChildren<DeathEffect>().GreenDeath(3);
                GameObject splash = Instantiate(splashFX, player.position, Quaternion.identity);
                splash.transform.parent = player.transform;
                PlayDeathSound(splash);
            }
            else if (Type == AreaType.Darkness)
            {
                GameObject.Find("UI").GetComponentInChildren<DeathEffect>().DarknessDeath(3);
            }
            if (resetOnDeath)
            {
                TriggerReset();
            }
        }
    }

    public void TriggerReset() 
    {
        if (trigger != null)
        {
            if (trigger.resetOnDeath)
            {
                trigger.ResetTrigger();
            }
        }

        if (GetComponent<SimpleBehaviours>() != null)
        {
            GetComponent<SimpleBehaviours>().ResetState(false);
        }
    }

    public void PlayDeathSound(GameObject obj)
    {
        obj.GetComponent<AudioSource>().PlayOneShot(deathSound);
    }
}
