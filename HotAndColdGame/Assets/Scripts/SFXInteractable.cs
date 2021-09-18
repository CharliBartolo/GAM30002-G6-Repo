using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SFXInteractable: MonoBehaviour
{
    [SerializeField]  
    public GameObject InteractingObject;
    public DeathEffect DE;
    private void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(DE != null)
        {
            if (DE.isResetting)
            {
                gameObject.GetComponent<VisualEffect>().enabled = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(InteractingObject != null)
        {
            if (other == InteractingObject.GetComponent<Collider>())
            {
                gameObject.GetComponent<VisualEffect>().enabled = false;
            }
        }
    }
}
