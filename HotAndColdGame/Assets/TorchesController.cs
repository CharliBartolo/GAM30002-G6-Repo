using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TorchesController : MonoBehaviour
{
    private CollectionSystem collections = null;

    [Header("Public Acticvators")]
    public bool[] LevelsComplete;

    [Header("Objects To Activate")]
    public GameObject[] Level1;
    public GameObject[] Level2;
    public GameObject[] Level3;
    public GameObject[] Level4;
    public GameObject[] Level5;
    public GameObject[] Level6;

    public GameObject[] OnAllCollected;

    public GameObject[] Extra;

    List<bool> results;


    // Start is called before the first frame update
    void Start()
    {
        if (GameMaster.instance != null && GameMaster.instance.GetComponent<CollectionSystem>() != null)
        {
            collections = GameMaster.instance.GetComponent<CollectionSystem>();
        }

       /* if(collections != null)
        {
            results = collections.Results();
            Debug.Log("RESULTS COUNT: " + results.Count);
            foreach (var item in results)
            {
                Debug.Log("result: " + item.ToString());
            }
        }*/


    }

    // Update is called once per frame
    void Update()
    {
        if (collections != null)
        {
            results = collections.Results();
            for (int i = 0; i < LevelsComplete.Length; i++)
            {
                LevelsComplete[i] = results[i];
            }

            EnableObjects(Level1, LevelsComplete[0]);
            EnableObjects(Level2, LevelsComplete[1]);
            EnableObjects(Level3, LevelsComplete[2]);
            EnableObjects(Level4, LevelsComplete[3]);
            EnableObjects(Level5, LevelsComplete[4]);
            EnableObjects(Level6, LevelsComplete[5]);
        }
    }

    public void EnableObjects(GameObject[] objects, bool option)
    {
        foreach (var item in objects)
        {
            item.SetActive(option);
        }
    }


}
