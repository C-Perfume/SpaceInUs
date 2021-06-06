using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class UItest : MonoBehaviour
{
    public Transform dot;
    RaycastHit hit; // 충돌된 객체
    GameObject currentObject;   // 가장 최근에 충돌한 객체를 저장하기 위한 객체

    public float raycastDistance = 100f; // 레이저 포인터 감지 거리

    LineRenderer lr;
    //죽고나서 나타나는 이미지
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

        // 충돌 감지 시
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance))
        {
           
            lr.SetPosition(1, hit.point);
            print("맞춘 오브젝트 이름: " + hit.transform.gameObject.name);

            // 충돌 객체의 태그가 Button인 경우
            if (hit.collider.gameObject.CompareTag("Button"))

            {
                dot.gameObject.SetActive(true);
                dot.position = hit.point;
                // 오큘러스 고 리모콘에 트리거 부분을 누를 경우
                if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch)||
                    (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch)))
                {
                    // 버튼에 등록된 onClick 메소드를 실행한다.
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
            // 최근 감지된 오브젝트가 Button인 경우
            // 버튼은 현재 눌려있는 상태이므로 이것을 풀어준다.
            if (currentObject != null)
            {
                currentObject.GetComponent<Button>().OnPointerExit(null);
                currentObject = null;
            }
        }
    }
}