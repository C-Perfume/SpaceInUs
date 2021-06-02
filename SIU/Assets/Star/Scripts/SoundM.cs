using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundM : MonoBehaviour
{
    public static SoundM instance;
    public AudioSource[] aS;
    public AudioClip[] clips;

    void Start()
    {
        instance = this;
        aS[1] = GetComponent<AudioSource>();
        StartCoroutine(warning());
    }

    void Update()
    {
       
    }

    public void playS(int num) {
        aS[1].clip = clips[num];
        aS[1].Play();
    }
    public void StopS(int num) {
        aS[1].clip = clips[num];
        aS[1].Stop();
    }

    IEnumerator warning() {
        yield return new WaitForSeconds(30);
        aS[0].loop = true;
        aS[0].clip = clips[1];
        aS[0].Play();
    }
}
