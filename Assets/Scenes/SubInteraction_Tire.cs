using UnityEngine;
using System.Collections;

public class SubInteraction_Tire : MonoBehaviour
{
    public Transform player;
    public float interactionRange = 3f;
    public float viewAngle = 45f;

    public GameObject exitTireObject;         // Exit 타이어 오브젝트
    public GameObject goTireObject;           // Go 타이어 오브젝트
    public GameObject interactionUIText;      // 상호작용 텍스트

    [Header("사운드")]
    public GameObject exitTireSoundObject;    // Exit 타이어 사운드 (AudioSource 포함)
    public GameObject goTireSoundObject;      // Go 타이어 사운드 (AudioSource 포함)

    public int priority = 2;                  // 우선순위

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
            goTireObject.SetActive(false); // 시작 시 GoTire 비활성화
    }

    private void Update()
    {
        if (isInteracting) return; // 상호작용 도중 입력 무시

        if (!CheckPlayerInRangeAndView()) // 범위/시야 체크
        {
            HandleUI(false);
            ReleasePriorityIfHeld();
            return;
        }

        // 우선순위 매니저에 상호작용 요청
        if (InteractionPriorityManager.Instance.RequestInteraction(priority, gameObject))
        {
            HandleUI(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(HandleTireSequence());
                InteractionPriorityManager.Instance.ReleaseInteraction(gameObject); // 상호작용 시작 시 우선순위 해제
            }
        }
        else
        {
            HandleUI(false); // 우선순위 밀렸으면 UI 숨김
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

        // Exit 타이어 표시 및 사운드
        if (exitTireObject != null)
        {
            exitTireObject.SetActive(true);
            PlaySound(exitTireSoundObject);
            Debug.Log("ExitTire 표시 시작.");
            yield return new WaitForSeconds(3f);
            exitTireObject.SetActive(false);
        }

        // Go 타이어 표시 및 사운드
        if (goTireObject != null)
        {
            goTireObject.SetActive(true);
            PlaySound(goTireSoundObject);
            Debug.Log("GoTire 표시 시작.");
        }

        isInteracting = false;
        Debug.Log("타이어 상호작용 완료.");
    }

    private void PlaySound(GameObject soundObj)
    {
        if (soundObj != null && SoundManager.Instance != null)
            SoundManager.Instance.PlayOneShot(soundObj);
    }
}
