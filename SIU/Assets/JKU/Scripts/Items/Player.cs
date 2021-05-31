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

    //죽었을 때 페이드아웃
    public GameObject Canvas_;
    public Image BlackBg;

    Rigidbody rb;

    //소리 
   public AudioSource Grap;
   public AudioSource Breath;

    void Start()
    {
        currentHp = maxHp;
        HpBar.SetMaxHpBar(maxHp);

        rb = GetComponent<Rigidbody>();

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

        float mspeed = GetSpeed();

        //rigidbody의 iskinematic이 꺼져있을때만 발동해라.

        if (rb.isKinematic == false)
        {

            if (mspeed > 10)
            {
                TimeDamage(10);
            }

        }
        if (currentHp == 0)
        {
            SceneManager.LoadScene("LostSpace");
        }

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
 
    private void OnTriggerEnter(Collider other)
    {
        //산소통 먹으면 나는 소리
        if (other.gameObject.tag == "Item")
        {
            PlusHp(10);
            Breath.Play();
            Destroy(other.gameObject);
        }
        
       
        //잡으면 나는 소리
        if (other.gameObject.tag == "Step")
        {
            Grap.Play();
        }

    }


    //속도값 얻기

    float GetSpeed()
    {
        float speed = (((transform.position - m_LastPosition).magnitude) / Time.deltaTime);
        m_LastPosition = transform.position;
        return speed;
    }


}
