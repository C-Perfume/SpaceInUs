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
        RanBox, //0번 1확률
        Rope, // 1  1
        FireEx, // 2  1
        Shield, // 3  1
        OxyCan // 4~9  6

    }

    public Type type;
    public TrapType tT;
    public ItemType iT;

    // 일반, 함정, 아이템, 돌색깔 설정 값
    public int itemNum = -1;

    // 색상
    public Material mat;

}
public class RockParent : MonoBehaviour
{

    //아이템 생성공장
    public GameObject[] ranBox;
    public GameObject rope;
    public GameObject fireEx;
    public GameObject shield;
    public GameObject oxyCan;

    //setparent용
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
                for (int i = 0; i < transform.childCount; i++) //하위오브젝트 숫자 받아온 걸로 포문을 돌리자
                {
                    int rand = Random.Range(1, 11); //랜덤레인지값을 하위 오브젝트 숫자만큼 뽑아내자
                    int tRand = Random.Range(0, 10); //0~9까지 0번이 Ranbox1인용
                    if (maxP > 1) { tRand = Random.Range(1, 11); } // 10번이  Ranbox다인용
                  //  int colorN = Random.Range(0, 8); // 8가지 색상

                    //뽑아낸 숫자만큼 알피씨를 보냄. RockParents 스크립트의 랜덤값 두개(rand, tRand)
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


    //포톤용
    [PunRPC]
    public void RpcRockstep(int i, int rand, int tRand 
      // , int colorNum
        ) //RockParents에 있는 함수 가져오기 (RPC로 만들기)
    {
        Value v = new Value();
        v.mat = transform.GetChild(i).GetComponent<MeshRenderer>().material;

        if (rand < 6) { v.type = Value.Type.Step; }
        else if (rand < 9) { v.type = Value.Type.Trap; }
        else { v.type = Value.Type.Item; }

        //처음 0, 1, 2번 지정된 값 넣기 스텝 / 화이트홀 / 산소통
        if (i == 0) { v.type = Value.Type.Step; }
        if (i == 1) { v.type = Value.Type.Trap; tRand = 2; }
        if (i == 2) { v.type = Value.Type.Item; tRand = 10; }

        //스텝이면 노란색
        if (v.type == Value.Type.Step) { v.mat.color = Color.yellow; }

        //트랩이면 빨강색 >> 개발로 수정 중  >> 아이템이랑 같은 색상 만들기
        else if (v.type == Value.Type.Trap)
        {
            v.mat.color = Color.red;

            //1번 우주미아 블랙홀, 2+3번 화이트홀, 4번 메테오, 나머지 캔
            if (tRand == 1) { v.tT = Value.TrapType.BHoleL; }
            else if (tRand == 2 || tRand == 3) { v.tT = Value.TrapType.BholeR; }
            else if (tRand == 4) { v.tT = Value.TrapType.Meteor; }
            else { v.tT = Value.TrapType.Can; }

        }

        // 아이템이면 파란색
        else
        {
            v.mat.color = Color.blue;
           
                // 1번 로프
                if (tRand == 1)
            {
                v.iT = Value.ItemType.Rope;
                Create(rope, transform.GetChild(i));
            }
            // 2번 소화기
            else if (tRand == 2)
            {
                v.iT = Value.ItemType.FireEx;
                Create(fireEx, transform.GetChild(i));
            }
            //3번 쉴드
            else if (tRand == 3)
            {
                v.iT = Value.ItemType.Shield;
                Create(shield, transform.GetChild(i));
            }
             //0번 ranBox 1인용
            else if (tRand == 0)
            {
                v.iT = Value.ItemType.RanBox;
                Create(ranBox[0], transform.GetChild(i));
            }
            //10번 ranBox 다인용
            else if (tRand == 10)
            {
                v.iT = Value.ItemType.RanBox;
                Create(ranBox[1], transform.GetChild(i));
            }
            //나머지 산소
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

    //아이템 생성
    public IEnumerator ShowUp()
    {
        yield return new WaitForSeconds(30);
        item_False[0].SetActive(true);
        items.Add(item_False[0]);
        item_False.RemoveAt(0);
    }
}
