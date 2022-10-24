using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    bool sceneLoading = false;

    public void LoadMain()
    {
        if (sceneLoading) return;

        sceneLoading = true;
        SceneManager.LoadScene("Main");
    }

    public void LoadStage1()
    {
        if (sceneLoading) return;
        
        sceneLoading = true;
        SceneManager.LoadScene("Stage1");
    }

    public void LoadStage2()
    {
        if (sceneLoading) return;

        sceneLoading = true;
        SceneManager.LoadScene("Stage2");
    }

    public void LoadStage3()
    {
        if (sceneLoading) return;

        sceneLoading = true;
        SceneManager.LoadScene("Stage3");
    }

    public void LoadThisScene()
    {
        if (sceneLoading) return;

        GameManager.Instance.Life = 100;

        sceneLoading = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
