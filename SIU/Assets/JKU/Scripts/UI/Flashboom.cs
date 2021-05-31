using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Flashboom : MonoBehaviour
{
    public GameObject flashbang;
    public GameObject imagetext;
    public float sec = 3.5f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(flash());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator flash()
    {
        yield return new WaitForSeconds(sec);
        flashbang.gameObject.SetActive(true);
        yield return new WaitForSeconds(4);
        imagetext.gameObject.SetActive(true);
        //SceneManager.LoadScene("Clear");
    }
}
