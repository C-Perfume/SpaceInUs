using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TrapManager : MonoBehaviour
{

   public GameObject meteorFactory;
    
    //현재 테스트용으로 can에 item태그를 붙여놔서 맞으면 피도 참

    public GameObject canFactory;

  
    float speed = 5f;
    float currtime = 0;
    float creatTime = 2;


    void Start()

    {



    }

    void Update()

    {
        currtime += Time.deltaTime;

        if (currtime > creatTime)

        {
            //  Create(meteorFactory);

              Create(canFactory);

            
            currtime = 0;
        }

    }

    public void Create(GameObject clone)
    {

        GameObject obj = Instantiate(clone);

        obj.transform.position = transform.position;
    }

}