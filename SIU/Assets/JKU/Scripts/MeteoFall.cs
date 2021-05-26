using System.Collections;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;


public class MeteoFall : MonoBehaviour

{

    GameObject target;


    public float speed = 5f;

    Vector3 dir;



    // Start is called before the first frame update

    void Start()

    {

        target = GameObject.Find("player");

        dir = target.transform.position - transform.position;

        dir.Normalize();

    }



    // Update is called once per frame

    void Update()

    {
        Vector3 dir1 = transform.position += dir * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}