using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ranbox : MonoBehaviour
{
    public Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetTrigger("Crash");
        StartCoroutine("Show");
        Destroy(gameObject, 1.5f);
    }

    IEnumerator Show() {
        yield return new WaitForSeconds(.5f);
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
