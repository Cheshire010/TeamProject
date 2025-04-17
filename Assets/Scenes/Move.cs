using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float moveSpeed = 5f;        // 이동 속도
    public float mouseSensitivity = 100f;  // 마우스 감도

    private float xRotation = 0f;
    public Transform playerBody;  // 플레이어 몸통
    public Transform cameraTransform;  // 카메라 Transform

    private float originalCameraY; // 카메라 원래 높이
    public float crouchHeightMultiplier = 0.5f; // 숙였을 때 비율
    public float crouchSmoothSpeed = 10f; // 부드럽게 이동하는 속도

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;  // 마우스 커서 잠금
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
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);  // 상하 회전 제한

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    void HandleCrouch()
    {
        if (cameraTransform == null) return;

        // 쉬프트(Shift)로 숙이기
        float targetY = Input.GetKey(KeyCode.LeftShift) ? originalCameraY * crouchHeightMultiplier : originalCameraY;
        Vector3 pos = cameraTransform.localPosition;
        pos.y = Mathf.Lerp(pos.y, targetY, Time.deltaTime * crouchSmoothSpeed);
        cameraTransform.localPosition = pos;
    }
}
