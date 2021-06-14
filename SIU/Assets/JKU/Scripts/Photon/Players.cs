using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Players : MonoBehaviourPun
{
    public GameObject model;

    void Start()
    {
    
        //�����̸� 
        if (PhotonNetwork.IsMasterClient)
        {
            //���ڸ� ã��
            Transform tr = GameManager.instance.GetEmptyTr();



            //�� pos���� rpc �����ϸ� -> �� pos������ ����
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
