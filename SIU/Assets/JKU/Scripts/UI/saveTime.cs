using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class saveTime : MonoBehaviour
{
    //���� �ð� �����ֱ�
    Text text_timer;
    PlayerM pm;
    Transform text;

    //�ð������ϱ�
    public float timesave;
     int minutes;
     int secends;

    //PhotonView pv;
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if(GameObject.Find("Canvas") != null) text = GameObject.Find("Canvas").transform.GetChild(2);
        text_timer = text.GetComponent<Text>();
        pm = text.root.GetComponent<PlayerM>();
        timesave = TimeSavee();

        minutes = (int)timesave % 3600 / 60;
        secends = (int)timesave % 3600 % 60;

        if (SceneManager.GetActiveScene().name == "Meteo" || 
            SceneManager.GetActiveScene().name == "LostSpace")
        {
            Destroy(gameObject);
        }
       if(text_timer != null) text_timer.text = "Time : " + minutes + " �� " + secends + " ��";
    }

    public float TimeSavee() {

        if (pm != null && pm.state == PlayerM.State.GameStart)
        {
            timesave += Time.deltaTime;
        }
        return timesave;
    }

   public IEnumerator Timesave()
    {
        while (true)
        {
            if (pm!=null && pm.state == PlayerM.State.GameStart)
            {

                timesave += Time.deltaTime;

            }
                yield return timesave;
                       
        }
    }

    
    }
