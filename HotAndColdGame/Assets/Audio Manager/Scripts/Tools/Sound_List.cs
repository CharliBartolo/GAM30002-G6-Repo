using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "SoundList", menuName = "AudioManager/SoundList", order = 1)]
public class Sound_List : ScriptableObject
{
    //[ShowOnly]
    public List<string> sounds = new List<string>();   
}


