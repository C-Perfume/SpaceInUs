using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //���� ��ġ
    public Transform[] spawnPos;

    //���� ��ġ�� ���� �ö󰬴��� ����
    public bool[] isEmpty;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        isEmpty = new bool[spawnPos.Length];
    }

    // Update is called once per frame
    void Update()
    {

    }
    public Transform GetEmptyTr()
    {
        for (int i = 0; i < isEmpty.Length; i++)
        {
            if (isEmpty[i] == false)
            {
                isEmpty[i] = true;
                return spawnPos[i];
            }
        }
        return null;
    }
}
