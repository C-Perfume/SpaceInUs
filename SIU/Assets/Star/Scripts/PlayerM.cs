using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class PlayerM : MonoBehaviour
{
    // 카메라리그에 붙는 스크립트라는 전제로 왼손 / 오른손 변수잡고 시작하기

    // 네트워크 위치값 전송용
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

    //걷기 잡기
    Vector3 origin;
    Vector3 pos;
    bool walkL = false;
    bool walkR = false;

    //문표시 아이콘
    public GameObject doorIndi;
    public GameObject doorIndi2;

    // 속력
    public bool floating = true;
    float vPower = .5f;

    //잡은거 손위치
    Transform[] hitTF = new Transform[2];

    //손에 잡은 오브젝트의 트랜스폼
    Transform catchHoldL;
    Transform catchHoldLpre;
    Transform catchHoldR;
    Transform catchHoldRpre;
    Transform catchItem;

    // 홀드찾기용
    public Transform rock;

    //아이템 찾기용
    public Transform item;
    public Transform free;

    //아이템 생성용
    public GameObject rope;
    public GameObject fire;
    public GameObject shield;
    public GameObject oxy;

    //획득아이템리스트
    public List<GameObject> myItem = new List<GameObject>();

    //카메라 리그의 리지드바디를 가져오자
    Rigidbody rb;

    //함정용 변수
    TrapManager tM;

    RaycastHit hit;

    // open 함수에 쓰이는 손에 잡은 오브젝트
    GameObject hitObj;

    // 클리어 도어로 가기 전 위치조정
    public Transform FootStepTransform;

    //잡는소리
    public AudioSource Grap;
    #region 컨트롤러 bool Vector3설정
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

    goPlay gp;
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
        gp = GetComponent<goPlay>();
    }

    void Update()
    {
        #region 컨트롤러 bool
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
                floating = false;
                // 풋스텝에서 멀리 떨어지면 다시 게임스타트로 되돌아가기 - 나중에 하자.
                //if (SceneManager.GetActiveScene().name != "Ready") { }

                    break;

            case State.GameStart:
                //플로팅
                if (floating) Float();

                if (!tM.bH) { Grab(); }
                SetFree();
                Rot();
                PwUp();

                //블랙홀 인력
                if (tM.bH) transform.position += tM.dir * tM.pullSpd * Time.deltaTime;

                //아이템 작용
                if (myItem.Count > 0)
                {

                    if (getDBtn1R)
                    {
                        print("아이템 사용");
                        myItem[0].SetActive(true);
                        GameObject used = myItem[0];
                        ItemM itm = myItem[0].GetComponent<ItemM>();
                        itm.active = true;
                        myItem.RemoveAt(0);
                        Destroy(used, 6);
                    }

                }

                //멀리 간 아이템 없에기
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

                if (gp.MenuManager.activeSelf) { state = State.GameOver; }
                break;

            case State.GameOver:
                Click(100);
                if (!gp.MenuManager.activeSelf) { state = State.GameStart; }

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


    void Walk()  //움직임을 위한 불 변수가 있었어야 했는데 그걸 생각 못해서 몇시간 고생했다 에휴
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

    void Walk(int audioNum)  //움직임을 위한 불 변수가 있었어야 했는데 그걸 생각 못해서 몇시간 고생했다 에휴
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
            if (hitObj.layer == LayerMask.NameToLayer("UI") )
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
    { //수정필요
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

            tM.up = true;
            if (catchHoldL != null &&
               catchHoldR != null &&
               catchHoldL == catchHoldR) { tM.up = false; }
           
        }

        if (walkL)
        {
            Collider[] hitsL = Physics.OverlapSphere(my[(int)Parts.LHand].position, 0.03f);

            if (hitsL.Length > 0)
            {
                hitTF[0] = hitsL[0].transform;

                if (hitTF[0].gameObject.name.Contains("Rock_01")) { }
                Grab(hitTF[0], my[(int)Parts.LHand], my[(int)Parts.RHand], ref catchHoldL, catchHoldLpre);
            }
        }

        // 오른손 움직임
        if (getDBtnIdxR)
        {
            walkR = true;
            walkL = false;
            origin = my[(int)Parts.RHand].position;

            tM.up = true;
            if (catchHoldL != null &&
               catchHoldR != null &&
               catchHoldL == catchHoldR) { tM.up = false; }
          
        }

        if (walkR)
        {
            Collider[] hitsR = Physics.OverlapSphere(my[(int)Parts.RHand].position, 0.03f);

            if (hitsR.Length > 0)
            {
                hitTF[1] = hitsR[0].transform;

                if (hitTF[1].gameObject.name.Contains("Rock_01")) { }

                Grab(hitTF[1], my[(int)Parts.RHand], my[(int)Parts.LHand], ref catchHoldR, catchHoldRpre);
            }

        }

    }


    void Grab(Transform hitTFN, Transform handG, Transform otherH, ref Transform Hold, Transform Holdpre )
    {
        Grap.Play();

        // 홀드 잡고 있는 중
        if (hitTFN.IsChildOf(rock))
        {
            Hold = hitTFN;
            if (Holdpre == Hold) { tM.up = false; }

            Rocks r = hitTFN.GetComponent<Rocks>();
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
            else { transform.position += origin - handG.position; }





        }else


        // 아이템 잡을 때 손에 생성
        if (hitTFN.IsChildOf(item))
        {

            Grap.Play();
            if (hitTFN.gameObject.name.Contains("Fire")) { CreateItem(fire, handG); }
            if (hitTFN.gameObject.name.Contains("Oxy")) { CreateItem(oxy, handG); }
            if (hitTFN.gameObject.name.Contains("Rope")) { CreateItem(rope, handG); }
            if (hitTFN.gameObject.name.Contains("Shield")) { CreateItem(shield, handG); }

            hitTFN.gameObject.SetActive(false);

        }else

        // 던져진 아이템이나 다른 손 아이템을 잡으면
        if (hitTFN.IsChildOf(free) || hitTFN.IsChildOf(otherH))
        {
            catchItem = hitTFN;

            Grap.Play();
            // 잡은 손으로 자식 만들고 0점으로 이동시키기
            catchItem.SetParent(handG);
            hitTFN.localPosition = Vector3.zero;

            // 물리 작용 off
            Rigidbody itemRb = hitTFN.gameObject.GetComponent<Rigidbody>();
            itemRb.isKinematic = true;
        }


    }

    //아이템 손에 만들기
    void CreateItem(GameObject clone, Transform hand)
    {
        GameObject a = Instantiate(clone);
        a.transform.SetParent(hand);
        a.transform.localPosition = Vector3.zero;
        catchItem = a.transform;
    }


       //놨을 때
    void SetFree()
    {

        //왼손
        if (getUBtnIdxL)
        {
            walkL = false;

            // 잡은게 없으면 리턴
            if (hitTF[0] == null) { return; }

            SetFree(hitTF[0], catchHoldL, my[(int)Parts.LHand], getVelL, getAngVelL);
            hitTF[0] = null;
            catchHoldLpre = catchHoldL;
        }

        //오른손
        if (getUBtnIdxR)
        {

            walkR = false;

            // 잡은게 없으면 리턴
            if (hitTF[1] == null)
            {
                return;
            }

            SetFree(hitTF[1], catchHoldR, my[(int)Parts.RHand], getVelR, getAngVelR);
            hitTF[1] = null;
            catchHoldRpre = catchHoldR;
        }
    }

    void SetFree(Transform hitTFN, Transform hold, Transform hand, Vector3 vel, Vector3 angVel)
    {

        // 홀드라면
        if (hitTFN == hold)
        {

            rb.isKinematic = false;
            rb.velocity = -transform.TransformDirection(vel) * vPower;

        }

        // 아이템 잡았다면
        if (catchItem != null && hitTFN == catchItem)
        {
            Rigidbody itemRb = catchItem.GetComponent<Rigidbody>();

            //두개 이하
            if (myItem.Count < 2)
            {

                Collider[] obj = Physics.OverlapSphere(my[(int)Parts.Body].position, 0.5f);
                print(obj[0].name);
                if (obj[0].transform.name == catchItem.name)
                {  
                 

                    // 획득리스트에 넣는다.
                    myItem.Add(catchItem.gameObject);

                    // 멀리 날려 안보이게 하자
                    myItem[0].transform.position = new Vector3(1000, 1000, 1000);
                    if (myItem.Count > 1) myItem[1].transform.position = new Vector3(1000, 1000, 1000);

                }
                //아이템이 0번이 아니면 던지자
                else
                {
                    itemRb.isKinematic = false;

                    itemRb.velocity = vel * vPower;
                    itemRb.angularVelocity = angVel;


                    catchItem.transform.SetParent(free);
                    print(obj[0].name + "이 먼저라 공중부양");
                }




            }
            // 2개 이상이면 던지자.
            else
            {
                itemRb.isKinematic = false;

                itemRb.velocity = vel * vPower;
                itemRb.angularVelocity = angVel;


                catchItem.transform.SetParent(free);
                print(catchItem + " 2개 이상");
            }


        }


        // 풋스텝을 잡고 놓은 상태라면
        if (hitTFN.gameObject.name.Contains("foot"))
        {
         
            Vector3.Lerp(transform.position, FootStepTransform.position, 3);
            Quaternion.Lerp(transform.rotation, FootStepTransform.rotation, 3);
           state = State.Ready;
            
        }
        

    }


}




