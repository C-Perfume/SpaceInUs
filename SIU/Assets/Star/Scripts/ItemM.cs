using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemM : MonoBehaviour
{
    // fire / rope ���
    public bool f = false;
    public bool r = false;

    PlayerPhoton pp;
    Rigidbody rb;

    //���� ������ ��ũ
    LineRenderer lr;
    public float ropeSpd = .03f;
    public GameObject hook;
    public GameObject h;
    //��ũ ���� �� ��ƼŬ+�Ҹ� ������
    public GameObject hookP;
    public bool isH = true;

    public GameObject shield;
    
    // ��ȭ�� ��ƼŬ
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
