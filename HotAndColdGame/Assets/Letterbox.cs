using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Letterbox : MonoBehaviour
{
    public Image[] boxes;

    // Start is called before the first frame update
    void Start()
    {
        boxes = GetComponentsInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // fade in
    public void FadeIn(float fadeDuration)
    {
        foreach (var item in boxes)
        {
            Color col = item.color;
            col.a = 1;
            item.color = col;
            item.CrossFadeAlpha(0, 0, false);
            item.CrossFadeAlpha(1, fadeDuration, true);
        }
    }

    // fade out
    public void FadeOut(float fadeDuration)
    {
        foreach (var item in boxes)
        {
            item.CrossFadeAlpha(0, fadeDuration, true);
        }
    }
}
