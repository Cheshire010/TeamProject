using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Transform targetPoint;  // �̵��� ��ǥ ��ġ
    public float moveSpeed = 2f;

    private bool isMoving = false;

    private void Update()
    {
        if (!isMoving || targetPoint == null) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            isMoving = false; // �����ϸ� ����
        }
    }

    public void StartMoving()
    {
        isMoving = true;
    }
}
