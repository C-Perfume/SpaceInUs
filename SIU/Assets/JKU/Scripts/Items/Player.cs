using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHp = 100;
    public int currentHp;
    public hpBar HpBar;

    float currtime = 0;
    float createTime = 2;

 
    void Start()
    {
        currentHp = maxHp;
        HpBar.SetMaxHpBar(maxHp);
    }

    // Update is called once per frame
    void Update()
    {
        currtime += Time.deltaTime;

        if (currtime > createTime)
        {

        TimeDamage(2);
        currtime = 0;
     
        }
    }
    void TimeDamage(int Damage)
    {
        currentHp -= Damage;
        HpBar.SetHpBar(currentHp);
    }

    void PlusHp(int plusHp)
    {
        currentHp += plusHp;
        HpBar.SetHpBar(currentHp);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Item")
        {
            PlusHp(10);
            Destroy(other.gameObject);
        }
    }
}
