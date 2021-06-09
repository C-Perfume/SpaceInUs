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

    public enum Parts
    {
        Head,
        LHand,
        RHand,
        Body
    }

    PhotonView pv;
    public Transform[] my;

    public State state;
    //�ȱ� ���
    Vector3 origin;
    Vector3 pos;
    bool walkL = false;
    bool walkR = false;

    //��ǥ�� ������
    public GameObject doorCanvas;
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

    //������ ã���
    Transform free;
    Transform mine;

    //������ ������
    public GameObject rope;
    public GameObject fire;
    public GameObject shield;
    public GameObject oxy;

    //ȹ������۸���Ʈ
    public List<GameObject> myItem = new List<GameObject>();

    // ������ ���� ����� �� ���� lr >> player�� �پ�����
    LineRenderer lr;

    //ī�޶� ������ ������ٵ� ��������
    Rigidbody rb;

    //������ ����
    TrapManager tM;

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

        if (SceneManager.GetActiveScene().name == "Game")
        { 
        rock = GameObject.Find("Rock").transform;
        rp = GetComponent<RockParent>();
         free = rp.free;
         mine = new GameObject(pv.Owner.NickName+"_Mine").transform;
        tM = GetComponent<TrapManager>();
        lr = GetComponent<LineRenderer>();
        FootStepTransform = GameObject.Find("FootStepTransform").transform;

        // �ٸ��ֵ��� �������� ���ƶ�..
            if (pv.IsMine == PhotonNetwork.IsMasterClient)
            {
                state = State.GameStart;
            }
            else {
                state = State.Wait;
            }
        
        }

        else if (SceneManager.GetActiveScene().name == "Ready")
        { state = State.Ready; }

        else
        { state = State.GameOver; }

        doorIndi = doorCanvas.transform.GetChild(0).gameObject;
        doorIndi2 = doorCanvas.transform.GetChild(1).gameObject;

        rb = GetComponent<Rigidbody>();
        lrL = my[(int)Parts.LHand].GetComponent<LineRenderer>();
        lrR = my[(int)Parts.RHand].GetComponent<LineRenderer>();
    }
    void Update()
    {

        if (!pv.IsMine)
        {
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

                float v = Input.GetAxis("Vertical");
                float h = Input.GetAxis("Horizontal");
                float f = 0;
                if (Input.GetKey(KeyCode.Space)) { f = 1; }
                if (Input.GetKey(KeyCode.LeftControl)) { f = -1; }
                Vector3 dir = new Vector3(h, v, f);
                transform.position += dir * 10 * Time.deltaTime;

                break;
            #endregion

            #region Ready
            case State.Ready:

                if (SceneManager.GetActiveScene().name == "Ready")
                { Walk(); Open(0.1f, "Game");
                    if (getDBtn1R) {
                        SceneManager.LoadScene("Game");
                    }
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

                    //������ �ۿ�
                    if (myItem.Count > 0)
                    {

                        if (getDBtn1R)
                        {
                            print("������ ���");
                            ItemM itm = GetComponent<ItemM>();
                            itm.active = true;
                            GameObject used = myItem[0];
                            Destroy(used, 6);
                        }

                    }


                }
                
                Rot();
                
                if (goPlay.instance.MenuManager.activeSelf) { state = State.GameOver; }
               

                break;
            #endregion

            #region GameStart
            case State.GameStart:

                //ġƮŰ
                if (getDBtn1L) {  }
                if (getDBtn2L) { }


                //�÷���
                // ���߷� ������
                if (floating) //Float();

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
                        ItemM itm = GetComponent<ItemM>();
                        itm.active = true;
                        GameObject used = myItem[0];
                        Destroy(used, 6);
                    }

                }


                if (goPlay.instance.MenuManager.activeSelf) { state = State.GameOver; }
                else {
                    lrL.enabled = false;
                    lrR.enabled = false;
                }
                
                break;
            #endregion

            #region GameOver
            case State.GameOver:
                ClickLR();

                if (!goPlay.instance.MenuManager.activeSelf) {
                    if (SceneManager.GetActiveScene().name == "Ready") 
                    { state = State.Ready;
                        lrL.enabled = false;
                        lrR.enabled = false;
                    }
                    else { state = State.GameStart;
                      
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
            origin = my[(int)Parts.LHand].position;
            SoundM.instance.playS(1, 0);

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
            SoundM.instance.StopS(1, 0);
            walkL = false;
        }

        if (getDTchTmbR)
        {
            walkL = false;
            walkR = true;
            origin = my[(int)Parts.RHand].position;
            SoundM.instance.playS(1, 0);

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
            origin = my[(int)Parts.LHand].position;
            SoundM.instance.playS(1, audioNum);

        }
        if (walkL)
        {
            transform.position += origin - my[(int)Parts.LHand].position;
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
            origin = my[(int)Parts.RHand].position;
            SoundM.instance.playS(1, audioNum);

        }

        if (walkR)
        {
            transform.position += origin - my[(int)Parts.RHand].position;
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
        Click(my[(int)Parts.LHand], lrL, doorIndi, getDBtnIdxL);
        Click(my[(int)Parts.RHand], lrR, doorIndi2, getDBtnIdxR);
      }

    void Click(Transform hand, LineRenderer lrC, GameObject indi, bool gDB) {

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
        if (getDBtnIdxL)
        {
            Collider[] hitsLL = Physics.OverlapSphere(my[(int)Parts.LHand].position, 0.01f);

            if (hitsLL.Length > 0)
            {
                SoundM.instance.playS(0, 5);
            }

            walkR = false;
            walkL = true;
            origin = my[(int)Parts.LHand].position;

            tM.up = true;

        }

        if (walkL)
        {

            Collider[] hitsL = Physics.OverlapSphere(my[(int)Parts.LHand].position, 0.03f);

            if (hitsL.Length > 0)
            {
                hitTF[0] = hitsL[0].transform;

                if (hitTF[0].gameObject.name.Contains("Rock_01")) { }
                Grab(hitTF[0], my[(int)Parts.LHand], my[(int)Parts.RHand], ref catchHoldL, ref catchHoldR, ref catchHoldLpre);
            }
        }

        // ������ ������
        if (getDBtnIdxR)
        {
            Collider[] hitsRR = Physics.OverlapSphere(my[(int)Parts.RHand].position, 0.01f);

            if (hitsRR.Length > 0)
            {
                SoundM.instance.playS(0, 5);
            }

            walkR = true;
            walkL = false;
            origin = my[(int)Parts.RHand].position;

            tM.up = true;
            
        }

        if (walkR)
        {

            Collider[] hitsR = Physics.OverlapSphere(my[(int)Parts.RHand].position, 0.03f);

            if (hitsR.Length > 0)
            {
                hitTF[1] = hitsR[0].transform;

                if (hitTF[1].gameObject.name.Contains("Rock_01")) { }

                Grab(hitTF[1], my[(int)Parts.RHand], my[(int)Parts.LHand], ref catchHoldR, ref catchHoldL, ref catchHoldRpre);
            }

        }

    }


    void Grab(Transform hitTFN, Transform handG, Transform otherH, ref Transform Hold, ref Transform Hold2, ref Transform Holdpre)
    {
            int idx = hitTFN.GetSiblingIndex();

        // Ȧ�� ��� �ִ� ��
        if (hitTFN.IsChildOf(rock))
        {
            
            Hold = hitTFN;
            if (Hold2 != null &&
                  Hold == Hold2) { tM.up = false; }

            if (Holdpre == Hold) { tM.up = false; }

            floating = false;
            rb.isKinematic = true;


            if (rp.holds[idx].type == Value.Type.Trap) {

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

            if (tM.bH) transform.position += tM.dir * tM.pullSpd * Time.deltaTime;
            else { transform.position += origin - handG.position; }

        }
        else

        // ������ ���� �� �տ� ����
        if (hitTFN.IsChildOf(rp.item))
        {

                SoundM.instance.playS(0, 5);
            if (hitTFN.gameObject.name.Contains("Fire")) { CreateItem(fire, handG); }
            if (hitTFN.gameObject.name.Contains("Oxy")) { CreateItem(oxy, handG); }
            if (hitTFN.gameObject.name.Contains("Rope")) { CreateItem(rope, handG); }
            if (hitTFN.gameObject.name.Contains("Shield")) { CreateItem(shield, handG); }

            rp.item_False.Add(hitTFN.gameObject);
            hitTFN.gameObject.SetActive(false);
            rp.items.Remove(hitTFN.gameObject);
            StartCoroutine(rp.ShowUp(hitTFN.gameObject));
        }
        else

        // ������ �������̳� �ٸ� �� �������� ������
        if (hitTFN.IsChildOf(free) || hitTFN.IsChildOf(otherH))
        {
            catchItem = hitTFN;

                SoundM.instance.playS(0, 5);
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
            if (hitTF[0] == null) { return; }

            SetFree(hitTF[0], catchHoldL, my[(int)Parts.LHand], getVelL, getAngVelL);
            hitTF[0] = null;
            catchHoldLpre = catchHoldL;
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
            hitTF[1] = null;
            catchHoldRpre = catchHoldR;
        }
    }

    void SetFree(Transform hitTFN, Transform hold, Transform hand, Vector3 vel, Vector3 angVel)
    {

        // Ȧ����
        if (hitTFN == hold)
        {

            rb.isKinematic = false;
            rb.velocity = -transform.TransformDirection(vel) * vPower;

        }

        // ������ ��Ҵٸ�
        if (catchItem != null && hitTFN == catchItem)
        {
            Rigidbody itemRb = catchItem.GetComponent<Rigidbody>();

            //�ΰ� ����
            if (myItem.Count < 2)
            {

               // int layer = 1 << LayerMask.NameToLayer("Item");
                Collider[] obj = Physics.OverlapSphere(my[(int)Parts.Body].position+(Vector3.up*.05f), 0.3f
                    //,layer
                    );

                //1���̻�   
                if (obj.Length > 0)
                {
                    //ã��
                    for (int i = 0; i < obj.Length; i++)
                    {
                        if (obj[i].name.Contains("Item"))
                        {
                            find = true;
                            print(obj[i].name+" "+i+"��");
                            break;
                        }
                    }

                    if (find)
                    {
                        print("ã�Ҵ�");
                        //  ȹ�渮��Ʈ�� �ִ´�.
                        catchItem.SetParent(mine);
                        myItem.Add(catchItem.gameObject);

                        // �ָ� ���� �Ⱥ��̰� ����
                       myItem[0].transform.position = new Vector3(1000, 1000, 1000);
                        if (myItem.Count > 1) myItem[1].transform.position = new Vector3(1000, 1000, 1000);
                        find = false;
                    }
                    else 
                    {
                        itemRb.isKinematic = false;

                        itemRb.velocity = vel * vPower;
                        itemRb.angularVelocity = angVel;


                        catchItem.transform.SetParent(free);
                        print(obj[0].name + " 0��. �������� ��Ͽ� ���� ");

                    }


                }

                //0��
                else
                {
                    itemRb.isKinematic = false;

                    itemRb.velocity = vel * vPower;
                    itemRb.angularVelocity = angVel;


                    catchItem.transform.SetParent(free);
                    print("�ɸ��� ���  ���ߺξ�");
                }
                
            }
            // 2�� �̻��̸� ������.
            else
            {
                itemRb.isKinematic = false;

                itemRb.velocity = vel * vPower;
                itemRb.angularVelocity = angVel;


                catchItem.transform.SetParent(free);
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

    IEnumerator StopFStep() {
        yield return new WaitForSeconds(3);
        fStep = false;
    }

}




