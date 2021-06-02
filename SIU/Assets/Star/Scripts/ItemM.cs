using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemM : MonoBehaviour
{
    public bool active = false;

    GameObject p1;
    Player ps;
    PlayerM pm;
    Rigidbody rb;
    public float ropeSpd = .03f;
    GameObject shield;
    void Start()
    {
        p1 = GameObject.Find("Player");
        ps = p1.GetComponent<Player>();
        pm = p1.GetComponent<PlayerM>();
        rb = p1.GetComponent<Rigidbody>();
        shield = ps.skillShield;
        shield.SetActive(false);
    }

    void Update()
    {
        if (active)
        {
            if (gameObject.name.Contains("Fire"))
            {
                print("Active fire");
                rb.isKinematic = false;
                rb.AddForce(-pm.my[(int)PlayerM.Parts.LHand].forward * ropeSpd, ForceMode.Impulse);
            }

            if (gameObject.name.Contains("Rope"))
            {
                rb.isKinematic = true;
                LineRenderer lr = p1.GetComponent<LineRenderer>();
                if (Physics.Raycast(origin: pm.my[(int)PlayerM.Parts.RHand].position, direction: pm.my[(int)PlayerM.Parts.RHand].forward, out RaycastHit hit, 5f))
                {
                    lr.enabled = true;
                    lr.SetPosition(0, pm.my[(int)PlayerM.Parts.RHand].position);
                    lr.SetPosition(1, hit.point);
                    Vector3 dir = hit.point - p1.transform.position;
                    dir.Normalize();
                    p1.transform.position += dir * ropeSpd*100 * Time.deltaTime;
                }
                print("Active rope");
            }

            if (gameObject.name.Contains("Shield"))
            {
                shield.SetActive(true);
                rb.isKinematic = true;
                print("Active shield");
            }

            if (name.Contains("Oxy"))
            {
                ps.PlusHp(30);
                print("add HP");
                active = false;
            }

            StartCoroutine(StopActive());
        }
    }
    IEnumerator StopActive() {
        yield return new WaitForSeconds(3);
        active = false;
        if (shield.activeSelf) { shield.SetActive(false); }
        print("StopActive working..");
        //yield break;
    }
}
