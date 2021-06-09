using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookMovement : MonoBehaviour
{
    float currT;
    // ��ũ �̵��ð� �� �Ÿ� 1.5m��ŭ Ƣ�����.
    float showT = 1.5f;
    public bool moving = true;

     Transform rockMap;
    void Start()
    {
        rockMap = GameObject.Find("Rock_map").transform;
        Destroy(gameObject, 6f);
    }

    void Update()
    {
        if (moving)
        {
            currT += Time.deltaTime;
            if (currT <= showT)
            {
                transform.position += transform.forward * 3 * Time.deltaTime;
            }
            else {
                transform.position -= transform.forward * 1.5f * Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root == rockMap) {
            print("hook stop ?");
            moving = false;
        }
    }
}
