using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RebindingDisplay : MonoBehaviour
{
    [SerializeField] private InputActionReference jumpAction = null;
    [SerializeField] private PlayerController playerController = null;
    [SerializeField] private Text bindingDisplayNameText = null;
    [SerializeField] private GameObject startRebindObject = null;
    [SerializeField] private GameObject waitingForInputObject = null;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    public void StartRebinding()
    {
        startRebindObject.SetActive(false);
        waitingForInputObject.SetActive(true);

        //playerController.playerControlState = PlayerController.PlayerState.ControlsDisabled;
        playerController.playerInput.SwitchCurrentActionMap("Menu");

        rebindingOperation = jumpAction.action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete())
            .Start();
    }

    private void RebindComplete()
    {
        rebindingOperation.Dispose();

        startRebindObject.SetActive(true);
        waitingForInputObject.SetActive(false);

        //playerController.playerControlState = PlayerController.PlayerState.MoveAndLook;
        playerController.playerInput.SwitchCurrentActionMap("Player");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
