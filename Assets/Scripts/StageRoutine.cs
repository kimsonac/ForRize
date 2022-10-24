using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageRoutine : MonoBehaviour
{
    public GameObject rayInteractor;
    public GameObject teleportPoint;
    public GameObject pointLight;
    public GameObject endingUI;
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip elevatorOpen; // ���������� ������ �Ҹ�
    public AudioClip elevatorMoving;
    public AudioClip stairSound;
    public AudioClip clearSound;

    public void ClearRoutine()
    {
        

        if (teleportPoint)
        {
            audioSource.PlayOneShot(clearSound);
            teleportPoint.SetActive(true);
        }

        //audioSource.PlayOneShot(clearSound);

        if (SceneManager.GetActiveScene().name == "Stage2")
        {
            pointLight.SetActive(true);
        }
        else if (SceneManager.GetActiveScene().name == "Stage3")
        {
            audioSource.Stop();
            audioSource.PlayOneShot(clearSound);

            rayInteractor.SetActive(true);

            Vector3 vHeadPos = Camera.main.transform.position;
            Vector3 vGazeDir = Camera.main.transform.forward; vGazeDir.y = 0;
            Vector3 vRot = Camera.main.transform.eulerAngles; vRot.x = 0; vRot.z = 0;

            Instantiate(endingUI, (vHeadPos + vGazeDir * 3), Quaternion.Euler(vRot));
        }
    }

    public void OpenDoor()
    {
        animator.SetTrigger("open");
        audioSource.PlayOneShot(elevatorOpen);
    }

    public void LoadSceneRoutine()
    {
        StartCoroutine(_LoadSceneRoutine());
    }

    private IEnumerator _LoadSceneRoutine()
    {
        //yield return new WaitForSeconds(2f); // ���̵� ������

        if (SceneManager.GetActiveScene().name == "Stage1")
        {
            animator.SetTrigger("close");
            audioSource.PlayOneShot(elevatorMoving); // �� ��� �ð� 12��
            yield return new WaitForSeconds(elevatorMoving.length + 2f); // ī�޶� ���ȰŸ� ������ ��������
        }
        else if (SceneManager.GetActiveScene().name == "Stage2")
        {
            audioSource.PlayOneShot(stairSound); // 18��
            yield return new WaitForSeconds(stairSound.length + 2f);
        }
        GameManager.Instance.LoadScene();
    }
}
