using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CutSenceCamera : MonoBehaviour
{
    public GameObject cutsceneCamera;
    public GameObject mainCamera;
    public GameObject playerControlScript;
    public CutSenceCameraMove moveScript;  // ���� ��ũ��Ʈ ����

    void Start()
    {
        cutsceneCamera.SetActive(true);
        mainCamera.SetActive(false);
        playerControlScript.SetActive(false);

        StartCoroutine(CheckCutsceneEnd());
    }

    IEnumerator CheckCutsceneEnd()
    {
        // CutSenceCameraMove�� ���� ������ ��ٸ�
        while (moveScript != null && moveScript.isMoving)
        {
            yield return null;
        }

        // ��� ������ ������ �ƽ� ����
        cutsceneCamera.SetActive(false);
        mainCamera.SetActive(true);
        playerControlScript.SetActive(true);

        Debug.Log("�ƽ� ����. �÷��̾� ���� ����!");
    }
}
