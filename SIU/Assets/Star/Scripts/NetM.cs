using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetM : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    void Update()
    {

    }

    public override void OnConnected()
    {
        base.OnConnected();
        print("OnConnected");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000);

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("OnJoinedLobby");

        PhotonNetwork.JoinOrCreateRoom("Star", new RoomOptions(), TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("OnCreatedRoom");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("OnJoinedRoom");

            PhotonNetwork.Instantiate("Player", new Vector3(-0.553f, 0, 0), Quaternion.identity);
    }
}
