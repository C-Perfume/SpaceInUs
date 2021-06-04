using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class textSpread : MonoBehaviour
{
    //�޴�â Ű�� ���°� �˾ƿ��°�
    public GameObject MenuController;

    //����
    public AudioSource audiosource;


    public GameObject howto;

    public GameObject image;

    [TextArea] //�ٹٲ�
    public string str = "";

    //����� string�� �Է�
    public int strNum = 0;
    //���� ������� string ����
    public float textSpeed = 0.1f;
    bool text = false;
    //�ؽ�Ʈ �ӵ�
    void Awake()
    {
        audiosource.Play();
        audiosource.loop = true;

        StartCoroutine("TextFuntion");
        //�ڷ�ƾ ����
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


        //���� string �������� ����Ѵ�
        this.GetComponent<Text>().text = str.Substring(0, strNum);

        if (strNum > str.Length)
        {
            StopAllCoroutines();
            //����� ������ �ڷ�ƾ�� �����
        }
        StartCoroutine(TextEnd());
    }

    IEnumerator TextFuntion()
    {
        while (strNum < str.Length)
        {//���� ��� ���� ���� string�� �ִ� ���̺��� ª���� ��� ����Ѵ�
            strNum++;
            yield return new WaitForSeconds(textSpeed);
            //�ؽ�Ʈ �ӵ� ��ŭ ���
        }
    }

    IEnumerator TextEnd()
    {
        if (strNum == str.Length)
        {
            //�Ҹ�����
            audiosource.Stop();

            //�ؽ�Ʈ
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