using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Action ����� ������ system�� �����
using System;

//Ŭ�� �Ǿ��� �� ȣ�� �Ǵ� �Լ� ����� �� �ִ� delegate ����
public delegate void ClickAction(string s);


public class RoominfoBtn : MonoBehaviour
{
    //public delegate void ClickAction(string s); 

    //Ŭ�� �Ǿ��� �� ȣ��Ǵ� �Լ�
    //public ClickAction clickAction;
    public Action<string> clickAction;

    //������ ������ �ؽ�Ʈ
    public Text info;


    //�� ����
    string room;

    //���� ����
    public void Setinfo(string roomName, int currPlayer, int maxPlayer)
    {
        //�� ���� ����
        room = roomName;

        //�� ����(���� �ο� / �ִ� �ο�)
        info.text = roomName + "(" + currPlayer + " / " + maxPlayer + ")";
    }

    public void OnClick()
    {
        //Ŭ���׼��� ���� �ִٸ�
        if (clickAction != null)
        {
            //�����ض�
            clickAction(room);
        }



        //    //LobbyManager/GameObject ã��
        //    GameObject go = GameObject.Find("LobbyManager");

        //    //LobbyManager Componenet ã��
        //    LobbyManager Im = go.GetComponent<LobbyManager>();

        //    //roomNameInput.text�� �� ������ ����
        //    Im.roomNameInput.text = room;

    }


}
