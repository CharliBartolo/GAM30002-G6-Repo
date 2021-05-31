using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadArea : MonoBehaviour
{
    [SerializeField] private GameMaster gm;//to get last respawn/checkpoint
    [SerializeField] private Transform player; //To get Player's position.

    //Sets new checkpoint
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            player.transform.position = gm.lastCheckPointPos.position;
    }
}
