using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firetest : MonoBehaviour
{
    public float speed;
    PlayerM pm;
    Rigidbody rb;
    GameObject p1;
    GameObject rot;
    // Start is called before the first frame update
    void Start()
    {
        rot = GameObject.Find("RightHandAnchor");
        p1 = GameObject.Find("Player");
        pm = p1.GetComponent<PlayerM>();
        rb = p1.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        //rb.AddForce(-pm.my[(int)PlayerM.Parts.LHand].up*speed, ForceMode.Impulse);
        rb.AddForce(-pm.my[(int)PlayerM.Parts.LHand].forward * speed, ForceMode.Impulse);
      //  transform.rotation = rot.transform.rotation;
            
    }
}
