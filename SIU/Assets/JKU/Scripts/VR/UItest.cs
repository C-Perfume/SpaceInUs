//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//public class UItest : MonoBehaviour
//{
////Camera Rig �� �޾Ƴ����� �� ��ũ��Ʈ
///
//    //������ ��ġ
//    public Transform rHand;

//    //�̹��� �޾ƿ���
//    public Transform dot;


//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {
//        //1. Ray�� �����.(������ ��ġ, ������ �չ���)
//        Ray ray = new Ray(rHand.position, rHand.forward);
//        //2. �ε����ٸ�
//        RaycastHit hit;

//        if (Physics.Raycast(ray, out hit, 100))
//        {
//            //���࿡ �ε��� ���� layer�� UI���
//            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("UI"))
//            {

//                //3. �� ��ġ�� �������� ��ġ��Ű��
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
//            //4. ���� �� �Ⱥ��̰�
//            dot.gameObject.SetActive(false);
//        }

//        //���࿡ ���� Ȱ��ȭ���¸�
//        if (dot.gameObject.activeSelf == true)
//        {
//            if(OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
//            {
//                //��ư ��ũ��Ʈ ��������
//               Button btn = hit.transform.GetComponent<Button>();
            
//                //���� ��ư�� null�� �ƴϸ�
//                if(btn != null)
//                {
//                    btn.onClick.Invoke();
//                }
//            }
//        }
//    }
//}
