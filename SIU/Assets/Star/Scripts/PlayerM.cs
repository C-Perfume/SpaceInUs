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
    byte maxP;
    NetManager net;
    public GameObject hpbar;
   public GameObject readytext;

    public State state;

    #region//�ȱ� ���
    Vector3 origin;
    Vector3 originP;
    Vector3 pos;
    bool walkL = false;
    bool walkR = false;
    #endregion
   
    #region //��ǥ�� ������
    Transform doorCan;
    GameObject doorIndi;
    GameObject doorIndi2;
    // Ŭ�� ���η����� ���
    LineRenderer lrL;
    LineRenderer lrR;
    #endregion

    #region // �ӷ�


    public bool floating = true;
    float vPower = .5f;
    #endregion
    
    #region//�׷� ����ġ


    Transform[] hitTF = new Transform[2];

    //�տ� ���� ������Ʈ�� Ʈ������
    Transform catchHoldL;
    Transform catchHoldLpre;
    Transform catchHoldR;
    Transform catchHoldRpre;
    Transform catchItem;

    //�׷� ��������
    RockParent rp;
    // Ȧ��ã���
    Transform rock;
    #endregion

    #region    //������ �κ��丮 �ֱ��
    bool find = false;

    //������ ������
    public GameObject rope;
    public GameObject fire;
    public GameObject shield;
    public GameObject oxy;
    public GameObject[] ranBox;

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
    // ��ȭ�� ���� �ӵ�
    public float fireSpd = .1f;
    //��ȭ�� ����� �ٽþ���
    public bool spdChange = false;

    //���� ����� �� �� + ��ҵ����� ���ְ� �ۺ�
    public bool r = false;
    //���� ������ ��ũ
    public GameObject hook;
    GameObject h;
    //��ũ ���� �� ��ƼŬ+�Ҹ� ������
    public GameObject hookP;
    bool isH = true;
    // ���� ��� �� ������ ����Ʈ
    public GameObject shieldEFT;
    //�����ڽ� ��� �� ������ ��..
    public GameObject[] ranEFT;
