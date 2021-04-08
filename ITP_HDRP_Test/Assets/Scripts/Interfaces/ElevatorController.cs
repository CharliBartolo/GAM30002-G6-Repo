using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ElevatorController : MonoBehaviour, IRemoteFunction, IRequireKey
{
    public MovingObject controlledObject;
    public Renderer Light;
    public Material RedLight;
    public Material GreenLight;

    public bool locked;
    public int keyRequired;

    private bool dir;

    public void RemoteControl()
    {
        if(PlayerHasKeyValidId())
        {
            Debug.Log("elevetor control used");
            controlledObject.PerformMove();
            if(keyRequired != -1)
                Activate();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Activate()
    {
        Light.material = GreenLight;
    }

    public bool PlayerHasKeyValidId()
    {
        List<int> player_keys = GameObject.Find("Player").GetComponent<PlayerInteractions>().key_ids;
        if (keyRequired == -1)
            return true;

        foreach (var key in player_keys)
        {
            if (key == keyRequired)
                return true;
        }

        return false;
    }
}
