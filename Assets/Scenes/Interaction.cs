using UnityEngine;

public class Interaction : MonoBehaviour
{
    public int priority = 1;

    public Transform player;
    public float interactionRange = 3f;
    public float viewAngle = 45f;

    public Animator targetAnimator;
    public GameObject interactionUIText;
    public GameObject targetObject;

    [Header("����")]
    public GameObject openSoundObject;   // AudioSource ����, Inspector���� �Ҵ�
    public GameObject closeSoundObject;  // AudioSource ����, Inspector���� �Ҵ�

    private Renderer targetRenderer;
    private Color originalColor;
    private bool isOpen = false;

    private void Start()
    {
        if (interactionUIText != null)
            interactionUIText.SetActive(false);

        if (targetObject != null)
        {
            targetRenderer = targetObject.GetComponent<Renderer>();
            if (targetRenderer != null)
                originalColor = targetRenderer.material.color;
            else
                Debug.LogWarning("targetObject�� Renderer�� �����ϴ�!");
        }
    }

    private void Update()
    {
        if (!IsHighestPriority()) return;
        HandleInteraction();
    }

    private bool IsHighestPriority()
    {
        // �켱���� �ý����� �ִٸ� ���, �ƴϸ� �׻� true
        return true;
    }

    private void HandleInteraction()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        bool canInteract = false;

        if (distance <= interactionRange)
        {
            Vector3 directionToTarget = (transform.position - player.position).normalized;
            float angle = Vector3.Angle(player.forward, directionToTarget);

            if (angle <= viewAngle)
                canInteract = true;
        }

        if (interactionUIText != null)
            interactionUIText.SetActive(canInteract);

        if (targetRenderer != null)
            targetRenderer.material.color = canInteract ? Color.red : originalColor;

        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            if (targetAnimator != null)
            {
                if (isOpen)
                {
                    targetAnimator.SetTrigger("CloseLid");
                    Debug.Log("�Ѳ� �ݱ�!");
                    PlaySound(closeSoundObject);
                }
                else
                {
                    targetAnimator.SetTrigger("OpenLid");
                    Debug.Log("�Ѳ� ����!");
                    PlaySound(openSoundObject);
                }

                isOpen = !isOpen;
            }
        }
    }

    private void PlaySound(GameObject soundObj)
    {
        if (soundObj != null && SoundManager.Instance != null)
            SoundManager.Instance.PlayOneShot(soundObj);
    }
}
