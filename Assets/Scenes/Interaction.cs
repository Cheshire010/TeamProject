using UnityEngine;

public class Interaction : MonoBehaviour
{
    public int priority = 1;                   // �켱����: 1 (���� ���ͷ����� 0������ ���)

    public Transform player;                  // �÷��̾� ��ġ
    public float interactionRange = 3f;       // ��ȣ�ۿ� ���� �Ÿ�
    public float viewAngle = 45f;             // �÷��̾� �þ߰�

    public Animator targetAnimator;           // ��� �ִϸ�����
    public GameObject interactionUIText;      // ��ȣ�ۿ� �ȳ� �ؽ�Ʈ
    public GameObject targetObject;           // ���� ������ ��ü (�ۺ����� ���)

    private Renderer targetRenderer;          // ������
    private Color originalColor;              // ���� ���� ����
    private bool isOpen = false;              // �Ѳ� ����/���� ����

    private void Start()
    {
        if (interactionUIText != null)
            interactionUIText.SetActive(false);

        // targetObject�� ���� Renderer ���
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
        // �켱���� �ý����� �ִٸ�: �� ������Ʈ�� ���� �ֿ켱 ����� ���� ����
        if (!IsHighestPriority()) return;

        HandleInteraction();
    }

    private bool IsHighestPriority()
    {
        // ���� InteractionManager ���� �ý����� �ִٸ� ���⼭ �켱���� ��
        // ��: return InteractionManager.Instance.IsCurrentTarget(this);
        return true;  // �ϴ� ���������� ���� �� �׻� true
    }

    private void HandleInteraction()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        bool canInteract = false;

        // �Ÿ� + �þ߰� üũ
        if (distance <= interactionRange)
        {
            Vector3 directionToTarget = (transform.position - player.position).normalized;
            float angle = Vector3.Angle(player.forward, directionToTarget);

            if (angle <= viewAngle)
                canInteract = true;
        }

        // �ؽ�Ʈ ǥ�� ����
        if (interactionUIText != null)
            interactionUIText.SetActive(canInteract);

        // ���� ����: ��ȣ�ۿ� ���� = ��ȫ��, �Ұ��� = ���� ����
        if (targetRenderer != null)
            targetRenderer.material.color = canInteract ? Color.red : originalColor;

        // E Ű �Է� �� ��ȣ�ۿ�
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            if (targetAnimator != null)
            {
                if (isOpen)
                {
                    targetAnimator.SetTrigger("CloseLid");
                    Debug.Log("�Ѳ� �ݱ�!");
                }
                else
                {
                    targetAnimator.SetTrigger("OpenLid");
                    Debug.Log("�Ѳ� ����!");
                }

                isOpen = !isOpen;  // ���� ��ȯ
            }
        }
    }
}
