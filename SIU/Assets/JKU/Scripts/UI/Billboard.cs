using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Billboard : MonoBehaviour
{

    void Update()
    {//1. Ray를 만든다(카메라위치, 카메라 앞방향)
        Ray ray = new Ray(
            Camera.main.transform.position,
            Camera.main.transform.forward);
        //2. 부딪힌 지점에 캔버스를 위치시킨다
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            //위치시킨다.
            transform.position = hit.point;
            //크기조절
            float dist = Vector3.Distance(
                Camera.main.transform.position,
                hit.point);
            transform.localScale = Vector3.one * dist;
        }
    }
}
