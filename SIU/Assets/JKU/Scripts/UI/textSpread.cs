using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class textSpread : MonoBehaviour
{

    public GameObject howto;

    public GameObject image;

    [TextArea] //�ٹٲ�
    public string str = "";

    //����� string�� �Է�
    public int strNum = 0;
    //���� ������� string ����
    public float textSpeed = 1.0f;
    //�ؽ�Ʈ �ӵ�
    void Awake()
    {

        StartCoroutine("TextFuntion");
        //�ڷ�ƾ ����
    }

    void Update()
    {

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