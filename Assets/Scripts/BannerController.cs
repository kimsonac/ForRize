using UnityEngine;
using UnityEngine.UI;

public class BannerController : MonoBehaviour
{
    public GameObject rayInteractor;
    public Sprite[] sprites;
    public Image imageUI;
    public GameObject button;

    private int spriteCount = 0;

    public void NextImage()
    {
        //if (spriteCount >= sprites.Length - 1)
        //{
        //}
        if (spriteCount >= sprites.Length - 2)
        {
            imageUI.sprite = sprites[++spriteCount];
            rayInteractor.SetActive(false);
            button.SetActive(false);
        }
        else
        {
            imageUI.sprite = sprites[++spriteCount];
        }
    }

    public void PrevImage()
    {
        if (spriteCount <= 0)
        {
            return;
        }
        else
        {
            --spriteCount;
            imageUI.sprite = sprites[spriteCount];
        }
    }
}