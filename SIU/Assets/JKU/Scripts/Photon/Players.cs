using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Players : MonoBehaviourPun
{
    public GameObject model;

    void Start()
    {
    
        //방장이면 
        if (PhotonNetwork.IsMasterClient)
        {
            //빈자리 찾기
            Transform tr = GameManager.instance.GetEmptyTr();



            //그 pos값을 rpc 전달하면 -> 그 pos값으로 세팅
            photonView.RPC("RpcSetInit", RpcTarget.AllBuffered, tr.position, tr.rotation);
        }
    }

    // Update is called once per frame


    [PunRPC]
    void RpcSetInit(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
    }
}
