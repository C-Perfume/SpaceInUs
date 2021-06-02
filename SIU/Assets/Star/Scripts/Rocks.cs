using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocks : MonoBehaviour
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

    // Ȯ�� ������ ����
    public int rand;
    public int tRand;

    // �Ϲ�, ����, ������ ���� ��
    public int num;
    // ����������
    public int trapNum;
    public int itNum;
    Material mat;

    //public Type type;
    //public TrapType tType;
    //public ItemType itType;

    //������ ��������
   public GameObject rope;
   public GameObject fireEx;
   public GameObject Shield;
   public GameObject OxyCan;
    //setparent��
    public Transform item;
    public List<GameObject> items = new List<GameObject>();

    //������ ��������
   public bool full = false;

    void Start()
    {
        if (gameObject.transform == gameObject.transform.parent.GetChild(0)) num = 0;
        else if (gameObject.transform == gameObject.transform.parent.GetChild(2)) num = 2;
        else
        {
            rand = Random.Range(1, 11);
            tRand = Random.Range(1, 11);
            if (rand < 6) { num = 0; } else if (rand < 9) { num = 1; } else { num = 2; }
        }
        
        mat = GetComponent<MeshRenderer>().material;

        if (num == 0) mat.color = Color.yellow;
        else if (num == 1) { mat.color = Color.red;
            if (tRand == 1) { trapNum = 0; } 
            else if (tRand < 6) { trapNum = 1; } 
            else if (tRand == 6) { trapNum = 2; } 
            else { trapNum = 3; }
        }
        else mat.color = Color.blue;
        if (tRand == 1) { itNum = 0; } 
        else if (tRand == 2) { itNum = 1; } 
        else if (tRand == 3) { itNum = 2; } 
        else { itNum = 3; } // 3

    }

    void Update()
    {
        if (items.Count == 0 &&
            !full && num == 2)
        {
            
            if (itNum == (int)ItemType.Rope)
            {
                Create(rope);
            }
            else if (itNum == (int)ItemType.FireEx)
            {
                Create(fireEx);
            }
            else if (itNum == (int)ItemType.Shield)
            {
                Create(Shield);
            }
            else
            {
                Create(OxyCan);
            }
           
        }

        if (full 
            && items.Count > 0 
            && !items[0].activeSelf)
        {
            print("GrabItem");
            StartCoroutine(ShowUp());
            full = false;
        }

    }


//������ ����    
    void Create(GameObject obj) {
        GameObject a = Instantiate(obj);
        a.transform.position =
            transform.position +
            //transform.right * Random.Range(0.05f, 0.15f)+
            transform.up * .1f; 
            //Random.Range(0.05f, 0.15f) + 
            //transform.forward*0.1f ;
        a.transform.SetParent(item);
        items.Add(a);
        full = true;
    }

    IEnumerator ShowUp() {
        yield return new WaitForSeconds(10);
        items[0].SetActive(true);
        full = true;
    }
}
