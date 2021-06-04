using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class textSpread : MonoBehaviour
{
    //메뉴창 키고 끄는거 알아오는거
    public GameObject MenuController;

    //사운드
    public AudioSource audiosource;


    public GameObject howto;

    public GameObject image;

    [TextArea] //줄바꿈
    public string str = "";

    //출력할 string값 입력
    public int strNum = 0;
    //현재 출력중인 string 순서
    public float textSpeed = 0.1f;
    bool text = false;
    //텍스트 속도
    void Awake()
    {
        audiosource.Play();
        audiosource.loop = true;

        StartCoroutine("TextFuntion");
        //코루틴 시작
    }

    void Update()
    {
        if (MenuController.activeSelf)
        {

            textSpeed = 0;
            StopAllCoroutines();

            audiosource.Pause();
            text = true;
        }


        else
        {
            if (text)
            {
                textSpeed = 0.1f;
                StartCoroutine("TextFuntion");
                audiosource.Play();
                text = false;
            }
        }


        //현재 string 순서까지 출력한다
        this.GetComponent<Text>().text = str.Substring(0, strNum);

        if (strNum > str.Length)
        {
            StopAllCoroutines();
            //출력이 끝나면 코루틴을 멈춘다
        }
        StartCoroutine(TextEnd());
    }

    IEnumerator TextFuntion()
    {
        while (strNum < str.Length)
        {//현재 출력 중인 값이 string의 최대 길이보다 짧으면 계속 출력한다
            strNum++;
            yield return new WaitForSeconds(textSpeed);
            //텍스트 속도 만큼 대기
        }
    }

    IEnumerator TextEnd()
    {
        if (strNum == str.Length)
        {
            //소리끄기
            audiosource.Stop();

            //텍스트
            yield return new WaitForSeconds(1);

            howto.gameObject.SetActive(true);

            gameObject.SetActive(false);

            if (howto.gameObject.activeSelf == false)
            {
                image.gameObject.SetActive(false);
            }
        }
    }
}