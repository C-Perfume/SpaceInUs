using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class PlayerM : MonoBehaviour
{
    // ī�޶󸮱׿� �ٴ� ��ũ��Ʈ��� ������ �޼� / ������ ������� �����ϱ�

    // ��Ʈ��ũ ��ġ�� ���ۿ�
    struct Sync
    {
        Vector3 pos;
        Quaternion rot;
    }

    public enum State
    {
        Ready,
        GameStart,
        GameOver,
        TowardsEnd,
        End
    }
    public enum Parts
    {
        Head,
        LHand,
        RHand,
        Body
    }

    public State state;
    public Transform[] my;
    public Transform[] others;

    //�ȱ� ���
    Vector3 origin;
    Vector3 pos;
    bool walkL = false;
    bool walkR = false;

    //��ǥ�� ������
    public GameObject doorIndi;
    public GameObject doorIndi2;

    // �ӷ�
    public bool floating = true;
    float vPower = .5f;

    //������ ����ġ
    Transform[] hitTF = new Transform[2];

    //�տ� ���� ������Ʈ�� Ʈ������
    Transform catchHoldL;
    Transform catchHoldR;
    Transform catchItem;

    // Ȧ��ã���
    public Transform rock;

    //������ ã���
    public Transform item;
    public Transform free;

    //������ ������
    public GameObject rope;
    public GameObject fire;
    public GameObject shield;
    public GameObject oxy;

    //ȹ������۸���Ʈ
    public List<GameObject> myItem = new List<GameObject>();

    //ī�޶� ������ ������ٵ� ��������
    Rigidbody rb;

    //������ ����
    TrapManager tM;

    RaycastHit hit;

    // open �Լ��� ���̴� �տ� ���� ������Ʈ
    GameObject hitObj;

    // Ŭ���� ����� ���� �� ��ġ����
    public Transform FootStepTransform;

    //��¼Ҹ�
    public AudioSource Grap;
    #region ��Ʈ�ѷ� bool Vector3����
    bool getTchTmbL;
    bool getDTchTmbL;
    bool getUTchTmbL;
    bool getTchTmbR;
    bool getDTchTmbR;
    bool getUTchTmbR;

    bool getBtnHandR;
    bool getBtnHandL;

    bool getBtnIdxL;
    bool getDBtnIdxL;
    bool getUBtnIdxL;
    bool getBtnIdxR;
    bool getDBtnIdxR;
    bool getUBtnIdxR;

    bool getDBtn1R;

    Vector3 getVelL;
    Vector3 getVelR;
    Vector3 getAngVelL;
    Vector3 getAngVelR;
    #endregion

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        { state = State.GameStart; }
        else if (SceneManager.GetActiveScene().name == "Ready")
        { state = State.Ready; }
        else
        { state = State.End; }

        tM = GetComponent<TrapManager>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        #region ��Ʈ�ѷ� bool
        getTchTmbL = OVRInput.Get(OVRInput.Touch.PrimaryThumbstick, OVRInput.Controller.LTouch);
        getDTchTmbL = OVRInput.GetDown(OVRInput.Touch.PrimaryThumbstick, OVRInput.Controller.LTouch);
        getUTchTmbL = OVRInput.GetUp(OVRInput.Touch.PrimaryThumbstick, OVRInput.Controller.LTouch);
        getTchTmbR = OVRInput.Get(OVRInput.Touch.PrimaryThumbstick, OVRInput.Controller.RTouch);
        getDTchTmbR = OVRInput.GetDown(OVRInput.Touch.PrimaryThumbstick, OVRInput.Controller.RTouch);
        getUTchTmbR = OVRInput.GetUp(OVRInput.Touch.PrimaryThumbstick, OVRInput.Controller.RTouch);

        getBtnHandR = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch);
        getBtnHandL = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch);

        getBtnIdxL = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
        getDBtnIdxL = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
        getUBtnIdxL = OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
        getBtnIdxR = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        getDBtnIdxR = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        getUBtnIdxR = OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);

        getDBtn1R = OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch);

        getVelL = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
        getVelR = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
        getAngVelL = OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.LTouch);
        getAngVelR = OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.RTouch);
        #endregion

        switch (state)
        {
            case State.Ready:
                if (SceneManager.GetActiveScene().name == "Ready") { Walk(); } else { Walk(0); }
                Rot();
                if (SceneManager.GetActiveScene().name == "Ready") { Open(0.1f, "Game"); }
                else { Open(0.1f, "Clear"); }

                // ǲ���ܿ��� �ָ� �������� �ٽ� ���ӽ�ŸƮ�� �ǵ��ư��� - ���߿� ����.
                //if (SceneManager.GetActiveScene().name != "Ready") { }

                    break;

            case State.GameStart:
                //�÷���
                if (floating) Float();

                if (!tM.bH) { Grab(); }
                SetFree();
                Rot();
                PwUp();

                //��Ȧ �η�
                if (tM.bH) transform.position += tM.dir * tM.pullSpd * Time.deltaTime;

                //������ �ۿ�
                if (myItem.Count > 0)
                {

                    if (getDBtn1R)
                    {
                        print("������ ���");
                        myItem[0].SetActive(true);
                        GameObject used = myItem[0];
                        ItemM itm = myItem[0].GetComponent<ItemM>();
                        itm.active = true;
                        myItem.RemoveAt(0);
                        Destroy(used, 6);
                    }

                }

                //�ָ� �� ������ ������
                if (free.childCount > 0)
                {
                    for (int i = 0; i < free.childCount; i++)
                    {

                        if (free.GetChild(i).position.x > transform.position.x * 10
                            || free.GetChild(i).position.x < transform.position.x * -10
                            || free.GetChild(i).position.y > transform.position.y * 10
                            || free.GetChild(i).position.y < transform.position.y * -10
                            || free.GetChild(i).position.z > transform.position.z * 10
                            || free.GetChild(i).position.z < transform.position.z * -10

                            )
                        {
                            Destroy(free.GetChild(i).gameObject);
                        }
                    }
                }

                break;

            case State.GameOver:
                Click(100);
                break;

            case State.TowardsEnd:
                break;

            case State.End:
                Click(100);
                break;
        }



    }

    void Float()
    {
        transform.position += (transform.up - transform.forward) * 0.02f * Time.deltaTime;

    }


    void Walk()  //�������� ���� �� ������ �־���� �ߴµ� �װ� ���� ���ؼ� ��ð� ����ߴ� ����
    {

        if (getDTchTmbL)
        {

            walkR = false;
            walkL = true;
            origin = my[(int)Parts.LHand].position;
            SoundM.instance.playS(0);

        }
        if (walkL)
        {
            transform.position += origin - my[(int)Parts.LHand].position;
            pos = transform.position;
            pos.y = 0;
            transform.position = pos;
        }
        if (getUTchTmbL)
        {
            SoundM.instance.StopS(0);
            walkL = false;
        }

        if (getDTchTmbR)
        {
            walkL = false;
            walkR = true;
            origin = my[(int)Parts.RHand].position;
            SoundM.instance.playS(0);

        }

        if (walkR)
        {
            transform.position += origin - my[(int)Parts.RHand].position;
            pos = transform.position;
            pos.y = 0;
            transform.position = pos;
        }

        if (getUTchTmbR)
        {
            walkR = false;
            SoundM.instance.StopS(0);
        }

    }

    void Walk(int audioNum)  //�������� ���� �� ������ �־���� �ߴµ� �װ� ���� ���ؼ� ��ð� ����ߴ� ����
    {

        if (getDTchTmbL)
        {

            walkR = false;
            walkL = true;
            origin = my[(int)Parts.LHand].position;
            SoundM.instance.playS(audioNum);

        }
        if (walkL)
        {
            transform.position += origin - my[(int)Parts.LHand].position;
        }
        if (getUTchTmbL)
        {
            SoundM.instance.StopS(audioNum);
            walkL = false;
        }

        if (getDTchTmbR)
        {
            walkL = false;
            walkR = true;
            origin = my[(int)Parts.RHand].position;
            SoundM.instance.playS(audioNum);

        }

        if (walkR)
        {
            transform.position += origin - my[(int)Parts.RHand].position;
        }

        if (getUTchTmbR)
        {
            walkR = false;
            SoundM.instance.StopS(audioNum);
        }

    }
    void Rot()
    {
        // ������ȯ
        if (getBtnHandL)
        { transform.Rotate(0, -.2f, 0); }
        if (getBtnHandR)
        { transform.Rotate(0, .2f, 0); }

        Vector2 joystickL = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
        if (joystickL.y > 0 || joystickL.y < 0)
        {
            transform.Rotate(joystickL.y * .2f, 0, 0);
        }

        if (joystickL.x > 0 || joystickL.x < 0)
        {
            transform.Rotate(0, 0, joystickL.x * .2f);
        }

    }

    void Open(float m, string scene)
    {

        if (Physics.Raycast(origin: my[(int)Parts.LHand].position, direction: my[(int)Parts.LHand].forward, out hit, m))// 0.5f))
        {

            hitObj = hit.transform.gameObject;
            if (hitObj.name == "Door")
            {

                doorIndi.SetActive(true);
                doorIndi.transform.position = hit.point;
                float dist = Vector3.Distance(
              Camera.main.transform.position, hit.point);
                doorIndi.transform.localScale = Vector3.one * dist;

                if (getDBtnIdxL)
                {
                    SceneManager.LoadScene(scene);
                }
            }
            else
            {
                doorIndi.SetActive(false);
            }
        }
        else
        {
            doorIndi.SetActive(false);
        }



        if (Physics.Raycast(origin: my[(int)Parts.RHand].position, direction: my[(int)Parts.RHand].forward, out hit, m))//0.5f))
        {
            hitObj = hit.transform.gameObject;
            if (hitObj.name == "Door")
            {
                doorIndi2.SetActive(true);
                doorIndi2.transform.position = hit.point;
                float dist = Vector3.Distance(
             Camera.main.transform.position, hit.point);
                doorIndi2.transform.localScale = Vector3.one * dist;

                if (getDBtnIdxR)
                {
                    SceneManager.LoadScene(scene);
                }
            }
            else
            {
                doorIndi2.SetActive(false);
            }
        }
        else
        {
            doorIndi2.SetActive(false);
        }


    }

    void Click(float m)
    {

        if (Physics.Raycast(origin: my[(int)Parts.LHand].position, direction: my[(int)Parts.LHand].forward, out hit, m))
        {

            hitObj = hit.transform.gameObject;
            if (hitObj.layer == LayerMask.NameToLayer("UI"))
            {

                doorIndi.SetActive(true);
                doorIndi.transform.position = hit.point;
                float dist = Vector3.Distance(
              Camera.main.transform.position, hit.point);
                doorIndi.transform.localScale = Vector3.one * dist;

                if (getDBtnIdxL)
                {

                     Button btn = hit.transform.GetComponent<Button>();
                    if (btn != null)
                    {
                        btn.onClick.Invoke();
                    }
                }
            }
            else
            {
                doorIndi.SetActive(false);
            }
        }
        else
        {
            doorIndi.SetActive(false);
        }



        if (Physics.Raycast(origin: my[(int)Parts.RHand].position, direction: my[(int)Parts.RHand].forward, out hit, m))
        {
            hitObj = hit.transform.gameObject;
            if (hitObj.layer == LayerMask.NameToLayer("UI"))
            {
                doorIndi2.SetActive(true);
                doorIndi2.transform.position = hit.point;
                float dist = Vector3.Distance(
             Camera.main.transform.position, hit.point);
                doorIndi2.transform.localScale = Vector3.one * dist;

                if (getDBtnIdxR)
                {
                    Button btn = hit.transform.GetComponent<Button>();
                    if (btn != null)
                    {
                        btn.onClick.Invoke();
                    }
                }
            }
            else
            {
                doorIndi2.SetActive(false);
            }
        }
        else
        {
            doorIndi2.SetActive(false);
        }


    }



    void PwUp()
    { //�����ʿ�
        if (getUBtnIdxR && getUBtnIdxL)
        {
            vPower *= 3;
        }

    }

    void Grab()
    {
        if (getDBtnIdxL)
        {

            walkR = false;
            walkL = true;
            origin = my[(int)Parts.LHand].position;
        }

        if (walkL)
        {
            Collider[] hitsL = Physics.OverlapSphere(my[(int)Parts.LHand].position, 0.05f);

            if (hitsL.Length > 0)
            {
                hitTF[0] = hitsL[0].transform;

                if (hitTF[0].gameObject.name.Contains("Rock_01")) { }

                //Ȧ�� ��� �ִ� ��
                if (hitTF[0].IsChildOf(rock))
                {
                    //��� Ȧ�带 ���� �����ϱ� ���� ���� catchHoldL / catchHoldR
                    catchHoldL = hitTF[0];
                    if (catchHoldL != catchHoldR) { tM.up = true; }
                    GrabHold(hitsL[0], my[(int)Parts.LHand]);

                }

                GrabItem(hitTF[0], my[(int)Parts.LHand], my[(int)Parts.RHand]);
            }
        }

        // ������ ������
        if (getDBtnIdxR)
        {
            walkR = true;
            walkL = false;
            origin = my[(int)Parts.RHand].position;
        }

        if (walkR)
        {
            Collider[] hitsR = Physics.OverlapSphere(my[(int)Parts.RHand].position, 0.05f);

            if (hitsR.Length > 0)
            {
                hitTF[1] = hitsR[0].transform;

                if (hitTF[1].gameObject.name.Contains("Rock_01")) { }

                //������ Ȧ�� ��� �ִ� ��
                if (hitTF[1].IsChildOf(rock))
                {
                    catchHoldR = hitTF[1];
                    if (catchHoldL != catchHoldR) { tM.up = true; }
                    GrabHold(hitsR[0], my[(int)Parts.RHand]);

                }

                GrabItem(hitTF[1], my[(int)Parts.RHand], my[(int)Parts.LHand]);


            }

        }

    }

    //Ȧ�� ��� �ִ� ��
    void GrabHold(Collider hits, Transform hand)
    {


        Grap.Play();
        Rocks r = hits.GetComponent<Rocks>();
        floating = false;
        rb.isKinematic = true;

        if (r.num == (int)Rocks.Type.Trap)
        {

            if (tM.up)
            {

                if (r.trapNum < (int)Rocks.TrapType.Meteor)
                {
                    tM.BHole(r);
                }
                else

                if (r.trapNum == (int)Rocks.TrapType.Meteor)
                {
                    tM.Create(tM.meteorFactory);
                }

                else //(r.trapType == (int)Rocks.TrapType.Can)
                {
                    tM.Create(tM.canFactory);

                }
                tM.up = false;

            }

        }

        if (tM.bH) transform.position += tM.dir * tM.pullSpd * Time.deltaTime;
        else { transform.position += origin - hand.position; }




    }

    //������ ��� ��
    void GrabItem(Transform hitTFN, Transform handG, Transform otherH)
    {

        // ������ ���� �� �޼� ����
        if (hitTFN.IsChildOf(item))
        {

            Grap.Play();
            if (hitTFN.gameObject.name.Contains("Fire")) { CreateItem(fire, handG); }
            if (hitTFN.gameObject.name.Contains("Oxy")) { CreateItem(oxy, handG); }
            if (hitTFN.gameObject.name.Contains("Rope")) { CreateItem(rope, handG); }
            if (hitTFN.gameObject.name.Contains("Shield")) { CreateItem(shield, handG); }

            hitTFN.gameObject.SetActive(false);

        }

        // ������ �������̳� �ٸ� �� �������� ������
        if (hitTFN.IsChildOf(free) || hitTFN.IsChildOf(otherH))
        {
            catchItem = hitTFN;

            Grap.Play();
            // ���� ������ �ڽ� ����� 0������ �̵���Ű��
            catchItem.SetParent(handG);
            hitTFN.localPosition = Vector3.zero;

            // ���� �ۿ� off
            Rigidbody itemRb = hitTFN.gameObject.GetComponent<Rigidbody>();
            itemRb.isKinematic = true;
        }


    }

    //������ �տ� �����
    void CreateItem(GameObject clone, Transform hand)
    {
        GameObject a = Instantiate(clone);
        a.transform.SetParent(hand);
        a.transform.localPosition = Vector3.zero;
        catchItem = a.transform;
    }


       //���� ��
    void SetFree()
    {

        //�޼�
        if (getUBtnIdxL)
        {
            walkL = false;

            // ������ ������ ����
            if (hitTF[0] == null)
            {
                return;
            }

            SetFree(hitTF[0], catchHoldL, my[(int)Parts.LHand], getVelL, getAngVelL);
        }

        //������
        if (getUBtnIdxR)
        {

            walkR = false;

            // ������ ������ ����
            if (hitTF[1] == null)
            {
                return;
            }

            SetFree(hitTF[1], catchHoldR, my[(int)Parts.RHand], getVelR, getAngVelR);

        }
    }

    void SetFree(Transform hitTFN, Transform hold, Transform hand, Vector3 vel, Vector3 angVel)
    {

        // Ȧ����
        if (hitTFN == hold)
        {

            rb.isKinematic = false;
            rb.velocity = -transform.TransformDirection(vel) * vPower;

            if (hitTFN == hitTF[0]) hitTF[0] = null;
            else { hitTF[1] = null; }



        }

        // ������ ��Ҵٸ�
        if (catchItem != null && hitTFN == catchItem)
        {

            Collider[] objs = Physics.OverlapSphere(hand.position, 0.1f, LayerMask.NameToLayer("Player"));
            Rigidbody itemRb = catchItem.GetComponent<Rigidbody>();

            // �չݰ����� �ɸ��� ������Ʈ�� 1���̻��̸�
            if (objs.Length > 0)
            {
                Transform objTF = objs[0].transform;

                //0��°�� ���̶��
                if (objTF == my[(int)Parts.Body])
                {

                    // ȹ�� �������� 2�� �̸��̸�
                    if (myItem.Count < 2)
                    {
                        // ȹ�渮��Ʈ�� �ִ´�.
                        myItem.Add(catchItem.gameObject);
                        // �ָ� ���� �Ⱥ��̰� ����
                        myItem[0].transform.position = new Vector3(1000, 1000, 1000);

                        if (myItem.Count == 2)
                        {
                            myItem[1].transform.position = new Vector3(1000, 1000, 1000);
                        }
                        objTF = null;
                        if (hitTFN == hitTF[0]) hitTF[0] = null;
                        else { hitTF[1] = null; }
                    }
                    // 2�� �̻��̸� ���տ��� ������.
                    else
                    {
                        catchItem.position = new Vector3(1000, 1000, 1000);
                        catchItem.SetParent(free);
                        print(catchItem + " 2�� �̻�");
                        objTF = null;
                    if (hitTFN == hitTF[0]) hitTF[0] = null;
                    else { hitTF[1] = null; }
                    }
                }
                //���� ���� ������
                else
                {

                    //������
                    itemRb.isKinematic = false;

                    itemRb.velocity = vel * vPower;
                    itemRb.angularVelocity = angVel;


                    catchItem.transform.SetParent(free);
                    print(catchItem + " ���ߺξ�");
                objTF = null;

                if (hitTFN == hitTF[0]) hitTF[0] = null;
                else { hitTF[1] = null; }
                }
            }
            // �ɸ��°� ���
            else {
                //������
                itemRb.isKinematic = false;

                itemRb.velocity = vel * vPower;
                itemRb.angularVelocity = angVel;


                catchItem.transform.SetParent(free);
                print(catchItem + " ������ ��� ���ߺξ�");

            if (hitTFN == hitTF[0]) hitTF[0] = null;
            else { hitTF[1] = null; }
            }
        }

        // ǲ������ ��� ���� ���¶��
        if (hitTFN.gameObject.name.Contains("foot"))
        {
           transform.position = FootStepTransform.position; 
           transform.rotation = FootStepTransform.rotation; 
           state = State.Ready;
        }
        

    }


}




