using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;
    public AudioManager audioManager;
    public ColourPalette colourPallete;
    //Vector 3 works, had problems with transform but maybe there is a way to use transform
    [SerializeField] public Transform lastCheckPointPos;

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
