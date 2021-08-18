using UnityEditor;
using UnityEngine;

public enum journalType { Human, Alien }; //Types of Journals
public class JournalTool : EditorWindow
{
    public string jounralLog = "Enter your Journal log: ";

    GameObject spawnobj;
    GameObject HumanJournal;
    GameObject AlienJournal;
    GameObject SpawnPosition;
    public journalType jt;

    [MenuItem("Tools/Journal Tool")]
    public static void ShowWindow()
    {
        GetWindow(typeof(JournalTool));
    }

    private void OnGUI()
    {
        GUILayout.Label("Craete Journal", EditorStyles.boldLabel);
        jt = (journalType)EditorGUILayout.EnumPopup("Journal Type:", jt);
        GUILayout.Label("Journal Entry:", EditorStyles.helpBox);
        jounralLog = jounralLog = EditorGUILayout.TextArea(jounralLog);
        HumanJournal = EditorGUILayout.ObjectField("Human Journal", HumanJournal, typeof(GameObject), false) as GameObject;
        AlienJournal = EditorGUILayout.ObjectField("Alien Journal", AlienJournal, typeof(GameObject), false) as GameObject;
        GUILayout.Label("Scene Transform Position: (Drag an empty gameobject of the desired postition)", EditorStyles.helpBox);
        SpawnPosition = EditorGUILayout.ObjectField("Spawn Position", SpawnPosition, typeof(GameObject), true) as GameObject;

        if (GUILayout.Button("Create Journal"))
        {
            CreateJournal();
        }
    }

    private void CreateJournal()
    {

        if (jt == journalType.Human)
        {
            spawnobj = HumanJournal;
        }
        if (jt == journalType.Alien)
        {
            spawnobj = AlienJournal;
        }

        if(spawnobj == null)
        {
            Debug.LogError("Error: No Object to be spawned.");
            return;
        }
        if (jounralLog == string.Empty)
        {
            Debug.LogError("Error: No journal entry has been added.");
            return;
        }
        if (SpawnPosition == null)
        {
            Debug.LogError("Error: No spawn position has been specified.");
            return;
        }
        if (spawnobj.GetComponent<Journal>() == null)
        {
            Debug.LogError("Error: Object has no journal component.");
            return;
        }

        Vector3 spawnpos = new Vector3(SpawnPosition.transform.position.x, SpawnPosition.transform.position.y, SpawnPosition.transform.position.z);
        DestroyImmediate(SpawnPosition);
        GameObject newJournal = Instantiate(spawnobj, spawnpos, Quaternion.identity);
        newJournal.name = spawnobj.name;
        newJournal.GetComponent<Journal>().EntryLog = jounralLog;
    }
}
