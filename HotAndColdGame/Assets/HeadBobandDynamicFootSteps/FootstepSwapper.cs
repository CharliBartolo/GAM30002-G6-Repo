using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FootstepSwapper : MonoBehaviour
{
    private TerrainChecker checker;
    private PlayerSoundControl sc;
    [SerializeField] private string currentLayer;
    [SerializeField] private CollectionType[] collectionTypes;

    // Start is called before the first frame update
    void Start()
    {
        checker = GetComponent <TerrainChecker>();
        sc = GetComponent<PlayerSoundControl>();
    }

    private void Update()
    {
        
    }

    // Update is called once per frame
    public void CheckSurface()
    {
       currentLayer = checker.GetCurrentSurface();
       Debug.Log(currentLayer);

        foreach (CollectionType collection in collectionTypes)
        {
            foreach (string surface in collection.surfaces)
            {
                Debug.Log(surface);
                if(currentLayer == surface)
                {
                    sc.SwapFootsteps(collection.collection);
                }
            }
        }
    }
}

[Serializable]
public class CollectionType
{
    [SerializeField] public FootstepCollection collection;
    [SerializeField] public string[] surfaces;
}