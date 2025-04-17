using UnityEngine;

public class InteractionPriorityManager : MonoBehaviour
{
    public static InteractionPriorityManager Instance { get; private set; }

    public int CurrentPriority { get; private set; } = -1;    // ���� ������ �켱����
    public GameObject CurrentOwner { get; private set; }      // ���� ��ȣ�ۿ� ���� ������Ʈ

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// �켱���� ��û: ���� �� true ��ȯ, �� ������Ʈ�� ��ȣ�ۿ� �����ڷ� ���
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
    /// �켱���� ����: ��û�ڰ� ���� �������� ���� ����.
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
