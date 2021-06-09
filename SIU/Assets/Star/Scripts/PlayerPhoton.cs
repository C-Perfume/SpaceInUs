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

    public Slider mine;
    public Slider other;

    void Start()
    {
        pv = GetComponent<PhotonView>();

        if (!pv.IsMine)
        {
            syncData = new Sync[my.Length];
        }

        //꼭 플레이나 빌드할 때 ovr카메라를 비활성화 하자
        //아니면 네트워크 접속 시 바로 ovr매니져 스크립트가 사라진다!!!!
        myModel.SetActive(pv.IsMine);
        otherModel.SetActive(!pv.IsMine);
        
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
            stream.SendNext(mine.value);
            stream.SendNext(transform.position);
            for (int i = 0; i < my.Length; i++)
            {
                stream.SendNext(my[i].position);
                stream.SendNext(my[i].rotation);

            }
        }

        if (stream.IsReading)
        {
            mine.value = (int)stream.ReceiveNext();
            photonPos = (Vector3)stream.ReceiveNext();
            if (syncData != null)
            {

                for (int i = 0; i < others.Length; i++)
                {
                   syncData[i].pos = (Vector3)stream.ReceiveNext();
                    syncData[i].rot = (Quaternion)stream.ReceiveNext();
                }
            }



        }
    }

}




