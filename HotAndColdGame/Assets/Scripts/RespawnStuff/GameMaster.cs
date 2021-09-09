using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;
    public AudioManager audioManager;
    public ColourPalette colourPallete;
    public GameObject playerRef;
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

        SearchForPlayer();
    }

    private void Update() 
    {
        if (playerRef == null)
        {
            SearchForPlayer();
        }
    }

    private void SearchForPlayer()
    {
        if (GameObject.Find("Player") != null)
        {
            playerRef = GameObject.FindGameObjectWithTag("Player");
            GameObject checkpointObject = new GameObject ("SpawnedCheckpoint");
            checkpointObject.transform.position = playerRef.transform.position;
            checkpointObject.transform.rotation = playerRef.transform.rotation;
            lastCheckPointPos = checkpointObject.transform;
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
