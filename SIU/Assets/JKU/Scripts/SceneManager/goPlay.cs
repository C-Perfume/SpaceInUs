using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class goPlay : MonoBehaviour
{
    public GameObject MenuManager;

    
    void Start()
    {
        
    }

    private void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.Start, OVRInput.Controller.LTouch))
        {
            MenuManager.gameObject.SetActive(true);
           
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
