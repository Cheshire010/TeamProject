using UnityEngine;

public class Interaction : MonoBehaviour
{
    public Transform player;             // 플레이어 트랜스폼
    public float interactionRange = 3f;  // 상호작용 거리
    public float viewAngle = 45f;        // 시야각

    public Animator targetAnimator;      // 상호작용 대상 애니메이터
    public GameObject interactionUIText; // 상호작용 텍스트 UI 오브젝트

    private bool isInteracted = false;   // 뚜껑 열림 상태 여부

    private void Start()
    {
        if (interactionUIText != null)
            interactionUIText.SetActive(false);
    }

    private void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        bool canInteract = false;

        if (distance <= interactionRange)
        {
            Vector3 dirToTarget = (transform.position - player.position).normalized;
            float angle = Vector3.Angle(player.forward, dirToTarget);

            if (angle <= viewAngle)
            {
                canInteract = true;
            }
        }

        // 상호작용 텍스트 표시 여부
        if (interactionUIText != null)
            interactionUIText.SetActive(canInteract);

        // E키 눌렀을 때 상호작용 실행
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void Interact()
    {
        if (targetAnimator != null)
        {
            if (!isInteracted)
            {
                targetAnimator.SetTrigger("OpenLid");  // 열기 애니메이션
                Debug.Log("뚜껑 열기!");
            }
            else
            {
                targetAnimator.SetTrigger("CloseLid"); // 닫기 애니메이션
                Debug.Log("뚜껑 닫기!");
            }

            isInteracted = !isInteracted; // 상태 토글
        }
        else
        {
            Debug.LogWarning("상호작용 대상에 애니메이터가 없습니다!");
        }
    }
}
