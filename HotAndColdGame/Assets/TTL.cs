using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTL : MonoBehaviour
{
    public float TimeToLive = 1;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroObject(TimeToLive));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator DestroObject(float ttl)
    {
        yield return new WaitForSeconds(ttl);

        Destroy(gameObject);
    }
}
