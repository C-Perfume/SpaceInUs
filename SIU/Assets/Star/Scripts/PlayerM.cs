using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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

    //잡은거 위치
    Transform hitTF;
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
    GameObject hitObj;

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
                Walk();
                Rot();
                Open(0.1f, "Game");
                break;

            case State.GameStart:
                //플로팅
                if (floating) Float();

                if (!tM.bH) { Grab(); }
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

                        if (free.GetChild(i).position.x > transform.position.x * 5
                            || free.GetChild(i).position.x < transform.position.x * -5
                            || free.GetChild(i).position.y > transform.position.y * 5
                            || free.GetChild(i).position.y < transform.position.y * -5
                            || free.GetChild(i).position.z > transform.position.z * 5
                            || free.GetChild(i).position.z < transform.position.z * -5

                            )
                        {
                            Destroy(free.GetChild(i).gameObject);
                        }
                    }
                }

                break;

            case State.GameOver:
                //Click( 100, "Ready");
                break;

            case State.End:
                //Click( 100, "Ready");
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

    void Open( float m, string scene)
    {  

        if (Physics.Raycast(origin: my[(int)Parts.LHand].position, direction: my[(int)Parts.LHand].forward, out hit, m))// 0.5f))
        {

        hitObj = hit.transform.gameObject;
            if (hitObj.name == "Door")
            {

                doorIndi.SetActive(true);
                doorIndi.transform.position = hit.point;
                float dist = Vector3.Distance(
              Camera.main.transform.position,              hit.point);
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



        if (Physics.Raycast(origin: my[(int)Parts.RHand].position, direction: my[(int)Parts.RHand].forward, out hit, m ))//0.5f))
        {
          hitObj = hit.transform.gameObject;
            if (hitObj.name == "Door")
            {
                doorIndi2.SetActive(true);
                doorIndi2.transform.position = hit.point;
                float dist = Vector3.Distance(
             Camera.main.transform.position,             hit.point);
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

    void Click(float m, string scene)
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


    void CreateItem(GameObject clone, Transform hand)
    {
        GameObject a = Instantiate(clone);
        a.transform.SetParent(hand);
        a.transform.localPosition = Vector3.zero;
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
            Grap.Play();
            walkR = false;
            walkL = true;
            origin = my[(int)Parts.LHand].position;
        }

        if (walkL)
        {

            Collider[] hits = Physics.OverlapSphere(my[(int)Parts.LHand].position, 0.05f);


            if (hits.Length > 0)
            {
                hitTF = hits[0].transform;
                    GameObject hitObj = hitTF.gameObject;

                if (hitObj.name.Contains("Rock_01")) { }
                //홀드 잡고 있는 중
                if (hitTF.IsChildOf(rock))
                {
                    Rocks r = hits[0].GetComponent<Rocks>();
                    floating = false;
                    rb.isKinematic = true;

                    if (tM.up)
                    {

                        if (r.num == (int)Rocks.Type.Trap)
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

                        }

                    }

                    tM.up = false;
                if (tM.bH) transform.position += tM.dir * tM.pullSpd * Time.deltaTime;
                else { transform.position += origin - my[(int)Parts.LHand].position; }
                
                }


                // 아이템 잡을 때
                    if (hitTF.IsChildOf(item))
                    {
                    print(hitTF + " 잡기");
                        if (hitObj.name.Contains("Fire")) { CreateItem(fire, my[(int)Parts.LHand]); }
                        if (hitObj.name.Contains("Oxy")) { CreateItem(oxy, my[(int)Parts.LHand]); }
                        if (hitObj.name.Contains("Rope")) { CreateItem(rope, my[(int)Parts.LHand]); }
                        if (hitObj.name.Contains("Shield")) { CreateItem(shield, my[(int)Parts.LHand]); }

                        hitObj.SetActive(false);
                    }

                    if (hitTF.IsChildOf(free) || hitTF.IsChildOf(my[(int)Parts.RHand]))
                    {
                        Rigidbody itemRb = hitObj.GetComponent<Rigidbody>();
                        itemRb.isKinematic = true;
                        hitTF.SetParent(my[(int)Parts.LHand]);
                        hitTF.localPosition = Vector3.zero;
                    }
                

            }
        }


        if (getUBtnIdxL)
        {
            walkL = false;

            // 잡은게 없으면 리턴
            if (hitTF == null)
            {
                return;
            }

            // 홀드라면
            if (hitTF.IsChildOf(rock))
            {

                rb.isKinematic = false;
                rb.velocity = -transform.TransformDirection(getVelL) * vPower;
                hitTF = null;

            }

            // 아이템 잡았다면
            if (my[(int)Parts.LHand].childCount > 2)
            {
                // 손의 마지막 자식은 hitem
                GameObject hItem = my[(int)Parts.LHand].GetChild(my[(int)Parts.LHand].childCount - 1).gameObject;
                Collider[] objs = Physics.OverlapSphere(my[(int)Parts.LHand].position, 0.1f);

                // 손반경으로 걸리는 오브젝트가 1개이상이면
                if (objs.Length > 0)
                {
                    Transform objTF = objs[0].transform;
                    print(objs[0].name + " 0번");

                    //0번째가 몸이라면
                    if (objTF == my[(int)Parts.Body])
                    {

                        // 획득 아이템이 2개 미만이면
                        if (myItem.Count < 2)
                        {
                            // 획득리스트에 넣는다.
                            myItem.Add(hItem);
                            myItem[0].transform.position = Vector3.zero;
                            // 안보이게 한다.
                            Color c = new Color();
                            c.a = 0;
                            myItem[0].GetComponent<MeshRenderer>().material.color = c;
                            myItem[0].SetActive(false);
                            if (myItem[1] != null) { myItem[1].transform.position = Vector3.zero;
                                myItem[1].GetComponent<MeshRenderer>().material.color = c;
                                myItem[1].SetActive(false);
                            }
                        }
                        //아니면 없엔다.
                        else
                        {
                            Destroy(hItem);
                        }

                    }
                    //몸에 넣지 않으면
                    else
                    {

                        //던지자
                        Rigidbody itemRb = hItem.GetComponent<Rigidbody>();
                        itemRb.isKinematic = false;

                            itemRb.velocity = getVelL * vPower;
                            itemRb.angularVelocity = getAngVelL;


                        hItem.transform.SetParent(free);
                        print(hItem);
                    }


                    objTF = null;
                }
                
            }

        }


        // 오른손 움직임
        if (getDBtnIdxR)
        {
            Grap.Play();
            walkR = true;
            walkL = false;
            origin = my[(int)Parts.RHand].position;
        }

        if (walkR)
        {

            Collider[] hits = Physics.OverlapSphere(my[(int)Parts.RHand].position, 0.05f);


            if (hits.Length > 0)
            {
                hitTF = hits[0].transform;
                GameObject hitObj = hitTF.gameObject;

                if (hitObj.name.Contains("Rock_01")) { }
                //홀드 잡고 있는 중
                if (hitTF.IsChildOf(rock))
                {
                    Rocks r = hits[0].GetComponent<Rocks>();
                    floating = false;
                    rb.isKinematic = true;

                    // 트랩 작동
                    if (tM.up)
                    {

                        if (r.num == (int)Rocks.Type.Trap)
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

                        }

                    }

                    tM.up = false;
                if (tM.bH) transform.position += tM.dir * tM.pullSpd * Time.deltaTime;
                else { transform.position += origin - my[(int)Parts.RHand].position; }
                }




                // 아이템 잡을 때
                if (hitTF.IsChildOf(item))
                {
                    if (hitObj.name.Contains("Fire")) { CreateItem(fire, my[(int)Parts.RHand]); }
                    if (hitObj.name.Contains("Oxy")) { CreateItem(oxy, my[(int)Parts.RHand]); }
                    if (hitObj.name.Contains("Rope")) { CreateItem(rope, my[(int)Parts.RHand]); }
                    if (hitObj.name.Contains("Shield")) { CreateItem(shield, my[(int)Parts.RHand]); }

                    hitObj.SetActive(false);
                } 

                if (hitTF.IsChildOf(free) || hitTF.IsChildOf(my[(int)Parts.LHand]))
                {
                    Rigidbody itemRb = hitObj.GetComponent<Rigidbody>();
                    itemRb.isKinematic = true;
                    hitTF.SetParent(my[(int)Parts.RHand]);
                    hitTF.localPosition = Vector3.zero;
                } 




            }
        }

        if (getUBtnIdxR)
        {
           
            walkR = false;

            // 잡은게 없으면 리턴
            if (hitTF == null)
            {
                return;
            }

            // 홀드라면
            if (hitTF.IsChildOf(rock))
            {

                rb.isKinematic = false;
                rb.velocity = -transform.TransformDirection(getVelR) * vPower;
                hitTF = null;

            }

            // 아이템 잡았다면
            if (my[(int)Parts.RHand].childCount > 3)
            {
                // 손의 마지막 자식은 hitem
                GameObject hItem = my[(int)Parts.RHand].GetChild(my[(int)Parts.RHand].childCount - 1).gameObject;
                Collider[] objs = Physics.OverlapSphere(my[(int)Parts.RHand].position, 0.1f);

                // 손반경으로 걸리는 오브젝트가 1개이상이면
                if (objs.Length > 0)
                {
                    Transform objTF = objs[0].transform;
                    print(objs[0].name + " 0번 오른손 사용중");

                    //0번째가 몸이라면
                    if (objTF == my[(int)Parts.Body])
                    {

                        // 획득 아이템이 2개 미만이면
                        if (myItem.Count < 2)
                        {
                            // 획득리스트에 넣는다.
                            myItem.Add(hItem);
                            myItem[0].transform.position = Vector3.zero;
                          
                            // 안보이게 한다.
                            Color c = new Color();
                            c.a = 0;
                            myItem[0].GetComponent<MeshRenderer>().material.color = c;
                            myItem[0].SetActive(false);
                            
                            if (myItem[1] != null)
                            {
                                myItem[1].transform.position = Vector3.zero;
                                myItem[1].GetComponent<MeshRenderer>().material.color = c;
                                myItem[1].SetActive(false);
                            }

                        }
                        //아니면 없엔다.
                        else
                        {
                            Destroy(hItem);

                        }

                    }
                    //몸에 넣지 않으면
                    else
                    {

                        //던지자
                        Rigidbody itemRb = hItem.GetComponent<Rigidbody>();
                        itemRb.isKinematic = false;

                        itemRb.velocity = getVelR * vPower;
                        itemRb.angularVelocity = getAngVelR;


                        hItem.transform.SetParent(free);
                        print(hItem+"오른손 출발");
                    }


                    objTF = null;
                }

            }
        }


    }



    


}

