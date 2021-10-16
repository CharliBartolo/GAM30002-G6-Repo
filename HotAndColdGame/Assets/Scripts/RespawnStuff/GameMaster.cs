using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Responsible for storing object states between scenes and managing game state.
/// Affected objects include maintaining a Player reference for other objects, AudioManager
/// , colour pallet, current difficulty, current checkpoint etc.
/// Last edit: Added OnLevelLoad delegate, expanded to manage backtracking
/// By: Charli - 15/10/21
/// </summary>
public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;
    public AudioManager audioManager;
    public ColourPalette colourPallete;
    public GameObject playerRef;    
    public enum RaygunSavedState {NoSavedState, NoGun, GunNoUpgrade, GunOneUpgrade, GunTwoUpgrade};
    public RaygunSavedState savedGunState;
    public GameObject start;
    public GameObject end;
    public int crntScene;
    public int prevScene;

    public int difficultyNum;   // 0 is standard, 1 is hard

    public bool LoadingCheckpoint;

    [SerializeField] public Transform lastCheckPointPos;

    //Main Menu Changes
    public MainMenu.ControlSettings CS = new MainMenu.ControlSettings(0.5f, 0.5f, 1);

    void Awake() 
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance.gameObject);
        }
        else if (gameObject.scene.buildIndex == 0)
        {
            Destroy(instance.gameObject);
            instance = this;
            DontDestroyOnLoad(instance.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SearchForPlayer();
    }

    private void Start() 
    {
        OnLevelLoad(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        SceneManager.sceneLoaded += OnLevelLoad;        
    }

    public void OnLevelLoad(Scene load, LoadSceneMode mode)
    {
        if (SearchForPlayer())
            LoadDifficulty();        

        LoadGunState();
        SearchForTriggers();
        crntScene = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("crnt: " + crntScene);
        Debug.Log("prev: " + prevScene);
        //checks if previous scene is the next scene in the build index
        if (crntScene < prevScene && end != null)
        {           
            lastCheckPointPos = end.transform;
        }
        else if (start != null)
        {
            lastCheckPointPos = start.transform;
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

    public void LoadGunState()
    {
        if ((int)savedGunState < 0 || (int)savedGunState > 4)
            savedGunState = (RaygunSavedState)0;

        //Debug.Log("Loading Gun State from previous level");
        // If a gun state hasn't been saved, do nothing.
        if (savedGunState == RaygunSavedState.NoSavedState)
            return;
        
        if (playerRef.TryGetComponent<PlayerController>(out PlayerController playerCntrlComp) &&
            playerRef.TryGetComponent<GunFXController>(out GunFXController gunFXComp))
            {
                //If there was no gun when state was saved, unequip it and set upgrade state to zero.
                if ((int)savedGunState == 1)
                {            
                    {
                        playerCntrlComp.isGunEnabled = false;
                        if (playerCntrlComp.playerInventory.Contains("Raygun"))
                            playerCntrlComp.playerInventory.Remove("Raygun");
                        gunFXComp.UnEquipTool();
                        playerCntrlComp.raygunScript.SetGunUpgradeState(0);
                    }
                    return;
                }
                // ...but if gun was saved, set it to equipped and set upgrade state accordingly.
                else if ((int)savedGunState > 1)
                {   
                    playerCntrlComp.isGunEnabled = true; 
                    if (!playerCntrlComp.playerInventory.Contains("Raygun"))
                        playerCntrlComp.playerInventory.Add("Raygun");                      
                    gunFXComp.EquipTool(false);                    
                    playerCntrlComp.raygunScript.SetGunUpgradeState((int)savedGunState - 2);                    
                }
            }        
        }
    

    public void SaveGunState()
    {
        print("Saved Gun State!");
        if (playerRef.TryGetComponent<GunFXController>(out GunFXController gunFXComp))
        {
            if (gunFXComp.equipped)
            {
                if (playerRef.TryGetComponent<PlayerController>(out PlayerController playerCntrlComp))
                {
                    savedGunState = (RaygunSavedState)playerCntrlComp.raygunScript.gunUpgradeState + 2;                 
                }
            }
            else
            {
                savedGunState = RaygunSavedState.NoGun;
            }
        } 
    }    

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);        
    }


    public void LoadCheckpoint()
    {
        LoadingCheckpoint = true;
    }

    public void SetDifficulty(int difficultyNum)
    {
        PlayerPrefs.SetInt("Difficulty", difficultyNum);
    }

    public void LoadDifficulty()
    {
        difficultyNum = PlayerPrefs.GetInt("Difficulty");
        //Debug.Log("Difficulty loaded!");

        switch (difficultyNum)
        {
            case 0:
                //Debug.Log("Story difficulty selected");
                if (playerRef != null)
                {
                    if (playerRef.TryGetComponent<PlayerTemperature>(out PlayerTemperature playerTempComp))
                    {
                        playerTempComp.TempStatesAllowed = ITemperature.tempStatesAllowed.OnlyNeutral;
                        playerTempComp.tempValueRange[0] = -50;
                        playerTempComp.tempValueRange[2] = 50;
                        playerTempComp.startingCountdownBeforeReturnToNeutral = 0.1f;
                        playerTempComp.incomingTempMod = 1f;
                    }
                    else
                    Debug.Log("Player temperature component not found!");
                }
                else
                   Debug.Log("Player not found!");
                break;
            case 1:
                //Debug.Log("Challenger difficulty selected");
                if (playerRef != null)
                    {
                        if (TryGetComponent<PlayerTemperature>(out PlayerTemperature playerTempComp))
                        {
                            playerTempComp.TempStatesAllowed = ITemperature.tempStatesAllowed.HotAndCold;
                            playerTempComp.tempValueRange[0] = -100;
                            playerTempComp.tempValueRange[2] = 100;
                            playerTempComp.startingCountdownBeforeReturnToNeutral = 2;
                            playerTempComp.incomingTempMod = 0.4f;
                        }
                    }
                break;
            default:
                Debug.Log("Invalid difficulty entered, loading Story (0) instead...");
                SetDifficulty(0);
                LoadDifficulty();
                break;
        }
    }

    private bool SearchForTriggers()
    {
        Debug.Log("GameManager is searching for triggers");
        //if (GameObject.Find("Player") != null)
        if (GameObject.FindGameObjectWithTag("Start") && GameObject.FindGameObjectWithTag("End"))
        {
            start = GameObject.FindGameObjectWithTag("Start");
            end = GameObject.FindGameObjectWithTag("End");
            Debug.Log("triggers found, reference stored!");
            return true;
        }

        Debug.Log("triggers not found by GameManager!");
        return false;
    }
}

