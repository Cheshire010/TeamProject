using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CutsceneManager : MonoBehaviour
{
    public GameObject[] cutsceneCameras;  // [0] : �� �� ī�޶�, [1] : ���� ī�޶�
    public GameObject mainCamera;
    public GameObject playerControlScript;

    void Start()
    {
        StartCoroutine(PlayCutscenes());
    }

    IEnumerator PlayCutscenes()
    {
        playerControlScript.SetActive(false);
        mainCamera.SetActive(false);

        //  [0] �� �� ī�޶� (������ ����)
        cutsceneCameras[0].SetActive(true);
        Debug.Log("��� �ƽ� ����");

        yield return new WaitForSeconds(5f);  // 5�ʵ��� ��� ������

        cutsceneCameras[0].SetActive(false);
        Debug.Log("��� �ƽ� ����");

        //  [1] ���� ī�޶� (CutSenceCameraMove ����)
        GameObject movingCam = cutsceneCameras[1];
        CutSenceCameraMove moveScript = movingCam.GetComponent<CutSenceCameraMove>();

        movingCam.SetActive(true);
        Debug.Log("���� �ƽ� ����");

        while (moveScript != null && moveScript.isMoving)
            yield return null;

        movingCam.SetActive(false);
        Debug.Log("���� �ƽ� ����");

        yield return new WaitForSeconds(1f);  // ��ȯ ������ (����)

        //  [2] ���� ī�޶�
        mainCamera.SetActive(true);
        playerControlScript.SetActive(true);
        Debug.Log("�ƽ� ����, �÷��̾� ���� ����!");
    }
}
