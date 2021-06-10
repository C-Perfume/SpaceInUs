using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemM : MonoBehaviour
{
    // fire / rope 기능
    public bool f = false;
    public bool r = false;

    PlayerPhoton pp;
    Rigidbody rb;

    //로프 복제용 후크
    LineRenderer lr;
    public float ropeSpd = .03f;
    public GameObject hook;
    public GameObject h;
    //후크 박힐 때 파티클+소리 복제용
    public GameObject hookP;
    public bool isH = true;

    public GameObject shield;
    
    // 소화기 파티클
    public GameObject particle;
    public GameObject p;

    void Start()
    {
        pp = GetComponent<PlayerPhoton>();
        rb = GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
    }
 
}
