using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Action 기능을 쓰려면 system을 써야함
using System;

//클릭 되었을 때 호출 되는 함수 등록할 수 있는 delegate 생성
public delegate void ClickAction(string s);


public class RoominfoBtn : MonoBehaviour
{
    //public delegate void ClickAction(string s); 

    //클릭 되었을 때 호출되는 함수
    //public ClickAction clickAction;
    public Action<string> clickAction;

    //정보를 보여줄 텍스트
    public Text info;


    //방 제목
    string room;

    //정보 세팅
    public void Setinfo(string roomName, int currPlayer, int maxPlayer)
    {
        //방 제목 저장
        room = roomName;

        //방 제목(현재 인원 / 최대 인원)
        info.text = roomName + "(" + currPlayer + " / " + maxPlayer + ")";
    }

    public void OnClick()
    {
        //클릭액션의 값이 있다면
        if (clickAction != null)
        {
            //실행해라
            clickAction(room);
        }



        //    //LobbyManager/GameObject 찾자
        //    GameObject go = GameObject.Find("LobbyManager");

        //    //LobbyManager Componenet 찾자
        //    LobbyManager Im = go.GetComponent<LobbyManager>();

        //    //roomNameInput.text에 방 제목을 넣자
        //    Im.roomNameInput.text = room;

    }


}
