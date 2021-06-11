using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class PlayerPhoton : MonoBehaviourPun, IPunObservable
{
    // ī�޶󸮱׿� �ٴ� ��ũ��Ʈ��� ������ �޼� / ������ ������� �����ϱ�

    // ��Ʈ��ũ ��ġ�� ���ۿ�
    struct Sync
    {
        public Vector3 pos;
        public Quaternion rot;
    }

    public enum Parts
    {
        Head,
        LHand,
        RHand,
        Body
    }

    Vector3 photonPos;
    public Transform[] my;
    public Transform[] others;
    Sync[] syncData;
    public GameObject myModel;
    public GameObject otherModel;
    PhotonView pv;
    Player pl;
    //ü�¹�
    public Slider hpother;
    //�÷��̾��̸�
    public Text Pt;

    //pm���� ���.. �� �����ϴ� ���� �ʹ� ��
    public Transform handL;
    public Transform handR;

    void Start()
    {
        pl = GetComponent<Player>();
        pv = GetComponent<PhotonView>();

        handL = my[(int)Parts.LHand];
        handR = my[(int)Parts.RHand];
    
        if (!pv.IsMine)
        {
            syncData = new Sync[my.Length];
            handL = others[(int)Parts.LHand];
            handR = others[(int)Parts.RHand];
        }        

        //�� �÷��̳� ������ �� ovrī�޶� ��Ȱ��ȭ ����
        //�ƴϸ� ��Ʈ��ũ ���� �� �ٷ� ovr�Ŵ��� ��ũ��Ʈ�� �������!!!!
        myModel.SetActive(pv.IsMine);
        otherModel.SetActive(!pv.IsMine);

        //�÷��̾� �̸�
        if (!pv.IsMine)
            {
                Pt.text = pv.Owner.NickName;
            }
        

    }
    void Update()
    {

        if (!pv.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, photonPos, .2f);
            for (int i = 0; i < others.Length; i++)
            {
                others[i].position = Vector3.Lerp(others[i].position, syncData[i].pos, .2f);
                others[i].rotation = Quaternion.Lerp(others[i].rotation, syncData[i].rot, .2f);

            }
        }

    }
    // ���� �� ������
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //���� ü�� �ޱ�
            stream.SendNext(pl.currentHp);
            stream.SendNext(transform.position);
            for (int i = 0; i < my.Length; i++)
            {
                stream.SendNext(my[i].position);
                stream.SendNext(my[i].rotation);

            }
        }

        if (stream.IsReading)//�ޱ�
        {
            //���� ü�� �ޱ�
            hpother.value = (int)stream.ReceiveNext();
            photonPos = (Vector3)stream.ReceiveNext();
            if (syncData != null)
            {
                for (int i = 0; i < others.Length; i++)//��
                {
                    syncData[i].pos = (Vector3)stream.ReceiveNext();
                    syncData[i].rot = (Quaternion)stream.ReceiveNext();
                }
            }
        }


    }
}






