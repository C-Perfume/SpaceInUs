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
    public int[] num = { 0, 0 };

    // 색상
    public Material mat;

}
public class RockParent : MonoBehaviour
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
                for (int i = 0; i < transform.childCount; i++) //하위오브젝트 숫자 받아온 걸로 포문을 돌리자
                {
                    int rand = Random.Range(1, 11); //랜덤레인지값을 하위 오브젝트 숫자만큼 뽑아내자
                    int tRand = Random.Range(1, 11);
                    //뽑아낸 숫자만큼 알피씨를 보냄. RockParents 스크립트의 체인지랜덤스탭 스크립트에 몇번째 하위 오브젝트인지(i), 랜덤값 두개(rand, tRand)
                    pv.RPC("RpcRockstep", RpcTarget.AllBuffered, i, rand, tRand);
                }
                isRandom = false;
               
            }
        }

    }

    //포톤용
    [PunRPC]
    public void RpcRockstep(int i, int rand, int tRand) //RockParents에 있는 함수 가져오기 (RPC로 만들기)
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

    //아이템 생성

    public IEnumerator ShowUp(GameObject item)
    {
        items.Add(item_False[0]);
        item_False.RemoveAt(0);
        yield return new WaitForSeconds(30);
        item.SetActive(true);
    }
}
