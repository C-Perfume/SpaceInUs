using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BholeRot : MonoBehaviour
{
   public  int value;
    void Start()
    {
    }

    void Update()
    {
        transform.Rotate(0, 400 * Time.deltaTime, 0); 
    }

}
