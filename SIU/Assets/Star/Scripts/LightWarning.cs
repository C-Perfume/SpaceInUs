using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightWarning : MonoBehaviour
{
    Light plight;
    int num = 1;
    float curT = 0;
    float last30 = 30;
    void Start()
    {
        plight = GetComponent<Light>();
        plight.range = 0;
    }

    void Update()
    {
        curT += Time.deltaTime;
        if(curT > last30)
        {
        plight.range += num * 0.1f;
        if (plight.range >= 7 || plight.range <= 0) { num *= -1; }


        }
    }
}
