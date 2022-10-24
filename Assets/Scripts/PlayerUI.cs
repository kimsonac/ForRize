using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Text enemyText;
    public Text lifeText;

    private void OnEnable()
    {
        if (GameManager.Instance == null)
        {
            gameObject.SetActive(false);
            return;
        }

        GameManager.Instance.OnEnemyChange += UpdateEnemy;
        GameManager.Instance.OnLifeChange += UpdateHearts;
    }

    private void OnDisable()
    {
        if (GameManager.Instance == null)
        {
            return;
        }

        GameManager.Instance.OnEnemyChange -= UpdateEnemy;
        GameManager.Instance.OnLifeChange -= UpdateHearts;
    }

    public void UpdateEnemy(int enemyCount)
    {
        enemyText.text = string.Format($"{enemyCount}/{GameManager.Instance.enemyMAX}");
    }

    public void UpdateHearts(int life)
    {
        lifeText.text = string.Format($"{life}");

        if (life > 20)
        {
            lifeText.color = Color.green;
        }
        else
        {
            lifeText.color = Color.red;
        }

        //if (0 <= life && life <= 3)
        //{
        //    for (int i = 0; i < life; i++)
        //    {
        //        hearts[i].SetActive(true);
        //    }
        //    for (int i = life; i < 3; i++)
        //    {
        //        hearts[i].SetActive(false);
        //    }
        //}
    }
}
