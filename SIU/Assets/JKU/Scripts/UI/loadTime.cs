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

    int minutes;
    int secends;

    //저장한거 찾고 없애기 
    GameObject TimeManager;
    
    void Start()
    {
        playtime = GameObject.Find("Savetime").GetComponent<saveTime>();
        Savetime = playtime.timesave;
        text_timer = GameObject.Find("timeScore").GetComponent<Text>();

        minutes = (int)Savetime % 3600 / 60;
        secends = (int)Savetime % 3600 % 60;

        if (text_timer != null) text_timer.text = "Time : " + minutes + " 분 " + secends + " 초";
        TimeManager = GameObject.Find("Savetime");
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
