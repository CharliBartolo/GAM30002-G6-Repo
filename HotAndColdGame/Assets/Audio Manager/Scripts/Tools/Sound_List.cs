using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ShowOnlyAttribute : PropertyAttribute
{
}

 [CustomPropertyDrawer(typeof(ShowOnlyAttribute))]
public class ShowOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        string valueStr;

        switch (prop.propertyType)
        {
            case SerializedPropertyType.Integer:
                valueStr = prop.intValue.ToString();
                break;
            case SerializedPropertyType.Boolean:
                valueStr = prop.boolValue.ToString();
                break;
            case SerializedPropertyType.Float:
                valueStr = prop.floatValue.ToString("0.00000");
                break;
            case SerializedPropertyType.String:
                valueStr = prop.stringValue;
                break;
            default:
                valueStr = "(not supported)";
                break;
        }

        EditorGUI.LabelField(position, label.text, valueStr);
    }
}

[CreateAssetMenu(fileName = "SoundList", menuName = "AudioManager/SoundList", order = 1)]
public class Sound_List : ScriptableObject
{
    [ShowOnly]
    public List<string> sounds = new List<string>();   
}


