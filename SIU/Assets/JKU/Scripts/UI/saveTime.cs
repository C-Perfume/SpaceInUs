using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class saveTime : MonoBehaviour
{
    //���� �ð� �����ֱ�
    public Text text_timer;

    //�ð������ϱ�
    public float timesave;
     int minutes;
     int secends;

    PlayerM pm;
    void Start()
    {
        pm = transform.root.GetComponent<PlayerM>();
        DontDestroyOnLoad(this);
        StartCoroutine(Timesave());
       
    }

    void Update()
    {
        minutes = (int)timesave % 3600 / 60;
        secends = (int)timesave % 3600 % 60;

        if (SceneManager.GetActiveScene().name == "Meteo" || 
            SceneManager.GetActiveScene().name == "LostSpace")
        {
            Destroy(gameObject);
        }
        text_timer.text = "Time : " + minutes + " �� " + secends + " ��";
    }

   public IEnumerator Timesave()
    {
        while (true)
        {
            if (pm.state == PlayerM.State.GameStart)
            {

                timesave += Time.deltaTime;

            }
                yield return timesave;
                       
        }
    }
}
