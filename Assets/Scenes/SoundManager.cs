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
    /// 지정된 오브젝트의 AudioSource를 1번 재생합니다.
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
            Debug.LogWarning($"{target.name}에 AudioSource가 없습니다!");
        }
    }
}
