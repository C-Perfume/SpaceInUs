using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class NetManager : MonoBehaviourPunCallbacks
{
    string ver = "1";
    //Loding(���� ��ٸ���)
    //public GameObject Loding;
    //private void Awake()
    //{
    //}
    PhotonView pv;
    public List<GameObject> playerList = new List<GameObject>();
    void Start()
    {
        pv = GetComponent<PhotonView>();
        PhotonNetwork.GameVersion = ver;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnected()
    {
        base.OnConnected();
        print("OnConnected");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.NickName = "Player" + Random.Range(0, 1000);
       PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("OnJoinedLobbyzz");
        RoomOptions option = new RoomOptions();

        //���߷� ���� ��
       //option.MaxPlayers = 1;
        option.MaxPlayers = 4;
        
        PhotonNetwork.JoinOrCreateRoom("JKaU", option, TypedLobby.Default);
    }
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("CreatedRoom");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("joinedroom");


        if (PhotonNetwork.IsMasterClient)//�����϶�
        {
            PhotonNetwork.Instantiate("Player", new Vector3(0, 0, -2), Quaternion.identity);

            ////���ڸ� ã��
            //Transform tr = GameManager.instance.GetEmptyTr();



            ////�� pos���� rpc �����ϸ� -> �� pos������ ����
            //photonView.RPC("RpcSetInit", RpcTarget.AllBuffered, tr.position, tr.rotation);

        }
        else
        {
            PhotonNetwork.Instantiate("Player", new Vector3(0, 0, 0), Quaternion.identity);
        }

    }
    
    //[PunRPC]
    //void RpcSetInit(Vector3 pos, Quaternion rot)
    //{
    //    model.SetActive(true);
    //    transform.position = pos;
    //    transform.rotation = rot;
    //}


    //�泪����
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        print("�泪��");
    }

    //�÷��̾� ������ �� üũ
    
    //public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    //{
    //    base.OnPlayerEnteredRoom(newPlayer);

    //    if (photonView.IsMine)//���� �϶�
    //    {
    //        Destroy(GameObject.Find("Player(Clone)"));
    //        PhotonNetwork.Instantiate("Player", new Vector3(-0.553f, 0, 0), Quaternion.identity);
    //    }
    //    else
    //    {
    //        PhotonNetwork.Instantiate("Player", new Vector3(-0.333f, 0, 0), Quaternion.identity);
    //    }

    //}



    //�÷��̾� ������ �� üũ
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        //���� ����� ���ӿ�����Ʈ�� ����Ʈ���� �����Ѵ�.
        for (int i = 0; i < playerList.Count; i++)
        {
            if (otherPlayer.NickName == playerList[i].GetComponent<PhotonView>().Owner.NickName)
            {
                playerList.Remove(playerList[i]);
                break;
            }
        }
    }
}
//    //�ε� â �����ֱ�
//    //IEnumerator LodingImg()
//    //{
//    //    Loding.transform.position = GameObject.Find("Player(Clone)").transform.position + new Vector3(0,0,0.4f);
//    //    Loding.SetActive(true);

//    //    yield return new WaitForSeconds(2);
//    //    Loding.SetActive(false);

//    //}
//}

