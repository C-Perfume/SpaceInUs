using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class loadTime : MonoBehaviour
{
    //저장 시간 불러오기 & 텍스트로 표기해주기
    saveTime playtime;
    float Savetime;
    Text text_timer;

    //저장한거 찾고 없애기 
    GameObject TimeManager;
    
    void Start()
    {
        playtime = GameObject.Find("saveTime").GetComponent<saveTime>();
        Savetime = playtime.timesave;
        text_timer = GameObject.Find("timeScore").GetComponent<Text>();

       
        text_timer.text = "Time : " + Savetime.ToString("N2");
        TimeManager = GameObject.Find("saveTime");
        Destroy(TimeManager);
       
    }
    private void Update()
    {
        if(text_timer.text == null)
        {
            Destroy(TimeManager);
        }
    }
}
