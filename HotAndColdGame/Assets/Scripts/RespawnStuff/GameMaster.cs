using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            DontDestroyOnLoad(instance.gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (GameObject.Find("Player") != null)
        {
            lastCheckPointPos = GameObject.Find("Player").transform;
        }        
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    void LoadCheckpoint()
    {

    }



}
