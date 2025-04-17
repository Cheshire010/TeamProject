using UnityEngine;
using System.Collections;

public class AirconFilterInteraction : MonoBehaviour
{
    [Header("플레이어 및 인터랙션 범위")]
    public Transform player;
    public float interactionRange = 2.5f;
    public float viewAngle = 45f;

    [Header("애니메이터 및 오브젝트")]
    public Animator targetAnimator;
    public GameObject targetObject;

    [Header("UI")]
    public GameObject interactionUIText;

    [Header("애니메이션 트리거 및 길이")]
    public string openTrigger = "OpenFilter";
    public string closeTrigger = "CloseFilter";
    public float openAnimDuration = 4.01f;    // OpenFilter 애니메이션 길이(초)
    public float closeAnimDuration = 2.0f;    // CloseFilter 애니메이션 길이(초)

    private bool isInteracting = false;
    private bool hasInteracted = false;
    private bool isOpen = false;
    private bool openEffectDone = false;

    private void Start()
    {
        if (interactionUIText != null)
            interactionUIText.SetActive(false);

        if (targetAnimator == null)
            Debug.LogError("targetAnimator가 할당되지 않았습니다!");
    }

    private void Update()
    {
        if (hasInteracted)
        {
            if (interactionUIText != null)
                interactionUIText.SetActive(false);
            return;
        }

        if (!CheckPlayerInRangeAndView() || isInteracting)
        {
            HandleUI(false);
            return;
        }

        HandleUI(true);
        HandleInput();
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
        {
            if (!isOpen)
                interactionUIText.SetActive(canInteract && !isInteracting);
            else
                interactionUIText.SetActive(canInteract && !isInteracting && openEffectDone);
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isInteracting)
        {
            if (!isOpen)
                StartCoroutine(OpenSequence());
            else if (openEffectDone)
                StartCoroutine(CloseSequence());
        }
    }

    private IEnumerator OpenSequence()
    {
        isInteracting = true;
        HandleUI(false);

        // 1. OpenFilter 애니메이션 실행 (4.01초)
        PlayAnimation(openTrigger);
        yield return new WaitForSeconds(openAnimDuration);

        isOpen = true;
        openEffectDone = true;
        isInteracting = false;
    }

    private IEnumerator CloseSequence()
    {
        isInteracting = true;
        HandleUI(false);

        // 1. CloseFilter 애니메이션 실행 (2초)
        PlayAnimation(closeTrigger);
        yield return new WaitForSeconds(closeAnimDuration);

        isInteracting = false;
        hasInteracted = true;
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
}
