using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// This script is responsible for creating and managing tutorial prompts placed in the scene.
/// Note: Must be placed on the Player object!!!
/// Recent change: Added a direction condition to the triggers. Needs the player to look at a cirtain direction to activate prompts.
/// Last edited by Charadey 30/9/2001
/// </summary>
public class TutorialPrompt : MonoBehaviour 
{
    /*
    public enum PlayerAction
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

    public enum InputType
    { 
        Keyboard,
        Xbox,
        PS,
        NULL
    };

    //Struct that defines the Tutorial Prompts
    [System.Serializable]
    public struct Prompt
    {
        //Fields
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
        private PlayerFPControls _playerAction;
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

        //Vision additions
        [SerializeField]
        [Tooltip("If true, the must look in a certain direction to activate the Tutorial Prompt. (Mainly for debug)")]
        private bool _needsVision;

        [SerializeField]
        [Tooltip("The minimum angle from the expected direction. -1 is behind, 0 is perpendicular and 1 is same direction")]
        [Range (-1, 1)]
        private float _visionWindow;

        [SerializeField]
        [Tooltip("A more specific point where you want the player to look at")]
        private Collider _pointOfInterestTrigger;

        //[SerializeField]
        //[Tooltip("Vector 3 for looking into the correct direction.")]
        private Vector3 _expectedDirection;

        //Button Icons Addition
        [SerializeField]
        [Tooltip("List of possible button icons for the prompt")]
        private List<ButtonIcon> _buttonIcons;

        //Constructor
        public Prompt(string label, string message, Collider startTrigger, Collider endTrigger, /*PlayerFPControls playerAction,*/ bool active, /*bool isShowingPrompt,*/ bool achieved, bool vision, float angle, Collider poi, Vector3 expectedDirection)
        {
            this._label = label;
            this._message = message;
            this._startTrigger = startTrigger;
            this._endTrigger = endTrigger;
            //this._playerAction = playerAction;
            this._isActive = active;
            //this._isShowingPrompt = isShowingPrompt;
            this._isAchieved = achieved;

            

            //Vision Addition
            this._needsVision = vision;

            if (vision)
            {
                this._visionWindow = angle;
                this._expectedDirection = expectedDirection;
                this._pointOfInterestTrigger = poi;
            }
            else
            {
                this._visionWindow = 0f;
                this._expectedDirection = new Vector3();
                this._pointOfInterestTrigger = null;
            }

            //Button Icon Additions
            this._buttonIcons = new List<ButtonIcon>();
        }

        //Properties
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

        /*
        public PlayerFPControls PlayerAction
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

        //Vision Additions
        public bool NeedsVision
        {
            get => _needsVision;
            set => _needsVision = value;
        }       

        public float VisionWindow
        {
            get => _visionWindow;
            set => _visionWindow = value;
        }

        public Collider PointOfInterestTrigger
        {
            get => _pointOfInterestTrigger;
            set => _pointOfInterestTrigger = value;
        } 

        public Vector3 ExpectedDirection
        {
            get => _expectedDirection;
            set => _expectedDirection = value;
        }
        
        //ButtonIcons
        public List<ButtonIcon> ButtonIcons
        {
            get => _buttonIcons;
            set => _buttonIcons = value;
        }
    }

    [System.Serializable]
    public struct ButtonIcon
    {
        //Fields
        [SerializeField]
        [Tooltip("Label for the Tutorial Prompt")]
        private string _label;

        [SerializeField]
        [Tooltip("The specific controls for the sprite")]
        private InputType _input;

        [SerializeField]
        [Tooltip("Text for Button Icon prompt")]
        private string _prompt;

        [SerializeField]
        [Tooltip("Sprite for specific control")]
        private Sprite _sprite;

        //Constructor
        public ButtonIcon(string label, InputType type, Sprite sprite, string prompt)
        {
            this._label = label;
            this._input = type;
            this._prompt = prompt;
            this._sprite = sprite;
        }

        //Properties
        public string Label
        {
            get => _label;
            set => _label = value;
        }

        public InputType Input
        {
            get => _input;
            set => _input = value;
        }

        public string Prompt
        {
            get => _prompt;
            set => _prompt = value;
        }

        public Sprite Sprite
        {
            get => _sprite;
            set => _sprite = value;

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
    private Text _textButtonIcon;
    public Text TextButtonIcon
    {
        get => _textButtonIcon;
        set
        {
            _textButtonIcon = value;          
        }
    }

    [SerializeField]
    private Image _imageButtonIcon;
    public Image ImageButtonIcon
    {
        get => _imageButtonIcon;
        set
        {
            _imageButtonIcon = value;
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

    /*
    private InputAction _currentAction;
    public InputAction CurrentAction
    {
        get => _currentAction;
        set => _currentAction = value;
    }
    */

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
    public GameObject Player;
    //public PlayerInput PlayerInputs;

    private void Awake()
    {
        //Player object
        if (!Player)
        {
            Player = GameObject.Find("Player");
        }

        /*
        //Player Inputs
        if (Player != null && !PlayerInputs)
        {
            PlayerInputs = Player.GetComponent<PlayerInput>();
        }
        */

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

        _textTutorialPrompt.enabled = _currentPrompt.IsActive;
        _imageTutorialPrompt.enabled = _currentPrompt.IsActive;
        _imageButtonIcon.enabled = _currentPrompt.IsActive;
        _textButtonIcon.enabled = _currentPrompt.IsActive;
    }

    private void FixedUpdate()
    {
        //Checks if all Tutorial Prompts have been completed by the player
        if (!IsCompleted)
        {
            //Checks if queue isn't empty
            if (_queueTutorialPrompts.Count != 0)
            {
                //Checks if player needs to look at a certain direction.
                if (_currentPrompt.NeedsVision)
                {
                    //Finds and normalizes the needed vectors.
                    var direction = Vector3.Normalize(Player.transform.TransformDirection(Vector3.forward));    //Players looking direction
                    var expectedDirection = Vector3.Normalize(_currentPrompt.PointOfInterestTrigger.transform.position - Player.transform.position);  //Vector from startTrigger to player
                    var dotProduct = Vector3.Dot(_currentPrompt.ExpectedDirection, direction);  //Dot product of above vectors

                    //Debug                 
                    //Debug.Log("Player Vector3: " + direction);
                    //Debug.Log("Dot product: " + dotProduct + "/n Vision Window: " + CurrentPrompt.VisionWindow); 

                    _currentPrompt.ExpectedDirection = expectedDirection;

                    //Tests if the player is looking at the expected direction within the acceptable angle.
                    if (dotProduct >= _currentPrompt.VisionWindow && _currentPrompt.IsActive)
                    {
                        //Toggles Tutorial Prompt Text based on the bool IsActive
                        _textTutorialPrompt.text = _currentPrompt.Message;
                        _textTutorialPrompt.enabled = _currentPrompt.IsActive;
                        _imageTutorialPrompt.enabled = _currentPrompt.IsActive;

                        //Button Icons Addittion
                        foreach (ButtonIcon bc in _currentPrompt.ButtonIcons)
                        {
                            if (bc.Input == DetermineInput())
                            {
                                _imageButtonIcon.sprite = bc.Sprite;
                                _textButtonIcon.text = bc.Prompt;

                                //Debug
                                //Debug.Log("Current input: " + DetermineInput());
                                //Debug.Log("Current sprite: " + bc.Sprite);
                            }
                        }
                        _imageButtonIcon.enabled = _currentPrompt.IsActive;
                        _textButtonIcon.enabled = _currentPrompt.IsActive;
                    }
                }
                else
                {
                    _textTutorialPrompt.text = _currentPrompt.Message;
                    _textTutorialPrompt.enabled = _currentPrompt.IsActive;
                    _imageTutorialPrompt.enabled = _currentPrompt.IsActive;
                    foreach (ButtonIcon bc in _currentPrompt.ButtonIcons)
                    {
                        if (bc.Input == DetermineInput())
                        {
                            _imageButtonIcon.sprite = bc.Sprite;
                            _textButtonIcon.text = bc.Prompt;

                            //Debug
                            Debug.Log("Current input: " + DetermineInput());
                            Debug.Log("Current sprite: " + bc.Sprite);
                        }
                    }
                    _imageButtonIcon.enabled = _currentPrompt.IsActive;
                    _textButtonIcon.enabled = _currentPrompt.IsActive;
                }
                

                //When the Prompt object has outlived its usefulness it returns nothingness
                if (_currentPrompt.IsAchieved)
                {
                    //Turn off text and image
                    _textTutorialPrompt.enabled = _currentPrompt.IsActive;
                    _imageTutorialPrompt.enabled = _currentPrompt.IsActive;

                    _listTutorialPrompts.RemoveAt(0);   //Removes said Prompt object from the list                   
                    ListToQueueTransfer(_listTutorialPrompts, _queueTutorialPrompts);   //Updates the Queue

                    //Updates the _currentPrompt to the first Prompt in queue
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

        //DEBUG
        /*
        if (true)
        {
            Debug.DrawRay(_currentPrompt.PointOfInterestTrigger.transform.position, Player.transform.position, Color.red);
            Debug.DrawRay(Player.transform.position, Player.transform.forward, Color.green);
            Debug.Log(Player.GetComponent<PlayerInput>().currentControlScheme);
        }
        */


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

            //Sets the Prompt object IsAchieved to false when touches the EndTrigger
            if (other == _currentPrompt.EndTrigger && _currentPrompt.IsActive)
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

    public InputType DetermineInput()
    {
        //(Player.GetComponent<PlayerInput>().currentControlScheme == "Keyboard and Mouse")
        if (Player.GetComponent<PlayerInput>().currentControlScheme == "Keyboard And Mouse")
        {
            return InputType.Keyboard;
        }
        else
        {
            if (Gamepad.current is UnityEngine.InputSystem.XInput.XInputController)
            {
                return InputType.Xbox;
            }
            else if (UnityEngine.InputSystem.Gamepad.current is UnityEngine.InputSystem.DualShock.DualShockGamepad)
            {
                return InputType.PS;
            }
        }

        Debug.LogWarning("UH-OH No devices connected (How does that even happen?)");
        return InputType.NULL;
    }
}
