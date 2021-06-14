using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
public class Connect : MonoBehaviourPunCallbacks
{
    void Start()
    {

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
        PhotonNetwork.NickName = "Eve" + Random.Range(0, 1000);
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        //방 옵션 
        RoomOptions roomOption = new RoomOptions();
        //0은 인원제한없음
        roomOption.MaxPlayers = 1;
        roomOption.IsVisible = false;
        PhotonNetwork.JoinOrCreateRoom("i"+Random.Range(0, 1000), roomOption, TypedLobby.Default);
    }
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print(PhotonNetwork.CurrentRoom.Name);
        SceneManager.LoadScene("Ready");
    }
}

