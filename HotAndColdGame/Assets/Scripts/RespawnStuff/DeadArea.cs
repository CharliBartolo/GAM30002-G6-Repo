using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadArea : MonoBehaviour
{
    [SerializeField] private GameMaster gm;//to get last respawn/checkpoint
    [SerializeField] private Transform player; //To get Player's position.


    private void Start()
    {
        player = GameObject.Find("Player").transform;
    }
    //Sets new checkpoint
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
            player.transform.position = gm.lastCheckPointPos.position;
    }
}
