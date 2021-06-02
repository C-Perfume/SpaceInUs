using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class saveTime : MonoBehaviour
{
    //현재 시간 보여주기
    public Text text_timer;

    //시간저장하기
    public float timesave;

    void Start()
    {
        DontDestroyOnLoad(this);
        StartCoroutine(Timesave());
    }

    void Update()
    {


        if(SceneManager.GetActiveScene().name == "Meteo" || 
            SceneManager.GetActiveScene().name == "LostSpace")
        {
            Destroy(gameObject);
        }
        text_timer.text = "Time : " + timesave.ToString("N2");
    }

   public IEnumerator Timesave()
    {
        while (true)
        {
            timesave += Time.deltaTime;

            yield return timesave;
            
        }
    }
}