#endregion
    
    #region //��Ÿ ����
  
    //������ ����
    TrapManager tm;
 
    //ī�޶� ������ ������ٵ� ��������
    Rigidbody rb;
    Player ps;
    
    // open �Լ��� ���̴� �տ� ���� ������Ʈ
    GameObject hitObj;
    RaycastHit hit;

    // Ŭ���� ����� ���� �� ��ġ����
    Transform FootStepTransform;
    bool fStep = false;
    #endregion

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
    bool getDBtn2R;
    bool getDBtn1L;
    bool getDBtn2L;

    Vector3 getVelL;
    Vector3 getVelR;
    Vector3 getAngVelL;
    Vector3 getAngVelR;
    #endregion

    void Start()
    {
        #region �ʱⰪ ����
        pv = GetComponent<PhotonView>();
        pp = GetComponent<PlayerPhoton>();
        tm = GetComponent<TrapManager>();
        ps = GetComponent<Player>();
        lr = GetComponent<LineRenderer>();

        doorCan = GameObject.Find("DoorCanvas").transform;
        doorIndi = doorCan.GetChild(0).gameObject;
        doorIndi2 = doorCan.GetChild(1).gameObject;

        rb = GetComponent<Rigidbody>();
        lrL = pp.handL.GetComponent<LineRenderer>();
        lrR = pp.handR.GetComponent<LineRenderer>();
        #endregion

        if (SceneManager.GetActiveScene().name == "Game")
        {
            net = GameObject.Find("NetManager").GetComponent<NetManager>();
            net.playerList.Add(gameObject);
            maxP = PhotonNetwork.CurrentRoom.MaxPlayers;
            rock = GameObject.Find("Rock").transform;
            rp = rock.GetComponent<RockParent>();
            FootStepTransform = GameObject.Find("FootStepTransform").transform;
            
            // �����ο��� �ȵ�� �� ��� wait ���·� ��ٸ���
            state = PlayerM.State.Wait;
            //���� �Ⱥ��̰� �ָ� ������ >> ��Ȱ��ȭ �� ���߿� ������ ��� ã�� ����
            hpbar.SetActive(false);

        StartCoroutine(WaitforStart());
        }

        else if (SceneManager.GetActiveScene().name == "Ready")
        { state = State.Ready; }

        else
        { state = State.GameOver; }

    }

    IEnumerator WaitforStart() {
        // �ִ��ο��� �ƴ� ��� ��ٸ���.
        while (maxP != net.playerList.Count)
        {         
        yield return new WaitForSeconds(.01f);
        }

        yield return new WaitForSeconds(1);
        // �ִ� �ο��̸� ���� ��ŸƮ, ���� ���ƿ���, ���ּ� ������ �� Ȱ��ȭ �� �÷��̾� ������ ��ġ�̵�
        hpbar.SetActive(true);
        readytext.SetActive(false);
        state = State.GameStart;
        if (pv.IsMine) {
            transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        if (GameObject.Find("StarParticle") != null) Destroy(GameObject.Find("StarParticle"));
        }
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
        getDBtn2R = OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch);
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
            #endregion

                #region //ġƮŰ

                if (getDBtn1L) { SceneManager.LoadScene("Meteo"); }
                if (getDBtn2L) { SceneManager.LoadScene("Clear"); }


                // ���� ������ Ű��������
                float v = Input.GetAxis("Vertical");
                float h = Input.GetAxis("Horizontal");
                float ff = 0;
                if (Input.GetKey(KeyCode.Space)) { ff = .1f; }
                if (Input.GetKey(KeyCode.LeftControl)) { ff = -.1f; }
                Vector3 dir = new Vector3(h, ff, v);
                transform.position += dir * 10 * Time.deltaTime;
                #endregion

                 //������
                if (Input.GetKeyDown(KeyCode.F1)) {
                     //���ϵ�2��
                    int idx = 2;

                    //�Ӽ��� �������̸�
                    if (rp.holds[idx].type == Value.Type.Item)
                    {
                        //�˾��� �ȵ� ���
                       if(!rp.holds[idx].popUp)
                        {
                            //rpc�� �ٲٱ�
                            pv.RPC("RpcPopUp", RpcTarget.All, idx);
                            
                        }
                    
                    }
                   
                }
                //������ ����
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    if (rp.holds[2].popUp)
                    {
                        pv.RPC("RpcItem", RpcTarget.All, 1, 0);
                    }
                }
                //������ �κ� ����
                if (Input.GetKeyDown(KeyCode.Alpha2)) 
                {
                    if (myTem.Count < 2)
                    {
                        if (catchItem != null)
                        {
                            SoundM.instance.playS(1, 10);
                            pv.RPC("RPCFind", RpcTarget.All, 1);
                        }
                    }
                    else
                    {
                        pv.RPC("RPCFree", RpcTarget.AllBuffered, 1, 1);
                        print("�ɸ��� ���  ���ߺξ�");
                    }
                }
                //���
                 if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    if (myTem.Count > 0)
                    {
                        //1�ο�
                        if (myTem[0] == 4)
                        {
                            int ranboxIdx = Random.Range(0, 4);
                            UseItem(ranboxIdx);
                            pv.RPC("RPCUse", RpcTarget.All, 4);
                        }
                        //2�ο�
                        else if (myTem[0] == 5)
                        {

                            int boxTrap = 4;
                                //Random.Range(0, 5);
                            pv.RPC("RPCUse", RpcTarget.All, 5);
                            if (boxTrap == 0 || boxTrap == 1)
                            {
                                int ox = Random.Range(0, 2);
                                pv.RPC("RPCBlackHole", RpcTarget.All, ox);
                                pv.RPC("RPCvib", RpcTarget.All);
                            }
                            else if (boxTrap == 4)
                            {
                                pv.RPC("RPCRanBoxUD", RpcTarget.All);
                            }
                            else
                            {
                                pv.RPC("RPCRanBoxTrapC", RpcTarget.All, boxTrap);
                            }

                        }
                        else { UseItem(myTem[0]); }

                        myTem.RemoveAt(0);
                        StartCoroutine(StopFR());
                    }

                }
                

                #region a
                    //�÷��� ���߷� ������
                    if (floating) { }  //Float();

                if (!tm.bH) { Grab(); }
                SetFree();
                UseItem();
                itemActive();
                Rot();
                PwUp();


                //��Ȧ �η�
                if (tm.bH) transform.position += tm.dir * tm.pullSpd * Time.deltaTime;
                pv.RPC("UDstate", RpcTarget.All);
