using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;


public class PlayerM : MonoBehaviour
{
    // ī�޶󸮱׿� �ٴ� ��ũ��Ʈ��� ������ �޼� / ������ ������� �����ϱ�

    public enum State
    {
        Wait,
        Ready,
        GameStart,
        GameOver
    }

    PhotonView pv;
    PlayerPhoton pp;

    public State state;

    //�ȱ� ���
    Vector3 origin;
    Vector3 pos;
    bool walkL = false;
    bool walkR = false;

    //��ǥ�� ������
    Transform doorCan;
    GameObject doorIndi;
    GameObject doorIndi2;
    // Ŭ�� ���η����� ���
    LineRenderer lrL;
    LineRenderer lrR;

    // �ӷ�
    public bool floating = true;
    float vPower = .5f;

    //�׷� ����ġ
    Transform[] hitTF = new Transform[2];

    //�տ� ���� ������Ʈ�� Ʈ������
    Transform catchHoldL;
    Transform catchHoldLpre;
    Transform catchHoldR;
    Transform catchHoldRpre;
    Transform catchItem;

    //������ �κ��丮 �ֱ��
    bool find = false;

    //�׷� ��������
    RockParent rp;
    // Ȧ��ã���
    Transform rock;

    //������ ������
    public GameObject rope;
    public GameObject fire;
    public GameObject shield;
    public GameObject oxy;

    //ȹ������۸���Ʈ
    public List<int> myTem = new List<int>();
     int[] mytem = { -1, -1 };

    // ������ ���� ����� �� ���� lr >> player�� �پ�����
    LineRenderer lr;
    //��ȭ�� ����� �� ��
    bool f = false;
    // ��ȭ�� ��ƼŬ
    public GameObject particle;
    GameObject p;

    //���� ����� �� �� + ��ҵ����� ���ְ� �ۺ�
    public bool r = false;
    //���� ������ ��ũ
    float ropeSpd = .05f;
    public GameObject hook;
    GameObject h;
    //��ũ ���� �� ��ƼŬ+�Ҹ� ������
    public GameObject hookP;
    bool isH = true;
    // ���� ��� �� ������ ����Ʈ
    public GameObject shieldEFT;


    //ī�޶� ������ ������ٵ� ��������
    Rigidbody rb;

    //������ ����
    TrapManager tM;
    Player ps;

    RaycastHit hit;

    // open �Լ��� ���̴� �տ� ���� ������Ʈ
    GameObject hitObj;

    // Ŭ���� ����� ���� �� ��ġ����
    Transform FootStepTransform;
    bool fStep = false;

    #region ��Ʈ�ѷ� bool Vector3����
    bool getDTchTmbL;
    bool getUTchTmbL;
    bool getDTchTmbR;
    bool getUTchTmbR;

    bool getBtnHandR;
    bool getBtnHandL;

    bool getDBtnIdxL;
    bool getUBtnIdxL;
    bool getDBtnIdxR;
    bool getUBtnIdxR;

    bool getDBtn1R;
    bool getDBtn1L;
    bool getDBtn2L;

    Vector3 getVelL;
    Vector3 getVelR;
    Vector3 getAngVelL;
    Vector3 getAngVelR;
    #endregion

