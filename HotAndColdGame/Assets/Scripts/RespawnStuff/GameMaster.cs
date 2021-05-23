using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private static GameMaster instance;
    //Vector 3 works, had problems with transform but maybe there is a way to use transform
    [SerializeField] public Vector3 lastCheckPointPos;

    void Awake() 
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else 
        {
            Destroy(gameObject);
        }
    }
}
