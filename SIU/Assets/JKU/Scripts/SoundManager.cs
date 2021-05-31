using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource warning;
    public AudioSource eve;
    public AudioSource spaceship;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(sounds());
    }

    // Update is called once per frame
    IEnumerator sounds()
    {
        warning.Play();
        eve.Play();
        spaceship.Play();

        yield return new WaitForSeconds(2.3f);
        warning.Stop();
        eve.Stop();
        spaceship.Stop();
    }
}
