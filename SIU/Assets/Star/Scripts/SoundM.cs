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
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Ready") { StartCoroutine(warning()); }
    }


    public void playS(int audio, int clip) {
        aS[audio].clip = clips[clip];
        aS[audio].Play();
    }
    public void StopS(int audio, int clip) {
        aS[audio].clip = clips[clip];
        aS[audio].Stop();
    }

    IEnumerator warning() {
        yield return new WaitForSeconds(30);
        aS[0].loop = true;
        aS[0].clip = clips[1];
        aS[0].Play();
    }
}
