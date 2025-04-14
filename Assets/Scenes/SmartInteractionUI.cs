using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SmartInteractionUI : MonoBehaviour
{
    public Transform player;             // �÷��̾� Ʈ������
    public float interactionRange = 3f;  // ��ȣ�ۿ� �Ÿ�
    public float viewAngle = 45f;        // �þ߰�

    public GameObject interactionUI;     // ��ȣ�ۿ� UI ������Ʈ (Text or Panel)

    public GameObject camera1;           // ���� ���� ī�޶�
    public GameObject camera2;           // �ε��� ī�޶�

    public GameObject carMoveObject;     // ������ �ڵ��� ������Ʈ
    public float carMoveSpeed = 2f;      // CarMove �̵��ӵ�

    private Vector3 carOriginalPos;      // �ڵ��� ���� ��ġ ����
    private bool isCarMoving = false;    // CarMove �̵� �� ����

    private void Start()
    {
        if (interactionUI != null)
            interactionUI.SetActive(false);

        if (camera1 != null) camera1.SetActive(true);
        if (camera2 != null) camera2.SetActive(false);

        if (carMoveObject != null)
            carOriginalPos = carMoveObject.transform.position;
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactionRange)
        {
            Vector3 dirToTarget = (transform.position - player.position).normalized;
            float angle = Vector3.Angle(player.forward, dirToTarget);

            if (angle <= viewAngle)
            {
                if (interactionUI != null)
                    interactionUI.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("EŰ ����! ��ȣ�ۿ� ����.");
                    StartCoroutine(EnterSituation());  // �ڷ�ƾ ����
                }
                return;
            }
        }

        if (interactionUI != null)
            interactionUI.SetActive(false);

        // CarMove�� �̵� ���� �� X������ �̵�
        if (isCarMoving && carMoveObject != null)
        {
            carMoveObject.transform.Translate(Vector3.left * carMoveSpeed * Time.deltaTime);
        }
    }

    IEnumerator EnterSituation()
    {
        if (interactionUI != null)
            interactionUI.SetActive(false);

        // �÷��̾� ��Ȱ��ȭ + ��ġ Z �� -2 �̵�
        player.gameObject.SetActive(false);
        Vector3 newPosition = player.position;
        newPosition.z -= 2f;
        player.position = newPosition;

        // ī�޶� ��ȯ
        if (camera1 != null) camera1.SetActive(false);
        if (camera2 != null) camera2.SetActive(true);

        // CarMove �̵� ����
        isCarMoving = true;

        // 4�� ���
        yield return new WaitForSeconds(4f);

        // CarMove ���� + ��ġ ����
        isCarMoving = false;
        if (carMoveObject != null)
            carMoveObject.transform.position = carOriginalPos;

        // ī�޶� ���󺹱�
        if (camera1 != null) camera1.SetActive(true);
        if (camera2 != null) camera2.SetActive(false);

        // �÷��̾� �ٽ� Ȱ��ȭ
        player.gameObject.SetActive(true);

        Debug.Log("�ε� ī�޶� ����. �÷��̾� ���� �簳!");
    }
}
