using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
public class Value
{
    public enum Type
    {
        Step,//4
        Trap,//2
        Item//4
    }
    public enum TrapType
    {
        BHoleL,
        BholeR,
        Meteor, 
        UpsideDown,
        Can
    }
  
    public Type type;
    public TrapType tT;
  
    // �Ϲ�, ����, ������, ������ ���� ��
    public GameObject rockItem;
    public bool popUp = false;
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
    public List<int> hold_Idx = new List<int>();
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
                    int colorN = Random.Range(0, 8); // 8���� ����

                    //�̾Ƴ� ���ڸ�ŭ ���Ǿ��� ����. RockParents ��ũ��Ʈ�� ������ �ΰ�(rand, tRand)
                    pv.RPC("RpcRockstep", RpcTarget.AllBuffered, i, rand, tRand, colorN);


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
    //RockParents�� �ִ� �Լ� �������� (RPC�� �����)
    [PunRPC]
    public void RpcRockstep(int i, int rand, int tRand, int colorNum)
    {
        Value v = new Value();
        v.mat = transform.GetChild(i).GetComponent<MeshRenderer>().material;

        if (rand < 5) { v.type = Value.Type.Step; }
        else if (rand < 7) { v.type = Value.Type.Trap; }
        else { v.type = Value.Type.Item; }

        //ó�� 0, 1, 2, 3�� ������ �� �ֱ� ���� / ȭ��ƮȦ / ����
        if (i == 0 || i == 1|| i == 2|| i == 3) { v.type = Value.Type.Step; }
        if (i == 4 || i == 9 || i == 6 || i == 11) { v.type = Value.Type.Trap; tRand = 3; }
        if (i == 8 || i == 5 || i == 10 || i == 7) { v.type = Value.Type.Item; tRand = 3; }

        //Ʈ������ 1:2:1:6Ȯ��
        if (v.type == Value.Type.Trap)
        {
            //1�� ���ֹ̾� ��Ȧ, 2�� ȭ��ƮȦ, 4�� ���׿�, 3,5 �Ųٷ� ������ ĵ
            if (tRand == 1) { v.tT = Value.TrapType.BHoleL; }
            else if (tRand == 2) { v.tT = Value.TrapType.BholeR; }
            else if (tRand == 4) { v.tT = Value.TrapType.Meteor; }
            else if (tRand == 5 || tRand == 3) { v.tT = Value.TrapType.UpsideDown; }
            else { v.tT = Value.TrapType.Can; }
        }

        //������ ���� 1:1:1:1:6 Ȯ��
        if (v.type == Value.Type.Item)
        {
            // 1�� ����
            if (tRand == 1) {Create(rope, transform.GetChild(i), v); }

            // 2�� ��ȭ��
            else if (tRand == 2) { Create(fireEx, transform.GetChild(i), v); }
           
            //3�� ����
            else if (tRand == 3){ Create(shield, transform.GetChild(i), v); }
           
             //0�� ranBox 1�ο�
            else if (tRand == 0){ Create(ranBox[0], transform.GetChild(i), v); }

            //10�� ranBox ���ο�
            else if (tRand == 10) { Create(ranBox[1], transform.GetChild(i), v); }

            //������ ���
            else { Create(oxyCan, transform.GetChild(i), v); }

           // ������ ������ �� � �������� popup�� ������ Ư���ϱ�
            hold_Idx.Add(i);
        }

            v.mat.color = Color.black;
                if (colorNum == 1) { v.mat.color = Color.red; }
                 else if (colorNum == 2) { v.mat.color = Color.magenta; }
                 else if (colorNum == 3) { v.mat.color = Color.yellow; }
                 else if (colorNum == 4) { v.mat.color = Color.green; }
                 else if (colorNum == 5) { v.mat.color = Color.cyan; }
                 else if (colorNum == 6) { v.mat.color = Color.blue; }
                 else if (colorNum == 7) { v.mat.color = Color.grey; }


            holds.Add(v);
    }

    [PunRPC]
    public void RpcIsMaster() { isMaster = true; }

    void Create(GameObject obj, Transform h, Value v)
    {
        GameObject a = Instantiate(obj);
        a.transform.position = h.position + h.forward * -.05f + h.up * .07f;
        a.transform.SetParent(item);
        // ������ ������ �� Ȱ��ȭ �� ������ Ư���ϱ�
        v.rockItem = a;
      
        // ����, �����̸� �˾� Ȱ��ȭ + �����۸���Ʈ�� �ֱ�
        if (obj == shield || obj == ranBox[0] || obj == ranBox[1]) { v.popUp = true; }
        // �ƴϸ� ��Ȱ��ȭ +��Ȱ��ȭ ����Ʈ�� �ֱ� 
        else { a.SetActive(false);  }
    }

    //������ 30�ʰ� ����Ƶ� �ȳ����� �ϱ�
    public IEnumerator ShowUp(int idx)
    {
        yield return new WaitForSeconds(30);
        holds[hold_Idx[idx]].popUp = false;

        GameObject a = item.GetChild(idx).gameObject;
        //���峪 �����̸�
        if (a.name.Contains("Shield")|| a.name.Contains("RanBox")) {
            //Ȱ��ȭ + �˾� �˸���
            a.SetActive(true);
            holds[hold_Idx[idx]].popUp = true;
        }
    }
}
