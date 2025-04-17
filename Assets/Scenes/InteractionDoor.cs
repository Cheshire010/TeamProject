using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class InteractionDoor : MonoBehaviour
{
    public Transform player;               // 플레이어 위치
    public float interactionRange = 3f;    // 인식 범위
    public float viewAngle = 45f;          // 시야각

    public int priority = 4;               // 우선순위

    public Animator doorAnimator;          // 도어 애니메이터
    public GameObject highlightObject;     // 색상 변경할 오브젝트
    public GameObject interactionUIText;   // 상호작용 UI 텍스트

    private Renderer highlightRenderer;
    private Color originalColor;
    private bool isOpen = false;

    private void Start()
    {
        if (highlightObject != null)
        {
            highlightRenderer = highlightObject.GetComponent<Renderer>();
            if (highlightRenderer != null)
            {
                // 중요: 재질의 고유 인스턴스 생성
                highlightRenderer.material = new Material(highlightRenderer.sharedMaterial);
                originalColor = highlightRenderer.material.color;
            }
            else
                Debug.LogWarning($"{highlightObject.name} 에 Renderer가 없습니다.");
        }

        if (interactionUIText != null)
            interactionUIText.SetActive(false);
    }


    private void Update()
    {
        if (!CheckPlayerInRangeAndView())
        {
            HandleUI(false);
            ReleasePriorityIfHeld();
            return;
        }

        if (InteractionPriorityManager.Instance.RequestInteraction(priority, gameObject))
        {
            HandleUI(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                ToggleDoor();
                InteractionPriorityManager.Instance.ReleaseInteraction(gameObject);  // 상호작용 시작시 해제
            }
        }
        else
        {
            HandleUI(false);
        }
    }

    private bool CheckPlayerInRangeAndView()
    {
        if (player == null)
        {
            Debug.LogError("Player 참조가 설정되지 않았습니다!");
            return false;
        }

        float distance = Vector3.Distance(player.position, transform.position);
        Vector3 directionToTarget = (transform.position - player.position).normalized;
        float angle = Vector3.Angle(player.forward, directionToTarget);

        Debug.Log($"거리: {distance}, 범위: {interactionRange}, 각도: {angle}, 시야각: {viewAngle}");

        return (distance <= interactionRange && angle <= viewAngle);
    }

    private void HandleUI(bool canInteract)
    {
        if (interactionUIText != null)
            interactionUIText.SetActive(canInteract);

        if (highlightRenderer != null)
        {
            if (canInteract)
            {
                // 다음 방법들을 순차적으로 시도해 보세요
                highlightRenderer.material.SetColor("_Color", Color.red);
                // 또는
                highlightRenderer.material.SetColor("_BaseColor", Color.red);
                // 또는
                highlightRenderer.material.SetColor("_EmissionColor", Color.red);
                highlightRenderer.material.EnableKeyword("_EMISSION");
            }
            else
            {
                highlightRenderer.material.color = originalColor;
                // 필요한 경우 다른 속성도 원래대로 복원
            }
        }
    }



    private void ReleasePriorityIfHeld()
    {
        if (InteractionPriorityManager.Instance.CurrentOwner == gameObject)
            InteractionPriorityManager.Instance.ReleaseInteraction(gameObject);
    }

    private void ToggleDoor()
    {
        if (doorAnimator != null)
        {
            string triggerName = isOpen ? "Close" : "Open";
            doorAnimator.SetTrigger(triggerName);
            Debug.Log($"도어 {(isOpen ? "닫는 중" : "여는 중")}");

            isOpen = !isOpen;
        }
    }
}
