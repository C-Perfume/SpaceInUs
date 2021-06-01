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

    //�׾��� �� ���̵�ƿ�
    public GameObject Canvas_;
    public Image BlackBg;

    Rigidbody rb;

    //�Ҹ� 
    
    public AudioSource Breath;

    void Start()
    {
        currentHp = maxHp;
        HpBar.SetMaxHpBar(maxHp);

        rb = GetComponent<Rigidbody>();

    }

    void Update()
    {
        currtime += Time.deltaTime;

        if (currtime > createTime)
        {

            TimeDamage(2);
            currtime = 0;
            print(currentHp);
        }

        float mspeed = GetSpeed();

        //rigidbody�� iskinematic�� ������������ �ߵ��ض�.

        if (rb.isKinematic == false)
        {

            if (mspeed > 3)
            {
                TimeDamage(10);
            }

        }

        if (currentHp == 0)
        {
           // SceneManager.LoadScene("LostSpace");
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


    //�ӵ��� ���

    float GetSpeed()
    {
        float speed = (transform.position - m_LastPosition).magnitude / Time.deltaTime;
        m_LastPosition = transform.position;
        return speed;
    }


}
