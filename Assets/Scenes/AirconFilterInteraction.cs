using UnityEngine;
using System.Collections;

public class AirconFilterInteraction : MonoBehaviour
{
    [Header("�÷��̾� �� ���ͷ��� ����")]
    public Transform player;
    public float interactionRange = 2.5f;
    public float viewAngle = 45f;

    [Header("�ִϸ����� �� ������Ʈ")]
    public Animator targetAnimator;
    public GameObject targetObject;

    [Header("UI")]
    public GameObject interactionUIText;

    [Header("�ִϸ��̼� Ʈ���� �� ����")]
    public string openTrigger = "OpenFilter";
    public string closeTrigger = "CloseFilter";
    public float openAnimDuration = 4.01f;    // OpenFilter �ִϸ��̼� ����(��)
    public float closeAnimDuration = 2.0f;    // CloseFilter �ִϸ��̼� ����(��)

    private bool isInteracting = false;
    private bool hasInteracted = false;
    private bool isOpen = false;
    private bool openEffectDone = false;

    private void Start()
    {
        if (interactionUIText != null)
            interactionUIText.SetActive(false);

        if (targetAnimator == null)
            Debug.LogError("targetAnimator�� �Ҵ���� �ʾҽ��ϴ�!");
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

        // 1. OpenFilter �ִϸ��̼� ���� (4.01��)
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

        // 1. CloseFilter �ִϸ��̼� ���� (2��)
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
