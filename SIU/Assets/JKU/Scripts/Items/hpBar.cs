using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class hpBar : MonoBehaviour
{
    Color save = new Color();
    public Slider slider;
     void Start()
    {
         //save = slider.GetComponent<Color>();
    }
    public void SetMaxHpBar(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
   public void SetHpBar(int health)
    {
        slider.value = health;
           // Color b = Color.red;
       
      //  Color a = slider.GetComponent<Color>();
        //if (health < 10)
        //{
        //    a = b;
        //}
        //else { 
        
        //}
    }
}
