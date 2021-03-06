using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    public int maxHp = 100;
    public int currentHp;
    public hpBar HpBar;

    float currtime = 0;
    float createTime = 2;

    //속도값 얻기
    private Vector3 m_LastPosition;

    Rigidbody rb;
    PlayerM pm;

    // 게임옵션
    goPlay gp;
    TrapManager tM;
    void Start()
    {
        gp = GetComponent<goPlay>();
        currentHp = maxHp;
        HpBar.SetMaxHpBar(maxHp);

        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerM>();
        tM = GetComponent<TrapManager>();
    }

    void Update()
    {
        //10이하로 떨어지면 빨간색으로 바꾸기
        if (pm.state == PlayerM.State.Wait) {
            return;
;        }

        currtime += Time.deltaTime;
        if (currtime > createTime)
        {
            if (gp.MenuManager.activeSelf) { TimeDamage(0); }
            else
            {
                TimeDamage(2);
            }
            currtime = 0;
        }

        float mspeed = GetSpeed();


        if (rb.isKinematic == true)
            {

            if (!tM.bH || !pm.r || !tM.isUD) { 
            
                if (mspeed > 5)
                {
                    TimeDamage(5);
                }
               
            }
            }
        

        if (currentHp <= 0)
        {
        //    NetManager.Instance.LeaveRoom();//방나가기 호출
            GameObject SavTime = GameObject.Find("saveTime");
            Destroy(SavTime);
            //print("Chock to death");
           
            SceneManager.LoadScene("LostSpace");
        }

        
    }

    void TimeDamage(int Damage)
    {
        currentHp -= Damage;
        HpBar.SetHpBar(currentHp);
    }

    public void PlusHp(int plusHp)
    {
        SoundM.instance.playS(2, 9);
        currentHp += plusHp;
        if (currentHp > maxHp) { currentHp = maxHp; }
        HpBar.SetHpBar(currentHp);
    }


    //속도값 얻기

    float GetSpeed()
    {
        float speed = (transform.position - m_LastPosition).magnitude / Time.deltaTime;
        m_LastPosition = transform.position;
        return speed;
    }


}
