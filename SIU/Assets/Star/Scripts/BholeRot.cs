using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BholeRot : MonoBehaviour
{
    int num;
   public  int value;
    void Start()
    {
        num = Random.Range(0, 10);
        if (num > 7) { value = 1; } else { value = 0; }
    }

    void Update()
    {
        transform.Rotate(0, 400 * Time.deltaTime, 0); 
    }

}
