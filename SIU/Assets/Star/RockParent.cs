using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Value : MonoBehaviourPun
{
    public enum Type
    {
        Step,//5
        Trap,//3
        Item//2
    }
    public enum TrapType
    {
        BHoleL, // 1 확률
        BholeR, // 화이트 홀 4
        Meteor, // 1
        Can // 4
    }
    public enum ItemType
    {
        Rope, // 1 확률
        FireEx, // 1
        Shield, // 1
        OxyCan // 7
    }

    public Type type;
    public TrapType tT;
    public ItemType iT;

    // 일반, 함정, 아이템 설정 값
    public int num;
    // 함정설정값
    public int trapNum;
    //아이템
    public int itNum;

    // 확률 결정용 변수
    public int rand;
    public int tRand;

    // 색상
    public Material mat;

}
public class RockParent : MonoBehaviourPun
{
    public List<Value> holds = new List<Value>();

    //아이템 생성공장
    public GameObject rope;
    public GameObject fireEx;
    public GameObject shield;
    public GameObject oxyCan;

    //setparent용
    public Transform item;
    public List<GameObject> items = new List<GameObject>();
    public List<GameObject> item_False = new List<GameObject>();
    public Transform rockParent;
    public Transform free;


    PhotonView pv;
    void Start()
    {
        pv = GetComponent<PhotonView>();
        //if (photonView.IsMine)         
            rockParent = GameObject.Find("Rock").transform;
        item = new GameObject("ItemList").transform;
        free = new GameObject("Free").transform;
        item.SetParent(rockParent.parent);
        free.SetParent(rockParent.parent);

        if (photonView.IsMine == PhotonNetwork.IsMasterClient)
        {

            if (PhotonNetwork.IsMasterClient)
            {
                for (int i = 0; i < rockParent.childCount; i++) //하위오브젝트 숫자 받아온 걸로 포문을 돌리자
                {
                    int rand = Random.Range(1, 11); //랜덤레인지값을 하위 오브젝트 숫자만큼 뽑아내자
                    int tRand = Random.Range(1, 11);
                    //뽑아낸 숫자만큼 알피씨를 보냄. RockParents 스크립트의 체인지랜덤스탭 스크립트에 몇번째 하위 오브젝트인지(i), 랜덤값 두개(rand, tRand)
                    pv.RPC("RpcRockstep", RpcTarget.AllBuffered, i, rand, tRand);

                }
            }
        }
    }

    //포톤용
    [PunRPC]
    public void RpcRockstep(int i, int rand, int tRand) //RockParents에 있는 함수 가져오기 (RPC로 만들기)
    {
        if(rockParent == null)
        {
            rockParent = GameObject.Find("Rock").transform;
        }
        Value v = new Value();
        v.rand = rand; //value클래스의 랜덤값 저장해주기
        v.tRand = tRand;

        if (v.rand < 6) { v.num = 0; v.type = Value.Type.Step; }
        else if (v.rand < 9) { v.num = 1; v.type = Value.Type.Trap; }
        else { v.num = 2; v.type = Value.Type.Item; }


        v.mat = rockParent.GetChild(i).GetComponent<MeshRenderer>().material;

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
                //pv.RPC("RPCCreate", RpcTarget.AllBuffered, rope, rockParent.GetChild(i)); 
                Create(rope, rockParent.GetChild(i));
            }
            else if (v.tRand == 2)
            {
                v.itNum = 1; v.iT = Value.ItemType.FireEx;

              //  pv.RPC("RPCCreate", RpcTarget.AllBuffered, fireEx, rockParent.GetChild(i)); 
               Create(fireEx, rockParent.GetChild(i));
            }
            else if (v.tRand == 3)
            {
                v.itNum = 2; v.iT = Value.ItemType.Shield;

                //pv.RPC("RPCCreate", RpcTarget.AllBuffered, shield, rockParent.GetChild(i)); 
              Create(shield, rockParent.GetChild(i));
            }
            else
            {
                v.itNum = 3; v.iT = Value.ItemType.OxyCan;

                //pv.RPC("RPCCreate", RpcTarget.AllBuffered, oxyCan, rockParent.GetChild(i)); 
                Create(oxyCan, rockParent.GetChild(i));
            }
        }

        holds.Add(v);
    }



    //[PunRPC]
    //public void RPCCreate(GameObject obj, Transform h)
    //{
    //    GameObject a = Instantiate(obj);
    //    a.transform.position = h.position + h.forward * -.05f + h.up * .1f;
    //    a.transform.SetParent(item);
    //    items.Add(a);
    //}

    void Create(GameObject obj, Transform h)
    {
        GameObject a = Instantiate(obj);
        a.transform.position = h.position + h.forward * -.05f + h.up * .1f;
        a.transform.SetParent(item);
        items.Add(a);
    }

    //아이템 생성

    public IEnumerator ShowUp(GameObject item)
    {
        items.Add(item_False[0]);
        item_False.RemoveAt(0);
        yield return new WaitForSeconds(30);
        item.SetActive(true);
    }
}
