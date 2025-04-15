using UnityEngine;
using System.Collections;

public class ManagerScript : MonoBehaviour
{
    public GameObject spotlight1;  // ù ��° ����Ʈ����Ʈ
    public GameObject spotlight2;  // �� ��° ����Ʈ����Ʈ
    public GameObject CarObject;

    void Start()
    {
        spotlight1.SetActive(false);
        spotlight2.SetActive(false);
        CarObject.SetActive(false);

        StartCoroutine(ActivateSpotlights());
    }

    IEnumerator ActivateSpotlights()
    {
        yield return new WaitForSeconds(2f);  // 2�� ���

        spotlight1.SetActive(true);
        spotlight2.SetActive(true);
        CarObject.SetActive(true);

        Debug.Log("����Ʈ����Ʈ ON!");
    }
}
