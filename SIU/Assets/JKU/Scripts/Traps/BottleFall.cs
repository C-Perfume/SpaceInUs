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
    // Start is called before the first frame update

    void Start()

    {
        black = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
       
        target = GameObject.Find("Player");

        dir = target.transform.position - transform.position;

    }


    void Update()

    {

        Vector3 dir1 = transform.position += dir * speed * Time.deltaTime;

        dir.Normalize();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {

            StartCoroutine(Black_());
        }
    }
    public IEnumerator Black_()

    {
        
        {
            black.gameObject.SetActive(true);

            yield return new WaitForSeconds(1);

            black.gameObject.SetActive(false);

            Destroy(gameObject, 5);
        }

    }
    
}