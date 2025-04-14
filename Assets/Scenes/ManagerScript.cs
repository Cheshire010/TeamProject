using UnityEngine;
using System.Collections;

public class ManagerScript : MonoBehaviour
{
    public GameObject spotlight1;  // ù ��° ����Ʈ����Ʈ
    public GameObject spotlight2;  // �� ��° ����Ʈ����Ʈ

    void Start()
    {
        spotlight1.SetActive(false);
        spotlight2.SetActive(false);

        StartCoroutine(ActivateSpotlights());
    }

    IEnumerator ActivateSpotlights()
    {
        yield return new WaitForSeconds(2f);  // 2�� ���

        spotlight1.SetActive(true);
        spotlight2.SetActive(true);

        Debug.Log("����Ʈ����Ʈ ON!");
    }
}
