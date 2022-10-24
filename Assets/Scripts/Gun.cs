using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR;

public class Gun : MonoBehaviour
{
    public Transform pivot;
    public Transform muzzle;
    public GameObject BulletImpact;
    public GameObject BulletImpactWHole;
    public AudioSource audioSource;
    public AudioClip pistolShot;
    public AudioClip pistolOutOfAmmo;
    public AudioClip pistolReloading;
    public Animator glockAnimator;
    public Text ammoText;
    public UnityEvent reloadEvent;
    public UnityEvent fireEvent;

    public int bulletCount;

    private Vector3 originScale;
    private Vector3 RightControllerVelocity;
    private InputDevice RightControllerDevice;
    private Animator muzzleAnimator;

    private bool isReloading = false;

    private void Start()
    {
        originScale = transform.localScale;
        RightControllerDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    private void FixedUpdate()
    {
        muzzleAnimator = muzzle.GetComponent<Animator>();
        RightControllerDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out RightControllerVelocity);
        if (RightControllerVelocity.y < -1.5f && bulletCount < 15)
        {
            StartCoroutine(Reload());
            reloadEvent.Invoke();
        }

    }

    public void Fire()
    {
        if (!isReloading)
        {
            if (bulletCount <= 0)
            {
                audioSource.PlayOneShot(pistolOutOfAmmo);
                return;
            }

            if (Physics.Raycast(muzzle.position, muzzle.TransformDirection(Vector3.forward), out RaycastHit hit))
            {
                if (hit.transform.tag == "Enemy")
                {
                    Enemy enemy = hit.transform.GetComponentInParent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.Shoted(hit.transform.gameObject);
                    }
                    EnemyBoss boss = hit.transform.GetComponentInParent<EnemyBoss>();
                    if (boss != null)
                    {
                        boss.Shoted(hit.point); // 보스가 맞은 부위 (충돌위치) 에 이펙트 주고 싶어서
                    }
                    EnemyDummy dummmy = hit.transform.GetComponentInParent<EnemyDummy>();
                    if (dummmy != null)
                    {
                        dummmy.Shoted(hit.transform.gameObject);
                    }
                }

                if (hit.transform.tag != "Player")
                {
                    if (hit.transform.tag == "Enemy")
                    {
                        Instantiate(BulletImpact, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                    }
                    else
                    {
                        Instantiate(BulletImpactWHole, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                    }
                }

                //Debug.DrawRay(barrel.position, barrel.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                //Debug.Log($"Did Hit: {hit.collider.gameObject.name}");
            }
            //else
            //{
            //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            //    Debug.Log("Did not Hit");
            //}

            bulletCount -= 1;
            ammoText.text = string.Format($"{bulletCount}");
            audioSource.PlayOneShot(pistolShot);
            muzzleAnimator.SetTrigger("shoot");
            fireEvent.Invoke();

            if (bulletCount <= 0)
            {
                glockAnimator.SetBool("empty", true);
            }
            else
            {
                glockAnimator.SetTrigger("shoot");
            }
        }
    }

    public void Magnet()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        transform.position = pivot.position;
        transform.rotation = pivot.rotation;
        transform.localScale = originScale;
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        bulletCount = 15;
        ammoText.text = "15";
        audioSource.PlayOneShot(pistolReloading);
        glockAnimator.SetBool("empty", false);
        glockAnimator.SetTrigger("reload");
        yield return new WaitForSeconds(1.2f);
        isReloading = false;
    }
}
