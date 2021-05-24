using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedObject : MonoBehaviour
{
    [SerializeField]
    private float _weight = 0.0f;
    [SerializeField]
    private Collider _collider;
    [SerializeField]
    private bool _touching;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public float Weight
    {
        get => _weight;
        set => _weight = value;
    }

    public Collider Collider
    {
        get => _collider;
    }

    
    public bool Touching
    {
        get => _touching;
        set => _touching = value;
        
    }
}
