using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CutSenceCamera : MonoBehaviour
{
    public GameObject cutsceneCamera;
    public GameObject mainCamera;
    public GameObject playerControlScript;
    public CutSenceCameraMove moveScript;  // 무빙 스크립트 참조

    void Start()
    {
        cutsceneCamera.SetActive(true);
        mainCamera.SetActive(false);
        playerControlScript.SetActive(false);

        StartCoroutine(CheckCutsceneEnd());
    }

    IEnumerator CheckCutsceneEnd()
    {
        // CutSenceCameraMove가 끝날 때까지 기다림
        while (moveScript != null && moveScript.isMoving)
        {
            yield return null;
        }

        // 경로 무빙이 끝나면 컷신 종료
        cutsceneCamera.SetActive(false);
        mainCamera.SetActive(true);
        playerControlScript.SetActive(true);

        Debug.Log("컷신 종료. 플레이어 조작 시작!");
    }
}
