//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//public class UItest : MonoBehaviour
//{
////Camera Rig 에 달아놓으면 됨 스크립트
///
//    //오른손 위치
//    public Transform rHand;

//    //이미지 받아오기
//    public Transform dot;


//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {
//        //1. Ray를 만든다.(오른손 위치, 오른손 앞방향)
//        Ray ray = new Ray(rHand.position, rHand.forward);
//        //2. 부딪혔다면
//        RaycastHit hit;

//        if (Physics.Raycast(ray, out hit, 100))
//        {
//            //만약에 부딪힌 놈의 layer가 UI라면
//            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("UI"))
//            {

//                //3. 그 위치에 빨간점을 위치시키자
//                dot.gameObject.SetActive(true);
//                dot.position = hit.point;

//            }
//            else
//            {
//                dot.gameObject.SetActive(false);

//            }
//        }
//        else
//        {
//            //4. 빨간 점 안보이게
//            dot.gameObject.SetActive(false);
//        }

//        //만약에 점이 활성화상태면
//        if (dot.gameObject.activeSelf == true)
//        {
//            if(OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
//            {
//                //버튼 스크립트 가져오기
//               Button btn = hit.transform.GetComponent<Button>();
            
//                //만약 버튼이 null이 아니면
//                if(btn != null)
//                {
//                    btn.onClick.Invoke();
//                }
//            }
//        }
//    }
//}
