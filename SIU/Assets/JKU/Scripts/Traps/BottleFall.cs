using System.Collections;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
public class BottleFall : MonoBehaviour

{
    GameObject black;

   public GameObject target;

    public float speed = 5f;

    Vector3 dir;

    bool isOpposit = false;
    void Start()

    {
        black = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
        Destroy(gameObject, 5);
    }


    void Update()

    {
        if (target != null)
        {
            dir = target.transform.position - transform.position;
            dir.Normalize();
        }

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
        if (other.gameObject.name.Contains("Shield")) {
            print("isOpposit working?");
            isOpposit = true;
        }
    }

    public IEnumerator Black_()

    {
        
            black.gameObject.SetActive(true);

            yield return new WaitForSeconds(1);

            black.gameObject.SetActive(false);


    }
    
}