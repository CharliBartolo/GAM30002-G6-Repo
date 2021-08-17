using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journal : MonoBehaviour
{
    [SerializeField]
    [TextArea(15, 20)]
    private string _entryLog;

    public string EntryLog
    {
        set
        {
            _entryLog = value;
        }

        get
        {
            return _entryLog;
        }
    }
}
