using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class end : MonoBehaviour
{ 
    public GameObject key;
    public GameObject mallet;
    public GameObject mallet2;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        key.SetActive(false);
        mallet.SetActive(false);
        mallet2.SetActive(false);
    }
}
