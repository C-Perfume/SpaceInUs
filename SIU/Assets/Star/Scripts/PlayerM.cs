using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;


public class PlayerM : MonoBehaviour
{
    // 카메라리그에 붙는 스크립트라는 전제로 왼손 / 오른손 변수잡고 시작하기

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

    //걷기 잡기
    Vector3 origin;
    Vector3 pos;
    bool walkL = false;
    bool walkR = false;

    //문표시 아이콘
    Transform doorCan;
    GameObject doorIndi;
    GameObject doorIndi2;
    // 클릭 라인렌더러 양손
    LineRenderer lrL;
    LineRenderer lrR;

    // 속력
    public bool floating = true;
    float vPower = .5f;

    //그랩 손위치
    Transform[] hitTF = new Transform[2];

    //손에 잡은 오브젝트의 트랜스폼
    Transform catchHoldL;
    Transform catchHoldLpre;
    Transform catchHoldR;
    Transform catchHoldRpre;
    Transform catchItem;

    //아이템 인벤토리 넣기용
    bool find = false;

    //그랩 종류구분
    RockParent rp;
    // 홀드찾기용
    Transform rock;

    //아이템 생성용
    public GameObject rope;
    public GameObject fire;
    public GameObject shield;
    public GameObject oxy;

    //획득아이템리스트
    public List<int> myTem = new List<int>();
     int[] mytem = { -1, -1 };

    // 아이템 로프 사용할 때 쓰는 lr >> player에 붙어있음
    LineRenderer lr;
    //소화기 사용할 때 씀
    bool f = false;
    // 소화기 파티클
    public GameObject particle;
    GameObject p;

    //로프 사용할 때 씀 + 산소데미지 안주게 퍼블릭
    public bool r = false;
    //로프 복제용 후크
    float ropeSpd = .05f;
    public GameObject hook;
    GameObject h;
    //후크 박힐 때 파티클+소리 복제용
    public GameObject hookP;
    bool isH = true;
    // 쉴드 사용 시 나오는 이펙트
    public GameObject shieldEFT;


    //카메라 리그의 리지드바디를 가져오자
    Rigidbody rb;

    //함정용 변수
    TrapManager tM;
    Player ps;

    RaycastHit hit;

    // open 함수에 쓰이는 손에 잡은 오브젝트
    GameObject hitObj;

    // 클리어 도어로 가기 전 위치조정
    Transform FootStepTransform;
    bool fStep = false;

    #region 컨트롤러 bool Vector3설정
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
        #region 컨트롤러 bool
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

                    // 풋스텝에서 멀리 떨어지면 다시 게임스타트로 되돌아가기 - 나중에 하자.

                    floating = false;

