using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class hpBar : MonoBehaviour
{
    public Slider slider;
    
    public void SetMaxHpBar(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
   public void SetHpBar(int health)
    {
        slider.value = health;
    }
}
