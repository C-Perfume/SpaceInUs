using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eveLostSpace : MonoBehaviour
{
    public float rotSpeed;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = transform.position += Vector3.forward * speed * Time.deltaTime;

        transform.Rotate(0, 0, rotSpeed * Time.deltaTime);
    }
}
