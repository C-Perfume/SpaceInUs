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

    //Loding(���� ��ٸ���)
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
        PhotonNetwork.NickName = "�÷��̾�" + Random.Range(0, 1000);

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


        if (PhotonNetwork.IsMasterClient)//�����϶�
        {

            PhotonNetwork.Instantiate("Player", new Vector3(-0.553f, 0, 0), Quaternion.identity);
           
        }
        else
        {
            PhotonNetwork.Instantiate("Player", new Vector3(-0.333f, 0, 0), Quaternion.identity);
            // StartCoroutine(LodingImg());
        }



    }

    //������ ��������, Ȧ�� ���� �ٲٱ� ȣ��
    public void RandomChange(int num) //RockParents���� ���� int num��(���� ������Ʈ�� ��ȣ��)
    {
        StartCoroutine(Co_RandomChange(num));
        print("����");
    }

    IEnumerator Co_RandomChange(int num)
    {
        while (a)
        {
            if (PhotonNetwork.IsConnected) //���濡 ���� 
            {
                
                if (Pv.IsMine&&PhotonNetwork.IsMasterClient) //�÷��̾ �����ǰ� ������Ŭ���̾�Ʈ���� Ȯ�� ���α��� ������ ������ 
                {
                    for (int i = 0; i < num; i++) //����������Ʈ ���� �޾ƿ� �ɷ� ������ ������
                    {
                        int rand = Random.Range(1, 11); //�������������� ���� ������Ʈ ���ڸ�ŭ �̾Ƴ���
                        int tRand = Random.Range(1, 11);

                        //�̾Ƴ� ���ڸ�ŭ ���Ǿ��� ����. RockParents ��ũ��Ʈ�� ü������������ ��ũ��Ʈ�� ���° ���� ������Ʈ����(i), ������ �ΰ�(rand, tRand)
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
    public void RpcRockstep(int num, int rand, int tRand) //RockParents�� �ִ� �Լ� �������� (RPC�� �����)
    {
        RockParent.Instance.ChangeRandomStep(num, rand, tRand);
    }

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
       
    //}

    ////�÷��̾� ������ �� üũ
    //public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    //{
    //    base.OnPlayerLeftRoom(otherPlayer);
    //    Destroy(GameObject.Find("Player(Clone)"));
    //}
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

