using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ktest : MonoBehaviour
{
    public GameObject Key;
    public GameObject Mallet;
    public GameObject Mallet2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnClick()
    {
        Key.SetActive(true);
        Mallet.SetActive(true);
        Mallet2.SetActive(true);
    }
    public void Exit()
    {
        Key.SetActive(false);
        Mallet.SetActive(false);
        Mallet2.SetActive(false);
    }
}
