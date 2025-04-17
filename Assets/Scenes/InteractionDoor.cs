using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class InteractionDoor : MonoBehaviour
{
    public Transform player;               // �÷��̾� ��ġ
    public float interactionRange = 3f;    // �ν� ����
    public float viewAngle = 45f;          // �þ߰�

    public int priority = 4;               // �켱����

    public Animator doorAnimator;          // ���� �ִϸ�����
    public GameObject highlightObject;     // ���� ������ ������Ʈ
    public GameObject interactionUIText;   // ��ȣ�ۿ� UI �ؽ�Ʈ

    private Renderer highlightRenderer;
    private Color originalColor;
    private bool isOpen = false;

    private void Start()
    {
        if (highlightObject != null)
        {
            highlightRenderer = highlightObject.GetComponent<Renderer>();
            if (highlightRenderer != null)
            {
                // �߿�: ������ ���� �ν��Ͻ� ����
                highlightRenderer.material = new Material(highlightRenderer.sharedMaterial);
                originalColor = highlightRenderer.material.color;
            }
            else
                Debug.LogWarning($"{highlightObject.name} �� Renderer�� �����ϴ�.");
        }

        if (interactionUIText != null)
            interactionUIText.SetActive(false);
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

            if (Input.GetKeyDown(KeyCode.E))
            {
                ToggleDoor();
                InteractionPriorityManager.Instance.ReleaseInteraction(gameObject);  // ��ȣ�ۿ� ���۽� ����
            }
        }
        else
        {
            HandleUI(false);
        }
    }

    private bool CheckPlayerInRangeAndView()
    {
        if (player == null)
        {
            Debug.LogError("Player ������ �������� �ʾҽ��ϴ�!");
            return false;
        }

        float distance = Vector3.Distance(player.position, transform.position);
        Vector3 directionToTarget = (transform.position - player.position).normalized;
        float angle = Vector3.Angle(player.forward, directionToTarget);

        Debug.Log($"�Ÿ�: {distance}, ����: {interactionRange}, ����: {angle}, �þ߰�: {viewAngle}");

        return (distance <= interactionRange && angle <= viewAngle);
    }

    private void HandleUI(bool canInteract)
    {
        if (interactionUIText != null)
            interactionUIText.SetActive(canInteract);

        if (highlightRenderer != null)
        {
            if (canInteract)
            {
                // ���� ������� ���������� �õ��� ������
                highlightRenderer.material.SetColor("_Color", Color.red);
                // �Ǵ�
                highlightRenderer.material.SetColor("_BaseColor", Color.red);
                // �Ǵ�
                highlightRenderer.material.SetColor("_EmissionColor", Color.red);
                highlightRenderer.material.EnableKeyword("_EMISSION");
            }
            else
            {
                highlightRenderer.material.color = originalColor;
                // �ʿ��� ��� �ٸ� �Ӽ��� ������� ����
            }
        }
    }



    private void ReleasePriorityIfHeld()
    {
        if (InteractionPriorityManager.Instance.CurrentOwner == gameObject)
            InteractionPriorityManager.Instance.ReleaseInteraction(gameObject);
    }

    private void ToggleDoor()
    {
        if (doorAnimator != null)
        {
            string triggerName = isOpen ? "Close" : "Open";
            doorAnimator.SetTrigger(triggerName);
            Debug.Log($"���� {(isOpen ? "�ݴ� ��" : "���� ��")}");

            isOpen = !isOpen;
        }
    }
}
