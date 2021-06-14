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
        O,
        RB,
        RB1
    }
    void Start()
    {
        pm = transform.root.GetComponent<PlayerM>();
    }

    void Update()
    {

        if (pm.myTem.Count == 0)
        {
            item1[(int)items.R].SetActive(false);
            item1[(int)items.F].SetActive(false);
            item1[(int)items.S].SetActive(false);
            item1[(int)items.O].SetActive(false);
            item1[(int)items.RB].SetActive(false);
            item1[(int)items.RB1].SetActive(false);

        }
        if (pm.myTem.Count == 1)
        {

            if (pm.myTem[0] == 0) item1[(int)items.R].SetActive(true);
            else item1[(int)items.R].SetActive(false);
            
            if (pm.myTem[0] == 1) item1[(int)items.F].SetActive(true);
            else item1[(int)items.F].SetActive(false);
            
            if (pm.myTem[0] == 2) item1[(int)items.S].SetActive(true);
            else item1[(int)items.S].SetActive(false);
            
            if (pm.myTem[0] == 3) item1[(int)items.O].SetActive(true);
            else item1[(int)items.O].SetActive(false);
            
            if (pm.myTem[0] == 4) item1[(int)items.RB].SetActive(true);
            else item1[(int)items.RB].SetActive(false);
            
            if (pm.myTem[0] == 5) item1[(int)items.RB1].SetActive(true);
            else item1[(int)items.RB1].SetActive(false);
            
            item2[(int)items.O].SetActive(false);
            item2[(int)items.F].SetActive(false);
            item2[(int)items.S].SetActive(false);
            item2[(int)items.R].SetActive(false);
            item2[(int)items.RB].SetActive(false);
            item2[(int)items.RB1].SetActive(false);
        }

        if (pm.myTem.Count == 2)
        {
            if (pm.myTem[0] == 0) item1[(int)items.R].SetActive(true);
            else item1[(int)items.R].SetActive(false);
            if (pm.myTem[0] == 1) item1[(int)items.F].SetActive(true);
            else item1[(int)items.F].SetActive(false);
            if (pm.myTem[0] == 2) item1[(int)items.S].SetActive(true);
            else item1[(int)items.S].SetActive(false);
            if (pm.myTem[0] == 3) item1[(int)items.O].SetActive(true);
            else item1[(int)items.O].SetActive(false);
            if (pm.myTem[0] == 4) item1[(int)items.RB].SetActive(true);
            else item1[(int)items.RB].SetActive(false);
            if (pm.myTem[0] == 5) item1[(int)items.RB1].SetActive(true);
            else item1[(int)items.RB1].SetActive(false);


            if (pm.myTem[1] == 0) item2[(int)items.R].SetActive(true); 
            else { item2[(int)items.R].SetActive(false); }
            if (pm.myTem[1] == 1) item2[(int)items.F].SetActive(true);
            else { item2[(int)items.F].SetActive(false); }
            if (pm.myTem[1] == 2) item2[(int)items.S].SetActive(true);
            else { item2[(int)items.S].SetActive(false); }
            if (pm.myTem[1] == 3) item2[(int)items.O].SetActive(true);
            else { item2[(int)items.O].SetActive(false); }
            if (pm.myTem[1] == 4) item2[(int)items.RB].SetActive(true);
            else { item2[(int)items.RB].SetActive(false); }
            if (pm.myTem[1] == 5) item2[(int)items.RB1].SetActive(true);
            else { item2[(int)items.RB1].SetActive(false); }

        }




    }


}
