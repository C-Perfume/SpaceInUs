using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class loadTime : MonoBehaviour
{
    //���� �ð� �ҷ����� & �ؽ�Ʈ�� ǥ�����ֱ�
    saveTime playtime;
    float Savetime;
    Text text_timer;

    int minutes;
    int secends;

    //�����Ѱ� ã�� ���ֱ� 
    GameObject TimeManager;
    
    void Start()
    {
        playtime = GameObject.Find("Savetime").GetComponent<saveTime>();
        Savetime = playtime.timesave;
        text_timer = GameObject.Find("timeScore").GetComponent<Text>();

        minutes = (int)Savetime % 3600 / 60;
        secends = (int)Savetime % 3600 % 60;

        if (text_timer != null) text_timer.text = "Time : " + minutes + " �� " + secends + " ��";
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
