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
    public float pullSpd = 0.1f;

    PlayerM pm;
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
            + transform.up * 2
            + transform.forward * -2;
    }


    // ��Ȧ ����
    public void BHole(Rocks r)
    {
        GameObject a = Instantiate(bHole);
        if (r.trapNum == (int)Rocks.TrapType.BHoleL)
        {
            a.transform.position = -transform.right * Random.Range(0.5f, 2)
                - transform.up * Random.Range(0.5f, 2)
                - transform.forward * Random.Range(0.5f, 2);
        }
        else
        {
            a.transform.position = transform.right * Random.Range(0.5f, 2)
                - transform.up * Random.Range(0.5f, 2)
                - transform.forward * Random.Range(0.5f, 2);
        }

        a.transform.up = transform.position - a.transform.position;
        Destroy(a, 5);
        bH = true;
        StartCoroutine(Pull(a));
    }

    //���� ���� ���
    IEnumerator Pull(GameObject obj)
    {
        dir = obj.transform.position - transform.position;
        dir.Normalize();
        yield return new WaitForSeconds(5);
        bH = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("BH"))
        {
            if (other.gameObject.GetComponent<BholeRot>().value == 1)
            {
                print("Lost Stars load");
                //���ֹ̾ƾ� �ε��ϱ�
                //SceneManager.LoadScene("LostSpace");
                pm.state = PlayerM.State.GameOver;
            }
            else { gameObject.transform.position = Vector3.zero;
                GetComponent<PlayerM>().floating = true;
            }

        }

        if (other.gameObject.name.Contains("Can"))
        {

            BottleFall btf = other.gameObject.GetComponent<BottleFall>();

            StartCoroutine(btf.Black_());

            Destroy(gameObject, 5);
        }

        if (other.gameObject.name.Contains("Meteor"))
        {
            print("meteor load");
            //���׿��� �ε�
            //  SceneManager.LoadScene("Meteor");
            pm.state = PlayerM.State.GameOver;
        }
    }
}