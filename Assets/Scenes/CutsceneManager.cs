using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CutsceneManager : MonoBehaviour
{
    public GameObject[] cutsceneCameras;  // [0] : 빈 방 카메라, [1] : 무빙 카메라
    public GameObject mainCamera;
    public GameObject playerControlScript;

    void Start()
    {
        StartCoroutine(PlayCutscenes());
    }

    IEnumerator PlayCutscenes()
    {
        playerControlScript.SetActive(false);
        mainCamera.SetActive(false);

        //  [0] 빈 방 카메라 (고정된 시점)
        cutsceneCameras[0].SetActive(true);
        Debug.Log("빈방 컷신 시작");

        yield return new WaitForSeconds(5f);  // 5초동안 빈방 보여줌

        cutsceneCameras[0].SetActive(false);
        Debug.Log("빈방 컷신 종료");

        //  [1] 무빙 카메라 (CutSenceCameraMove 동작)
        GameObject movingCam = cutsceneCameras[1];
        CutSenceCameraMove moveScript = movingCam.GetComponent<CutSenceCameraMove>();

        movingCam.SetActive(true);
        Debug.Log("무빙 컷신 시작");

        while (moveScript != null && moveScript.isMoving)
            yield return null;

        movingCam.SetActive(false);
        Debug.Log("무빙 컷신 종료");

        yield return new WaitForSeconds(1f);  // 전환 딜레이 (선택)

        //  [2] 메인 카메라
        mainCamera.SetActive(true);
        playerControlScript.SetActive(true);
        Debug.Log("컷신 종료, 플레이어 조작 시작!");
    }
}
