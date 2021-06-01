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


    // 블랙홀 생성
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

    //당기는 방향 잡기
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
                //우주미아씬 로드하기
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
            //메테오씬 로드
            //  SceneManager.LoadScene("Meteor");
            pm.state = PlayerM.State.GameOver;
        }
    }
}