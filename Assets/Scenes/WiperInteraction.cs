using UnityEngine;
using System.Collections;

public class WiperInteraction : MonoBehaviour
{
    public Transform player;                    // 플레이어 위치
    public float interactionRange = 3f;         // 상호작용 가능 거리
    public float viewAngle = 45f;               // 시야각

    public GameObject interactionUIText;        // 상호작용 텍스트
    public Animator wiperAnimator1;             // 와이퍼 애니메이터 1
    public Animator wiperAnimator2;             // 와이퍼 애니메이터 2
    public GameObject[] targetObjects;          // 색상 바꿀 오브젝트 배열 (4개까지 가능)

    [Header("사운드")]
    public GameObject exitSoundObject;          // Exit 단계 사운드 (AudioSource 포함)
    public GameObject changeSoundObject;        // Change 단계 사운드 (AudioSource 포함)

    public int priority = 1;                    // 우선순위

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
                        Debug.LogWarning($"{targetObjects[i].name} 오브젝트에 Renderer가 없습니다!");
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

        // 1단계: Exit (Wipe, Wipe2 애니메이션 트리거) + 사운드
        if (wiperAnimator1 != null && wiperAnimator2 != null)
        {
            PlaySound(exitSoundObject);
            wiperAnimator1.SetTrigger("Exit");   // Wipe
            wiperAnimator2.SetTrigger("Exit");   // Wipe2
            Debug.Log("Wipe, Wipe2 애니메이션 시작 (Exit)");

            yield return new WaitForSeconds(3f);  // 애니메이션 대기 시간
        }

        // 2단계: Change (Wipe3, Wipe4 애니메이션 트리거) + 사운드
        if (wiperAnimator1 != null && wiperAnimator2 != null)
        {
            PlaySound(changeSoundObject);
            wiperAnimator1.SetTrigger("Change");  // Wipe3
            wiperAnimator2.SetTrigger("Change");  // Wipe4
            Debug.Log("Wipe3, Wipe4 애니메이션 시작 (Change)");

            yield return new WaitForSeconds(3f);  // 애니메이션 대기 시간
        }

        isInteracting = false;
        Debug.Log("와이퍼 상호작용 완료.");
    }

    private void PlaySound(GameObject soundObj)
    {
        if (soundObj != null && SoundManager.Instance != null)
            SoundManager.Instance.PlayOneShot(soundObj);
    }
}
