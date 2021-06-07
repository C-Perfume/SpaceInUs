using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TrapManager : MonoBehaviour

{

    public GameObject meteorFactory;
    public GameObject canFactory;

    //블랙홀 공장
    public GameObject bHole;
    // 함정 한번만 작동하기
    public bool up = true;
    // 블랙홀 인력유지용
    public bool bH = false;
    // 인력 방향
    public Vector3 dir;
    // 블랙홀 인력속도
    public  float pullSpd = 1f;

    PlayerM pm;

    public AudioSource[] trapS;

    void Start()

    {
        pm = GetComponent<PlayerM>();


    }

    void Update()

    {

    }

    public void Create(GameObject clone)
    {


        GameObject obj = Instantiate(clone);
        
        obj.transform.position = transform.position
            + transform.up * 10
            + transform.forward * -10;
   
        if (clone.name.Contains("Meteo"))
        {
            trapS[1].Play();
        }
        if (clone.name.Contains("Can"))
        {
            trapS[2].Play();
            
        }
        StartCoroutine(Vibrate(.5f));
        StopCoroutine(Vibrate(5));

    }


    // 블랙홀 생성
    public void BHole(Value v)
    {
        trapS[0].Play();
        GameObject a = Instantiate(bHole);
        BholeRot bHR = a.GetComponent<BholeRot>();
        if (v.tT == Value.TrapType.BHoleL)
        {
            a.transform.position = -transform.right * Random.Range(3, 5f)
            - transform.up * Random.Range(3, 5f)
                - transform.forward * Random.Range(3, 5f);
            bHR.value = 0;
        }
        else
        {
            a.transform.position = transform.right * Random.Range(3, 5f)
               - transform.up * Random.Range(3, 5f)
                - transform.forward * Random.Range(3, 5f);
            bHR.value = 1;
        }
        a.transform.up = transform.position - a.transform.position;
        Destroy(a, 5);
        bH = true;
        StartCoroutine(Pull(a));

        StartCoroutine(Vibrate(.5f));
        StopCoroutine(Vibrate(1));
    }

    //당기는 방향 잡기
    IEnumerator Pull(GameObject obj)
    {
        dir = obj.transform.position - transform.position;
        dir.Normalize();
        yield return new WaitForSeconds(5);
        bH = false;
    }

    // 함정에 부딪히면 처리되는 함수
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("BH"))
        {
            if (other.gameObject.GetComponent<BholeRot>().value == 0)
            {
                GameObject SavTime = GameObject.Find("saveTime");
                Destroy(SavTime);
                
                //우주미아씬 로드하기
                SceneManager.LoadScene("LostSpace");
                pm.state = PlayerM.State.GameOver;
            }
            else {
                print("화이트홀 작동하니?");
                bH = false;
                gameObject.transform.position = new Vector3(0, 0, 0);
                GetComponent<PlayerM>().floating = true;
            }
            Destroy(other.gameObject, 5);
        }

        if (other.gameObject.name.Contains("Can"))
        {

            BottleFall btf = other.gameObject.GetComponent<BottleFall>();
            trapS[2].Stop();
            StartCoroutine(btf.Black_());

        }


        if (other.gameObject.name.Contains("Meteo"))
        {
            GameObject SavTime = GameObject.Find("saveTime");
            Destroy(SavTime);
            //메테오씬 로드

              SceneManager.LoadScene("Meteo");
            pm.state = PlayerM.State.GameOver;

            Destroy(other.gameObject, 5);
        }
    }

    //진동 일으키기
    IEnumerator Vibrate(float sec)
    {

        OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.LTouch);
        OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);

        yield return new WaitForSeconds(sec);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);

    }
}