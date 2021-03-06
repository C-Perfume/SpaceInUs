using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;


public class TrapManager : MonoBehaviourPun
{

    public GameObject meteorFactory;
    public GameObject canFactory;


    //블랙홀 공장
    public GameObject bHole;
    // 함정 한번만 작동하기
    public bool up = true;
    // 블랙홀 인력유지용
    public bool bH = false;
    // 방향반대 유지용
    public bool isUD = false;
    // 인력 방향
    public Vector3 dir;
    // 블랙홀 인력속도
    public float pullSpd = 1f;

    PlayerM pm;
    PhotonView pv;
    NetManager net;
   public int viewID;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        pm = GetComponent<PlayerM>();
        viewID = pv.ViewID;
        net = GameObject.Find("NetManager").GetComponent<NetManager>();

    }


    // 2 메테오 3캔
    public void Create(GameObject clone)
    {
       
            if (clone.name.Contains("Meteo"))
            {
                SoundM.instance.playS(4, 8);
                pv.RPC("RPCTrapC", RpcTarget.All, 2);
            }
            if (clone.name.Contains("Can"))
            {
                SoundM.instance.playS(4, 7);
                pv.RPC("RPCTrapC", RpcTarget.All, 3);
            }
            StartCoroutine(Vibrate(.5f));
            StopCoroutine(Vibrate(5));
        
    }

    void TrapDir(int type, GameObject obj, GameObject target) {
        if (type == 2)
        {
            obj.GetComponent<MeteoFall>().target = target;
        }
        else
        {
            obj.GetComponent<BottleFall>().target = target;
        }

        obj.transform.position = transform.position
            + transform.up * 10
            + transform.forward * -10;
    }

    [PunRPC]
    void RPCTrapC(int type) {

        GameObject clone = meteorFactory;
        if (type == 3) clone = canFactory;
        GameObject obj = Instantiate(clone);
        GameObject target = gameObject;


        if (pv.IsMine)
        {
            TrapDir(type, obj, target);
        }
        else {

            for (int i = 0; i < net.playerList.Count; i++)
            {
                if (net.playerList[i].GetComponent<TrapManager>().viewID == viewID) {
                    target = net.playerList[i];
                    break;
                }
            }

            TrapDir(type, obj, target);
            
        }

    }

    [PunRPC]
    void RPCRanBoxUD()
    {
        // 접속 인원 수 만큼 isUD 상태 바꾸기
        for (int i = 0; i < net.playerList.Count; i++)
        {
          TrapManager tm = net.playerList[i].GetComponent<TrapManager>();

            // 내가 랜덤박스 쓴 경우
            if (pv.IsMine)
            {
                // 리스트 인원이 내가 아니면
                if (net.playerList[i] != gameObject)
                {
                    StartCoroutine(tm.UD());
                }
            }
            //남이 쓴 경우
            else
            {
                //해당 오브젝트의 뷰아이디와 다르면 함정 타겟이 된다
                if (tm.viewID != viewID)
                {
                    StartCoroutine(tm.UD());
                }
            }
        }
    }

            [PunRPC]
    void RPCRanBoxTrapC(int type)
    {
        GameObject clone = meteorFactory;
        if (type == 3) clone = canFactory;

        // 접속 인원 수 만큼 함정 복제
        for (int i = 0; i < net.playerList.Count ; i++)
        {
           
            // 내가 랜덤박스 쓴 경우
            if (pv.IsMine)
            {
            
                // 리스트 인원이 내가 아니면
                if (net.playerList[i] != gameObject )
                {
                    // 타겟은 리스트 상 오브젝트이다
                    GameObject obj = Instantiate(clone);
                    GameObject target = net.playerList[i];
                    TrapDir(type, obj, target);
                   
                }

            }
                //남이 쓴 경우
            else 
            {
                //해당 오브젝트의 뷰아이디와 다르면 함정 타겟이 된다
                if (net.playerList[i].GetComponent<TrapManager>().viewID != viewID)
                {
                    GameObject obj = Instantiate(clone);
                    GameObject target = net.playerList[i];
                    TrapDir(type, obj, target);

                    if (type == 2) SoundM.instance.playS(4, 8);
                    else SoundM.instance.playS(4, 7);
                    StartCoroutine(Vibrate(.5f));
                    StopCoroutine(Vibrate(5));

                }

            }

        }


    }

        // 블랙홀 생성
        public void BHole(Value v)
    {
        pv.RPC("RPCBlackHole", RpcTarget.All, (int)v.tT);
        StartCoroutine(Vibrate(.5f));
        StopCoroutine(Vibrate(1));
    }

    [PunRPC]
     void RPCvib() {
        StartCoroutine(Vibrate(.5f));
        StopCoroutine(Vibrate(1));
    }

    //0 블랙홀 1 화이트홀
    [PunRPC]
    void RPCBlackHole(int bh) {

        SoundM.instance.playS(4, 6);
        GameObject a = Instantiate(bHole);
        BholeRot bHR = a.GetComponent<BholeRot>();


        if (bh == 0) {
            a.transform.position = -transform.right * Random.Range(3, 5f)
               - transform.up * Random.Range(3, 5f)
                   - transform.forward * Random.Range(3, 5f);
            bHR.value = 0;
        }
        else {
            a.transform.position = transform.right * Random.Range(3, 5f)
       - transform.up * Random.Range(3, 5f)
        - transform.forward * Random.Range(3, 5f);
            bHR.value = 1;
        }
        a.transform.up = transform.position - a.transform.position;
        Destroy(a, 5);
        
        for (int i = 0; i < net.playerList.Count; i++)
        {
                TrapManager tm = net.playerList[i].GetComponent<TrapManager>();
                tm.bH = true;
                tm.StartCoroutine(tm.Pull(a));
        
        }

    }

    //당기는 방향 잡기
    IEnumerator Pull(GameObject obj)
    {
        dir = obj.transform.position - transform.position;
        dir.Normalize();
        yield return new WaitForSeconds(5);
        bH = false;
    }

    public IEnumerator UD()
    {
        SoundM.instance.playS(4, 11);
        isUD = true;
        yield return new WaitForSeconds(5);
        isUD = false;
    }

    // 함정에 부딪히면 처리되는 함수
    private void OnTriggerEnter(Collider other)
    {
        if (pv == null) pv = GetComponent<PhotonView>();

        if (other.gameObject.name.Contains("BH"))
        {
            if (other.gameObject.GetComponent<BholeRot>().value == 0)
            {
                if (pv.IsMine)
                {
                    GameObject SavTime = GameObject.Find("saveTime");
                    Destroy(SavTime);

                    //우주미아씬 로드하기
                    SceneManager.LoadScene("LostSpace");
                    pm.state = PlayerM.State.GameOver;
                }
            }
            else {
                if (pv.IsMine)
                {
                    bH = false;
                gameObject.transform.position = new Vector3(0, 0, 0);
                GetComponent<PlayerM>().floating = true;
            }
            }
        }

        if (other.gameObject.name.Contains("Can"))
        {
            if (pv.IsMine)
            {
                BottleFall btf = other.gameObject.GetComponent<BottleFall>();
                StartCoroutine(btf.Black_());
            }
            Destroy(other.gameObject);
        }


        if (other.gameObject.name.Contains("Meteo"))
        {
            if (pv.IsMine)
            {
                GameObject SavTime = GameObject.Find("saveTime");
                Destroy(SavTime);
                //메테오씬 로드

             pm.state = PlayerM.State.GameOver;
            SceneManager.LoadScene("Meteo");
            }
            Destroy(other.gameObject);
        }


        
    }

    //진동 일으키기
    IEnumerator Vibrate(float sec)
    {

        OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.LTouch);
        OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);

        yield return new WaitForSeconds(sec);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);

    }

   
}