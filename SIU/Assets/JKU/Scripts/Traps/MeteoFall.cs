using System.Collections;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;


public class MeteoFall : MonoBehaviour

{

    public GameObject target;


    public float speed = 5f;

    Vector3 dir;

    bool isOpposit = false;
    void Start()

    {
        if (target != null)
        {
            dir = target.transform.position - transform.position;
            dir.Normalize();
        }
        
        Destroy(gameObject, 5);
    }




    void Update()

    {

        if (isOpposit)
        {
            transform.position -= dir * speed * Time.deltaTime;
        }
        else
        {
            transform.position += dir * speed * Time.deltaTime;
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Shield"))
        {
            isOpposit = true;
        }
    }
}