using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyBoss : MonoBehaviour
{
    private Vector3 playerPosition;
    private Vector3 targetPosition;

    public float duration;
    public float outRange;
    public float innerRange;

    public GameObject shotedEffect;
    public GameObject spawnEffect;
    public GameObject spellEffect;
    public GameObject[] enemyPrefabs;
    public Transform spellEffectPosition;
    public Image healthImage;

    public int maxHealth;

    public int damage;
    public int pageStartsAt;
    private int bossHealth;
    public int waveMAX;
    public float spellRate;
    public float pageRate;

    private Animator animator;

    private float nextSpawnTime;
    private bool doUpdate = true;
    private bool dead = false;

    private void Start()
    {
        bossHealth = maxHealth;

        playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        targetPosition = new Vector3(playerPosition.x, transform.position.y, playerPosition.z + 100f);
        animator = GetComponent<Animator>();
        animator.speed = 0.5f;
        animator.SetBool("walk", true);
        spellEffect.SetActive(false);
        StartCoroutine(LerpPosition(targetPosition, duration));
        nextSpawnTime = Time.time + 5f;
    }

    private void Update()
    {
        if (doUpdate && nextSpawnTime < Time.time)
        {
            nextSpawnTime = Time.time + spellRate;
            StartCoroutine(Spell());
        }

        if (Input.GetKeyDown(KeyCode.T))
            Shoted(transform.Find("Body").position);
        else if (Input.GetKeyDown(KeyCode.Q))
            bossHealth = 0;
    }

    public void Shoted(Vector3 hitPos)
    {
        bossHealth -= damage;

        if (!dead && bossHealth <= 0)
        {
            doUpdate = false;
            StartCoroutine(Dead());
        }
        else if (bossHealth < pageStartsAt)
        {
            spellRate = pageRate;
        }
        healthImage.fillAmount = (float)bossHealth / maxHealth;
        GameObject gameObject = Instantiate(shotedEffect, hitPos, Quaternion.identity);
        gameObject.transform.localScale = gameObject.transform.localScale * 0.5f;
        Destroy(gameObject, 0.3f);
    }

    private IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        doUpdate = false;
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        animator.SetBool("walk", false);
        animator.SetTrigger("attack");
        yield return new WaitForSeconds(1f);
        GameManager.Instance.Life -= 999;
    }

    private IEnumerator Spell()
    {
        //Destroy(Instantiate(spawnEffect, spellEffectPosition.position, spellEffect.transform.rotation.normalized), 5f);
        animator.SetBool("spell", true);
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("spell", false);
        yield return new WaitForSeconds(3f);
        SpawnRandomPoints();
    }

    private void SpawnRandomPoints()
    {
        int spawnCount = 0;
        while (spawnCount < waveMAX)
        {
            Vector3 randomPoint = playerPosition + Random.insideUnitSphere * outRange;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas) && Vector3.Distance(randomPoint, playerPosition) < innerRange)
            {
                ++spawnCount;
                Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], hit.position, Quaternion.LookRotation(playerPosition));
                Destroy(Instantiate(spawnEffect, hit.position, Quaternion.identity), 1f);
            }
        }
    }

    private IEnumerator Dead()
    {
        animator.SetTrigger("dead");
        healthImage.transform.parent.gameObject.SetActive(false);

        //CameraShake.Shake(2f, 2f);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var item in enemies)
        {
            Enemy enemy = item.transform.parent.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.onDead();
            }
        }

        yield return new WaitForSeconds(3f);

        GameManager.Instance.EnemyCount = 0;
        Destroy(gameObject);
    }
}
