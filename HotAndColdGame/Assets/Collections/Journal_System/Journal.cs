using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A class dedicated to storing journal entries for use by Journal_Reader.cs
/// Last edit: Expanded _entryLog to be a list of strings titled _entryLogs.
/// By: Charli - 23/9/21
/// </summary>
public class Journal : MonoBehaviour
{
    [SerializeField]
    [TextArea(15, 20)]
    private List<string> _entryLogs;

    public List<string> EntryLog
    {
        set
        {
            _entryLogs = value;
        }

        get
        {
            return _entryLogs;
        }
    }
}
