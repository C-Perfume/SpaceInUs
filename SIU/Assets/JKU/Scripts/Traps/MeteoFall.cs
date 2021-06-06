using System.Collections;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;


public class MeteoFall : MonoBehaviour

{

    GameObject target;


    public float speed = 5f;

    Vector3 dir;

    ItemM iM;
    bool isOpposit = false;
    void Start()

    {
        target = GameObject.Find("EveB (1)");
        iM = GameObject.Find("Player").GetComponent<ItemM>();

        dir = target.transform.position - transform.position;
        dir.Normalize();
        
        Destroy(gameObject, 10);
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