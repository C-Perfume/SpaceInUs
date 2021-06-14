using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Fade : MonoBehaviour
{
    Image im;
    Color color;
    private void Start()
    {
        color = new Color(im.color.r, im.color.g, im.color.b, im.color.a);
        StartCoroutine(Co_ChangeAlpha());
    }
    private void Update()
    {
        //  color.a += Time.deltaTime * 0.5f;
    }


    IEnumerator Co_ChangeAlpha()
    {
        while (true)
        {
            color.a = Mathf.PingPong(Time.time, 1f);
            im.color = color;
            yield return null;
        }
    }
}
