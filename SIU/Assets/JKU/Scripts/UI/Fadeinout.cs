using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Fadeinout : MonoBehaviour
{

    //public static Manager instance;

    public GameObject loading;
    public Image loadingBg;
    public GameObject bg;

    public GameObject Solo;
    public GameObject Vs;
    private void Start()
    {
        StartCoroutine(fadeinout());
    }

    //IEnumerator fadeinout()
    //{

    //    Color bgColor = loadingBg.color;

     
        
    //    while (true)
    //    {
    //        bgColor.a += Time.deltaTime * 0.5f;
    //        loadingBg.color = bgColor;

    //        if (bgColor.a >= 1)
    //            while (true)
    //            {
    //                bg.SetActive(false);
    //                bgColor.a += Time.deltaTime * -0.5f;
    //                loadingBg.color = bgColor;
    //                yield return new WaitForSeconds(0.01f);
                    
    //                if(bgColor.a <= 0)
    //                {
    //                Solo.SetActive(true);
    //                Vs.SetActive(true);
    //                }
    //            }

    //        yield return null;
    //    }


    //}

    IEnumerator fadeinout()
    {
        Color bgColor = loadingBg.color;
        float bgAlpha = 0;//변동되는 알파값을 담을 변수. 와일 조건문에 넣기위해 따로 만듬. 안그러면 bgColor의 알파값을 계속 조회해서 오기때문에 아주조금의 차이지만 성능이 떨어짐.
        bgColor.a = bgAlpha;//최초에 일단 알파값을 0으로 강제조정해줌. 혹시 모를 예외처리용(컴포넌트에서 알파값이 실수로 1로 설정해놓았다던가).

        bg.SetActive(false);//알파값 다 들어왔으면 끔
        while (bgAlpha < 1)//변동되는 알파값 변수가 1이 넘지 않을 동안 안의 내용 실행
        {
            bgAlpha += Time.deltaTime * 0.5f;
            bgColor.a = bgAlpha;
            loadingBg.color = bgColor;
            yield return null;
        }

        yield return new WaitForSeconds(1f);//1초 보여줌

        while (bgAlpha > 0)//변동되는 알파값 변수가 0보다 클 동안 안의 내용 실행
        {
            bgAlpha -= Time.deltaTime * 0.5f;
            bgColor.a = bgAlpha;
            loadingBg.color = bgColor;
            yield return null;
        }
            Solo.SetActive(true);
            Vs.SetActive(true);
    }
}

