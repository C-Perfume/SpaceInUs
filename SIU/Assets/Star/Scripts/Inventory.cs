using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    PlayerM pm;
   public GameObject[] item1;
   public GameObject[] item2;

    enum items
    {
        R,
        F,
        S,
        O
    }
    void Start()
    {
        pm = GameObject.Find("Player").GetComponent<PlayerM>();
    }

    void Update()
    {

        if(pm.myItem.Count < 1)
        {
            item1[(int)items.O].SetActive(false);
            item1[(int)items.F].SetActive(false);
            item1[(int)items.S].SetActive(false);
            item1[(int)items.R].SetActive(false);

        }

        if (pm.myItem.Count == 1)
        {
            item2[(int)items.O].SetActive(false);
            item2[(int)items.F].SetActive(false);
            item2[(int)items.S].SetActive(false);
            item2[(int)items.R].SetActive(false);

                if (pm.myItem[0].name.Contains("Rope"))
                {
                    item1[(int)items.R].SetActive(true);
                }
                if (pm.myItem[0].name.Contains("Fire"))
                {
                    item1[(int)items.F].SetActive(true);
                }
                if (pm.myItem[0].name.Contains("Shield"))
                {
                    item1[(int)items.S].SetActive(true);
                }
                if (pm.myItem[0].name.Contains("Oxy"))
                {
                    item1[(int)items.O].SetActive(true);
                }

            }

            if (pm.myItem.Count == 2)
            {

                if (pm.myItem[1].name.Contains("Rope"))
                {
                    item2[(int)items.R].SetActive(true);
                }
                if (pm.myItem[1].name.Contains("Fire"))
                {
                    item2[(int)items.F].SetActive(true);
                }
                if (pm.myItem[1].name.Contains("Shield"))
                {
                    item2[(int)items.S].SetActive(true);
                }
                if (pm.myItem[1].name.Contains("Oxy"))
                {
                    item2[(int)items.O].SetActive(true);
                }

        }




    }


}
