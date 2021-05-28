using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayTime : MonoBehaviour
{
    public Text text_timer;

    void Start()
    {
       
    }

    void Update()
    {

        text_timer.text = "Time : " + Time.time.ToString("N2");

    }
}
