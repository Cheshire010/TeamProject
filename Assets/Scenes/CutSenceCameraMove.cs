using UnityEngine;

public class CutSenceCameraMove : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveDuration = 2f;

    private int currentIndex = 0;
    private float elapsedTime = 0f;
    public bool isMoving = true;

    void Start()
    {
        if (waypoints.Length < 2)
        {
            Debug.LogError("��� ����Ʈ�� 2�� �̻� �ʿ��մϴ�!");
            isMoving = false;
            return;
        }

        transform.position = waypoints[0].position;
        transform.rotation = waypoints[0].rotation;
    }

    void Update()
    {
        if (!isMoving) return;
        if (currentIndex >= waypoints.Length - 1)
        {
            isMoving = false;
            Debug.Log("ī�޶� ��� ���� �Ϸ�!");
            return;
        }

        Transform start = waypoints[currentIndex];
        Transform end = waypoints[currentIndex + 1];

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / moveDuration);

        transform.position = Vector3.Lerp(start.position, end.position, t);
        transform.rotation = Quaternion.Lerp(start.rotation, end.rotation, t);

        if (t >= 1f)
        {
            currentIndex++;
            elapsedTime = 0f;
        }
    }
}
