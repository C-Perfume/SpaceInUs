using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
public class Value
{
    public enum Type
    {
        Step,//5
        Trap,//3
        Item//2
    }
    public enum TrapType
    {
        BHoleL, // 1 Ȯ��
        BholeR, // ȭ��Ʈ Ȧ 4
        Meteor, // 1
        Can // 4
    }
    public enum ItemType
    {
        RanBox, //0�� 1Ȯ��
        Rope, // 1  1
        FireEx, // 2  1
        Shield, // 3  1
        OxyCan // 4~9  6

    }

    public Type type;
    public TrapType tT;
    public ItemType iT;

    // �Ϲ�, ����, ������, ������ ���� ��
    public int itemNum = -1;

    // ����
    public Material mat;

}
public class RockParent : MonoBehaviour
{

    //������ ��������
    public GameObject[] ranBox;
    public GameObject rope;
    public GameObject fireEx;
    public GameObject shield;
    public GameObject oxyCan;

    //setparent��
    public Transform item;
    public List<Value> holds = new List<Value>();
    public List<GameObject> items = new List<GameObject>();
    public List<GameObject> item_False = new List<GameObject>();
    public Transform free;

    PhotonView pv;

    bool isMaster = false;
    Vector3 rockOriginPos;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        item = GameObject.Find("Item").transform;
        free = GameObject.Find("Free").transform;
        rockOriginPos = transform.parent.position;
        transform.parent.position -= Vector3.up * 10000;
        StartCoroutine(SetRock());
    }


    public IEnumerator SetRock()
    {
        while (pv.IsMine != PhotonNetwork.IsMasterClient)
        {
            yield return null;
        }

        byte maxP = PhotonNetwork.CurrentRoom.MaxPlayers;
        NetManager net = GameObject.Find("NetManager").GetComponent<NetManager>();

        if (!isMaster)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                for (int i = 0; i < transform.childCount; i++) //����������Ʈ ���� �޾ƿ� �ɷ� ������ ������
                {
                    int rand = Random.Range(1, 11); //�������������� ���� ������Ʈ ���ڸ�ŭ �̾Ƴ���
                    int tRand = Random.Range(0, 10); //0~9���� 0���� Ranbox1�ο�
                    if (maxP > 1) { tRand = Random.Range(1, 11); } // 10����  Ranbox���ο�
                  //  int colorN = Random.Range(0, 8); // 8���� ����

                    //�̾Ƴ� ���ڸ�ŭ ���Ǿ��� ����. RockParents ��ũ��Ʈ�� ������ �ΰ�(rand, tRand)
                    pv.RPC("RpcRockstep", RpcTarget.AllBuffered, i, rand, tRand
                  //      , colorN
                        );
                }

            }
            pv.RPC("RpcIsMaster", RpcTarget.AllBuffered);
        }

        while (maxP != net.playerList.Count)
        {
            yield return null;
        }
        transform.parent.position = rockOriginPos;
    }


    //�����
    [PunRPC]
    public void RpcRockstep(int i, int rand, int tRand 
      // , int colorNum
        ) //RockParents�� �ִ� �Լ� �������� (RPC�� �����)
    {
        Value v = new Value();
        v.mat = transform.GetChild(i).GetComponent<MeshRenderer>().material;

        if (rand < 6) { v.type = Value.Type.Step; }
        else if (rand < 9) { v.type = Value.Type.Trap; }
        else { v.type = Value.Type.Item; }

        //ó�� 0, 1, 2�� ������ �� �ֱ� ���� / ȭ��ƮȦ / �����
        if (i == 0) { v.type = Value.Type.Step; }
        if (i == 1) { v.type = Value.Type.Trap; tRand = 2; }
        if (i == 2) { v.type = Value.Type.Item; tRand = 10; }

        //�����̸� �����
        if (v.type == Value.Type.Step) { v.mat.color = Color.yellow; }

        //Ʈ���̸� ������ >> ���߷� ���� ��  >> �������̶� ���� ���� �����
        else if (v.type == Value.Type.Trap)
        {
            v.mat.color = Color.red;

            //1�� ���ֹ̾� ��Ȧ, 2+3�� ȭ��ƮȦ, 4�� ���׿�, ������ ĵ
            if (tRand == 1) { v.tT = Value.TrapType.BHoleL; }
            else if (tRand == 2 || tRand == 3) { v.tT = Value.TrapType.BholeR; }
            else if (tRand == 4) { v.tT = Value.TrapType.Meteor; }
            else { v.tT = Value.TrapType.Can; }

        }

        // �������̸� �Ķ���
        else
        {
            v.mat.color = Color.blue;
           
                // 1�� ����
                if (tRand == 1)
            {
                v.iT = Value.ItemType.Rope;
                Create(rope, transform.GetChild(i));
            }
            // 2�� ��ȭ��
            else if (tRand == 2)
            {
                v.iT = Value.ItemType.FireEx;
                Create(fireEx, transform.GetChild(i));
            }
            //3�� ����
            else if (tRand == 3)
            {
                v.iT = Value.ItemType.Shield;
                Create(shield, transform.GetChild(i));
            }
             //0�� ranBox 1�ο�
            else if (tRand == 0)
            {
                v.iT = Value.ItemType.RanBox;
                Create(ranBox[0], transform.GetChild(i));
            }
            //10�� ranBox ���ο�
            else if (tRand == 10)
            {
                v.iT = Value.ItemType.RanBox;
                Create(ranBox[1], transform.GetChild(i));
            }
            //������ ���
            else
            {
                v.iT = Value.ItemType.OxyCan;
                Create(oxyCan, transform.GetChild(i));
            }
        }

        //v.mat.color = Color.black;
        //if (colorNum == 1) { v.mat.color = Color.red; }
        //else if (colorNum == 2) { v.mat.color = Color.magenta; }
        //else if (colorNum == 3) { v.mat.color = Color.yellow; }
        //else if (colorNum == 4) { v.mat.color = Color.green; }
        //else if (colorNum == 5) { v.mat.color = Color.cyan; }
        //else if (colorNum == 6) { v.mat.color = Color.blue; }
        //else if (colorNum == 7) { v.mat.color = Color.grey; }

        
        holds.Add(v);
    }

    [PunRPC]
    public void RpcIsMaster() { isMaster = true; }

    void Create(GameObject obj, Transform h)
    {
        GameObject a = Instantiate(obj);
        a.transform.position = h.position + h.forward * -.05f + h.up * .07f;
        a.transform.SetParent(item);
        items.Add(a);
    }

    //������ ����
    public IEnumerator ShowUp()
    {
        yield return new WaitForSeconds(30);
        item_False[0].SetActive(true);
        items.Add(item_False[0]);
        item_False.RemoveAt(0);
    }
}
