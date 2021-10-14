using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube_Test_Player : MonoBehaviour
{
    public float speed;
    [SerializeField]
    private Journal_Reader journalReader;
    private void Update()
    {
       /* Rigidbody rb = GetComponent<Rigidbody>();
        if (Input.GetKey(KeyCode.A))
            rb.AddForce(Vector3.left * speed);
        if (Input.GetKey(KeyCode.D))
            rb.AddForce(Vector3.right * speed);
        if (Input.GetKey(KeyCode.W))
            rb.AddForce(Vector3.up * speed);
        if (Input.GetKey(KeyCode.S))
            rb.AddForce(Vector3.down * speed);*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Journal")
        {
            string entry = other.GetComponent<Journal>().EntryLog[0];
            journalReader.SendMessage("Display_Journal", entry);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Journal")
        {
            journalReader.SendMessage("Exit_Journal", "");
        }
    }
}
