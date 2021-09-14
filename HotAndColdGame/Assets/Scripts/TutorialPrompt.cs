using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class TutorialPrompt : MonoBehaviour
{
    /*
    public enum PlayerActions
    {
        Interact,
        Jump,
        Look,
        Movement,
        Shoot,
        SwapBeam,
        Walk
    };
    */

    //Struct that defines the Tutorial Prompts
    [System.Serializable]
    public struct Prompt
    {
        [SerializeField]
        [Tooltip("Label for the Tutorial Prompt")]
        private string _label;

        [SerializeField]
        [Tooltip("Message for the Tutorial Prompt")]
        [TextArea(15, 20)]
        private string _message;

        [SerializeField]
        [Tooltip("Trigger object to start the Tutorial Prompt")]
        private Collider _startTrigger;

        [SerializeField]
        [Tooltip("Trigger object to end the Tutorial Prompt")]
        private Collider _endTrigger;
        /*
        [SerializeField]
        [Tooltip("The Action that the Tutorial Prompt is trying to teach")]
        private PlayerActions _playerAction;
        */
        [SerializeField]
        [Tooltip("True if the player is completing the Tutorial Prompt")]
        private bool _isActive;

        /*
        [SerializeField]
        [Tooltip("True if the player is completing the Tutorial Prompt")]
        private bool _isShowingPrompt;
        */

        [SerializeField]
        [Tooltip("True if the player has completed the Tutorial Prompt")]
        private bool _isAchieved;

        public string Label
        {
            get => _label;
            set => _label = value;
        }
        public string Message
        {
            get => _message;
            set => _message = value;
        }
        public Collider StartTrigger
        {
            get => _startTrigger;
            set => _startTrigger = value;
        }
        public Collider EndTrigger
        {
            get => _endTrigger;
            set => _endTrigger = value;
        }

        /*public PlayerActions PlayerAction
        {
            get => _playerAction;
            set => _playerAction = value;
        }
        */
        public bool IsActive
        {
            get => _isActive;
            set => _isActive = value;
        }

        /*
        public bool IsShowingPrompt
        {
            get => _isShowingPrompt;
            set => _isShowingPrompt = value;
        }
        */
        public bool IsAchieved
        {
            get => _isAchieved;
            set => _isAchieved = value;
        }

        public Prompt(string label, string message, Collider startTrigger, Collider endTrigger, /*PlayerActions playerAction,*/ bool active, /*bool isShowingPrompt,*/ bool achieved)
        {
            this._label = label;
            this._message = message;
            this._startTrigger = startTrigger;
            this._endTrigger = endTrigger;
            //this._playerAction = playerAction;
            this._isActive = active;
            //this._isShowingPrompt = isShowingPrompt;
            this._isAchieved = achieved;
        }
    }

    [Header("Tutorial Prompts")]
    [SerializeField]
    private Text _textTutorialPrompt;
    public Text TextTutorialPrompt
    {
        get => _textTutorialPrompt;
        set
        {
            _textTutorialPrompt = value;
            _textTutorialPrompt.text = QueueTutorialPrompts.Peek().Label;
        }
    }

    [SerializeField]
    private Image _imageTutorialPrompt;
    public Image ImageTutorialPrompt
    {
        get => _imageTutorialPrompt;
        set
        {
            _imageTutorialPrompt = value;
        }
    }


    [SerializeField]
    private List<Prompt> _listTutorialPrompts;
    public List<Prompt> ListTutorialPrompts
    {
        get => _listTutorialPrompts;
        set => _listTutorialPrompts = value;
    }

    private Queue<Prompt> _queueTutorialPrompts;
    public Queue<Prompt> QueueTutorialPrompts
    {
        get => _queueTutorialPrompts;
        set => _queueTutorialPrompts = value;
    }
    private Prompt _currentPrompt;
    public Prompt CurrentPrompt
    {
        get => _currentPrompt;
        set => _currentPrompt = value;
    }
    private InputAction _currentAction;
    public InputAction CurrentAction
    {
        get => _currentAction;
        set => _currentAction = value;
    }

    /*
    private Dictionary<PlayerActions, InputAction> _dictPlayerActions;
    public Dictionary<PlayerActions, InputAction> DictPlayerActions
    {
        get => _dictPlayerActions;
        set => _dictPlayerActions = value;
    }
    */
    public bool IsCompleted;

    [Header("References")]
    public PlayerInput PlayerInputs;

    private void Awake()
    {
        //Initalise collections
        //_listTutorialPrompts = new List<Prompt>();
        _queueTutorialPrompts = new Queue<Prompt>();
        //_dictPlayerActions = new Dictionary<PlayerActions, InputAction>();
    }

    // Start is called before the first frame update
    void Start()
    {
        /*
        if (PlayerInputs.currentActionMap.name == ("Player"))
        {
            _dictPlayerActions.Add(PlayerActions.Interact, PlayerInputs.currentActionMap.FindAction("Interact"));
            _dictPlayerActions.Add(PlayerActions.Jump, PlayerInputs.currentActionMap.FindAction("Jump"));
            _dictPlayerActions.Add(PlayerActions.Look, PlayerInputs.currentActionMap.FindAction("Look"));
            _dictPlayerActions.Add(PlayerActions.Movement, PlayerInputs.currentActionMap.FindAction("Movement"));
            _dictPlayerActions.Add(PlayerActions.Shoot, PlayerInputs.currentActionMap.FindAction("Shoot"));
            _dictPlayerActions.Add(PlayerActions.SwapBeam, PlayerInputs.currentActionMap.FindAction("Swap Beam"));
            _dictPlayerActions.Add(PlayerActions.Walk, PlayerInputs.currentActionMap.FindAction("Walk"));
        }
        */
        //Updates the starting queue
        ListToQueueTransfer(_listTutorialPrompts, _queueTutorialPrompts); 

        //Gets the first Prompt object
        _currentPrompt = _queueTutorialPrompts.Peek();
    }

    private void FixedUpdate()
    {
        if (!IsCompleted)
        {
            //Checks if queue isn't empty
            if (_queueTutorialPrompts.Count != 0)
            {
                //Toggles Tutorial Prompt Text based on the bool IsActive
                _textTutorialPrompt.enabled = _currentPrompt.IsActive;
                _imageTutorialPrompt.enabled = _currentPrompt.IsActive;
                
                //When the Prompt object has outlived its usefulness
                if (_currentPrompt.IsAchieved)
                {                  
                    _listTutorialPrompts.RemoveAt(0); //Removes said Prompt object from the list                   
                    ListToQueueTransfer(_listTutorialPrompts, _queueTutorialPrompts); //Updates the Queue

                    //
                    if (_queueTutorialPrompts.Count != 0)
                    {
                        _currentPrompt = _queueTutorialPrompts.Peek();
                    }
                    else
                    {
                        IsCompleted = true;
                    }
                }
            }
            else
            {
                IsCompleted = true;
            }

            if (_listTutorialPrompts.Count != 0)
            {
                _listTutorialPrompts[0] = _currentPrompt;
                ListToQueueTransfer(_listTutorialPrompts, _queueTutorialPrompts);
            }
        }
        else
        {
            this.enabled = false;
        }
    }

    // OnTrigger code
    private void OnTriggerEnter(Collider other)
    {
        //If all the Prompt objects has not been set to IsAchieved = true
        if (!IsCompleted)
        {
            //Sets the Prompt object IsActive to true when touches the StartTrigger
            if (other == _currentPrompt.StartTrigger)
            {
                _currentPrompt.IsActive = true;
                _textTutorialPrompt.text = _currentPrompt.Message;
            }

            //Sets the Prompt object Ischieved to false when touches the EndTrigger
            if (other == _currentPrompt.EndTrigger)
            {
                _currentPrompt.IsActive = false;
                _currentPrompt.IsAchieved = true;
            }

            _listTutorialPrompts[0] = _currentPrompt;
            ListToQueueTransfer(_listTutorialPrompts, _queueTutorialPrompts);
        }   
    }

    /// <summary>
    /// Transfers the Prompts from the List object into a Queue object. Done to bypass queue limitatiuons but keep inspector visibility. 
    /// </summary>
    /// <param name="list"></param>
    /// <param name="queue"></param>
    public void ListToQueueTransfer(List<Prompt> list, Queue<Prompt> queue)
    {
        queue.Clear();

        foreach (Prompt tp in list)
        {
            queue.Enqueue(tp);
        }          
    }         
}
