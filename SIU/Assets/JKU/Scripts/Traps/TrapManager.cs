using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TrapManager : MonoBehaviour

{

    public GameObject meteorFactory;
    public GameObject canFactory;

    //��Ȧ ����
    public GameObject bHole;
    // ���� �ѹ��� �۵��ϱ�
    public bool up = true;
    // ��Ȧ �η�������
    public bool bH = false;
    // �η� ����
    public Vector3 dir;
    // ��Ȧ �η¼ӵ�
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


    // ��Ȧ ����
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

    //���� ���� ���
    IEnumerator Pull(GameObject obj)
    {
        dir = obj.transform.position - transform.position;
        dir.Normalize();
        yield return new WaitForSeconds(5);
        bH = false;
    }

    // ������ �ε����� ó���Ǵ� �Լ�
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("BH"))
        {
            if (other.gameObject.GetComponent<BholeRot>().value == 0)
            {
                GameObject SavTime = GameObject.Find("saveTime");
                Destroy(SavTime);
                
                //���ֹ̾ƾ� �ε��ϱ�
                SceneManager.LoadScene("LostSpace");
                pm.state = PlayerM.State.GameOver;
            }
            else {
                print("ȭ��ƮȦ �۵��ϴ�?");
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
            //���׿��� �ε�

              SceneManager.LoadScene("Meteo");
            pm.state = PlayerM.State.GameOver;

            Destroy(other.gameObject, 5);
        }
    }

    //���� ����Ű��
    IEnumerator Vibrate(float sec)
    {

        OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.LTouch);
        OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);

        yield return new WaitForSeconds(sec);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);

    }
}