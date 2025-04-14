using UnityEngine;
using System.Collections;

public class ManagerScript : MonoBehaviour
{
    public GameObject spotlight1;  // 첫 번째 스포트라이트
    public GameObject spotlight2;  // 두 번째 스포트라이트

    void Start()
    {
        spotlight1.SetActive(false);
        spotlight2.SetActive(false);

        StartCoroutine(ActivateSpotlights());
    }

    IEnumerator ActivateSpotlights()
    {
        yield return new WaitForSeconds(2f);  // 2초 대기

        spotlight1.SetActive(true);
        spotlight2.SetActive(true);

        Debug.Log("스포트라이트 ON!");
    }
}
