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

    void Start()
    {
        DontDestroyOnLoad(this);
        StartCoroutine(Timesave());
    }

    void Update()
    {

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
