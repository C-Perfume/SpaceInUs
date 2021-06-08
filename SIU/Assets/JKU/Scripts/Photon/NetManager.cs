using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;


public class NetManager : MonoBehaviourPunCallbacks
{
    string ver = "1";
    PhotonView Pv;
    //  public StepListCreater SL;
    public static NetManager Instance;

    bool a = true;

    //Loding(상대방 기다리기)
    //public GameObject Loding;
    private void Awake()
    {

        Instance = this;
    }

    void Start()
    {
        PhotonNetwork.GameVersion = ver;
        PhotonNetwork.ConnectUsingSettings();
         Pv = GetComponent<PhotonView>();
    }

    public override void OnConnected()
    {
        base.OnConnected();
        print("OnConnected");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.NickName = "플레이어" + Random.Range(0, 1000);

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

            PhotonNetwork.Instantiate("Player", new Vector3(-0.553f, 0, 0), Quaternion.identity);
           
        }
        else
        {
            PhotonNetwork.Instantiate("Player", new Vector3(-0.333f, 0, 0), Quaternion.identity);
            // StartCoroutine(LodingImg());
        }



    }

    //아이템 랜덤생성, 홀드 색깔 바꾸기 호출
    public void RandomChange(int num) //RockParents에서 만든 int num값(하위 오브젝트의 번호값)
    {
        StartCoroutine(Co_RandomChange(num));
        print("시작");
    }

    IEnumerator Co_RandomChange(int num)
    {
        while (a)
        {
            if (PhotonNetwork.IsConnected) //포톤에 접속 
            {
                
                if (Pv.IsMine&&PhotonNetwork.IsMasterClient) //플레이어가 생성되고 마스터클라이언트인지 확인 여부까지 조건이 맞으면 
                {
                    for (int i = 0; i < num; i++) //하위오브젝트 숫자 받아온 걸로 포문을 돌리자
                    {
                        int rand = Random.Range(1, 11); //랜덤레인지값을 하위 오브젝트 숫자만큼 뽑아내자
                        int tRand = Random.Range(1, 11);

                        //뽑아낸 숫자만큼 알피씨를 보냄. RockParents 스크립트의 체인지랜덤스탭 스크립트에 몇번째 하위 오브젝트인지(i), 랜덤값 두개(rand, tRand)
                        Pv.RPC("RpcRockstep", RpcTarget.AllBuffered, i, rand, tRand);

                        a = false;
                    }
                    
                    print("S");
                    break;
                }
            }
            yield return null;
        }

    }

    [PunRPC]
    public void RpcRockstep(int num, int rand, int tRand) //RockParents에 있는 함수 가져오기 (RPC로 만들기)
    {
        RockParent.Instance.ChangeRandomStep(num, rand, tRand);
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
       
    //}

    ////플레이어 나갔을 때 체크
    //public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    //{
    //    base.OnPlayerLeftRoom(otherPlayer);
    //    Destroy(GameObject.Find("Player(Clone)"));
    //}
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

