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
        OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.LTouch);
        OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);
        yield return new WaitForSeconds(3.5f);
        imagetext.gameObject.SetActive(true);
    }
}
