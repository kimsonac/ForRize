using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderToAnimator : MonoBehaviour
{
    public Slider _speedSlider;

    public Slider _directionSlider;

    public Animator _animator;

    public string _varName;

    private void Update() {
        _animator.SetFloat(_varName, _speedSlider.value);
        if(_directionSlider != null)
            _animator.transform.Rotate(Vector3.up, _directionSlider.value);
    }
}
