using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Value : MonoBehaviour
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
    public int num;
    // ����������
    public int trapNum;
    //������
    public int itNum;

    // Ȯ�� ������ ����
    public int rand;
    public int tRand;

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

    private void Start()
    {
        int num = transform.childCount; //int�� ������ �������ְ� �Ʒ��� �����Ǵ� Ȧ��(Step)���� ī��Ʈ��ȣ�� ������
        NetManager.Instance.RandomChange(num); //netmanager�� ����ü���� �Լ�(���� ����(num)ȣ��
        item = new GameObject("ItemList").transform;
        item.SetParent(transform.parent);
    }

    //�����
    public void ChangeRandomStep(int i, int rand, int tRand)
    {
        Value v = new Value();
        v.rand = rand; //valueŬ������ ������ �������ֱ�
        v.tRand = tRand;

        if (v.rand < 6) { v.num = 0; v.type = Value.Type.Step; }
        else if (v.rand < 9) { v.num = 1; v.type = Value.Type.Trap; }
        else { v.num = 2; v.type = Value.Type.Item; }


        v.mat = transform.GetChild(i).GetComponent<MeshRenderer>().material;

        if (i == 0) { v.num = 0; v.type = Value.Type.Step; }
        if (i == 1) { v.num = 1; v.type = Value.Type.Trap; v.tRand = 2; v.tT = Value.TrapType.BholeR; }
        if (i == 2) { v.num = 2; v.type = Value.Type.Item; v.tRand = 5; v.iT = Value.ItemType.OxyCan; }

        if (v.num == 0) { v.mat.color = Color.yellow; }
        else if (v.num == 1)
        {
            v.mat.color = Color.red;
            if (v.tRand == 1) { v.trapNum = 0; v.tT = Value.TrapType.BHoleL; }
            else if (v.tRand == 2 || v.tRand == 3) { v.trapNum = 1; v.tT = Value.TrapType.BholeR; }
            else if (v.tRand == 4) { v.trapNum = 2; v.tT = Value.TrapType.Meteor; }
            else { v.trapNum = 3; v.tT = Value.TrapType.Can; }
        }
        else
        {
            v.mat.color = Color.blue;
            if (v.tRand == 1 || v.tRand == 4)
            {
                v.itNum = 0; v.iT = Value.ItemType.Rope;
                Create(rope, transform.GetChild(i));
            }
            else if (v.tRand == 2)
            {
                v.itNum = 1; v.iT = Value.ItemType.FireEx;

                Create(fireEx, transform.GetChild(i));
            }
            else if (v.tRand == 3)
            {
                v.itNum = 2; v.iT = Value.ItemType.Shield;

                Create(shield, transform.GetChild(i));
            }
            else
            {
                v.itNum = 3; v.iT = Value.ItemType.OxyCan;

                Create(oxyCan, transform.GetChild(i));
            }
        }

        holds.Add(v);
    }
    //void Start() // �̱��÷���
    //{
    //    item = new GameObject("ItemList").transform;
    //    item.SetParent(transform.parent);

    //    for (int i = 0; i < transform.childCount; i++)
    //    {

    //        Value v = new Value();
    //        v.rand = Random.Range(1, 11);
    //        v.tRand = Random.Range(1, 11);

    //        if (v.rand < 6) { v.num = 0; v.type = Value.Type.Step; } 
    //        else if (v.rand < 9) { v.num = 1; v.type = Value.Type.Trap; } 
    //        else { v.num = 2; v.type = Value.Type.Item; }


    //        v.mat = transform.GetChild(i).GetComponent<MeshRenderer>().material;

    //        if (i == 0) { v.num = 0; v.type = Value.Type.Step; }
    //        if (i == 1) { v.num = 1;  v.type = Value.Type.Trap; v.tRand = 2;  v.tT = Value.TrapType.BholeR; }
    //        if (i == 2) { v.num = 2;  v.type = Value.Type.Item; v.tRand = 5;       v.iT = Value.ItemType.OxyCan; }

    //        if (v.num == 0) { v.mat.color = Color.yellow; }
    //        else if (v.num == 1)
    //        {
    //            v.mat.color = Color.red;
    //            if (v.tRand == 1) { v.trapNum = 0; v.tT = Value.TrapType.BHoleL; }
    //            else if (v.tRand == 2 || v.tRand == 3) { v.trapNum = 1; v.tT = Value.TrapType.BholeR; }
    //            else if (v.tRand == 4) { v.trapNum = 2; v.tT = Value.TrapType.Meteor; }
    //            else { v.trapNum = 3; v.tT = Value.TrapType.Can; }
    //        }
    //        else
    //        {
    //            v.mat.color = Color.blue;
    //            if (v.tRand == 1 || v.tRand == 4) { v.itNum = 0; v.iT = Value.ItemType.Rope;
    //                Create(rope, transform.GetChild(i));
    //            }
    //            else if (v.tRand == 2) { v.itNum = 1; v.iT = Value.ItemType.FireEx; 

    //                Create(fireEx, transform.GetChild(i));
    //            }
    //            else if (v.tRand == 3) { v.itNum = 2; v.iT = Value.ItemType.Shield; 

    //                Create(shield, transform.GetChild(i));
    //            }
    //            else { v.itNum = 3; v.iT = Value.ItemType.OxyCan; 

    //                Create(oxyCan, transform.GetChild(i));
    //            }
    //        }

    //            holds.Add(v);
    //    }

    //}
    void Create(GameObject obj, Transform h)
    {
        GameObject a = Instantiate(obj);
        a.transform.position = h.position + h.forward * -.05f +        h.up * .1f;
        a.transform.SetParent(item);
        items.Add(a);
    }

     void Update()
    {
        if (item_False.Count > 0) {
            StartCoroutine(ShowUp());
        }
    }

    //������ ����

    IEnumerator ShowUp()
    {
        items.Add(item_False[0]);
        print(item_False[0].name + "����� Ȯ�� ��Ȱ��ȭ 0��");
        item_False.RemoveAt(0);
        if(item_False.Count > 0) print(item_False[0].name + "����� Ȯ�� ��Ȱ��ȭ 0�� ���� ��");
        yield return new WaitForSeconds(30);
        items[items.Count-1].SetActive(true);
        print(items[items.Count - 1].name +"����� Ȯ�� Ȱ��ȭ��� �� ������");
    }
}
