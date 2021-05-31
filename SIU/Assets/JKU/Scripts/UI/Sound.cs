using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public GameObject Text;
    public AudioSource audiosource;

    // Start is called before the first frame update
    void Start()
    {
        audiosource.Play();

    }

    // Update is called once per frame
    void Update()
    {

        if(Text.activeSelf == true)
        {
            audiosource.loop = true;
        }
        else
        {
            audiosource.Stop();
        }
    }
}
