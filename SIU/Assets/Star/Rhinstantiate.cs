using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhinstantiate : MonoBehaviour
{
    public GameObject hook;
    public Transform rH;
    Hookk hk;
    GameObject h;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            h = Instantiate(hook);
            h.SetActive(true);
            h.transform.position = rH.position;
            h.transform.forward = rH.forward;
            hk = h.GetComponent<Hookk>();
        }

        if (h != null && !hk.a) {
            transform.position = Vector3.Lerp(transform.position, h.transform.position, 1
               // *Time.deltaTime
                );
          // h = null;
        }
    }
}
