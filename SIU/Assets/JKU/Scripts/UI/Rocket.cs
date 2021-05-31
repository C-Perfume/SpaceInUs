using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rb;
   public float boost;
    // Start is called before the first frame update
    void Start()
    {
        rb =GameObject.Find("Player").GetComponent<Rigidbody>();
      
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(aboost());
        //if(소화기 오브젝트 들고 있을 때(지정되는 배열일때))
        //{
        //if(버튼 누르면)
        //{
        // rb.AddForce(transform.forward * boost);  --> 이거 실행해라. 
        //}
        //}

    }
    IEnumerator aboost()
    {
        rb.AddForce(transform.forward * boost);
        yield return new WaitForSeconds(1);
        rb.AddForce(-transform.forward * boost);
        Destroy(gameObject, 2);
    }
}
