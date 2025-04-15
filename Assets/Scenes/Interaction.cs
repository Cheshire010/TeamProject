using UnityEngine;

public class Interaction : MonoBehaviour
{
    public Transform player;             // �÷��̾� Ʈ������
    public float interactionRange = 3f;  // ��ȣ�ۿ� �Ÿ�
    public float viewAngle = 45f;        // �þ߰�

    public Animator targetAnimator;      // ��ȣ�ۿ� ��� �ִϸ�����
    public GameObject interactionUIText; // ��ȣ�ۿ� �ؽ�Ʈ UI ������Ʈ

    private bool isInteracted = false;   // �Ѳ� ���� ���� ����

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

        // ��ȣ�ۿ� �ؽ�Ʈ ǥ�� ����
        if (interactionUIText != null)
            interactionUIText.SetActive(canInteract);

        // EŰ ������ �� ��ȣ�ۿ� ����
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
                targetAnimator.SetTrigger("OpenLid");  // ���� �ִϸ��̼�
                Debug.Log("�Ѳ� ����!");
            }
            else
            {
                targetAnimator.SetTrigger("CloseLid"); // �ݱ� �ִϸ��̼�
                Debug.Log("�Ѳ� �ݱ�!");
            }

            isInteracted = !isInteracted; // ���� ���
        }
        else
        {
            Debug.LogWarning("��ȣ�ۿ� ��� �ִϸ����Ͱ� �����ϴ�!");
        }
    }
}
