using System.Collections;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
public class BottleFall : MonoBehaviour

{
    GameObject black;

    GameObject target;

    public float speed = 5f;

    Vector3 dir;

    ItemM iM;

    bool isOpposit = false;
    void Start()

    {
        black = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
        target = GameObject.Find("EveB (1)");
        iM = target.transform.root.GetComponent<ItemM>();
        Destroy(gameObject, 10);
    }


    void Update()

    {
       dir = target.transform.position - transform.position;
       dir.Normalize();


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

        if (gameObject != null) { Destroy(gameObject); }

    }
    
}