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

    //�ӵ��� ���
    private Vector3 m_LastPosition;

    Rigidbody rb;
    PlayerM pm;
    ItemM iM;

    //�Ҹ� 
    

    // ���ӿɼ�
    goPlay gp;
    TrapManager tM;
    void Start()
    {
        gp = GetComponent<goPlay>();
        currentHp = maxHp;
        HpBar.SetMaxHpBar(maxHp);

        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerM>();
        iM = GetComponent<ItemM>();
        tM = GetComponent<TrapManager>();
    }

    void Update()
    {
        //10���Ϸ� �������� ���������� �ٲٱ�
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

            if (!tM.bH || !iM.r) { 
            
                if (mspeed > 5)
                {
                    TimeDamage(5);
                }
               
            }
            }
        

        if (currentHp <= 0)
        {
        //    NetManager.Instance.LeaveRoom();//�泪���� ȣ��
            GameObject SavTime = GameObject.Find("saveTime");
            Destroy(SavTime);
            print("Chock to death");
           
            //�����߿� ��������..
            //SceneManager.LoadScene("LostSpace");
        }

        #region canvas GameOver

        //if (canvas_.activeself == true)
        //{
        //    color bgcolor = loadingbg.color;
        //    bgcolor.a += time.deltatime * 0.5f;
        //    loadingbg.color = bgcolor;
        //    if (bgcolor.a <= 0)
        //    {
        //        loading.setactive(false);
        //    }
        //}

        #endregion
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


    //�ӵ��� ���

    float GetSpeed()
    {
        float speed = (transform.position - m_LastPosition).magnitude / Time.deltaTime;
        m_LastPosition = transform.position;
        return speed;
    }


}
