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

    //속도값 얻기
    private Vector3 m_LastPosition;
    Vector3 dir;

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

    void SpeedDamage(int sDamage)
    {
        GetSpeed();
        
        currentHp -= sDamage;
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


    //속도값 얻기

    float GetSpeed()
    {
        float speed = (((transform.position - m_LastPosition).magnitude) / Time.deltaTime);
        m_LastPosition = transform.position;


        if(speed > 0)
        {
            SpeedDamage(10);
        }
        return speed;
    }

    
}
