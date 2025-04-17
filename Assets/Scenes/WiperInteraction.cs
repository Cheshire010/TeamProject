using UnityEngine;
using System.Collections;

public class WiperInteraction : MonoBehaviour
{
    public Transform player;                    // �÷��̾� ��ġ
    public float interactionRange = 3f;         // ��ȣ�ۿ� ���� �Ÿ�
    public float viewAngle = 45f;               // �þ߰�

    public GameObject interactionUIText;        // ��ȣ�ۿ� �ؽ�Ʈ
    public Animator wiperAnimator1;             // ������ �ִϸ����� 1
    public Animator wiperAnimator2;             // ������ �ִϸ����� 2
    public GameObject[] targetObjects;          // ���� �ٲ� ������Ʈ �迭 (4������ ����)

    [Header("����")]
    public GameObject exitSoundObject;          // Exit �ܰ� ���� (AudioSource ����)
    public GameObject changeSoundObject;        // Change �ܰ� ���� (AudioSource ����)

    public int priority = 1;                    // �켱����

    private Renderer[] targetRenderers;
    private Color[] originalColors;
    private bool isInteracting = false;

    private void Start()
    {
        if (interactionUIText != null)
            interactionUIText.SetActive(false);

        if (targetObjects != null && targetObjects.Length > 0)
        {
            targetRenderers = new Renderer[targetObjects.Length];
            originalColors = new Color[targetObjects.Length];

            for (int i = 0; i < targetObjects.Length; i++)
            {
                if (targetObjects[i] != null)
                {
                    Renderer renderer = targetObjects[i].GetComponent<Renderer>();
                    targetRenderers[i] = renderer;

                    if (renderer != null)
                        originalColors[i] = renderer.material.color;
                    else
                        Debug.LogWarning($"{targetObjects[i].name} ������Ʈ�� Renderer�� �����ϴ�!");
                }
            }
        }
    }

    private void Update()
    {
        if (isInteracting) return;

        bool canInteract = CheckPlayerInRangeAndView();

        HandleUI(canInteract);

        if (canInteract && Input.GetKeyDown(KeyCode.E))
            StartCoroutine(HandleWiperSequence());
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

        if (targetRenderers != null)
        {
            for (int i = 0; i < targetRenderers.Length; i++)
            {
                if (targetRenderers[i] != null)
                    targetRenderers[i].material.color = canInteract ? Color.red : originalColors[i];
            }
        }
    }

    private IEnumerator HandleWiperSequence()
    {
        isInteracting = true;

        // 1�ܰ�: Exit (Wipe, Wipe2 �ִϸ��̼� Ʈ����) + ����
        if (wiperAnimator1 != null && wiperAnimator2 != null)
        {
            PlaySound(exitSoundObject);
            wiperAnimator1.SetTrigger("Exit");   // Wipe
            wiperAnimator2.SetTrigger("Exit");   // Wipe2
            Debug.Log("Wipe, Wipe2 �ִϸ��̼� ���� (Exit)");

            yield return new WaitForSeconds(3f);  // �ִϸ��̼� ��� �ð�
        }

        // 2�ܰ�: Change (Wipe3, Wipe4 �ִϸ��̼� Ʈ����) + ����
        if (wiperAnimator1 != null && wiperAnimator2 != null)
        {
            PlaySound(changeSoundObject);
            wiperAnimator1.SetTrigger("Change");  // Wipe3
            wiperAnimator2.SetTrigger("Change");  // Wipe4
            Debug.Log("Wipe3, Wipe4 �ִϸ��̼� ���� (Change)");

            yield return new WaitForSeconds(3f);  // �ִϸ��̼� ��� �ð�
        }

        isInteracting = false;
        Debug.Log("������ ��ȣ�ۿ� �Ϸ�.");
    }

    private void PlaySound(GameObject soundObj)
    {
        if (soundObj != null && SoundManager.Instance != null)
            SoundManager.Instance.PlayOneShot(soundObj);
    }
}
