using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Laser : MonoBehaviour
{
    //public GameObject key;
    //public GameObject key2;
    //public GameObject MalletL;
    //public GameObject MalletR;

    // Ŭ�� ���η����� ���

    public GameObject indi;
    RaycastHit hit; // �浹�� ��ü
    GameObject currentObject;   // ���� �ֱٿ� �浹�� ��ü�� �����ϱ� ���� ��ü

    public float raycastDistance = 100f; // ������ ������ ���� �Ÿ�

    LineRenderer lr;
    //�װ��� ��Ÿ���� �̹���
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        lr.SetPosition(0, transform.position);


        // Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.green, 0.5f);

        // �浹 ���� ��
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance))
        {

            lr.SetPosition(1, hit.point);
            print("���� ������Ʈ �̸�: " + hit.transform.gameObject.name);

            // �浹 ��ü�� �±װ� Button�� ���
            if (hit.collider.gameObject.CompareTag("Button"))

            {
                indi.gameObject.SetActive(true);
                indi.transform.position = hit.point;
                // ��ŧ���� �� �����ܿ� Ʈ���� �κ��� ���� ���
                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) ||
                    (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch)))
                {
                    // ��ư�� ��ϵ� onClick �޼ҵ带 �����Ѵ�.
                    hit.collider.gameObject.GetComponent<Button>().onClick.Invoke();
                }
                else
                {
                    hit.collider.gameObject.GetComponent<Button>().OnPointerEnter(null);
                    currentObject = hit.collider.gameObject;
                }
            }
            else
            {
                indi.gameObject.SetActive(false);
            }
        }

        else
        {
            lr.SetPosition(1, transform.forward * raycastDistance);
            indi.gameObject.SetActive(false);
            // �ֱ� ������ ������Ʈ�� Button�� ���
            // ��ư�� ���� �����ִ� �����̹Ƿ� �̰��� Ǯ���ش�.
            if (currentObject != null)
            {
                currentObject.GetComponent<Button>().OnPointerExit(null);
                currentObject = null;
            }
        }
    }


    //if (OVRInput.GetDown(OVRInput.Button.Start, OVRInput.Controller.LTouch))
    //{
    //    //a = !a;
    //    key.SetActive(!a);
    //    MalletL.SetActive(!a);
    //    MalletR.SetActive(!a);
    //    //lr.enabled = !a;
    //    key2.SetActive(!a);
    //}
}



