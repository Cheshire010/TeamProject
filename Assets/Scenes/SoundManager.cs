using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// ������ ������Ʈ�� AudioSource�� 1�� ����մϴ�.
    /// </summary>
    public void PlayOneShot(GameObject target)
    {
        if (target == null) return;

        AudioSource source = target.GetComponent<AudioSource>();
        if (source != null)
        {
            source.PlayOneShot(source.clip);
        }
        else
        {
            Debug.LogWarning($"{target.name}�� AudioSource�� �����ϴ�!");
        }
    }
}
