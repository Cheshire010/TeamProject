using UnityEngine;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    public GameObject cutsceneCamera1;  // �� �� ī�޶� (������ ����)
    public GameObject cutsceneCamera2;  // ���� ī�޶�
    public GameObject mainCamera;       // ���� ī�޶�
    public GameObject playerControlScript;
    public GameObject aim;

    void Start()
    {
        StartCoroutine(PlayCutscenes());
    }

    IEnumerator PlayCutscenes()
    {
        // �÷��̾��� ���� ��Ȱ��ȭ
        playerControlScript.SetActive(false);
        mainCamera.SetActive(false);  // ���� ī�޶� ��Ȱ��ȭ
        aim.SetActive(false);

        //  [0] �� �� ī�޶� (������ ����)
        cutsceneCamera1.SetActive(true);
        Debug.Log("��� �ƽ� ����");

        yield return new WaitForSeconds(5f);  // 5�ʵ��� ��� ������

        cutsceneCamera1.SetActive(false);
        Debug.Log("��� �ƽ� ����");

        //  [1] ���� ī�޶� (CutSenceCameraMove ����)
        GameObject movingCam = cutsceneCamera2;
        CutSenceCameraMove moveScript = movingCam.GetComponent<CutSenceCameraMove>();

        movingCam.SetActive(true);
        Debug.Log("���� �ƽ� ����");

        // ���� ī�޶� ���� ������ ���
        while (moveScript != null && moveScript.isMoving)
            yield return null;

        movingCam.SetActive(false);
        Debug.Log("���� �ƽ� ����");

        yield return new WaitForSeconds(1f);  // ��ȯ ������ (����)

        // ��� ī�޶� ��Ȱ��ȭ �� ���� ī�޶� Ȱ��ȭ
        cutsceneCamera1.SetActive(false); // �� �� ī�޶� ��Ȱ��ȭ
        cutsceneCamera2.SetActive(false); // ���� ī�޶� ��Ȱ��ȭ

        // ���� ī�޶� Ȱ��ȭ
        mainCamera.SetActive(true);
        // ī�޶��� ��ġ�� ȸ������ �ٽ� �����Ͽ� ���ϴ� ��ġ�� �ʱ�ȭ

        // �÷��̾� ���� Ȱ��ȭ
        playerControlScript.SetActive(true);
        aim.SetActive(true);
        Debug.Log("�ƽ� ����, �÷��̾� ���� ����!");
    }
}
