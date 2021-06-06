using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hookk : MonoBehaviour
{
    public bool a = true;

    void Update()
    {
        if (a) {
            transform.position += transform.forward * 1.5f * Time.deltaTime;
            StartCoroutine(stopA());
        }
    }

    IEnumerator stopA() {
        yield return new WaitForSeconds(3);
        a = false;
    }
}
