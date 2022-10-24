using UnityEngine;

public class Door : MonoBehaviour
{
    public MeshRenderer MeshRenderer;

    public void OnMesh()
    {
        MeshRenderer.enabled = true;
    }

    public void OffMesh()
    {
        MeshRenderer.enabled = false;
    }

    public void LoadScene()
    {
        GameManager.Instance.LoadScene();
    }
}
