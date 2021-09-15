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
    public int difficultyNum;   // 0 is standard, 1 is hard

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
        SearchForPlayer();
        LoadDifficulty();
    }
    

    private void Update() 
    {
        if (playerRef == null)
        {
            if (SearchForPlayer())
                LoadDifficulty();
        }
    }

    private bool SearchForPlayer()
    {
        Debug.Log("GameManager is searching for a Player object...");
        //if (GameObject.Find("Player") != null)
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            playerRef = GameObject.FindGameObjectWithTag("Player");
            GameObject checkpointObject = new GameObject ("SpawnedCheckpoint");
            checkpointObject.transform.position = playerRef.transform.position;
            checkpointObject.transform.rotation = playerRef.transform.rotation;
            lastCheckPointPos = checkpointObject.transform;
            Debug.Log("Player object found, reference stored!");
            return true;
        } 

        Debug.Log("Player not found by GameManager!");
        return false;            
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);        
    }


    void LoadCheckpoint()
    {

    }

    public void SetDifficulty(int difficultyNum)
    {
        PlayerPrefs.SetInt("Difficulty", difficultyNum);
    }

    public void LoadDifficulty()
    {
        difficultyNum = PlayerPrefs.GetInt("Difficulty");
        Debug.Log("Difficulty loaded!");

        switch (difficultyNum)
        {
            case 0:
                Debug.Log("Standard difficulty selected");
                if (playerRef != null)
                {
                    if (playerRef.TryGetComponent<PlayerTemperature>(out PlayerTemperature playerTempComp))
                    {
                        playerTempComp.TempStatesAllowed = ITemperature.tempStatesAllowed.OnlyNeutral;
                        playerTempComp.tempValueRange[0] = -50;
                        playerTempComp.tempValueRange[2] = 50;
                    }
                    else
                    Debug.Log("Player temperature component not found!");
                }
                else
                    Debug.Log("Player not found!");
                break;
            case 1:
                if (playerRef != null)
                    {
                        if (TryGetComponent<PlayerTemperature>(out PlayerTemperature playerTempComp))
                        {
                            playerTempComp.TempStatesAllowed = ITemperature.tempStatesAllowed.HotAndCold;
                            playerTempComp.tempValueRange[0] = -100;
                            playerTempComp.tempValueRange[2] = 100;
                        }
                    }
                break;
            default:
                Debug.Log("Invalid difficulty entered, loading Standard (0) instead...");
                SetDifficulty(0);
                LoadDifficulty();
                break;
        }
    }
}
