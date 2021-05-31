using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploParticle : MonoBehaviour
{
    public GameObject Explo1;
    public GameObject Explo2;
    public GameObject Explo3;
    public GameObject Explo4;

    public GameObject spaceship;
    // Start is called before the first frame update
    void Start()
    {
      
        StartCoroutine(explo());
      
    }

    // Update is called once per frame
    void Update()
    {

    
    }
    IEnumerator explo()
    {
        Rigidbody rb = spaceship.GetComponent<Rigidbody>();
        
        yield return new WaitForSeconds(0.5f);
        Explo1.gameObject.SetActive(false);
        if (Explo1.gameObject.activeSelf == false)
        {
            Explo2.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            Explo2.gameObject.SetActive(false);

            if (Explo2.gameObject.activeSelf == false)
            {
                Explo3.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.5f);
                Explo3.gameObject.SetActive(false);

                if (Explo3.gameObject.activeSelf == false)
                {
                    Explo4.gameObject.SetActive(true);

                    if(Explo4.gameObject.activeSelf == true)
                    {
                        yield return new WaitForSeconds(0.5f);
                        rb.useGravity = true;
                    }
                }
            }
        }
    }
}
