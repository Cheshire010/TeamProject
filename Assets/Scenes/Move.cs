using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float moveSpeed = 5f;        // �̵� �ӵ�
    public float mouseSensitivity = 100f;  // ���콺 ����

    private float xRotation = 0f;
    public Transform playerBody;  // �÷��̾� ����
    public Transform cameraTransform;  // ī�޶� Transform

    private float originalCameraY; // ī�޶� ���� ����
    public float crouchHeightMultiplier = 0.5f; // ������ �� ����
    public float crouchSmoothSpeed = 10f; // �ε巴�� �̵��ϴ� �ӵ�

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;  // ���콺 Ŀ�� ���
        if (cameraTransform != null)
            originalCameraY = cameraTransform.localPosition.y;
    }

    void Update()
    {
        move();
        LookAround();
        HandleCrouch();
    }

    void move()
    {
        float x = Input.GetAxis("Horizontal");  // A, D
        float z = Input.GetAxis("Vertical");    // W, S

        float currentSpeed = moveSpeed;

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

    void HandleCrouch()
    {
        if (cameraTransform == null) return;

        // ����Ʈ(Shift)�� ���̱�
        float targetY = Input.GetKey(KeyCode.LeftShift) ? originalCameraY * crouchHeightMultiplier : originalCameraY;
        Vector3 pos = cameraTransform.localPosition;
        pos.y = Mathf.Lerp(pos.y, targetY, Time.deltaTime * crouchSmoothSpeed);
        cameraTransform.localPosition = pos;
    }
}
