using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class PlayerPhoton : MonoBehaviourPun, IPunObservable
{
    // 카메라리그에 붙는 스크립트라는 전제로 왼손 / 오른손 변수잡고 시작하기

    // 네트워크 위치값 전송용
    struct Sync
    {
        public Vector3 pos;
        public Quaternion rot;
    }

    public enum Parts
    {
        Head,
        LHand,
        RHand,
        Body
    }

    Vector3 photonPos;
    public Transform[] my;
    public Transform[] others;
    Sync[] syncData;
    public GameObject myModel;
    public GameObject otherModel;
    PhotonView pv;
    Player pl;
    //체력바
    public Slider hpother;
    //플레이어이름
    public Text Pt;

    //pm에서 사용.. 손 지정하는 글이 너무 김
    public Transform handL;
    public Transform handR;

    void Start()
    {
        pl = GetComponent<Player>();
        pv = GetComponent<PhotonView>();

        handL = my[(int)Parts.LHand];
        handR = my[(int)Parts.RHand];
    
        if (!pv.IsMine)
        {
            syncData = new Sync[my.Length];
            handL = others[(int)Parts.LHand];
            handR = others[(int)Parts.RHand];
        }        

        //꼭 플레이나 빌드할 때 ovr카메라를 비활성화 하자
        //아니면 네트워크 접속 시 바로 ovr매니져 스크립트가 사라진다!!!!
        myModel.SetActive(pv.IsMine);
        otherModel.SetActive(!pv.IsMine);

        //플레이어 이름
        if (!pv.IsMine)
            {
                Pt.text = pv.Owner.NickName;
            }
        

    }
    void Update()
    {

        if (!pv.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, photonPos, .2f);
            for (int i = 0; i < others.Length; i++)
            {
                others[i].position = Vector3.Lerp(others[i].position, syncData[i].pos, .2f);
                others[i].rotation = Quaternion.Lerp(others[i].rotation, syncData[i].rot, .2f);

            }
        }

    }
    // 포톤 몸 움직임
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //현재 체력 받기
            stream.SendNext(pl.currentHp);
            stream.SendNext(transform.position);
            for (int i = 0; i < my.Length; i++)
            {
                stream.SendNext(my[i].position);
                stream.SendNext(my[i].rotation);

            }
        }

        if (stream.IsReading)//받기
        {
            //현재 체력 받기
            hpother.value = (int)stream.ReceiveNext();
            photonPos = (Vector3)stream.ReceiveNext();
            if (syncData != null)
            {
                for (int i = 0; i < others.Length; i++)//몸
                {
                    syncData[i].pos = (Vector3)stream.ReceiveNext();
                    syncData[i].rot = (Quaternion)stream.ReceiveNext();
                }
            }
        }


    }
}






