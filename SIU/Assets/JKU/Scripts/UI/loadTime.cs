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

    //�����Ѱ� ã�� ���ֱ� 
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
