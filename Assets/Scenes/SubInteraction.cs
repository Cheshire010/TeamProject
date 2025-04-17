using UnityEngine;
using System.Collections;

public class SubInteraction : MonoBehaviour
{
    [Header("참조 설정")]
    public Transform player;
    public float interactionRange = 3f;
    public float viewAngle = 45f;
    public Animator targetAnimator;
    public GameObject targetObject;
    public GameObject interactionUIText;
    public GameObject playerControlScript;

    [Header("상호작용 설정")]
    [Tooltip("0이 가장 높은 우선순위")]
    public int priority = 0;
    public string openTrigger = "OpenWashing";
    public string closeTrigger = "CloseWashing";
    [Tooltip("열기 애니메이션 길이(초)")]
    public float openAnimDuration = 1.0f;
    [Tooltip("닫기 애니메이션 길이(초)")]
    public float closeAnimDuration = 1.0f;

    [Header("사운드")]
    public GameObject openSoundObject;   // 열기 사운드용 (AudioSource 포함)
    public GameObject closeSoundObject;  // 닫기 사운드용 (AudioSource 포함)

    private Renderer targetRenderer;
    private Color originalColor;
    private bool isOpen = false;
    private bool isAnimating = false;

    private void Start()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        if (interactionUIText != null)
            interactionUIText.SetActive(false);

        if (targetObject != null)
        {
            targetRenderer = targetObject.GetComponent<Renderer>();
            if (targetRenderer != null)
            {
                targetRenderer.material = new Material(targetRenderer.sharedMaterial);
                originalColor = targetRenderer.material.color;
            }
            else
                Debug.LogError("TargetObject에 Renderer 컴포넌트가 없습니다!");
        }

        if (targetAnimator == null)
            Debug.LogError("Animator가 할당되지 않았습니다!");
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
        TogglePlayerControl(false);

        bool willOpen = !isOpen;
        string trigger = willOpen ? openTrigger : closeTrigger;
        float animDuration = willOpen ? openAnimDuration : closeAnimDuration;

        PlaySound(willOpen ? openSoundObject : closeSoundObject);
        PlayAnimation(trigger);

        yield return new WaitForSeconds(animDuration);

        isOpen = !isOpen;
        TogglePlayerControl(true);
        isAnimating = false;
    }

    private void PlayAnimation(string triggerName)
    {
        if (targetAnimator != null)
        {
            targetAnimator.ResetTrigger(openTrigger);
            targetAnimator.ResetTrigger(closeTrigger);
            targetAnimator.SetTrigger(triggerName);
            Debug.Log($"애니메이션 트리거: {triggerName}");
        }
    }

    private void PlaySound(GameObject soundObj)
    {
        if (soundObj != null && SoundManager.Instance != null)
            SoundManager.Instance.PlayOneShot(soundObj);
    }

    private bool CheckPlayerInRangeAndView()
    {
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

        if (targetRenderer != null)
            targetRenderer.material.color = canInteract ? Color.red : originalColor;
    }

    private void ReleasePriorityIfHeld()
    {
        if (InteractionPriorityManager.Instance.CurrentOwner == gameObject)
            InteractionPriorityManager.Instance.ReleaseInteraction(gameObject);
    }

    private void TogglePlayerControl(bool enable)
    {
        if (playerControlScript != null)
            playerControlScript.SetActive(enable);
    }
}
