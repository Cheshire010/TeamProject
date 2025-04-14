using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float moveSpeed = 5f;        // 기본 속도
    public float sprintMultiplier = 1.5f;  // 쉬프트 속도 배수
    public float mouseSensitivity = 100f;  // 마우스 감도

    private float xRotation = 0f;
    public Transform playerBody;  // 플레이어 몸통
    public Transform cameraTransform;  // 카메라 Transform

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;  // 마우스 커서 잠금
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
            currentSpeed *= sprintMultiplier;  // 쉬프트 누르면 속도 증가

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
}
