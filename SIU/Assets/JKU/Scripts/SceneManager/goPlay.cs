using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class goPlay : MonoBehaviour
{
    public static goPlay instance;
    public GameObject MenuManager;
    public GameObject hpBar;
    public bool a = false;

    void Start()
    {
        instance = this;
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start, OVRInput.Controller.LTouch))
       
        {
            a = !a;
            MenuManager.gameObject.SetActive(a);
            hpBar.gameObject.SetActive(!a);
            if (a)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }


    public void onClickRetry()
    {
        SceneManager.LoadScene("Ready");
    }
    public void onClickExit()
    {
        Application.Quit();
    }
}