                    if (fStep)
                    {
                        print("fStep 위치 이동 작동?");
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

                //치트키
                if (getDBtn1L) { SceneManager.LoadScene("Meteo"); }
                if (getDBtn2L) { SceneManager.LoadScene("Clear"); }


                // 개발 수정중 키보드조작
                float v = Input.GetAxis("Vertical");
                float h = Input.GetAxis("Horizontal");
                float f = 0;
                if (Input.GetKey(KeyCode.Space)) { f = .1f; }
                if (Input.GetKey(KeyCode.LeftControl)) { f = -.1f; }
                Vector3 dir = new Vector3(h, f, v);
                transform.position += dir * 10 * Time.deltaTime;

                //플로팅
                // 개발로 수정중
                if (floating) { }  //Float();

                if (!tM.bH) { Grab(); }
                SetFree();
                UseItem();
                itemActive();
                Rot();
                PwUp();


                //개발 수정중
                //  if (goPlay.instance.MenuManager.activeSelf) { state = State.GameOver; }
                // else
                //{
                //  lrL.enabled = false;
                //lrR.enabled = false;
                //}

                //블랙홀 인력
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


    //레디씬에서 사용 Y값 고정
    void Walk()  //움직임을 위한 불 변수가 있었어야 했는데 그걸 생각 못해서 몇시간 고생했다 에휴
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

    // 게임 클리어 도어를 향한 움직임
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
        // 방향전환
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

    //도어오픈 + 씬전환
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

    // 메뉴선택 게임오버든 뭐든 이걸로 사용
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
    { //수정필요
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

        // 오른손 움직임
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

                if (hitTF[1].gameObject.name.Contains("Big")) { print("문제0 big"); }

                Grab(hitTF[1], ref pp.handR, pp.handL, ref catchHoldR, ref catchHoldL, ref catchHoldRpre);

            }

        }

    }

    //개발로 수정중 레퍼런스 안넣으면 왜 작동하는건지 알기
    void Grab(Transform hitTFN, ref Transform handG, Transform otherH, ref Transform Hold, ref Transform Hold2, ref Transform Holdpre)
    {
        int idx = hitTFN.GetSiblingIndex();

        // 홀드 잡고 있는 중
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
            //0 : 왼손, 1 : 오른손
            int hand = 1;
            if (walkL) hand = 0;
            // 0 = rp.free, 1 = 다른손
            int pa = 0;

            // 아이템 잡을 때 손에 생성
            if (hitTFN.IsChildOf(rp.item))
            {
                pv.RPC("RpcItem", RpcTarget.All, hand, idx);
            }

            // 던져진 아이템이나 다른 손 아이템을 잡으면
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
        // 손에 잡힌 아이템 특정
        GameObject item = rp.item.GetChild(itemIdx).gameObject;

        // 어떤손인지 특정
        Transform handG = pp.handL;

        if (hand == 1) handG = pp.handR;

        // 손에 복제
        //각 아이템마다 고유 번호를 써서 인트 리스트를 통해 사용하기
        if (item.name.Contains("Rope")) { CreateItem(rope, handG); mytem[hand] = 0; }
        if (item.name.Contains("Fire")) { CreateItem(fire, handG); mytem[hand] = 1; }
        if (item.name.Contains("Shield")) { CreateItem(shield, handG); mytem[hand] = 2; }
        if (item.name.Contains("Oxy")) { CreateItem(oxy, handG); mytem[hand] = 3; }

        // 아이템 풀 돌리기 30초
        rp.item_False.Add(item);
        item.SetActive(false);
        rp.items.Remove(item);
        StartCoroutine(rp.ShowUp());
    }

    //아이템 손에 만들기 함수
    void CreateItem(GameObject clone, Transform hand)
    {
        GameObject a = Instantiate(clone);
        catchItem = a.transform;
        catchItem.SetParent(hand);
        catchItem.localPosition = Vector3.zero;
    }


    //공중에 뜬 / 다른손의 아이템
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

        //공중에 뜬 아이템 잡으면 템리스트에 넣을 인트값 변경
        if (parent == 0) {
            for (int i = 0; i < 4; i++)
            {
                if (catchItem.name.Contains(i.ToString())) {
                    mytem[hand] = i;
                    break;
                }
            }
        }

        // 물리 작용 off
        Rigidbody itemRb = item.GetComponent<Rigidbody>();
        itemRb.isKinematic = true;
    }




    //놨을 때
    void SetFree()
    {
        //0 : 왼손, 1 : 오른손
        int hh = 0;
        //왼손
        if (getUBtnIdxL)
        {
            walkL = false;

            // 잡은게 없으면 리턴
            if (hitTF[0] == null)
            {
                return;
            }

            SetFree(hitTF[0], catchHoldL, pp.my[(int)PlayerPhoton.Parts.Body], getVelL, getAngVelL, hh);
            hitTF[0] = null;
            catchHoldLpre = catchHoldL;
        }

        //오른손
        if (getUBtnIdxR)
        {
            hh = 1;
            walkR = false;

            // 잡은게 없으면 리턴
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

        // 홀드라면
        if (hitTFN == hold)
        {
            rb.isKinematic = false;
            rb.velocity = -transform.TransformDirection(ve) * vPower;

        }


        // 아이템 잡았다면
        if (catchItem != null && hitTFN == catchItem)
        {
            //두개 이하
            if (myTem.Count < 2)
            {

                int item = 1 << LayerMask.NameToLayer("Item");
                Collider[] obj = Physics.OverlapSphere(body.position + (Vector3.up * .01f), 0.3f, item);

                //1개이상   
                if (obj.Length > 0)
                {

                    for (int i = 0; i < obj.Length; i++)
                    {
                        print(i + "번    " + obj[i].name);
                    }

                    //찾기
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
                    //    print("포문으로 찾아지지 않음");
                    //}

                }

                //0개
                else
                {
                    pv.RPC("RPCFree", RpcTarget.AllBuffered, ve, angVe);
                    print("걸린게 없어서  공중부양");
                }


            }
            // 2개 이상이면 던지자.
            else
            {
                pv.RPC("RPCFree", RpcTarget.AllBuffered, ve, angVe);
                print("아이템이  2개 이상");
            }


        }


        // 풋스텝을 잡고 놓은 상태라면
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

        //아이템 작용
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
                    //블랙홀 지우기
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

    // 아이템 사용 시 복제동기화 함수
    // 012 로프, 소화기, 쉴드 / 3 후크
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

    //rpc해야될 것!

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




