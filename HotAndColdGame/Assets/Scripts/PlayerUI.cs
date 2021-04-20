using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    //Variables
    [SerializeField]
    private Slider _sliderTemp; //Player's temperature guage as a slider
    [SerializeField]
    private PlayerTemperature _playerTemp; //Reference to script that handles the player's temperature
    private string _temp; //Temperature variable

    // Start is called before the first frame update
    void Start()
    {  
        _temp = "Neutral"; //Starting temperature
    }

    // Update is called once per frame
    void Update()
    {
        /*
        //For debug purposes
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _temp = "Neutral";
            Debug.Log("Temperature of location: Neutral");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _temp = "Cold";
            Debug.Log("Temperature of location: Cold");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _temp = "Heat";
            Debug.Log("Temperature of location: Hot");
        }
        */

        //For demo purposes- used to change the temperature of player
        //LocationCheck();

        _playerTemp.ValueChange(_sliderTemp, _temp);
    }

    /// <summary>
    /// Demo purpose function that uses a raycast to detect the plane the player is standing on.
    /// </summary>
    private void LocationCheck()
    {
        //Raycast shooting a ray downwards
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            //Changes the _temp variable (Player's variable)
            switch (hit.transform.gameObject.tag)
            {
                case "Neutral Area":
                    _temp = "Neutral";
                    break;

                case "Cold Area":
                    _temp = "Cold";
                    break;

                case "Heat Area":
                    _temp = "Heat";
                    break;
            }
        }
    }
}