    void Start()
    {
        pv = GetComponent<PhotonView>();
        pp = GetComponent<PlayerPhoton>();
        tM = GetComponent<TrapManager>();
        ps = GetComponent<Player>();

        if (SceneManager.GetActiveScene().name == "Game")
        {
            rock = GameObject.Find("Rock").transform;
            rp = rock.GetComponent<RockParent>();
            lr = GetComponent<LineRenderer>();
            FootStepTransform = GameObject.Find("FootStepTransform").transform;
            state = State.GameStart;

        }

        else if (SceneManager.GetActiveScene().name == "Ready")
        { state = State.Ready; }

        else
        { state = State.GameOver; }

        doorCan = GameObject.Find("DoorCanvas").transform;
        doorIndi = doorCan.GetChild(0).gameObject;
        doorIndi2 = doorCan.GetChild(1).gameObject;

        rb = GetComponent<Rigidbody>();
        lrL = pp.handL.GetComponent<LineRenderer>();
        lrR = pp.handR.GetComponent<LineRenderer>();

    }
    void Update()
    {

        if (!pv.IsMine)
        {
            rb.isKinematic = true;
            return;
        }
        #region ��Ʈ�ѷ� bool
        getDTchTmbL = OVRInput.GetDown(OVRInput.Touch.PrimaryThumbstick, OVRInput.Controller.LTouch);
        getUTchTmbL = OVRInput.GetUp(OVRInput.Touch.PrimaryThumbstick, OVRInput.Controller.LTouch);
        getDTchTmbR = OVRInput.GetDown(OVRInput.Touch.PrimaryThumbstick, OVRInput.Controller.RTouch);
        getUTchTmbR = OVRInput.GetUp(OVRInput.Touch.PrimaryThumbstick, OVRInput.Controller.RTouch);

        getBtnHandR = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch);
        getBtnHandL = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch);

        getDBtnIdxL = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
        getUBtnIdxL = OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
        getDBtnIdxR = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        getUBtnIdxR = OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);

        getDBtn1R = OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch);
        getDBtn1L = OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch);
        getDBtn2L = OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch);

        getVelL = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
        getVelR = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
        getAngVelL = OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.LTouch);
        getAngVelR = OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.RTouch);
        #endregion

        switch (state)
        {
            #region Wait
            case State.Wait:

                break;
            #endregion

            #region Ready
            case State.Ready:

                if (SceneManager.GetActiveScene().name == "Ready")
                {
                    Walk(); Open(0.1f, "Game");
                    if (getDBtn1R) { SceneManager.LoadScene("Game"); }
                }
                else
                {
                    Walk(0); Open(0.1f, "Clear");

                    // ǲ���ܿ��� �ָ� �������� �ٽ� ���ӽ�ŸƮ�� �ǵ��ư��� - ���߿� ����.

                    floating = false;

                    if (fStep)
                    {
                        print("fStep ��ġ �̵� �۵�?");
                        transform.position = Vector3.Lerp(transform.position, FootStepTransform.position, 3 * Time.deltaTime);
                        transform.rotation = Quaternion.Lerp(transform.rotation, FootStepTransform.rotation, 3 * Time.deltaTime);
                        StartCoroutine(StopFStep());
                    }

                    UseItem();
                    itemActive();

                }

                Rot();

                if (goPlay.instance.MenuManager.activeSelf) { state = State.GameOver; }


                break;
            #endregion

            #region GameStart
            case State.GameStart:

                //ġƮŰ
                if (getDBtn1L) { SceneManager.LoadScene("Meteo"); }
                if (getDBtn2L) { SceneManager.LoadScene("Clear"); }


                // ���� ������ Ű��������
                float v = Input.GetAxis("Vertical");
                float h = Input.GetAxis("Horizontal");
                float f = 0;
                if (Input.GetKey(KeyCode.Space)) { f = .1f; }
                if (Input.GetKey(KeyCode.LeftControl)) { f = -.1f; }
                Vector3 dir = new Vector3(h, f, v);
                transform.position += dir * 10 * Time.deltaTime;

                //�÷���
                // ���߷� ������
                if (floating) { }  //Float();

                if (!tM.bH) { Grab(); }
                SetFree();
                UseItem();
                itemActive();
                Rot();
                PwUp();


                //���� ������
                //  if (goPlay.instance.MenuManager.activeSelf) { state = State.GameOver; }
                // else
                //{
                //  lrL.enabled = false;
                //lrR.enabled = false;
                //}

                //��Ȧ �η�
                if (tM.bH) transform.position += tM.dir * tM.pullSpd * Time.deltaTime;

                break;
            #endregion

            #region GameOver
            case State.GameOver:
                ClickLR();

                if (!goPlay.instance.MenuManager.activeSelf)
                {
                    if (SceneManager.GetActiveScene().name == "Ready")
                    {
                        state = State.Ready;
                        lrL.enabled = false;
                        lrR.enabled = false;
                    }
                    else
                    {
                        state = State.GameStart;

                    }
                }

                break;
                #endregion
        }



    }


    void Float()
    {
        transform.position += (transform.up - transform.forward) * 0.02f * Time.deltaTime;

    }


    //��������� ��� Y�� ����
    void Walk()  //�������� ���� �� ������ �־���� �ߴµ� �װ� ���� ���ؼ� ��ð� ����ߴ� ����
    {

        if (getDTchTmbL)
        {

            walkR = false;
            walkL = true;
            origin = pp.handL.position;
            SoundM.instance.playS(1, 0);

        }

        if (walkL)
        {
            transform.position += origin - pp.handL.position;
            pos = transform.position;
            pos.y = 0;
            transform.position = pos;
        }

        if (getUTchTmbL)
        {
            SoundM.instance.StopS(1, 0);
            walkL = false;
        }

        if (getDTchTmbR)
        {
            walkL = false;
            walkR = true;
            origin = pp.handR.position;
            SoundM.instance.playS(1, 0);

        }

        if (walkR)
        {
            transform.position += origin - pp.handR.position;
            pos = transform.position;
            pos.y = 0;
            transform.position = pos;
        }

        if (getUTchTmbR)
        {
            walkR = false;
            SoundM.instance.StopS(1, 0);
        }

    }

    // ���� Ŭ���� ��� ���� ������
    void Walk(int audioNum)
    {

        if (getDTchTmbL)
        {

            walkR = false;
            walkL = true;
            origin = pp.handL.position;
            SoundM.instance.playS(1, audioNum);

        }

        if (walkL)
        {
            transform.position += origin - pp.handL.position;
        }

        if (getUTchTmbL)
        {
            SoundM.instance.StopS(1, audioNum);
            walkL = false;
        }

        if (getDTchTmbR)
        {
            walkL = false;
            walkR = true;
            origin = pp.handR.position;
            SoundM.instance.playS(1, audioNum);

        }

        if (walkR)
        {
            transform.position += origin - pp.handR.position;
        }

        if (getUTchTmbR)
        {
            walkR = false;
            SoundM.instance.StopS(1, audioNum);
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

    //������� + ����ȯ
    void Open(float m, string scene)
    {

        if (Physics.Raycast(origin: pp.handL.position, direction: pp.handL.forward, out hit, m))// 0.5f))
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
                    SoundM.instance.playS(0, 4);
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



        if (Physics.Raycast(origin: pp.handR.position, direction: pp.handR.forward, out hit, m))//0.5f))
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
                    SoundM.instance.playS(0, 4);
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

    // �޴����� ���ӿ����� ���� �̰ɷ� ���
    void ClickLR()
    {
        Click(pp.handL, lrL, doorIndi, getDBtnIdxL);
        Click(pp.handR, lrR, doorIndi2, getDBtnIdxR);
    }

    void Click(Transform hand, LineRenderer lrC, GameObject indi, bool gDB)
    {

        lrC.enabled = true;

        lrC.SetPosition(0, hand.position);
        lrC.SetPosition(1, hand.forward * 100);

        if (Physics.Raycast(origin: hand.position, direction: hand.forward, out hit, 100))
        {
            if (hit.collider.CompareTag("Button"))
            {

                lrC.SetPosition(1, hit.point);
                indi.SetActive(true);
                indi.transform.localScale = Vector3.one * .5f;
                indi.transform.forward = hand.forward;
                indi.transform.position = hit.point;


                if (gDB)
                {
                    Button btn = hit.collider.transform.GetComponent<Button>();
                    if (btn != null)
                    {
                        btn.onClick.Invoke();
                    }
                }
            }
            else
            {
                lrC.SetPosition(1, hand.forward * 100);
                indi.SetActive(false);
            }
        }
        else
        {

            indi.SetActive(false);
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
        Collider[] hitsL = Physics.OverlapSphere(pp.handL.position, 0.01f);
        Collider[] hitsR = Physics.OverlapSphere(pp.handR.position, 0.01f);

        if (getDBtnIdxL)
        {

            if (hitsL.Length > 0)
            {
                SoundM.instance.playS(0, 5);
            }

            walkR = false;
            walkL = true;
            origin = pp.handL.position;
            tM.up = true;

        }

        if (walkL)
        {

            if (hitsL.Length > 0)
            {
                hitTF[0] = hitsL[0].transform;

                if (hitTF[0].gameObject.name.Contains("Big")) { }
                Grab(hitTF[0], ref pp.handL, pp.handR, ref catchHoldL, ref catchHoldR, ref catchHoldLpre);

            }
        }

        // ������ ������
        if (getDBtnIdxR)
        {
            if (hitsR.Length > 0)
            {
                SoundM.instance.playS(0, 5);
            }

            walkR = true;
            walkL = false;
            origin = pp.handR.position;

            tM.up = true;

        }

        if (walkR)
        {


            if (hitsR.Length > 0)
            {
                hitTF[1] = hitsR[0].transform;

                if (hitTF[1].gameObject.name.Contains("Big")) { print("����0 big"); }

                Grab(hitTF[1], ref pp.handR, pp.handL, ref catchHoldR, ref catchHoldL, ref catchHoldRpre);

            }

        }

    }

    //���߷� ������ ���۷��� �ȳ����� �� �۵��ϴ°��� �˱�
    void Grab(Transform hitTFN, ref Transform handG, Transform otherH, ref Transform Hold, ref Transform Hold2, ref Transform Holdpre)
    {
        int idx = hitTFN.GetSiblingIndex();

        // Ȧ�� ��� �ִ� ��
        if (hitTFN.IsChildOf(rock))
        {
            floating = false;
            rb.isKinematic = true;

            if (tM.bH) { transform.position += tM.dir * tM.pullSpd * Time.deltaTime; }
            else
            {
                transform.position += origin - handG.position;
            }

            Hold = hitTFN;
            if (Hold2 != null &&
                  Hold == Hold2) { tM.up = false; }

            if (Holdpre == Hold) { tM.up = false; }



            if (rp.holds[idx].type == Value.Type.Trap)
            {

                if (tM.up)
                {

                    if (rp.holds[idx].tT == Value.TrapType.BHoleL
                        || rp.holds[idx].tT == Value.TrapType.BholeR
                        )
                    {
                        tM.BHole(rp.holds[idx]);
                    }
                    else

                    if (rp.holds[idx].tT == Value.TrapType.Meteor)
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


        }
        else
        {
            //0 : �޼�, 1 : ������
            int hand = 1;
            if (walkL) hand = 0;
            // 0 = rp.free, 1 = �ٸ���
            int pa = 0;

            // ������ ���� �� �տ� ����
            if (hitTFN.IsChildOf(rp.item))
            {
                pv.RPC("RpcItem", RpcTarget.All, hand, idx);
            }

            // ������ �������̳� �ٸ� �� �������� ������
            if (hitTFN.IsChildOf(rp.free))
            {
                pv.RPC("RPCFreeItem", RpcTarget.All, pa, hand, idx);
            }
            if (hitTFN.IsChildOf(otherH))
            {
                pa = 1;
                pv.RPC("RPCFreeItem", RpcTarget.All, pa, hand, idx);
            }
        }

    }

    [PunRPC]
    void RPCTrapM()
    {
        GameObject clone = tM.meteorFactory;
        GameObject obj = Instantiate(clone);

        obj.transform.position = transform.position
            + transform.up * 10
            + transform.forward * -10;
    }


    [PunRPC]
    void RpcItem(int hand, int itemIdx)
    {
        // �տ� ���� ������ Ư��
        GameObject item = rp.item.GetChild(itemIdx).gameObject;

        // ������� Ư��
        Transform handG = pp.handL;

        if (hand == 1) handG = pp.handR;

        // �տ� ����
        //�� �����۸��� ���� ��ȣ�� �Ἥ ��Ʈ ����Ʈ�� ���� ����ϱ�
        if (item.name.Contains("Rope")) { CreateItem(rope, handG); mytem[hand] = 0; }
        if (item.name.Contains("Fire")) { CreateItem(fire, handG); mytem[hand] = 1; }
        if (item.name.Contains("Shield")) { CreateItem(shield, handG); mytem[hand] = 2; }
        if (item.name.Contains("Oxy")) { CreateItem(oxy, handG); mytem[hand] = 3; }

        // ������ Ǯ ������ 30��
        rp.item_False.Add(item);
        item.SetActive(false);
        rp.items.Remove(item);
        StartCoroutine(rp.ShowUp());
    }

    //������ �տ� ����� �Լ�
    void CreateItem(GameObject clone, Transform hand)
    {
        GameObject a = Instantiate(clone);
        catchItem = a.transform;
        catchItem.SetParent(hand);
        catchItem.localPosition = Vector3.zero;
    }


    //���߿� �� / �ٸ����� ������
    [PunRPC]
    void RPCFreeItem(int parent, int hand, int itemIdx)
    {
        Transform otherH = pp.handR;
        Transform handG = pp.handL;
        if (hand == 1) { otherH = pp.handL; handG = pp.handR; }
        Transform pa = otherH;
        if (parent == 0) pa = rp.free;

        Transform item = pa.GetChild(itemIdx);
        catchItem = item;

        catchItem.SetParent(handG);
        item.localPosition = Vector3.zero;

        //���߿� �� ������ ������ �۸���Ʈ�� ���� ��Ʈ�� ����
        if (parent == 0) {
            for (int i = 0; i < 4; i++)
            {
                if (catchItem.name.Contains(i.ToString())) {
                    mytem[hand] = i;
                    break;
                }
            }
        }

        // ���� �ۿ� off
        Rigidbody itemRb = item.GetComponent<Rigidbody>();
        itemRb.isKinematic = true;
    }




    //���� ��
    void SetFree()
    {
        //0 : �޼�, 1 : ������
        int hh = 0;
        //�޼�
        if (getUBtnIdxL)
        {
            walkL = false;

            // ������ ������ ����
            if (hitTF[0] == null)
            {
                return;
            }

            SetFree(hitTF[0], catchHoldL, pp.my[(int)PlayerPhoton.Parts.Body], getVelL, getAngVelL, hh);
            hitTF[0] = null;
            catchHoldLpre = catchHoldL;
        }

        //������
        if (getUBtnIdxR)
        {
            hh = 1;
            walkR = false;

            // ������ ������ ����
            if (hitTF[1] == null)
            {
                return;
            }

            SetFree(hitTF[1], catchHoldR, pp.my[(int)PlayerPhoton.Parts.Body], getVelR, getAngVelR, hh);
            hitTF[1] = null;
            catchHoldRpre = catchHoldR;
        }
    }

    void SetFree(Transform hitTFN, Transform hold, Transform body, Vector3 ve, Vector3 angVe, int hand)
    {

        // Ȧ����
        if (hitTFN == hold)
        {
            rb.isKinematic = false;
            rb.velocity = -transform.TransformDirection(ve) * vPower;

        }


        // ������ ��Ҵٸ�
        if (catchItem != null && hitTFN == catchItem)
        {
            //�ΰ� ����
            if (myTem.Count < 2)
            {

                int item = 1 << LayerMask.NameToLayer("Item");
                Collider[] obj = Physics.OverlapSphere(body.position + (Vector3.up * .01f), 0.3f, item);

                //1���̻�   
                if (obj.Length > 0)
                {

                    for (int i = 0; i < obj.Length; i++)
                    {
                        print(i + "��    " + obj[i].name);
                    }

                    //ã��
                    for (int i = 0; i < obj.Length; i++)
                    {
                        if (obj[i].name.Contains("Item"))
                        {
                            find = true;
                            break;
                        }
                    }

                    if (find)
                    {
                        pv.RPC("RPCFind", RpcTarget.All, hand);
                        find = false;

                    }
                    //else
                    //{
                    //    pv.RPC("RPCFree", RpcTarget.AllBuffered, ve, angVe);
                    //    print("�������� ã������ ����");
                    //}

                }

                //0��
                else
                {
                    pv.RPC("RPCFree", RpcTarget.AllBuffered, ve, angVe);
                    print("�ɸ��� ���  ���ߺξ�");
                }


            }
            // 2�� �̻��̸� ������.
            else
            {
                pv.RPC("RPCFree", RpcTarget.AllBuffered, ve, angVe);
                print("��������  2�� �̻�");
            }


        }


        // ǲ������ ��� ���� ���¶��
        if (hitTFN.gameObject.name.Contains("foot"))
        {
            fStep = true;
            state = State.Ready;
        }
    }

    [PunRPC]
    void RPCFind(int hh)
    {
        if (mytem[hh] != -1) myTem.Add(mytem[hh]);
        mytem[hh] = -1;
        Destroy(catchItem.gameObject);
    }


    [PunRPC]
    void RPCFree(Vector3 velo, Vector3 angVelo)
    {
        Rigidbody itemRb = catchItem.GetComponent<Rigidbody>();

        itemRb.isKinematic = false;
        itemRb.velocity = velo * vPower;
        itemRb.angularVelocity = angVelo;

        catchItem.SetParent(rp.free);
    }


    void UseItem() {

        //������ �ۿ�
        if (myTem.Count > 0)
        {
            if (getDBtn1R)
            {

                if (myTem[0] == 0)
                {
                    pv.RPC("RPCUse", RpcTarget.All, myTem[0]);
                    r = true;
                    isH = true;
                    SoundM.instance.playS(2, 2);
                }
                if (myTem[0] == 1)
                {
                    f = true;
                    pv.RPC("RPCUse", RpcTarget.All, myTem[0]);
                }

                if (myTem[0] == 2)
                {
                    rb.isKinematic = true;
                    //��Ȧ �����
                    tM.bH = false;
                    pv.RPC("RPCUse", RpcTarget.All, myTem[0]);
                }

                if (myTem[0] == 3)
                {
                    ps.PlusHp(30);
                }

                myTem.RemoveAt(0);
                StartCoroutine(StopFR());
            }

        }

    }

    void itemActive() {

        if (f)
        {
            rb.isKinematic = false;
            rb.AddForce(-pp.handL.forward * ropeSpd * 10);
            pv.RPC("RPCFireEx", RpcTarget.All);
        }
        else
        {

            if (p != null)
            {
                pv.RPC("RPCFNull", RpcTarget.All);
            }
        }

        if (r)
        {
            pv.RPC("RPCRLr", RpcTarget.All);
            HookMovement hhook = h.GetComponent<HookMovement>();
            if (!hhook.moving)
            {
                if (isH)
                {
                    rb.isKinematic = true;
                    pv.RPC("RPCUse", RpcTarget.All, 3);
                    isH = false;
                }
                transform.position = Vector3.Lerp(transform.position, h.transform.position, 1 * Time.deltaTime);
            }
        }
        else
        {
            pv.RPC("RPCRLrFalse", RpcTarget.All);
        }

    }

    // ������ ��� �� ��������ȭ �Լ�
    // 012 ����, ��ȭ��, ���� / 3 ��ũ
    [PunRPC]
    void RPCUse(int itIdx) {

        GameObject[] item = new GameObject[4];

        GameObject clone = shieldEFT;
        Transform handG = transform;

        if (itIdx == 0) { clone = hook; handG = pp.handR; }
        if (itIdx == 1) { clone = particle; handG = pp.handL; }
        if (itIdx == 3) { clone = hookP; handG = h.transform; }

        item[itIdx] = Instantiate(clone);
        item[itIdx].transform.position = handG.position;
        item[itIdx].transform.forward = handG.forward;
        if (itIdx == 3) item[itIdx].transform.forward = -handG.forward;

        h = item[0];
        p = item[1];
        if (itIdx != 0) Destroy(item[itIdx], 5.5f);
    }

    [PunRPC]
    void RPCRLrFalse()
    {
        lr.enabled = false;
        h = null;
    }

    //rpc�ؾߵ� ��!

    [PunRPC]
    void RPCFireEx()
    {
        p.transform.position = pp.handL.position;
        p.transform.forward = pp.handL.forward;
    }

    [PunRPC]
    void RPCFNull()
    {
        p = null;
    }

    [PunRPC]
    void RPCRLr()
    {
            lr.enabled = true;
            lr.SetPosition(0, pp.handR.position);
            lr.SetPosition(1, h.transform.position);
    }
    IEnumerator StopFR()
    {
        yield return new WaitForSeconds(5);
        f = false; r = false;
    }
    IEnumerator StopFStep()
    {
        yield return new WaitForSeconds(3);
        fStep = false;
    }


}




