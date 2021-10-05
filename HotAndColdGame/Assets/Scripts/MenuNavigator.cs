using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuNavigator : MonoBehaviour
{
    public EventSystem currentEventSystem;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetActiveButton(GameObject buttonToSet)
    {
        if (currentEventSystem != null)
        {
            currentEventSystem.SetSelectedGameObject(buttonToSet, new BaseEventData (currentEventSystem));
        }
    }
}
