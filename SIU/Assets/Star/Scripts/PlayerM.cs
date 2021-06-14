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
    byte maxP;
    NetManager net;
    public GameObject hpbar;
   public GameObject readytext;

    public State state;

    #region//걷기 잡기
    Vector3 origin;
    Vector3 originP;
    Vector3 pos;
    bool walkL = false;
    bool walkR = false;
    #endregion
   
    #region //문표시 아이콘
    Transform doorCan;
    GameObject doorIndi;
    GameObject doorIndi2;
    // 클릭 라인렌더러 양손
    LineRenderer lrL;
    LineRenderer lrR;
    #endregion

    #region // 속력


    public bool floating = true;
    float vPower = .5f;
    #endregion
    
    #region//그랩 손위치


    Transform[] hitTF = new Transform[2];

    //손에 잡은 오브젝트의 트랜스폼
    Transform catchHoldL;
    Transform catchHoldLpre;
    Transform catchHoldR;
    Transform catchHoldRpre;
    Transform catchItem;

    //그랩 종류구분
    RockParent rp;
    // 홀드찾기용
    Transform rock;
    #endregion

    #region    //아이템 인벤토리 넣기용
    bool find = false;

    //아이템 생성용
    public GameObject rope;
    public GameObject fire;
    public GameObject shield;
    public GameObject oxy;
    public GameObject[] ranBox;

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
    // 소화기 사용시 속도
    public float fireSpd = .1f;
    //소화기 멈췄다 다시쓰기
    public bool spdChange = false;

    //로프 사용할 때 씀 + 산소데미지 안주게 퍼블릭
    public bool r = false;
    //로프 복제용 후크
    public GameObject hook;
    GameObject h;
    //후크 박힐 때 파티클+소리 복제용
    public GameObject hookP;
    bool isH = true;
    // 쉴드 사용 시 나오는 이펙트
    public GameObject shieldEFT;
    //랜덤박스 사용 시 나오는 것..
    public GameObject[] ranEFT;
