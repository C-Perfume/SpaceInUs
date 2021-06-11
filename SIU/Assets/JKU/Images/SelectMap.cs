using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectMap : MonoBehaviour
{
  
    // Start is called before the first frame update
    void Start()
    {

    }


    public void OnClickSolo()
    {
        SceneManager.LoadScene("Connect");
    }
    public void OnClickFight()
    {
        SceneManager.LoadScene("LobbyScene");
    }
}
