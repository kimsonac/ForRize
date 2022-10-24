using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameObject attackEffect;
    public GameObject bodyShotedEffect;
    public Transform attackPos;
    public AudioClip walkSound;
    public AudioClip attackSound;
    public AudioClip shotedSound;
    public AudioClip deadSound;

    public float attackRange;
    public float attackRate;
    public float sternTime;

    private Transform target;
    private NavMeshAgent agent;
    private Animator animator;
    private AudioSource audioSource;

    private float nextAttackTime;
    private float distance;
    private bool doUpdate = true;
    private bool dead = false;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (doUpdate && nextAttackTime < Time.time)
        {
            distance = Vector3.Distance(transform.position, target.position);

            if (distance < attackRange)
            {
                nextAttackTime = Time.time + attackRate;
                StartCoroutine(Attack());
            }
            else
            {
                agent.enabled = true;
                agent.SetDestination(target.position);
                animator.SetBool("walk", true);
                WalkSoundPlay(true);
            }
        }
    }

    private void WalkSoundPlay(bool walkPlay)
    {
        if (walkPlay && !audioSource.isPlaying)
        {
            audioSource.clip = walkSound;
            audioSource.Play();
            audioSource.loop = true;
        }
        else if (!walkPlay)
        {
            audioSource.Stop();
            audioSource.loop = false;
        }
    }

    public void Shoted(GameObject part)
    {
        audioSource.PlayOneShot(shotedSound);

        if (part.name == "Helmet")
        {
            Destroy(part);
            StopCoroutine(Attack());
            animator.SetTrigger("damaged");
        }
        else if (part.name == "ArmoredBody")
        {
            StartCoroutine(Sterned());
        }
        else if (!dead)
        {
            dead = true;
            onDead();
        }
    }

    private IEnumerator Attack()
    {
        agent.enabled = false;
        transform.LookAt(target);
        WalkSoundPlay(false);
        animator.SetBool("walk", false);
        animator.SetBool("attack", true);
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("attack", false);
        yield return new WaitForSeconds(0.25f);
        Destroy(Instantiate(attackEffect, attackPos.position, Quaternion.identity), 1f);
        audioSource.PlayOneShot(attackSound);
        GameManager.Instance.Life -= 5;
    }

    //private IEnumerator Damaged()
    //{
    //    StopCoroutine(Attack());

    //    doUpdate = false;
    //    animator.SetBool("damaged", true);
    //    yield return new WaitForSeconds(0.1f);
    //    animator.SetBool("damaged", false);
    //    doUpdate = true;
    //}

    private IEnumerator Sterned()
    {
        StopCoroutine(Attack());

        doUpdate = false;
        agent.enabled = false;
        Destroy(Instantiate(bodyShotedEffect, transform.position, Quaternion.identity), 1f);
        WalkSoundPlay(false);
        animator.SetTrigger("damaged");
        animator.SetBool("isSterned", true);
        yield return new WaitForSeconds(sternTime);
        agent.enabled = true;
        animator.SetBool("isSterned", false);
        doUpdate = true;
    }

    public void onDead()
    {
        StartCoroutine(Dead());
    }

    private IEnumerator Dead()
    {
        StopCoroutine(Attack());

        doUpdate = false;
        agent.enabled = false;

        Collider[] body = GetComponentsInChildren<Collider>();
        foreach (var item in body)
        {
            item.enabled = false;
        }

        WalkSoundPlay(false);
        animator.SetBool("dead", true);
        yield return new WaitForSeconds(0.4f);
        GameManager.Instance.EnemyCount -= 1;
        audioSource.PlayOneShot(deadSound);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
