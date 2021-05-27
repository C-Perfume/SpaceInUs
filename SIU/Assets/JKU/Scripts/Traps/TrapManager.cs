using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class TrapManager : MonoBehaviour

{

    public GameObject ObjFactory;



    public float speed = 5f;



    float currtime = 0;

    public float creatTime = 2;



    void Start()

    {



    }



    // Update is called once per frame

    void Update()

    {

        currtime += Time.deltaTime;

        if (currtime > creatTime)

        {



            GameObject obj = Instantiate(ObjFactory);

            obj.transform.position = transform.position;



            currtime = 0;

        }


        
    }

}