#endregion
    
    #region //기타 함정
  
    //함정용 변수
    TrapManager tm;
 
    //카메라 리그의 리지드바디를 가져오자
    Rigidbody rb;
    Player ps;
    
    // open 함수에 쓰이는 손에 잡은 오브젝트
    GameObject hitObj;
    RaycastHit hit;

    // 클리어 도어로 가기 전 위치조정
    Transform FootStepTransform;
    bool fStep = false;
    #endregion

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
        #region 초기값 세팅
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
            
            // 지정인원이 안들어 온 경우 wait 상태로 기다리기
            state = PlayerM.State.Wait;
            //배경맵 안보이게 멀리 보내기 >> 비활성화 시 나중에 들어오는 경우 찾지 못함
            hpbar.SetActive(false);

        StartCoroutine(WaitforStart());
        }

        else if (SceneManager.GetActiveScene().name == "Ready")
        { state = State.Ready; }

        else
        { state = State.GameOver; }

    }

    IEnumerator WaitforStart() {
        // 최대인원이 아닌 경우 기다린다.
        while (maxP != net.playerList.Count)
        {         
        yield return new WaitForSeconds(.01f);
        }

        yield return new WaitForSeconds(1);
        // 최대 인원이면 게임 스타트, 배경맵 돌아오기, 우주선 터지는 거 활성화 및 플레이어 밑으로 위치이동
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
            #endregion

                #region //치트키

                if (getDBtn1L) { SceneManager.LoadScene("Meteo"); }
                if (getDBtn2L) { SceneManager.LoadScene("Clear"); }


                // 개발 수정중 키보드조작
                float v = Input.GetAxis("Vertical");
                float h = Input.GetAxis("Horizontal");
                float ff = 0;
                if (Input.GetKey(KeyCode.Space)) { ff = .1f; }
                if (Input.GetKey(KeyCode.LeftControl)) { ff = -.1f; }
                Vector3 dir = new Vector3(h, ff, v);
                transform.position += dir * 10 * Time.deltaTime;
                #endregion

                 //돌집기
                if (Input.GetKeyDown(KeyCode.F1)) {
                     //차일드2번
                    int idx = 2;

                    //속성이 아이템이면
                    if (rp.holds[idx].type == Value.Type.Item)
                    {
                        //팝업이 안된 경우
                       if(!rp.holds[idx].popUp)
                        {
                            //rpc로 바꾸기
                            pv.RPC("RpcPopUp", RpcTarget.All, idx);
                            
                        }
                    
                    }
                   
                }
                //아이템 집기
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    if (rp.holds[2].popUp)
                    {
                        pv.RPC("RpcItem", RpcTarget.All, 1, 0);
                    }
                }
                //아이템 인벤 보관
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
                        print("걸린게 없어서  공중부양");
                    }
                }
                //사용
                 if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    if (myTem.Count > 0)
                    {
                        //1인용
                        if (myTem[0] == 4)
                        {
                            int ranboxIdx = Random.Range(0, 4);
                            UseItem(ranboxIdx);
                            pv.RPC("RPCUse", RpcTarget.All, 4);
                        }
                        //2인용
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
                    //플로팅 개발로 수정중
                    if (floating) { }  //Float();

                if (!tm.bH) { Grab(); }
                SetFree();
                UseItem();
                itemActive();
                Rot();
                PwUp();


                //블랙홀 인력
                if (tm.bH) transform.position += tm.dir * tm.pullSpd * Time.deltaTime;
                pv.RPC("UDstate", RpcTarget.All);
//                if (tm.isUD) pp.udimg.SetActive(true);
  //              else pp.udimg.SetActive(false);

                ////들어갔다 나가면 오류 싱글톤이라 그런건가?
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

        //수정필요
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

        // 오른손 움직임
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

                if (hitTF[1].gameObject.name.Contains("Big")) { print("문제0 big"); }

                Grab(hitTF[1], ref pp.handR, pp.handL, ref catchHoldR, ref catchHoldL, ref catchHoldRpre);

            }

        }

    }

    //개발로 수정중 레퍼런스 안넣으면 왜 작동 안하는건지 알기
    void Grab(Transform hitTFN, ref Transform handG, Transform otherH, ref Transform Hold, ref Transform Hold2, ref Transform Holdpre)
    {
        int idx = hitTFN.GetSiblingIndex();

        //플레이어 잡으면
        if (hitTFN.name.Contains("Player") && hitTFN !=transform) {
            floating = false;
            rb.isKinematic = true;

            if (tm.bH) { transform.position += tm.dir * tm.pullSpd * Time.deltaTime; }
            else if (tm.isUD) { transform.position += (origin + handG.position) + (hitTFN.position - originP); ; }
            else { transform.position += (origin - handG.position) + (hitTFN.position - originP); }

        }
        // 홀드 잡고 있는 중
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


                //트랩이면
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

                //아이템돌이면
                if (rp.holds[idx].type == Value.Type.Item)
                {
                    //팝업이 안된 경우
                    if (!rp.holds[idx].popUp)
                    {
                        //rpc로 바꾸기
                        pv.RPC("RpcPopUp", RpcTarget.All, idx);
                        
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
    void RpcPopUp (int idx)
    {
        // 활성화 한다.
        rp.holds[idx].rockItem.SetActive(true);
        // 팝업 되었다고 알려준다.
        rp.holds[idx].popUp = true;

    }


    [PunRPC]
    void RpcItem(int hand, int itemIdx)
    {
        // 손에 잡힌 아이템 특정
        GameObject item = rp.item.GetChild(itemIdx).gameObject;

        // 어떤손인지 특정
        Transform handG = pp.handL;
        if (hand == 1) handG = pp.handR;

        //랜덤박스 특정
        byte rIdx = 0;
        if (maxP > 1) { rIdx = 1; }

        // 손에 복제
        //각 아이템마다 고유 번호를 써서 인트 리스트를 통해 사용하기
        if (item.name.Contains("Rope")) { CreateItem(rope, handG); mytem[hand] = 0; }
        if (item.name.Contains("Fire")) { CreateItem(fire, handG); mytem[hand] = 1; }
        if (item.name.Contains("Shield")) { CreateItem(shield, handG); mytem[hand] = 2; }
        if (item.name.Contains("Oxy")) { CreateItem(oxy, handG); mytem[hand] = 3; }
        if (item.name.Contains("Ran")) { CreateItem(ranBox[rIdx], handG); mytem[hand] = 4+ rIdx; }

        // 아이템 풀 돌리기 30초
        item.SetActive(false);
        StartCoroutine(rp.ShowUp(itemIdx));
    
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
            //내 템리스트가 비엇거나 1개인 경우
            if (myTem.Count < 2)
            {

                int item = 1 << LayerMask.NameToLayer("Item");
                Collider[] obj = Physics.OverlapSphere(body.position + (Vector3.up * .05f), 0.5f, item);

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
                        SoundM.instance.playS(1, 10);
                        pv.RPC("RPCFind", RpcTarget.All, hand);
                        find = false;

                    }
                   
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
        if (getDBtn1R)
        {

            if (myTem.Count > 0)
            {
                if (myTem[0] == 4)
                {
                    //0 블랙홀 1 화이트홀 2 메테오 3 캔 4 거꾸로
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
            //블랙홀 지우기
            tm.bH = false;
            //거꾸로 지우기
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
                //개발로 수정중
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

 // 아이템 사용 시 복제동기화 함수
 // 012 로프, 소화기, 쉴드 / 3 후크
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