//                if (tm.isUD) pp.udimg.SetActive(true);
  //              else pp.udimg.SetActive(false);

                ////���� ������ ���� �̱����̶� �׷��ǰ�?
                //if (goPlay.instance.MenuManager.activeSelf) { state = State.GameOver; }
                //else
                //{
                //    lrL.enabled = false;
                //    lrR.enabled = false;
                //}

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

    [PunRPC]
    void UDstate() {
        if (tm.isUD) pp.udimg.SetActive(true);
        else pp.udimg.SetActive(false);
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

        Vector2 joystickR = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
        if (joystickR.y > 0 || joystickR.y < 0)
        {
            transform.Rotate(joystickR.y * .2f, 0, 0);
        }

        if (joystickR.x > 0 || joystickR.x < 0)
        {
            transform.Rotate(0, 0, joystickR.x * .2f);
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

        //�����ʿ�
    void PwUp()
    {
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
                hitTF[0] = hitsL[0].transform;
                originP = hitTF[0].position;
            }

            walkR = false;
            walkL = true;
            origin = pp.handL.position;

            tm.up = true;

        }

        if (walkL)
        {
            if (hitsL.Length > 0)
            {
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
                hitTF[1] = hitsR[0].transform;
                originP = hitTF[1].position;
            }

            walkR = true;
            walkL = false;
            origin = pp.handR.position;

            tm.up = true;

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

    //���߷� ������ ���۷��� �ȳ����� �� �۵� ���ϴ°��� �˱�
    void Grab(Transform hitTFN, ref Transform handG, Transform otherH, ref Transform Hold, ref Transform Hold2, ref Transform Holdpre)
    {
        int idx = hitTFN.GetSiblingIndex();

        //�÷��̾� ������
        if (hitTFN.name.Contains("Player") && hitTFN !=transform) {
            floating = false;
            rb.isKinematic = true;

            if (tm.bH) { transform.position += tm.dir * tm.pullSpd * Time.deltaTime; }
            else if (tm.isUD) { transform.position += (origin + handG.position) + (hitTFN.position - originP); ; }
            else { transform.position += (origin - handG.position) + (hitTFN.position - originP); }

        }
        // Ȧ�� ��� �ִ� ��
        if (hitTFN.IsChildOf(rock))
        {
            floating = false;
            rb.isKinematic = true;

            if (tm.bH) { transform.position += tm.dir * tm.pullSpd * Time.deltaTime; }
            else if (tm.isUD) { transform.position += origin + handG.position; }
            else
            {
                transform.position += origin - handG.position;
            }

            Hold = hitTFN;
            if (Hold2 != null &&
                  Hold == Hold2) { tm.up = false; }

            if (Holdpre == Hold) { tm.up = false; }


                //Ʈ���̸�
                if (rp.holds[idx].type == Value.Type.Trap)
                {

                    if (tm.up)
                    {

                        if (rp.holds[idx].tT == Value.TrapType.BHoleL
                            || rp.holds[idx].tT == Value.TrapType.BholeR
                            )
                        {
                            tm.BHole(rp.holds[idx]);
                        }

                        else
                        if (rp.holds[idx].tT == Value.TrapType.Meteor)
                        {
                            tm.Create(tm.meteorFactory);
                        }
                        else
                        if (rp.holds[idx].tT == Value.TrapType.UpsideDown)
                        { 
                            StartCoroutine(tm.UD()); 
                        }
                        else 
                        {
                            tm.Create(tm.canFactory);
                        }
                        
                        tm.up = false;

                    }
                }

                //�����۵��̸�
                if (rp.holds[idx].type == Value.Type.Item)
                {
                    //�˾��� �ȵ� ���
                    if (!rp.holds[idx].popUp)
                    {
                        //rpc�� �ٲٱ�
                        pv.RPC("RpcPopUp", RpcTarget.All, idx);
                        
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
    void RpcPopUp (int idx)
    {
        // Ȱ��ȭ �Ѵ�.
        rp.holds[idx].rockItem.SetActive(true);
        // �˾� �Ǿ��ٰ� �˷��ش�.
        rp.holds[idx].popUp = true;

    }


    [PunRPC]
    void RpcItem(int hand, int itemIdx)
    {
        // �տ� ���� ������ Ư��
        GameObject item = rp.item.GetChild(itemIdx).gameObject;

        // ������� Ư��
        Transform handG = pp.handL;
        if (hand == 1) handG = pp.handR;

        //�����ڽ� Ư��
        byte rIdx = 0;
        if (maxP > 1) { rIdx = 1; }

        // �տ� ����
        //�� �����۸��� ���� ��ȣ�� �Ἥ ��Ʈ ����Ʈ�� ���� ����ϱ�
        if (item.name.Contains("Rope")) { CreateItem(rope, handG); mytem[hand] = 0; }
        if (item.name.Contains("Fire")) { CreateItem(fire, handG); mytem[hand] = 1; }
        if (item.name.Contains("Shield")) { CreateItem(shield, handG); mytem[hand] = 2; }
        if (item.name.Contains("Oxy")) { CreateItem(oxy, handG); mytem[hand] = 3; }
        if (item.name.Contains("Ran")) { CreateItem(ranBox[rIdx], handG); mytem[hand] = 4+ rIdx; }

        // ������ Ǯ ������ 30��
        item.SetActive(false);
        StartCoroutine(rp.ShowUp(itemIdx));
    
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
            //�� �۸���Ʈ�� ����ų� 1���� ���
            if (myTem.Count < 2)
            {

                int item = 1 << LayerMask.NameToLayer("Item");
                Collider[] obj = Physics.OverlapSphere(body.position + (Vector3.up * .05f), 0.5f, item);

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
                        SoundM.instance.playS(1, 10);
                        pv.RPC("RPCFind", RpcTarget.All, hand);
                        find = false;

                    }
                   
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
        if (getDBtn1R)
        {

            if (myTem.Count > 0)
            {
                if (myTem[0] == 4)
                {
                    //0 ��Ȧ 1 ȭ��ƮȦ 2 ���׿� 3 ĵ 4 �Ųٷ�
                    int ranboxIdx = Random.Range(0, 5);
                    UseItem(ranboxIdx);
                    pv.RPC("RPCUse", RpcTarget.All, 4);
                }
                else if (myTem[0] == 5) {
                   
                    int boxTrap = Random.Range(0, 4);
                    pv.RPC("RPCUse", RpcTarget.All, 5);
                    if (boxTrap == 0 || boxTrap == 1)
                    {
                        int ox = Random.Range(0, 2);
                        pv.RPC("RPCBlackHole", RpcTarget.All, ox);
                        pv.RPC("RPCvib", RpcTarget.All);
                    }
                    else if (boxTrap == 4) {
                       pv.RPC("RPCRanBoxUD", RpcTarget.All);
                    }
                    else
                        {
                            pv.RPC("RPCRanBoxTrapC", RpcTarget.All, boxTrap);
                        }

                }
                else { UseItem(myTem[0]); }
                
                myTem.RemoveAt(0);
                StartCoroutine(StopFR());
            }

        }

    }
    void UseItem(int item) {

        if (item == 0)
        {
            pv.RPC("RPCUse", RpcTarget.All, item);
            r = true;
            isH = true;
            SoundM.instance.playS(2, 2);
        }
        if (item == 1)
        {
            f = true;
            pv.RPC("RPCUse", RpcTarget.All, item);
        }

        if (item == 2)
        {
            rb.isKinematic = true;
            //��Ȧ �����
            tm.bH = false;
            //�Ųٷ� �����
            tm.isUD = false;
            pv.RPC("RPCUse", RpcTarget.All, item);
        }

        if (item == 3)
        {
            ps.PlusHp(30);
        }
    }

    void itemActive() {

 
        if (f)
        {
            rb.isKinematic = false;
            rb.velocity = -pp.handL.forward * fireSpd;
            if (!spdChange)  fireSpd = Mathf.Lerp(fireSpd, 2, 1 * Time.deltaTime) ; 
            else fireSpd = Mathf.Lerp(fireSpd, .1f, 3 * Time.deltaTime);

            pv.RPC("RPCFireEx", RpcTarget.All);
            

            if (getDBtn2R
                //���߷� ������
                ||  Input.GetKeyDown(KeyCode.Alpha4)
                ) {
                pv.RPC("RPCFireEFStop", RpcTarget.All);
            }
        }
        else
        {
            spdChange = false;
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

    [PunRPC]
    void RPCFireEFStop() {
        ParticleSystem ps = p.transform.GetComponent<ParticleSystem>();
        spdChange = !spdChange;
        if (spdChange) { ps.Stop(true); }
        else ps.Play(true);
    }

 // ������ ��� �� ��������ȭ �Լ�
 // 012 ����, ��ȭ��, ���� / 3 ��ũ
 [PunRPC]
    void RPCUse(int itIdx) {

        GameObject item;

        GameObject clone = shieldEFT;
        Transform handG = transform;

        if (itIdx == 0) { clone = hook; handG = pp.handR; }
        if (itIdx == 1) { clone = particle; handG = pp.handL; }
        if (itIdx == 3) { clone = hookP; handG = h.transform; }
        if (itIdx == 4 || itIdx == 5) { clone = ranEFT[itIdx-4]; }

        item = Instantiate(clone);
        item.transform.position = handG.position;
        if (itIdx == 4 || itIdx == 5) item.transform.position = handG.position + handG.forward*.5f;
        item.transform.forward = handG.forward;
        if (itIdx == 3) item.transform.forward = -handG.forward;

        if (itIdx == 1)  p = item;

        
        if (itIdx == 0) h = item;
        else if (itIdx == 4 || itIdx == 5) { }
        else Destroy(item, 5.5f);
    
    }

    [PunRPC]
    void RPCRLrFalse()
    {
        if(lr != null) lr.enabled = false;
        if (h != null ) h = null;
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




