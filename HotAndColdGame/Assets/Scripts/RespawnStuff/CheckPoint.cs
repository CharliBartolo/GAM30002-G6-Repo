using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private GameMaster gm;//reference game master script

    // Start is called before the first frame update
    void Start()
    {
        //get game master last position
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
    }

    //Sets new checkpoint
    void OnTriggerEnter(Collider other)
    {
        //check if player enters
        if (other.CompareTag("Player")) 
        {
            gm.lastCheckPointPos = transform;
        }
    }
}
