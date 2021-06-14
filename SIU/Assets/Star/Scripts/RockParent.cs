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
  
    // 일반, 함정, 아이템, 돌색깔 설정 값
    public GameObject rockItem;
    public bool popUp = false;
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
                for (int i = 0; i < transform.childCount; i++) //하위오브젝트 숫자 받아온 걸로 포문을 돌리자
                {
                    int rand = Random.Range(1, 11); //랜덤레인지값을 하위 오브젝트 숫자만큼 뽑아내자
                    int tRand = Random.Range(0, 10); //0~9까지 0번이 Ranbox1인용
                    if (maxP > 1) { tRand = Random.Range(1, 11); } // 10번이  Ranbox다인용
                    int colorN = Random.Range(0, 8); // 8가지 색상

                    //뽑아낸 숫자만큼 알피씨를 보냄. RockParents 스크립트의 랜덤값 두개(rand, tRand)
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


    //포톤용
    //RockParents에 있는 함수 가져오기 (RPC로 만들기)
    [PunRPC]
    public void RpcRockstep(int i, int rand, int tRand, int colorNum)
    {
        Value v = new Value();
        v.mat = transform.GetChild(i).GetComponent<MeshRenderer>().material;

        if (rand < 5) { v.type = Value.Type.Step; }
        else if (rand < 7) { v.type = Value.Type.Trap; }
        else { v.type = Value.Type.Item; }

        //처음 0, 1, 2, 3번 지정된 값 넣기 스텝 / 화이트홀 / 쉴드
        if (i == 0 || i == 1|| i == 2|| i == 3) { v.type = Value.Type.Step; }
        if (i == 4 || i == 9 || i == 6 || i == 11) { v.type = Value.Type.Trap; tRand = 3; }
        if (i == 8 || i == 5 || i == 10 || i == 7) { v.type = Value.Type.Item; tRand = 3; }

        //트랩설정 1:2:1:6확률
        if (v.type == Value.Type.Trap)
        {
            //1번 우주미아 블랙홀, 2번 화이트홀, 4번 메테오, 3,5 거꾸로 나머지 캔
            if (tRand == 1) { v.tT = Value.TrapType.BHoleL; }
            else if (tRand == 2) { v.tT = Value.TrapType.BholeR; }
            else if (tRand == 4) { v.tT = Value.TrapType.Meteor; }
            else if (tRand == 5 || tRand == 3) { v.tT = Value.TrapType.UpsideDown; }
            else { v.tT = Value.TrapType.Can; }
        }

        //아이템 설정 1:1:1:1:6 확률
        if (v.type == Value.Type.Item)
        {
            // 1번 로프
            if (tRand == 1) {Create(rope, transform.GetChild(i), v); }

            // 2번 소화기
            else if (tRand == 2) { Create(fireEx, transform.GetChild(i), v); }
           
            //3번 쉴드
            else if (tRand == 3){ Create(shield, transform.GetChild(i), v); }
           
             //0번 ranBox 1인용
            else if (tRand == 0){ Create(ranBox[0], transform.GetChild(i), v); }

            //10번 ranBox 다인용
            else if (tRand == 10) { Create(ranBox[1], transform.GetChild(i), v); }

            //나머지 산소
            else { Create(oxyCan, transform.GetChild(i), v); }

           // 아이템 집었을 때 어떤 돌멩이의 popup을 꺼줄지 특정하기
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
        // 돌맹이 집었을 때 활성화 될 아이템 특정하기
        v.rockItem = a;
      
        // 쉴드, 랜박이면 팝업 활성화 + 아이템리스트에 넣기
        if (obj == shield || obj == ranBox[0] || obj == ranBox[1]) { v.popUp = true; }
        // 아니면 비활성화 +비활성화 리스트에 넣기 
        else { a.SetActive(false);  }
    }

    //아이템 30초간 돌잡아도 안나오게 하기
    public IEnumerator ShowUp(int idx)
    {
        yield return new WaitForSeconds(30);
        holds[hold_Idx[idx]].popUp = false;

        GameObject a = item.GetChild(idx).gameObject;
        //쉴드나 랜박이면
        if (a.name.Contains("Shield")|| a.name.Contains("RanBox")) {
            //활성화 + 팝업 알리기
            a.SetActive(true);
            holds[hold_Idx[idx]].popUp = true;
        }
    }
}
