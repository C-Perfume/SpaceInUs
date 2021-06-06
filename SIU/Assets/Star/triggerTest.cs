using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerTest : MonoBehaviour
{
    public GameObject target;

    Vector3 dir;

    bool isOpposit = false;
    void Start()
    {
        dir = target.transform.position - transform.position;
        dir.Normalize();

    }

    public void OnTriggerEnter(Collider other)
    {
            print("OnTriggerEnter");
        if (other.name.Contains("Shield"))
        {
            print("OnTriggerEnter work??");
            isOpposit = true;
        }
    }

    void Update()
    {
        if (isOpposit)
        {
            transform.position -= dir * 5 * Time.deltaTime;
        }
        else
        {
            transform.position += dir * 5 * Time.deltaTime;
        }
    }
}
