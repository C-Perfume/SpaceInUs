using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedLight : MonoBehaviour
{
    public Light image;
    Color color;

    private void Start()
    {
        color = new Color(image.color.r, image.color.g, image.color.b, image.color.a);
        StartCoroutine(Co_ChangeAlpha());
    }

    IEnumerator Co_ChangeAlpha()
    {
        while (true)
        {
            color.r = Mathf.PingPong(Time.time, 1f);
            image.color = color;
            yield return null;
        }
    }
}