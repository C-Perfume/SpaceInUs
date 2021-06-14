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
        float bgAlpha = 0;//�����Ǵ� ���İ��� ���� ����. ���� ���ǹ��� �ֱ����� ���� ����. �ȱ׷��� bgColor�� ���İ��� ��� ��ȸ�ؼ� ���⶧���� ���������� �������� ������ ������.
        bgColor.a = bgAlpha;//���ʿ� �ϴ� ���İ��� 0���� ������������. Ȥ�� �� ����ó����(������Ʈ���� ���İ��� �Ǽ��� 1�� �����س��Ҵٴ���).

        bg.SetActive(false);//���İ� �� �������� ��
        while (bgAlpha < 1)//�����Ǵ� ���İ� ������ 1�� ���� ���� ���� ���� ���� ����
        {
            bgAlpha += Time.deltaTime * 0.5f;
            bgColor.a = bgAlpha;
            loadingBg.color = bgColor;
            yield return null;
        }

        yield return new WaitForSeconds(1f);//1�� ������

        while (bgAlpha > 0)//�����Ǵ� ���İ� ������ 0���� Ŭ ���� ���� ���� ����
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

