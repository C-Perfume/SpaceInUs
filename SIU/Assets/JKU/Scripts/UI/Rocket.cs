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
        //if(��ȭ�� ������Ʈ ��� ���� ��(�����Ǵ� �迭�϶�))
        //{
        //if(��ư ������)
        //{
        // rb.AddForce(transform.forward * boost);  --> �̰� �����ض�. 
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
