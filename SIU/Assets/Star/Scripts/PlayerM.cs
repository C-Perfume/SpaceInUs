using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerM : MonoBehaviour
{
    // 朝五虞軒益拭 細澗 什滴験闘虞澗 穿薦稽 図謝 / 神献謝 痕呪説壱 獣拙馬奄

    // 革闘趨滴 是帖葵 穿勺遂
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

    //鞍奄 説奄
    Vector3 origin;
    Vector3 pos;
    bool walkL = false;
    bool walkR = false;

    //庚妊獣 焼戚嬬
    public GameObject doorIndi;
    public GameObject doorIndi2;

    // 紗径
    public bool floating = true;
    float vPower = 1f;

    //説精暗 是帖
    Transform hitTF;
    // 筈球達奄遂
    public Transform rock;

    //焼戚奴 達奄遂
    public Transform item;
    public Transform free;

    //焼戚奴 持失遂
    public GameObject rope;
    public GameObject fire;
    public GameObject shield;
    public GameObject oxy;

    //塙究焼戚奴軒什闘
    public List<GameObject> myItem = new List<GameObject>();

    //朝五虞 軒益税 軒走球郊巨研 亜閃神切
    Rigidbody rb;

    //敗舛遂 痕呪
    TrapManager tM;

    RaycastHit hit;
    GameObject hitObj;

    //説澗社軒
    public AudioSource Grap;
    #region 珍闘継君 bool Vector3竺舛
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
        #region 珍闘継君 bool
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
                Open(0.5f, "Game");
                break;

            case State.GameStart:
                //巴稽特
                if (floating) Float();

                if (!tM.bH) { Grab(); }
                Rot();
                PwUp();

                //鷺窟筈 昔径
                if (tM.bH) transform.position += tM.dir * tM.pullSpd * Time.deltaTime;

                //焼戚奴 拙遂
                if (myItem.Count > 0)
                {

                    if (getDBtn1R)
                    {
                        print("焼戚奴 紫遂");
                        myItem[0].SetActive(true);
                        GameObject used = myItem[0];
                        ItemM itm = myItem[0].GetComponent<ItemM>();
                        itm.active = true;
                        myItem.RemoveAt(0);
                        Destroy(used, 6);
                    }

                }

                //菰軒 娃 焼戚奴 蒸拭奄
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


    void Walk()  //崇送績聖 是廃 災 痕呪亜 赤醸嬢醤 梅澗汽 益杏 持唖 公背辞 護獣娃 壱持梅陥 拭妃
    {

        if (getDTchTmbL)
        {

            walkR = false;
            walkL = true;
            origin = my[(int)Parts.LHand].position;

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
            walkL = false;
        }

        if (getDTchTmbR)
        {
            walkL = false;
            walkR = true;
            origin = my[(int)Parts.RHand].position;

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
        }

    }

    void Rot()
    {
        // 号狽穿発
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
    { //呪舛琶推
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
                //筈球 説壱 赤澗 掻
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


                // 焼戚奴 説聖 凶
                    if (hitTF.IsChildOf(item))
                    {
                    print(hitTF + " 説奄");
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

            // 説精惟 蒸生檎 軒渡
            if (hitTF == null)
            {
                return;
            }

            // 筈球虞檎
            if (hitTF.IsChildOf(rock))
            {

                rb.isKinematic = false;
                rb.velocity = -transform.TransformDirection(getVelL) * vPower;
                hitTF = null;

            }

            // 焼戚奴 説紹陥檎
            if (my[(int)Parts.LHand].childCount > 2)
            {
                // 謝税 原走厳 切縦精 hitem
                GameObject hItem = my[(int)Parts.LHand].GetChild(my[(int)Parts.LHand].childCount - 1).gameObject;
                Collider[] objs = Physics.OverlapSphere(my[(int)Parts.LHand].position, 0.2f);

                // 謝鋼井生稽 杏軒澗 神崎詮闘亜 1鯵戚雌戚檎
                if (objs.Length > 0)
                {
                    Transform objTF = objs[0].transform;
                    print(objs[0].name + " 0腰");

                    //0腰属亜 倖戚虞檎
                    if (objTF == my[(int)Parts.Body])
                    {

                        // 塙究 焼戚奴戚 2鯵 耕幻戚檎
                        if (myItem.Count < 2)
                        {
                            // 塙究軒什闘拭 隔澗陥.
                            myItem.Add(hItem);
                            // 照左戚惟 廃陥. 搾醗失鉢 馬檎 穣汽戚闘亜 照宜焼娃陥 ばばばばばば
                            // 伽 照左戚惟 馬澗 依引 搾醗失鉢 馬澗 依税 託戚研 硝切!
                            hItem.GetComponent<MeshRenderer>().enabled = false;
                            hItem.SetActive(false);
                        }
                        //焼艦檎 蒸殖陥.
                        else
                        {
                            Destroy(hItem);
                        }

                    }
                    //倖拭 隔走 省生檎
                    else
                    {

                        //揮走切
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


        // 神献謝 崇送績
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
                //筈球 説壱 赤澗 掻
                if (hitTF.IsChildOf(rock))
                {
                    Rocks r = hits[0].GetComponent<Rocks>();
                    floating = false;
                    rb.isKinematic = true;

                    // 闘窪 拙疑
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




                // 焼戚奴 説聖 凶
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

            // 説精惟 蒸生檎 軒渡
            if (hitTF == null)
            {
                return;
            }

            // 筈球虞檎
            if (hitTF.IsChildOf(rock))
            {

                rb.isKinematic = false;
                rb.velocity = -transform.TransformDirection(getVelR) * vPower;
                hitTF = null;

            }

            // 焼戚奴 説紹陥檎
            if (my[(int)Parts.RHand].childCount > 2)
            {
                // 謝税 原走厳 切縦精 hitem
                GameObject hItem = my[(int)Parts.RHand].GetChild(my[(int)Parts.RHand].childCount - 1).gameObject;
                Collider[] objs = Physics.OverlapSphere(my[(int)Parts.RHand].position, 0.3f);

                // 謝鋼井生稽 杏軒澗 神崎詮闘亜 1鯵戚雌戚檎
                if (objs.Length > 0)
                {
                    Transform objTF = objs[0].transform;
                    print(objs[0].name + " 0腰 神献謝 紫遂掻");

                    //0腰属亜 倖戚虞檎
                    if (objTF == my[(int)Parts.Body])
                    {

                        // 塙究 焼戚奴戚 2鯵 耕幻戚檎
                        if (myItem.Count < 2)
                        {
                            // 塙究軒什闘拭 隔澗陥.
                            myItem.Add(hItem);
                            // 照左戚惟 廃陥.
                            hItem.GetComponent<MeshRenderer>().enabled = false;
                            hItem.SetActive(false);
                        }
                        //焼艦檎 蒸殖陥.
                        else
                        {
                            Destroy(hItem);
                        }

                    }
                    //倖拭 隔走 省生檎
                    else
                    {

                        //揮走切
                        Rigidbody itemRb = hItem.GetComponent<Rigidbody>();
                        itemRb.isKinematic = false;

                        itemRb.velocity = getVelR * vPower;
                        itemRb.angularVelocity = getAngVelR;


                        hItem.transform.SetParent(free);
                        print(hItem+"神献謝 窒降");
                    }


                    objTF = null;
                }

            }
        }


    }



    


}

