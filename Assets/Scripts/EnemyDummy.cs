using System.Collections;
using UnityEngine;

public class EnemyDummy : MonoBehaviour
{
    public GameObject bodyShotedEffect;
    public AudioClip shotedSound;
    public AudioClip deadSound;

    public float sternTime;
    private Animator animator;
    private AudioSource audioSource;

    private bool bodyShoted = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Shoted(GameObject part)
    {
        audioSource.PlayOneShot(shotedSound);

        if (part.name == "Helmet" && bodyShoted)
        {
            Destroy(part);
            StartCoroutine(Damaged());
        }
        else if (part.name == "Helmet")
        {
            StartCoroutine(Sterned());
        }

        else if (part.name == "ArmoredBody")
        {
            StartCoroutine(Sterned());
            bodyShoted = true;
        }
        else
        {
            onDead();
        }
    }

    private IEnumerator Damaged()
    {
        animator.SetBool("damaged", true);
        animator.SetBool("isSterned", false);
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("damaged", false);
        animator.SetBool("isSterned", true);
    }

    private IEnumerator Sterned()
    {
        Destroy(Instantiate(bodyShotedEffect, transform.position, Quaternion.identity), 1f);
        animator.SetBool("damaged", true);
        animator.SetBool("isSterned", false);
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("damaged", false);
        animator.SetBool("isSterned", true);
    }

    public void onDead()
    {
        StartCoroutine(Dead());
    }

    private IEnumerator Dead()
    {
        animator.SetBool("dead", true);
        yield return new WaitForSeconds(0.4f);
        audioSource.PlayOneShot(deadSound);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
