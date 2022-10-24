using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SliderToAnimator2D : MonoBehaviour
{
    public Slider _xSlider;

    public Slider _ySlider;

    public Animator _animator;

    public string _xVarName;
    public string _yVarName;

    private void Update()
    {
        _animator.SetFloat(_xVarName, _xSlider.value);
        _animator.SetFloat(_yVarName, _ySlider.value);
    }
}
