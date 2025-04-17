using UnityEngine;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    public GameObject cutsceneCamera1;  // 빈 방 카메라 (고정된 시점)
    public GameObject cutsceneCamera2;  // 무빙 카메라
    public GameObject mainCamera;       // 메인 카메라
    public GameObject playerControlScript;
    public GameObject aim;

    void Start()
    {
        StartCoroutine(PlayCutscenes());
    }

    IEnumerator PlayCutscenes()
    {
        // 플레이어의 조작 비활성화
        playerControlScript.SetActive(false);
        mainCamera.SetActive(false);  // 메인 카메라 비활성화
        aim.SetActive(false);

        //  [0] 빈 방 카메라 (고정된 시점)
        cutsceneCamera1.SetActive(true);
        Debug.Log("빈방 컷신 시작");

        yield return new WaitForSeconds(5f);  // 5초동안 빈방 보여줌

        cutsceneCamera1.SetActive(false);
        Debug.Log("빈방 컷신 종료");

        //  [1] 무빙 카메라 (CutSenceCameraMove 동작)
        GameObject movingCam = cutsceneCamera2;
        CutSenceCameraMove moveScript = movingCam.GetComponent<CutSenceCameraMove>();

        movingCam.SetActive(true);
        Debug.Log("무빙 컷신 시작");

        // 무빙 카메라가 끝날 때까지 대기
        while (moveScript != null && moveScript.isMoving)
            yield return null;

        movingCam.SetActive(false);
        Debug.Log("무빙 컷신 종료");

        yield return new WaitForSeconds(1f);  // 전환 딜레이 (선택)

        // 모든 카메라 비활성화 후 메인 카메라 활성화
        cutsceneCamera1.SetActive(false); // 빈 방 카메라 비활성화
        cutsceneCamera2.SetActive(false); // 무빙 카메라 비활성화

        // 메인 카메라 활성화
        mainCamera.SetActive(true);
        // 카메라의 위치와 회전값을 다시 설정하여 원하는 위치로 초기화

        // 플레이어 조작 활성화
        playerControlScript.SetActive(true);
        aim.SetActive(true);
        Debug.Log("컷신 종료, 플레이어 조작 시작!");
    }
}
