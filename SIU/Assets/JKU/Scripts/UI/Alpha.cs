using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alpha : MonoBehaviour
{
    public Text text;
    Color color;

    private void Start()
    {
        color = new Color(text.color.r, text.color.g, text.color.b, text.color.a);
        StartCoroutine(Co_ChangeAlpha());
    }

    IEnumerator Co_ChangeAlpha()
    {
        while (true)
        {
            color.a = Mathf.PingPong(Time.time, 1f);
            text.color = color;
            yield return null;
        }
    }
}