using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject aa;
    void Start()
    {
        
    }

    void Update()
    {
        if (aa.activeSelf) {
            gameObject.SetActive(false);
        }
    }
}
