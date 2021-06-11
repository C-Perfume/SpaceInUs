using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class NetManager : MonoBehaviourPunCallbacks
{
    string ver = "1";
    //Loding(상대방 기다리기)
    //public GameObject Loding;
    //private void Awake()
    //{
    //}
    PhotonView pv;

    public List<GameObject> playerList = new List<GameObject>();
    GameObject a;
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
        PhotonNetwork.JoinOrCreateRoom("JKU", new RoomOptions(), TypedLobby.Default);
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


        if (PhotonNetwork.IsMasterClient)//방장일때
        {
            PhotonNetwork.Instantiate("Player", new Vector3(0, 0, -2), Quaternion.identity);
        }
        else
        {
             PhotonNetwork.Instantiate("Player", new Vector3(0, 0, 0), Quaternion.identity);
        }

    }

    public void AddPlayer(GameObject pl) {
       playerList.Add(pl);
    }
    

    //방나가기
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        print("방나감");
    }

    //플레이어 들어왔을 때 체크
    
    //public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    //{
    //    base.OnPlayerEnteredRoom(newPlayer);

    //    if (photonView.IsMine)//내꺼 일때
    //    {
    //        Destroy(GameObject.Find("Player(Clone)"));
    //        PhotonNetwork.Instantiate("Player", new Vector3(-0.553f, 0, 0), Quaternion.identity);
    //    }
    //    else
    //    {
    //        PhotonNetwork.Instantiate("Player", new Vector3(-0.333f, 0, 0), Quaternion.identity);
    //    }

    //}



    //플레이어 나갔을 때 체크
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
    }
}
//    //로딩 창 보여주기
//    //IEnumerator LodingImg()
//    //{
//    //    Loding.transform.position = GameObject.Find("Player(Clone)").transform.position + new Vector3(0,0,0.4f);
//    //    Loding.SetActive(true);

//    //    yield return new WaitForSeconds(2);
//    //    Loding.SetActive(false);

//    //}
//}

