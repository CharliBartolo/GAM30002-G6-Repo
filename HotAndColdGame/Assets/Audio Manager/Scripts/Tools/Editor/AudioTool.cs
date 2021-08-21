using UnityEditor;
using UnityEngine;

public class AudioTool : EditorWindow
{
    [MenuItem("Tools/Audio Tool")]
    public static void ShowWindow()
    {
        GetWindow(typeof(AudioTool));
    }

    public GameObject audio_manager;

    private void OnGUI()
    {
        GUILayout.Label("Press 'Play' to update list.", EditorStyles.boldLabel);
        audio_manager = EditorGUILayout.ObjectField("AudioManager:", audio_manager, typeof(GameObject), true) as GameObject;
        ScriptableObject target = audio_manager.GetComponent<AudioManager>().list;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty stringsProperty = so.FindProperty("sounds");

        EditorGUILayout.PropertyField(stringsProperty, true); // True means show children
        so.ApplyModifiedProperties(); // Remember to apply modified properties
    }
}
