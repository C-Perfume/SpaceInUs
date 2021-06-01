using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class goPlay : MonoBehaviour
{
    public GameObject MenuManager;

    bool a = false;
    void Start()
    {
        
    }

    private void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.Start, OVRInput.Controller.LTouch))
        {
            MenuManager.gameObject.SetActive(true);
            //a = true;
            //if (a == true && OVRInput.GetDown(OVRInput.Button.Start, OVRInput.Controller.LTouch))
            //{
            //    MenuManager.gameObject.SetActive(false);
            //    a = false;
            //}
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            SceneManager.LoadScene("Lv1");
        }
    }


    public void onClickRetry()
    {
        SceneManager.LoadScene("SpaceShip");
    }
    public void onClickExit()
    {
        Application.Quit();
    }

    public void Exitmenu()
    {
       MenuManager.gameObject.SetActive(false);
    }
}
