using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Fadeinout : MonoBehaviour
{

    //public static Manager instance;

    public GameObject loading;
    public Image loadingBg;

    void Update()
    {
        Color bgColor = loadingBg.color;
        bgColor.a += Time.deltaTime * -0.5f;
        loadingBg.color = bgColor;
        if (bgColor.a <= 0)
        {
            loading.SetActive(false);
        }
    }
}
