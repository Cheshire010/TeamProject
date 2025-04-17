using UnityEngine;
using System.Collections;

public class OpenSubClose : MonoBehaviour
{
    [Header("�÷��̾� �� ���ͷ��� ����")]
    public Transform player;                // �÷��̾� Transform
    public float interactionRange = 3f;     // ��ȣ�ۿ� �Ÿ�
    public float viewAngle = 45f;           // ��ȣ�ۿ� ����

    [Header("�ִϸ����� �� ������Ʈ")]
    public Animator targetAnimator;         // �ִϸ�����
    public GameObject targetObject;         // ��ȣ�ۿ� ������Ʈ

    [Header("UI �� �켱����")]
    public GameObject interactionUIText;    // ��ȣ�ۿ� �ؽ�Ʈ UI
    public int priority = 3;                // �켱����

    [Header("�ִϸ��̼� Ʈ���� �̸�")]
    public string openTrigger = "OpenSub";
    public string closeTrigger = "CloseSub";
    public float openAnimDuration = 1.0f;   // ���� �ִϸ��̼� ����(��)
    public float closeAnimDuration = 1.0f;  // �ݱ� �ִϸ��̼� ����(��)

    private bool isOpen = false;
    private bool isAnimating = false;

    private void Start()
    {
        if (interactionUIText != null)
            interactionUIText.SetActive(false);

        if (targetAnimator == null)
            Debug.LogError("targetAnimator�� �Ҵ���� �ʾҽ��ϴ�!");
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
