using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrateTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //컨트롤러에 진동 일으키기

        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            //기본진동
            //OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.LTouch);

            //짧은 진동 일으키기 (코루틴으로 빼서 만들었다)
            StartCoroutine(Vibrate(.5f));
            StopCoroutine(Vibrate(5));
        }
    }

    IEnumerator Vibrate(float sec)
    {
        
        OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.LTouch);
        yield return new WaitForSeconds(sec);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
    }
}
