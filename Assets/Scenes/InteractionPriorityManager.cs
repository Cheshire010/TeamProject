using UnityEngine;

public class InteractionPriorityManager : MonoBehaviour
{
    public static InteractionPriorityManager Instance { get; private set; }

    public int CurrentPriority { get; private set; } = -1;    // 현재 점유한 우선순위
    public GameObject CurrentOwner { get; private set; }      // 현재 상호작용 중인 오브젝트

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// 우선순위 요청: 성공 시 true 반환, 이 오브젝트를 상호작용 점유자로 등록
    /// </summary>
    public bool RequestInteraction(int requestedPriority, GameObject requester)
    {
        if (requester == null) return false;

        if (requestedPriority > CurrentPriority || CurrentOwner == requester)
        {
            CurrentPriority = requestedPriority;
            CurrentOwner = requester;
            return true;
        }

        return false;
    }

    /// <summary>
    /// 우선순위 해제: 요청자가 현재 점유자일 때만 해제.
    /// </summary>
    public void ReleaseInteraction(GameObject requester)
    {
        if (requester == null) return;

        if (CurrentOwner == requester)
        {
            CurrentPriority = -1;
            CurrentOwner = null;
        }
    }
}
