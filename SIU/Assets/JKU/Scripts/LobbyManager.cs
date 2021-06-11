using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
public class LobbyManager : MonoBehaviourPunCallbacks
{
    //방 이름, 최대 인원 InputField를 담을 수 있는 변수
    public TMP_InputField roomNameInput;
    public TMP_InputField maxUserInput;
    
    //방 목록 캐시
    Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>();


    //join버튼
    public Button joinBtn;

    // maxUserBtn버튼
    public Button maxUserBtn;

    //Scrollview - content
    public Transform content;
    //Roominfo버튼 공장
    public GameObject roominfoFactory;
    
    //버젼
    string ver = "1";

    //static 선언
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

        //방 옵션 
        RoomOptions roomOption = new RoomOptions();
        //0은 인원제한없음
        roomOption.MaxPlayers = byte.Parse(maxUserInput.text);
        //방 리스트를 보여줄지 말지(비밀방 만들때)
        roomOption.IsVisible = true;
        //비밀번호 걸려있는 방 만들기(목록에는 있는데 들어갈 수 있는지 없는지 여부)
        roomOption.IsOpen = true;


        //방을 만든다.
        PhotonNetwork.CreateRoom(roomNameInput.text, roomOption, TypedLobby.Default);
        //PhotonNetwork.JoinOrCreateRoom(roomNameInput.text, roomOption, TypedLobby.Default);


    }




    //방에 생성 성공
    public override void OnCreatedRoom()
    {
        print("방 생성 성공!");
    }





    //방 생성 실패
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning("방 생성 실패");
    }

    //방 접속하기
    public void JoinRoom()
    {
      //  PhotonNetwork.JoinRandomRoom();
        PhotonNetwork.JoinRoom(roomNameInput.text);
      //   PhotonNetwork.LoadLevel("Game");
    }

    //방 접속실패
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning("방 접속 실패");
    }


    //방 접속 성공
    public override void OnJoinedRoom()
    {
        print("방 접속 성공");
        print(PhotonNetwork.CurrentRoom.Name);
        //GameScene으로 이동
        PhotonNetwork.LoadLevel("Game");
      
    }

    //현재 방 정보 갱신
    //최초에는 전체 방 리스트를 준다
    //그 다음은 추가/삭제 된 방 정보만 들어온다.
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {

        for (int i = 0; i < roomList.Count; i++)
        {
            print(roomList[i].Name);

            //roomCache.Add(roomList[i].Name, roomList[i]);
        }
        //현재 만들어진 UI를 삭제하자(겹쳐서 만들어짐)
        DeleteRoomList();

        //roomCache에 있는 정보를 갱신하자
        UpdateRoomCache(roomList);
        //UI를 새롭게 만들자
        CreateRoomList();
    }
    //roomCache갱신
    void UpdateRoomCache(List<RoomInfo> roomList)
    {
        roomCache.Clear();//dictionary내용 날리기
        for (int i = 0; i < roomList.Count; i++)
        {
            //만약에 변경 또는 추가 된 방이 roomCache에 있으면 
            if (roomCache.ContainsKey(roomList[i].Name))
            {
                //만약에 그 방을 지워야 한다면 
                if (roomList[i].RemovedFromList)
                {
                    //지워주어라
                    roomCache.Remove(roomList[i].Name);
                    continue;
                }
            }

            roomCache.Add(roomList[i].Name, roomList[i]);
            //방을 roomCache에 변경 또는 추가
            //roomCache[roomList[i].Name] = roomList[i];


        }
    }




    //방 정보 삭제
    void DeleteRoomList()
    {
        foreach (Transform tr in content)
        {
            Destroy(tr.gameObject);
        }
    }



    //방 정보 생성
    void CreateRoomList()
    {
        //dictionary의 값을 순차적으로 뽑아낼 때 사용함
        foreach (RoomInfo info in roomCache.Values)
        {
            //roominfo 버튼 공장에서 rooninfo 버튼 생성
            GameObject room = Instantiate(roominfoFactory);

            //만들어진 roominfo버튼을 content의 자식으로 세팅
            room.transform.SetParent(content);

            //룸 위치 정해주기
            room.transform.localScale = Vector3.one;
            room.transform.localPosition = Vector3.zero;

            //만들어진 roominfo버튼에서 roominfoBtn 컴포넌트 가져온다.
            RoominfoBtn btn = room.GetComponent<RoominfoBtn>();

            //가져온 컴포넌트의 Setinfo함수 호출
            btn.Setinfo(info.Name, info.PlayerCount, info.MaxPlayers);

            //클릭 되었을 때 함수를 등록
            btn.clickAction = OnClickRoominfo;

        }
    }
    void OnClickRoominfo(string roomName)
    {
        roomNameInput.text = roomName;
    }

    void OnchangedRoomName(string roomName)
    {
        #region 다른방법 
        //만약에 룸네임의 길이가 0보다 크면

        //if(roomName.Length > 0)
        //{
        //    joinBtn.interactable = true;   //join 버튼의 interactable 활성화       
        //}

        ////그게 아니면 
        //else
        //{
        //    joinBtn.interactable = false; //join 버튼의 interactable비활성화
        //}
        #endregion
        joinBtn.interactable = roomName.Length > 0;

        //roomName이 바뀌었을 경우 방 생성 창이 활성화 되도록 하는 것.
        OnchangedmaxUser(maxUserInput.text);
    }

    void OnchangedmaxUser(string maxUser)
    {
        //방 생성 시 제목이 없을때는 방 생성 버튼이 활성화되지 않게 해주어야하기 때문에 조건이 추가된다.
        maxUserBtn.interactable = maxUser.Length > 0 && roomNameInput.text.Length > 0;

    }
}
