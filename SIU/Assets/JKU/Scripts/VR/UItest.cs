using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class UItest : MonoBehaviour
{
    public Transform dot;
    RaycastHit hit; // �浹�� ��ü
    GameObject currentObject;   // ���� �ֱٿ� �浹�� ��ü�� �����ϱ� ���� ��ü

    public float raycastDistance = 100f; // ������ ������ ���� �Ÿ�

    LineRenderer lr;
    //�װ��� ��Ÿ���� �̹���
    public GameObject deadinfo;
     void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        lr.SetPosition(0, transform.position);

        if (SceneManager.GetActiveScene().name == "ExploScene"
            || SceneManager.GetActiveScene().name == "Meteo"
            || SceneManager.GetActiveScene().name == "LostSpace"
            )
        {
            if (deadinfo.activeSelf)
            {
                lr.enabled = true;
            }
        }

        // Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.green, 0.5f);

        // �浹 ���� ��
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance))
        {
           
            lr.SetPosition(1, hit.point);
            print("���� ������Ʈ �̸�: " + hit.transform.gameObject.name);

            // �浹 ��ü�� �±װ� Button�� ���
            if (hit.collider.gameObject.CompareTag("Button"))

            {
                dot.gameObject.SetActive(true);
                dot.position = hit.point;
                // ��ŧ���� �� �����ܿ� Ʈ���� �κ��� ���� ���
                if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch)||
                    (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch)))
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
                dot.gameObject.SetActive(false);
            }
        }

        else
        {
            lr.SetPosition(1, transform.forward * raycastDistance);
            dot.gameObject.SetActive(false);
            // �ֱ� ������ ������Ʈ�� Button�� ���
            // ��ư�� ���� �����ִ� �����̹Ƿ� �̰��� Ǯ���ش�.
            if (currentObject != null)
            {
                currentObject.GetComponent<Button>().OnPointerExit(null);
                currentObject = null;
            }
        }
    }
}