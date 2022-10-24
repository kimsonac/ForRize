using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public UnityEvent OnPress = null;
    public AudioClip buttonSound;

    private AudioSource audioSource;
    private Vector3 originPositon;
    private bool isPressed = false;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        originPositon = transform.position;
    }

    public void ButtonPressed()
    {
        transform.position = originPositon - new Vector3(0, 0.003f, 0);
        audioSource.PlayOneShot(buttonSound);

        if (!isPressed)
        {
            isPressed = true;
            OnPress.Invoke();
        }
    }

    public void ButtonExit()
    {
        transform.position = originPositon;
    }
}
