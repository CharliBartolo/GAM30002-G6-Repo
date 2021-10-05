using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadArea : MonoBehaviour
{
    [SerializeField] private GameMaster gm;//to get last respawn/checkpoint
    [SerializeField] private Transform player; //To get Player's position.

    // FX prefab(s)
    [SerializeField] public GameObject splashFX; //To get Player's position.
    [SerializeField] public AudioClip deathSound; //To get Player's position.

    public enum AreaType { Green, Darkness}

    public AreaType Type = AreaType.Green;
    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        player = GameObject.Find("Player").transform;
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
        }
    }

    public void PlayDeathSound(GameObject obj)
    {
        obj.GetComponent<AudioSource>().PlayOneShot(deathSound);
    }
}
