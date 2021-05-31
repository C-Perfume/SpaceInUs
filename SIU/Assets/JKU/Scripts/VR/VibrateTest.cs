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
        //��Ʈ�ѷ��� ���� ����Ű��

        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            //�⺻����
            //OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.LTouch);

            //ª�� ���� ����Ű�� (�ڷ�ƾ���� ���� �������)
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
