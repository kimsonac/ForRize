using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event Action<int> OnEnemyChange;
    public event Action<int> OnLifeChange;

    public GameObject gameOverUI;
    public GameObject enemyA;
    public GameObject enemyB;
    public GameObject enemyC;
    public GameObject boss;

    public int enemyOnStage1;
    public int enemyOnStage2;
    public int enemyMAX;
    public float spawnDelay;

    private GameObject playerUI;
    private GameObject rayInteractor;
    private GameObject[] enemyPrefabs;
    private GameObject[] spawnPoints;
    private PostProcessVolume turnGray;

    private string nextScene = "Stage1";
    private bool sceneLoading;
    private bool isAlive = true;
    private int spawnCount;
    private int enemyCount;
    private int life;
    private float duration = 5;
    private float smoothness = 0.02f;

    public int EnemyCount
    {
        get
        {
            return enemyCount;
        }
        set
        {
            enemyCount = value;
            OnEnemyChange(enemyCount);
        }
    }

    public int Life
    {
        get
        {
            return life;
        }
        set
        {
            life = value;
            OnLifeChange(life);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnEnemyChange += IsLevelClear;
        OnLifeChange += IsGameOver;

        playerUI = GameObject.Find("XR Origin/Camera Offset/RightHand Controller/smartwatch001_unity/Player UI");
        playerUI.SetActive(true);

        Life = 100;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1;
        sceneLoading = false;
        Instance.StopAllCoroutines();

        playerUI = GameObject.Find("XR Origin/Camera Offset/RightHand Controller/smartwatch001_unity/Player UI");
        rayInteractor = GameObject.Find("XR Origin/Camera Offset/RightHand Controller/Ray Interactor");
        spawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
        turnGray = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PostProcessVolume>();
        
        turnGray.enabled = false;
        playerUI.SetActive(true);
        Life = Life;
        spawnCount = 0;

        if (scene.name.Equals("Stage1"))
        {
            isAlive = true;
            enemyPrefabs = new GameObject[] { enemyA };
            enemyMAX = enemyOnStage1;
            EnemyCount = enemyMAX;
            nextScene = "Stage2";
        }
        else if (scene.name.Equals("Stage2"))
        {
            isAlive = true;
            enemyPrefabs = new GameObject[] { enemyB, enemyC };
            enemyMAX = enemyOnStage2;
            EnemyCount = enemyMAX;
            nextScene = "Stage3";
        }
        else if (scene.name.Equals("Stage3"))
        {
            isAlive = true;
            EnemyCount = 666;
        }
        else if (scene.name == "Main")
        {
            Life = 100;
            nextScene = "Stage1";
        }

    }

    public void LoadScene()
    {
        if (sceneLoading) return;

        sceneLoading = true;
        SceneManager.LoadScene(nextScene);
    }

    private void IsLevelClear(int enemyCount)
    {
        if (isAlive && enemyCount == 0)
        {
            LightChange(new Vector4(1, 0.7594216f, 0.4669811f, 1));
            GameObject.Find("Level Clear Routine").GetComponent<StageRoutine>().ClearRoutine();
        }
    }

    private void IsGameOver(int life)
    {
        if (isAlive && life <= 0)
        {
            Time.timeScale = 0.1f;

            isAlive = false;
            turnGray.enabled = true;
            rayInteractor.SetActive(true);

            Vector3 vHeadPos = Camera.main.transform.position;
            Vector3 vGazeDir = Camera.main.transform.forward; vGazeDir.y = 0;
            Vector3 vRot = Camera.main.transform.eulerAngles; vRot.x = 0; vRot.z = 0;

            Instantiate(gameOverUI, (vHeadPos + vGazeDir * 2), Quaternion.Euler(vRot));
            Invoke("EnemiesKill", 1f);
        }
    }

    private void EnemiesKill()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var item in enemies)
        {
            Enemy enemy = item.transform.parent.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.onDead();
            }
        }
    }

    private IEnumerator SpawnLoop()
    {
        while (isAlive && spawnCount < enemyMAX)
        {
            ++spawnCount;
            GameObject sp = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
            Instantiate(enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length)], sp.transform.position, sp.transform.rotation);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void RadioButtonPressed()
    {
        LightChange(Color.blue);

        if (SceneManager.GetActiveScene().name.Equals("Stage3"))
        {
            Instantiate(boss, new Vector3(0, -50, 1000), Quaternion.Euler(new Vector3(0, 180, 0)));
        }
        else Invoke("StartWave", 4f);
    }

    private void StartWave()
    {
        StartCoroutine(SpawnLoop());
    }

    private void LightChange(Color color)
    {
        GameObject[] lights = GameObject.FindGameObjectsWithTag("Light");
        float progress;
        float increment = smoothness / duration;

        for (int i = 0; i < lights.Length; i++)
        {
            progress = 0;
            Light light = lights[i].GetComponent<Light>();
            StartCoroutine(_LightChange(progress, increment, light, color));
        }

    }

    private IEnumerator _LightChange(float progress, float increment, Light light, Color color)
    {
        while (progress < 1)
        {
            light.color = Color.Lerp(light.color, color, progress);
            progress += increment;
            yield return new WaitForSeconds(smoothness);
        }
    }
}
