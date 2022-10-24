using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public ParticleSystem[] particles;
    ParticleSystem.MinMaxGradient originalColor;

    private void Start()
    {
        var main = particles[0].main;
        originalColor = main.startColor;
    }

    public void ActiveColorChange()
    {
        foreach (var item in particles)
        {
            var main = item.main;
            main.startColor = Color.white;
        }
    }

    public void DeactiveColorChange()
    {
        foreach (var item in particles)
        {
            var main = item.main;
            main.startColor = originalColor;
        }
    }

    // 이동 스팟 active (중간다리/마지막)
    public void SpotActive()
    {
        gameObject.SetActive(true);
    }
}
