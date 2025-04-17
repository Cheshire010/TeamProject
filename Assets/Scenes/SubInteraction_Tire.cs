using UnityEngine;
using System.Collections;

public class SubInteraction_Tire : MonoBehaviour
{
    public Transform player;
    public float interactionRange = 3f;
    public float viewAngle = 45f;

    public GameObject exitTireObject;         // Exit Ÿ�̾� ������Ʈ
    public GameObject goTireObject;           // Go Ÿ�̾� ������Ʈ
    public GameObject interactionUIText;      // ��ȣ�ۿ� �ؽ�Ʈ

    [Header("����")]
    public GameObject exitTireSoundObject;    // Exit Ÿ�̾� ���� (AudioSource ����)
    public GameObject goTireSoundObject;      // Go Ÿ�̾� ���� (AudioSource ����)

    public int priority = 2;                  // �켱����

    private Renderer exitTireRenderer;
    private Color originalColor;
    private bool isInteracting = false;

    private void Start()
    {
        if (interactionUIText != null)
            interactionUIText.SetActive(false);

        if (exitTireObject != null)
        {
            exitTireRenderer = exitTireObject.GetComponent<Renderer>();
            if (exitTireRenderer != null)
                originalColor = exitTireRenderer.material.color;
        }

        if (goTireObject != null)
            goTireObject.SetActive(false); // ���� �� GoTire ��Ȱ��ȭ
    }

    private void Update()
    {
        if (isInteracting) return; // ��ȣ�ۿ� ���� �Է� ����

        if (!CheckPlayerInRangeAndView()) // ����/�þ� üũ
        {
            HandleUI(false);
            ReleasePriorityIfHeld();
            return;
        }

        // �켱���� �Ŵ����� ��ȣ�ۿ� ��û
        if (InteractionPriorityManager.Instance.RequestInteraction(priority, gameObject))
        {
            HandleUI(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(HandleTireSequence());
                InteractionPriorityManager.Instance.ReleaseInteraction(gameObject); // ��ȣ�ۿ� ���� �� �켱���� ����
            }
        }
        else
        {
            HandleUI(false); // �켱���� �з����� UI ����
        }
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

        if (exitTireRenderer != null)
            exitTireRenderer.material.color = canInteract ? Color.red : originalColor;
    }

    private void ReleasePriorityIfHeld()
    {
        if (InteractionPriorityManager.Instance.CurrentPriority == priority)
            InteractionPriorityManager.Instance.ReleaseInteraction(gameObject);
    }

    private IEnumerator HandleTireSequence()
    {
        isInteracting = true;

        // Exit Ÿ�̾� ǥ�� �� ����
        if (exitTireObject != null)
        {
            exitTireObject.SetActive(true);
            PlaySound(exitTireSoundObject);
            Debug.Log("ExitTire ǥ�� ����.");
            yield return new WaitForSeconds(3f);
            exitTireObject.SetActive(false);
        }

        // Go Ÿ�̾� ǥ�� �� ����
        if (goTireObject != null)
        {
            goTireObject.SetActive(true);
            PlaySound(goTireSoundObject);
            Debug.Log("GoTire ǥ�� ����.");
        }

        isInteracting = false;
        Debug.Log("Ÿ�̾� ��ȣ�ۿ� �Ϸ�.");
    }

    private void PlaySound(GameObject soundObj)
    {
        if (soundObj != null && SoundManager.Instance != null)
            SoundManager.Instance.PlayOneShot(soundObj);
    }
}
