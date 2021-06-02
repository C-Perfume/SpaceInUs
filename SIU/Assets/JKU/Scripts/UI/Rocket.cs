using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rb;
    GameObject rot;
   public float boost;
    // Start is called before the first frame update
    void Start()
    {
        rb =GameObject.Find("Player").GetComponent<Rigidbody>();
        rot = GameObject.Find("RightHandAnchor");
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(aboost());
   
    }
    IEnumerator aboost()
    {
        rb.AddForce(transform.up * boost);
        rb.AddForce(-transform.forward * boost);
       transform.rotation = rot.transform.rotation; 
        yield return null;
       
    }
}
