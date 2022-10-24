using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    public float maxRandom;
    private Light light;
    private float waitTime;

    private void Start()
    {
        light = GetComponent<Light>();
        StartCoroutine(LightRandomBlink());
    }
    private IEnumerator LightRandomBlink()
    {
        while (true)
        {
            waitTime = Random.Range(0f, maxRandom); // float�� �Ҽ������ε� ������? => f�� ������ �Ǽ��� ��ȯ
            yield return new WaitForSeconds(waitTime);
            light.enabled = false;
            waitTime = Random.Range(0f, maxRandom);
            yield return new WaitForSeconds(waitTime);
            light.enabled = true;
        }
    }
}
