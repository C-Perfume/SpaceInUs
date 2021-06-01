using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 7);
    }

    void Update()
    {
        transform.position -= transform.up *0.6f* Time.deltaTime;
    }
}
