using UnityEngine;
using System.Collections;

public class OpenSubClose : MonoBehaviour
{
    [Header("플레이어 및 인터랙션 범위")]
    public Transform player;                // 플레이어 Transform
    public float interactionRange = 3f;     // 상호작용 거리
    public float viewAngle = 45f;           // 상호작용 각도

    [Header("애니메이터 및 오브젝트")]
    public Animator targetAnimator;         // 애니메이터
    public GameObject targetObject;         // 상호작용 오브젝트

    [Header("UI 및 우선순위")]
    public GameObject interactionUIText;    // 상호작용 텍스트 UI
    public int priority = 3;                // 우선순위

    [Header("애니메이션 트리거 이름")]
    public string openTrigger = "OpenSub";
    public string closeTrigger = "CloseSub";
    public float openAnimDuration = 1.0f;   // 열기 애니메이션 길이(초)
    public float closeAnimDuration = 1.0f;  // 닫기 애니메이션 길이(초)

    private bool isOpen = false;
    private bool isAnimating = false;

    private void Start()
    {
        if (interactionUIText != null)
            interactionUIText.SetActive(false);

        if (targetAnimator == null)
            Debug.LogError("targetAnimator가 할당되지 않았습니다!");
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
            HandleInput();
        }
        else
        {
            HandleUI(false);
        }
    }

    private bool CheckPlayerInRangeAndView()
    {
        if (player == null) return false;

        float distance = Vector3.Distance(player.position, transform.position);
        if (distance > interactionRange) return false;

        Vector3 directionToTarget = (transform.position - player.position).normalized;
        float angle = Vector3.Angle(player.forward, directionToTarget);
        return angle <= viewAngle;
    }

    private void HandleUI(bool canInteract)
    {
        if (interactionUIText != null)
            interactionUIText.SetActive(canInteract);
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isAnimating)
        {
            StartCoroutine(InteractionRoutine());
            InteractionPriorityManager.Instance.ReleaseInteraction(gameObject);
        }
    }

    private IEnumerator InteractionRoutine()
    {
        isAnimating = true;

        string trigger = isOpen ? closeTrigger : openTrigger;
        float animDuration = isOpen ? closeAnimDuration : openAnimDuration;

        PlayAnimation(trigger);

        yield return new WaitForSeconds(animDuration);

        isOpen = !isOpen;
        isAnimating = false;
    }

    private void PlayAnimation(string triggerName)
    {
        if (targetAnimator != null)
        {
            targetAnimator.ResetTrigger(openTrigger);
            targetAnimator.ResetTrigger(closeTrigger);
            targetAnimator.SetTrigger(triggerName);
        }
    }

    private void ReleasePriorityIfHeld()
    {
        if (InteractionPriorityManager.Instance.CurrentOwner == gameObject)
            InteractionPriorityManager.Instance.ReleaseInteraction(gameObject);
    }
}
