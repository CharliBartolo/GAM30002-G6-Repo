using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTH : MonoBehaviour
{
    public float TimeToHide = 1;

    private void OnEnable()
    {
        StartCoroutine(HideObject(TimeToHide));
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator HideObject(float tth)
    {
        yield return new WaitForSeconds(tth);

        gameObject.SetActive(false);
    }
}
