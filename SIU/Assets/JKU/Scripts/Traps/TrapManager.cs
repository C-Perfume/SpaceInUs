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
    // 인력 방향
    public Vector3 dir;
    // 블랙홀 인력속도
    public float pullSpd = 1f;

    PlayerM pm;
    PhotonView pv;
    NetManager net;
    int viewID;

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
            pv.RPC("RPCTrapC", RpcTarget.All, 2, viewID);

        }
        if (clone.name.Contains("Can"))
        {
            SoundM.instance.playS(4, 7);
            pv.RPC("RPCTrapC", RpcTarget.All, 3, viewID);

        }
        StartCoroutine(Vibrate(.5f));
        StopCoroutine(Vibrate(5));

    }

    [PunRPC]
    void RPCTrapC(int type, int id) {

        GameObject clone = meteorFactory;
        if (type == 3) clone = canFactory;
        GameObject obj = Instantiate(clone);
        GameObject target = gameObject;

        obj.transform.position = transform.position
            + transform.up * 10
            + transform.forward * -10;

        if (pv.IsMine)
        {

            if (type == 2)
            {
                obj.GetComponent<MeteoFall>().target = target;
            }
            else
            {
                obj.GetComponent<BottleFall>().target = target;
            }

        }
        else {

            for (int i = 0; i < net.playerList.Count; i++)
            {
                if (net.playerList[i].GetComponent<TrapManager>().viewID == viewID) {
                    target = net.playerList[i];
                    break;
                }
            }

            if (type == 2)
            {
                obj.GetComponent<MeteoFall>().target = target;
            }
            else
            {
                obj.GetComponent<BottleFall>().target = target;
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