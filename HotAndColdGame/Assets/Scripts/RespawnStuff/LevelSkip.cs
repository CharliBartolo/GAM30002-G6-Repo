using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is responsible for letting Andy skip levels
/// 
/// By: Jason - 02/10/2021
/// </summary>

public class LevelSkip : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //loads next scene
        if ((Input.GetKeyDown(KeyCode.PageUp)))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        //loads previous scene
        else if (Input.GetKeyDown(KeyCode.PageDown)) 
        {
            GameMaster.instance.TravelledBackward = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
}
