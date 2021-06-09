using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class hpBar : MonoBehaviour
{
    Color save = new Color();
    public Slider slider;
    public Image Hp10;
     void Start()
    {
         save = Hp10.color;
    }
    public void SetMaxHpBar(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
   public void SetHpBar(int health)
    {
        slider.value = health;
        Color b = Color.red;
         Color a = Hp10.color;
       
        if (health < 30)
        {
            
            Hp10.color = b;
        }
        else
        {
            Hp10.color = save;
        }
    }
}
