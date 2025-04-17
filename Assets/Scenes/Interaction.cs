using UnityEngine;

public class Interaction : MonoBehaviour
{
    public int priority = 1;                   // 우선순위: 1 (서브 인터랙션이 0순위일 경우)

    public Transform player;                  // 플레이어 위치
    public float interactionRange = 3f;       // 상호작용 가능 거리
    public float viewAngle = 45f;             // 플레이어 시야각

    public Animator targetAnimator;           // 대상 애니메이터
    public GameObject interactionUIText;      // 상호작용 안내 텍스트
    public GameObject targetObject;           // 색상 변경할 물체 (퍼블릭으로 등록)

    private Renderer targetRenderer;          // 렌더러
    private Color originalColor;              // 원래 색상 저장
    private bool isOpen = false;              // 뚜껑 열림/닫힘 상태

    private void Start()
    {
        if (interactionUIText != null)
            interactionUIText.SetActive(false);

        // targetObject를 통한 Renderer 등록
        if (targetObject != null)
        {
            targetRenderer = targetObject.GetComponent<Renderer>();
            if (targetRenderer != null)
                originalColor = targetRenderer.material.color;
            else
                Debug.LogWarning("targetObject에 Renderer가 없습니다!");
        }
    }

    private void Update()
    {
        // 우선순위 시스템이 있다면: 이 오브젝트가 현재 최우선 대상일 때만 실행
        if (!IsHighestPriority()) return;

        HandleInteraction();
    }

    private bool IsHighestPriority()
    {
        // 만약 InteractionManager 같은 시스템이 있다면 여기서 우선순위 비교
        // 예: return InteractionManager.Instance.IsCurrentTarget(this);
        return true;  // 일단 독립적으로 실행 시 항상 true
    }

    private void HandleInteraction()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        bool canInteract = false;

        // 거리 + 시야각 체크
        if (distance <= interactionRange)
        {
            Vector3 directionToTarget = (transform.position - player.position).normalized;
            float angle = Vector3.Angle(player.forward, directionToTarget);

            if (angle <= viewAngle)
                canInteract = true;
        }

        // 텍스트 표시 제어
        if (interactionUIText != null)
            interactionUIText.SetActive(canInteract);

        // 색상 변경: 상호작용 가능 = 진홍색, 불가능 = 원래 색상
        if (targetRenderer != null)
            targetRenderer.material.color = canInteract ? Color.red : originalColor;

        // E 키 입력 시 상호작용
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            if (targetAnimator != null)
            {
                if (isOpen)
                {
                    targetAnimator.SetTrigger("CloseLid");
                    Debug.Log("뚜껑 닫기!");
                }
                else
                {
                    targetAnimator.SetTrigger("OpenLid");
                    Debug.Log("뚜껑 열기!");
                }

                isOpen = !isOpen;  // 상태 전환
            }
        }
    }
}
