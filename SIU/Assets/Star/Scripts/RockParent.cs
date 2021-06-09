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
        Rope, // 1 Ȯ��
        FireEx, // 1
        Shield, // 1
        OxyCan // 7
    }

    public Type type;
    public TrapType tT;
    public ItemType iT;

    // �Ϲ�, ����, ������ ���� ��
    public int[] num = { 0, 0 };

    // ����
    public Material mat;

}
public class RockParent : MonoBehaviour
{
    public List<Value> holds = new List<Value>();

    //������ ��������
    public GameObject rope;
    public GameObject fireEx;
    public GameObject shield;
    public GameObject oxyCan;

    //setparent��
    public Transform item;
    public List<GameObject> items = new List<GameObject>();
    public List<GameObject> item_False = new List<GameObject>();
    public Transform free;

    PhotonView pv;

    bool isMaster = false;
    bool isRandom= false;
    void Start()
    {
        pv = GetComponent<PhotonView>();
        item = GameObject.Find("Item").transform;
        free = GameObject.Find("Free").transform;
      
    }
    void Update()
    {
    
        if (!isMaster)
        {
            if (pv.IsMine == PhotonNetwork.IsMasterClient)
            {
                isRandom = true;
                pv.RPC("RpcIsRandom", RpcTarget.AllBuffered);

            }
        }

        if (isRandom)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                for (int i = 0; i < transform.childCount; i++) //����������Ʈ ���� �޾ƿ� �ɷ� ������ ������
                {
                    int rand = Random.Range(1, 11); //�������������� ���� ������Ʈ ���ڸ�ŭ �̾Ƴ���
                    int tRand = Random.Range(1, 11);
                    //�̾Ƴ� ���ڸ�ŭ ���Ǿ��� ����. RockParents ��ũ��Ʈ�� ü������������ ��ũ��Ʈ�� ���° ���� ������Ʈ����(i), ������ �ΰ�(rand, tRand)
                    pv.RPC("RpcRockstep", RpcTarget.AllBuffered, i, rand, tRand);
                }
                isRandom = false;
               
            }
        }

    }

    //�����
    [PunRPC]
    public void RpcRockstep(int i, int rand, int tRand) //RockParents�� �ִ� �Լ� �������� (RPC�� �����)
    {
        //if(free == null)
        //{
        //    item = GameObject.Find("Item").transform;
        //    free = GameObject.Find("Free").transform;
        //}

        Value v = new Value();
        v.num[0] = rand;
        v.num[1] = tRand;
        v.mat = transform.GetChild(i).GetComponent<MeshRenderer>().material;

        if (v.num[0] < 6) { v.type = Value.Type.Step; }
        else if (v.num[0] < 9) { v.type = Value.Type.Trap; }
        else { v.type = Value.Type.Item; }

        if (i == 0) {  v.type = Value.Type.Step; }
        if (i == 1) {  v.type = Value.Type.Trap; v.tT = Value.TrapType.BholeR; }
        if (i == 2) {  v.type = Value.Type.Item; v.iT = Value.ItemType.OxyCan; }

        if (v.type == Value.Type.Step) { v.mat.color = Color.yellow; }
        else if (v.type == Value.Type.Trap)  {  v.mat.color = Color.red;
           
            if (v.num[1] == 1) { v.tT = Value.TrapType.BHoleL; }
            else if (v.num[1] == 2 || v.num[1] == 3) { v.tT = Value.TrapType.BholeR; }
            else if (v.num[1] == 4) { v.tT = Value.TrapType.Meteor; }
            else { v.tT = Value.TrapType.Can; }

        }
        else  {  v.mat.color = Color.blue;
          
       
            if (v.num[1] == 1 || v.num[1] == 4) 
            {
                v.iT = Value.ItemType.Rope;
                Create(rope, transform.GetChild(i));
            }
            else if (v.num[1] == 2)
            {
                v.iT = Value.ItemType.FireEx;
               Create(fireEx, transform.GetChild(i));
            }
            else if (v.num[1] == 3)
            {
               v.iT = Value.ItemType.Shield;
              Create(shield, transform.GetChild(i));
            }
            else
            {
                v.iT = Value.ItemType.OxyCan;
                Create(oxyCan, transform.GetChild(i));
            }
        }

        holds.Add(v);
    }

    [PunRPC]
    public void RpcIsRandom() { isMaster = true;  }

    void Create(GameObject obj, Transform h)
    {
        GameObject a = Instantiate(obj);
        a.transform.position = h.position + h.forward * -.05f + h.up * .1f;
        a.transform.SetParent(item);
        items.Add(a);
    }

    //������ ����

    public IEnumerator ShowUp(GameObject item)
    {
        items.Add(item_False[0]);
        item_False.RemoveAt(0);
        yield return new WaitForSeconds(30);
        item.SetActive(true);
    }
}
