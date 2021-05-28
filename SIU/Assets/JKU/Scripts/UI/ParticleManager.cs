using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public GameObject StepItem;
    public GameObject OxyFactory;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (StepItem.tag == "Player")
        {
            GameObject oxy = Instantiate(OxyFactory);
            oxy.transform.position = StepItem.transform.position;

            print("½ÇÇà");
        }
    }
}

