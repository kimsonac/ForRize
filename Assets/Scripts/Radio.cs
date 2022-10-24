using System.Collections;
using UnityEngine;

public class Radio : MonoBehaviour
{
    public GameObject radioUI;
    public AudioClip radioNoise; // ���� ���� ����
    public AudioClip radioMusic; // ��׶���
    public AudioSource subAudio;

    private AudioSource audioSource;
    private float fadeTime = 3; // fade time in seconds
    private float defaultVol = 1;
    private float volTemp;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        volTemp = fadeTime;

        PlayNoise();
        PlayBackground();
    }

    public void ButtonPressed()
    {
        Destroy(radioUI, 1f);
        GameManager.Instance.RadioButtonPressed();
    }

    private void PlayNoise()
    {
        audioSource.volume = 0.6f;
        audioSource.clip = radioNoise;
        audioSource.loop = true;
        audioSource.Play();
    }
    private void PlayBackground()
    {
        subAudio.clip = radioMusic;
        subAudio.loop = true;
        subAudio.Play();

    }
    public void StopNoise() // �ڷ���Ʈ�� ���� �� ���� �Ҹ� ����
    {
        StartCoroutine(FadeSound(audioSource, 0.6f));
    }

    private IEnumerator FadeSound(AudioSource audio, float vol)
    {
        while (volTemp > 0)
        {
            yield return null;
            volTemp -= Time.deltaTime;
            audio.volume = volTemp / fadeTime - vol;
        }

        audio.Stop();
        volTemp = fadeTime;
        audio.volume = defaultVol;
        yield break;
    }
}