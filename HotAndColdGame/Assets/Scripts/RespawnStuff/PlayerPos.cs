using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPos : MonoBehaviour
{
    private GameMaster gm;//reference game master script

    // Start is called before the first frame update
    void Start()
    {
        //get game master last position
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        //set player to last checkpoint position
        transform.position = gm.lastCheckPointPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        //used if wanting to reset scene
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
