using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemM : MonoBehaviour
{
    //myitem[0] �۵�
    public bool active = false;
    // fire / rope ���
    public bool f = false;
    public bool r = false;

    Player ps;
    PlayerM pm;
    PlayerPhoton pp;
    Rigidbody rb;

    //���� ������ ��ũ
    LineRenderer lr;
    public float ropeSpd = .03f;
    public GameObject hook;
    GameObject h;
    //��ũ ���� �� ��ƼŬ+�Ҹ� ������
    public GameObject hookP;
    bool isH = true;

    public GameObject shield;
   

    TrapManager tm;
    
    // ��ȭ�� ��ƼŬ
    public GameObject particle;
    GameObject p;

    void Start()
    {
        pp = GetComponent<PlayerPhoton>();
        ps = GetComponent<Player>();
        pm = GetComponent<PlayerM>();
        rb = GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();
        tm = GetComponent<TrapManager>(); 
    }

    void Update()
    {
        if (f)
        {
            rb.isKinematic = false;
            rb.AddForce(-pp.my[(int)PlayerPhoton.Parts.LHand].forward * ropeSpd*10);
            p.transform.position = pp.my[(int)PlayerPhoton.Parts.LHand].position;
                p.transform.forward = pp.my[(int)PlayerPhoton.Parts.LHand].forward;
        }
        else {

            if (p != null)
            {
                p = null;
            }
        }

        if (r)
        {
            lr.enabled = true;
            lr.SetPosition(0, pp.my[(int)PlayerPhoton.Parts.RHand].position);
            lr.SetPosition(1, h.transform.position);
            HookMovement hhook = h.GetComponent<HookMovement>();
            if (!hhook.moving) {
                if (isH)
                {
            rb.isKinematic = true;
                GameObject hEFT = Instantiate(hookP);
                hEFT.transform.position = h.transform.position;
                hEFT.transform.forward = -h.transform.forward;
                ParticleSystem hP = hEFT.GetComponent<ParticleSystem>();
                hP.Play();
                AudioSource hAs = hEFT.GetComponent<AudioSource>();
                hAs.Play();
                Destroy(hEFT, 10);
                    isH = false;
                }
                transform.position = Vector3.Lerp(transform.position, h.transform.position, 1*Time.deltaTime);
            }
        }
        else {
            lr.enabled = false;
            h = null;
        }

        if (active)
        {
            if (pm.myItem[0].name.Contains("Fire"))
            {
            print("Active - F");
                f = true;
                p = Instantiate(particle);
                p.transform.position = pp.my[(int)PlayerPhoton.Parts.LHand].position;
                p.transform.forward = pp.my[(int)PlayerPhoton.Parts.LHand].forward;
                Destroy(p, 5.5f);
            }

            if (pm.myItem[0].name.Contains("Rope"))
            {
                print("Active - R");
                h = Instantiate(hook);
                h.transform.position = pp.my[(int)PlayerPhoton.Parts.RHand].position;
                h.transform.forward = pp.my[(int)PlayerPhoton.Parts.RHand].forward;
                r = true;
                isH = true;
                SoundM.instance.playS(2, 2);
            }

            if (pm.myItem[0].name.Contains("Shield"))
            {
                print("Active shield");
                GameObject s = Instantiate(shield);
                s.transform.position = transform.position;

                rb.isKinematic = true;
                //��Ȧ �����
                tm.bH =false;
                Destroy(s, 5);
            }

            if (pm.myItem[0].name.Contains("Oxy"))
            {
                ps.PlusHp(30);
                print("add HP");
            }
            active = false;
            pm.myItem.RemoveAt(0);
            StartCoroutine(StopFR());
        }
    }
    IEnumerator StopFR() {
        yield return new WaitForSeconds(5);
        f = false; r = false;
        print("Stop Item Funtion..");
        //yield break;
    }
}
