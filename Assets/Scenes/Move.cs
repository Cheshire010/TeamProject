using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float moveSpeed = 5f;        // �⺻ �ӵ�
    public float sprintMultiplier = 1.5f;  // ����Ʈ �ӵ� ���
    public float mouseSensitivity = 100f;  // ���콺 ����

    private float xRotation = 0f;
    public Transform playerBody;  // �÷��̾� ����
    public Transform cameraTransform;  // ī�޶� Transform

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;  // ���콺 Ŀ�� ���
    }

    void Update()
    {
        move();
        LookAround();
    }

    void move()
    {
        float x = Input.GetAxis("Horizontal");  // A, D
        float z = Input.GetAxis("Vertical");    // W, S

        float currentSpeed = moveSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
            currentSpeed *= sprintMultiplier;  // ����Ʈ ������ �ӵ� ����

        Vector3 move = transform.right * x + transform.forward * z;
        transform.position += move * currentSpeed * Time.deltaTime;
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);  // ���� ȸ�� ����

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
