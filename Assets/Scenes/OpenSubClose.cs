using UnityEngine;
using System.Collections;

public class OpenSubClose : MonoBehaviour
{
    [Header("플레이어 및 인터랙션 범위")]
    public Transform player;
    public float interactionRange = 3f;
    public float viewAngle = 45f;

    [Header("애니메이터 및 오브젝트")]
    public Animator targetAnimator;
    public GameObject targetObject;

    [Header("UI 및 우선순위")]
    public GameObject interactionUIText;
    public int priority = 3;

    [Header("애니메이션 트리거 이름")]
    public string openTrigger = "OpenSub";
    public string closeTrigger = "CloseSub";
    public float openAnimDuration = 1.0f;
    public float closeAnimDuration = 1.0f;

    [Header("사운드")]
    public GameObject openSoundObject;   // 열기 사운드용 (AudioSource 포함)
    public GameObject closeSoundObject;  // 닫기 사운드용 (AudioSource 포함)

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

        bool willOpen = !isOpen;
        string trigger = willOpen ? openTrigger : closeTrigger;
        float animDuration = willOpen ? openAnimDuration : closeAnimDuration;

        PlaySound(willOpen ? openSoundObject : closeSoundObject);
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

    private void PlaySound(GameObject soundObj)
    {
        if (soundObj != null && SoundManager.Instance != null)
            SoundManager.Instance.PlayOneShot(soundObj);
    }

    private void ReleasePriorityIfHeld()
    {
        if (InteractionPriorityManager.Instance.CurrentOwner == gameObject)
            InteractionPriorityManager.Instance.ReleaseInteraction(gameObject);
    }
}
