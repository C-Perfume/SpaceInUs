using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
public class LobbyManager : MonoBehaviourPunCallbacks
{
    //�� �̸�, �ִ� �ο� InputField�� ���� �� �ִ� ����
    public TMP_InputField roomNameInput;
    public TMP_InputField maxUserInput;
    
    //�� ��� ĳ��
    Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>();


    //join��ư
    public Button joinBtn;

    // maxUserBtn��ư
    public Button maxUserBtn;

    //Scrollview - content
    public Transform content;
    //Roominfo��ư ����
    public GameObject roominfoFactory;
    
    //����
    string ver = "1";

    //static ����
    public static LobbyManager instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        PhotonNetwork.GameVersion = ver;
        PhotonNetwork.ConnectUsingSettings();

        roomNameInput.onValueChanged.AddListener(OnchangedRoomName);
        maxUserInput.onValueChanged.AddListener(OnchangedmaxUser);
    }
    //private void Update()
    //{
    //    roomNameInput.Select();
    //    maxUserInput.Select();
    //}
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
        //PhotonNetwork.JoinOrCreateRoom("JKU", new RoomOptions(), TypedLobby.Default);
    }
    public void CreatRoom()

    {

        //�� �ɼ� 
        RoomOptions roomOption = new RoomOptions();
        //0�� �ο����Ѿ���
        roomOption.MaxPlayers = byte.Parse(maxUserInput.text);
        //�� ����Ʈ�� �������� ����(��й� ���鶧)
        roomOption.IsVisible = true;
        //��й�ȣ �ɷ��ִ� �� �����(��Ͽ��� �ִµ� �� �� �ִ��� ������ ����)
        roomOption.IsOpen = true;


        //���� �����.
        PhotonNetwork.CreateRoom(roomNameInput.text, roomOption, TypedLobby.Default);
        //PhotonNetwork.JoinOrCreateRoom(roomNameInput.text, roomOption, TypedLobby.Default);


    }




    //�濡 ���� ����
    public override void OnCreatedRoom()
    {
        print("�� ���� ����!");
    }





    //�� ���� ����
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning("�� ���� ����");
    }

    //�� �����ϱ�
    public void JoinRoom()
    {
      //  PhotonNetwork.JoinRandomRoom();
        PhotonNetwork.JoinRoom(roomNameInput.text);
      //   PhotonNetwork.LoadLevel("Game");
    }

    //�� ���ӽ���
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning("�� ���� ����");
    }


    //�� ���� ����
    public override void OnJoinedRoom()
    {
        print("�� ���� ����");
        print(PhotonNetwork.CurrentRoom.Name);
        //GameScene���� �̵�
        PhotonNetwork.LoadLevel("Game");
      
    }

    //���� �� ���� ����
    //���ʿ��� ��ü �� ����Ʈ�� �ش�
    //�� ������ �߰�/���� �� �� ������ ���´�.
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {

        for (int i = 0; i < roomList.Count; i++)
        {
            print(roomList[i].Name);

            //roomCache.Add(roomList[i].Name, roomList[i]);
        }
        //���� ������� UI�� ��������(���ļ� �������)
        DeleteRoomList();

        //roomCache�� �ִ� ������ ��������
        UpdateRoomCache(roomList);
        //UI�� ���Ӱ� ������
        CreateRoomList();
    }
    //roomCache����
    void UpdateRoomCache(List<RoomInfo> roomList)
    {
        roomCache.Clear();//dictionary���� ������
        for (int i = 0; i < roomList.Count; i++)
        {
            //���࿡ ���� �Ǵ� �߰� �� ���� roomCache�� ������ 
            if (roomCache.ContainsKey(roomList[i].Name))
            {
                //���࿡ �� ���� ������ �Ѵٸ� 
                if (roomList[i].RemovedFromList)
                {
                    //�����־��
                    roomCache.Remove(roomList[i].Name);
                    continue;
                }
            }

            roomCache.Add(roomList[i].Name, roomList[i]);
            //���� roomCache�� ���� �Ǵ� �߰�
            //roomCache[roomList[i].Name] = roomList[i];


        }
    }




    //�� ���� ����
    void DeleteRoomList()
    {
        foreach (Transform tr in content)
        {
            Destroy(tr.gameObject);
        }
    }



    //�� ���� ����
    void CreateRoomList()
    {
        //dictionary�� ���� ���������� �̾Ƴ� �� �����
        foreach (RoomInfo info in roomCache.Values)
        {
            //roominfo ��ư ���忡�� rooninfo ��ư ����
            GameObject room = Instantiate(roominfoFactory);

            //������� roominfo��ư�� content�� �ڽ����� ����
            room.transform.SetParent(content);

            //�� ��ġ �����ֱ�
            room.transform.localScale = Vector3.one;
            room.transform.localPosition = Vector3.zero;

            //������� roominfo��ư���� roominfoBtn ������Ʈ �����´�.
            RoominfoBtn btn = room.GetComponent<RoominfoBtn>();

            //������ ������Ʈ�� Setinfo�Լ� ȣ��
            btn.Setinfo(info.Name, info.PlayerCount, info.MaxPlayers);

            //Ŭ�� �Ǿ��� �� �Լ��� ���
            btn.clickAction = OnClickRoominfo;

        }
    }
    void OnClickRoominfo(string roomName)
    {
        roomNameInput.text = roomName;
    }

    void OnchangedRoomName(string roomName)
    {
        #region �ٸ���� 
        //���࿡ ������� ���̰� 0���� ũ��

        //if(roomName.Length > 0)
        //{
        //    joinBtn.interactable = true;   //join ��ư�� interactable Ȱ��ȭ       
        //}

        ////�װ� �ƴϸ� 
        //else
        //{
        //    joinBtn.interactable = false; //join ��ư�� interactable��Ȱ��ȭ
        //}
        #endregion
        joinBtn.interactable = roomName.Length > 0;

        //roomName�� �ٲ���� ��� �� ���� â�� Ȱ��ȭ �ǵ��� �ϴ� ��.
        OnchangedmaxUser(maxUserInput.text);
    }

    void OnchangedmaxUser(string maxUser)
    {
        //�� ���� �� ������ �������� �� ���� ��ư�� Ȱ��ȭ���� �ʰ� ���־���ϱ� ������ ������ �߰��ȴ�.
        maxUserBtn.interactable = maxUser.Length > 0 && roomNameInput.text.Length > 0;

    }
}
