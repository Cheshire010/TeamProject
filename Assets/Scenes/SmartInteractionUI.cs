using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SmartInteractionUI : MonoBehaviour
{
    public Transform player;             // 플레이어 트랜스폼
    public float interactionRange = 3f;  // 상호작용 거리
    public float viewAngle = 45f;        // 시야각

    public GameObject interactionUI;     // 상호작용 UI 오브젝트 (Text or Panel)

    public GameObject camera1;           // 현재 메인 카메라
    public GameObject camera2;           // 로딩용 카메라

    public GameObject carMoveObject;     // 움직일 자동차 오브젝트
    public float carMoveSpeed = 2f;      // CarMove 이동속도

    private Vector3 carOriginalPos;      // 자동차 원래 위치 저장
    private bool isCarMoving = false;    // CarMove 이동 중 여부

    private void Start()
    {
        if (interactionUI != null)
            interactionUI.SetActive(false);

        if (camera1 != null) camera1.SetActive(true);
        if (camera2 != null) camera2.SetActive(false);

        if (carMoveObject != null)
            carOriginalPos = carMoveObject.transform.position;
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactionRange)
        {
            Vector3 dirToTarget = (transform.position - player.position).normalized;
            float angle = Vector3.Angle(player.forward, dirToTarget);

            if (angle <= viewAngle)
            {
                if (interactionUI != null)
                    interactionUI.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("E키 눌림! 상호작용 시작.");
                    StartCoroutine(EnterSituation());  // 코루틴 실행
                }
                return;
            }
        }

        if (interactionUI != null)
            interactionUI.SetActive(false);

        // CarMove가 이동 중일 때 X축으로 이동
        if (isCarMoving && carMoveObject != null)
        {
            carMoveObject.transform.Translate(Vector3.left * carMoveSpeed * Time.deltaTime);
        }
    }

    IEnumerator EnterSituation()
    {
        if (interactionUI != null)
            interactionUI.SetActive(false);

        // 플레이어 비활성화 + 위치 Z 축 -2 이동
        player.gameObject.SetActive(false);
        Vector3 newPosition = player.position;
        newPosition.z -= 2f;
        player.position = newPosition;

        // 카메라 전환
        if (camera1 != null) camera1.SetActive(false);
        if (camera2 != null) camera2.SetActive(true);

        // CarMove 이동 시작
        isCarMoving = true;

        // 4초 대기
        yield return new WaitForSeconds(4f);

        // CarMove 멈춤 + 위치 원복
        isCarMoving = false;
        if (carMoveObject != null)
            carMoveObject.transform.position = carOriginalPos;

        // 카메라 원상복귀
        if (camera1 != null) camera1.SetActive(true);
        if (camera2 != null) camera2.SetActive(false);

        // 플레이어 다시 활성화
        player.gameObject.SetActive(true);

        Debug.Log("로딩 카메라 종료. 플레이어 조작 재개!");
    }
}
