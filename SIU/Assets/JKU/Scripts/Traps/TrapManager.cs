using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;


public class TrapManager : MonoBehaviourPun
{

    public GameObject meteorFactory;
    public GameObject canFactory;


    //��Ȧ ����
    public GameObject bHole;
    // ���� �ѹ��� �۵��ϱ�
    public bool up = true;
    // ��Ȧ �η�������
    public bool bH = false;
    // �η� ����
    public Vector3 dir;
    // ��Ȧ �η¼ӵ�
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


    // 2 ���׿� 3ĵ
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
    void RPCTrapC(int type, int id) {

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
    void RPCRanBoxTrapC(int type, int id)
    {
        GameObject clone = meteorFactory;
        if (type == 3) clone = canFactory;

        // ���� �ο� �� ��ŭ ���� ����
        for (int i = 0; i < net.playerList.Count ; i++)
        {
           
            // ���� �����ڽ� �� ���
            if (pv.IsMine)
            {
            
                // ����Ʈ �ο��� ���� �ƴϸ�
                if (net.playerList[i] != gameObject 
                    && net.playerList[i] !=null
                    )
                    {
                
                    // Ÿ���� ����Ʈ �� ������Ʈ�̴�
                    GameObject obj = Instantiate(clone);
                    GameObject target = net.playerList[i];
                    TrapDir(type, obj, target);
                   
                }

            }
                //���� �� ���
            else 
            {
                //�ش� ������Ʈ�� ����̵�� �ٸ��� ���� Ÿ���� �ȴ�
                if (net.playerList[i].GetComponent<TrapManager>().viewID != viewID
                      && net.playerList[i] != null
                    )
                {
                    GameObject obj = Instantiate(clone);
                    GameObject target = net.playerList[i];
                    TrapDir(type, obj, target);
                    //���� �ɷȴٸ� �������� �˸���
                    if (net.playerList[i] == gameObject) {
                        StartCoroutine(Vibrate(.5f));
                        StopCoroutine(Vibrate(5));
                    }

                }
                
            }

        }


    }

        // ��Ȧ ����
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

    //0 ��Ȧ 1 ȭ��ƮȦ
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
            if (net.playerList[i] != null)
            {
                TrapManager tm = net.playerList[i].GetComponent<TrapManager>();
                tm.bH = true;
                tm.StartCoroutine(tm.Pull(a));
            }
        
        }

    }

    //���� ���� ���
    IEnumerator Pull(GameObject obj)
    {
        dir = obj.transform.position - transform.position;
        dir.Normalize();
        yield return new WaitForSeconds(5);
        bH = false;
    }

    // ������ �ε����� ó���Ǵ� �Լ�
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

                    //���ֹ̾ƾ� �ε��ϱ�
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
                print("�۵�???");
            }
            Destroy(other.gameObject);
        }


        if (other.gameObject.name.Contains("Meteo"))
        {
            if (pv.IsMine)
            {
                GameObject SavTime = GameObject.Find("saveTime");
                Destroy(SavTime);
                //���׿��� �ε�

             pm.state = PlayerM.State.GameOver;
            SceneManager.LoadScene("Meteo");
            }
            Destroy(other.gameObject);
        }


        
    }

    //���� ����Ű��
    IEnumerator Vibrate(float sec)
    {

        OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.LTouch);
        OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);

        yield return new WaitForSeconds(sec);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);

    }

   
}