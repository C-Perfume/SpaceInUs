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
    ItemM iM;

    //소리 
    
    public AudioSource Breath;

    // 게임옵션
    goPlay gp;
    void Start()
    {
        gp = GetComponent<goPlay>();
        currentHp = maxHp;
        HpBar.SetMaxHpBar(maxHp);

        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerM>();
        iM = GetComponent<ItemM>();
    }

    void Update()
    {
        //10이하로 떨어지면 빨간색으로 바꾸기

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

        //rigidbody의 iskinematic이 꺼져있을때만 발동해라.

        if (!iM.f && rb.isKinematic == false)
            {

                if (mspeed > 3)
                {
                    TimeDamage(10);
                }

            }

        if (!iM.r && rb.isKinematic == false)
        {

            if (mspeed > 3)
            {
                TimeDamage(10);
            }

        }

        if (currentHp == 0)
        {
            GameObject SavTime = GameObject.Find("saveTime");
            Destroy(SavTime);
            print("Chock to death");
           SceneManager.LoadScene("LostSpace");
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
        Breath.Play();
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
