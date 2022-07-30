using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HeardAnimation : MonoBehaviour
{
    [SerializeField] private float standardMaxIncreaseScaleX;
    [SerializeField] private float dangerMaxIncreaseScaleX;
    [SerializeField] private float standardWaitTime;
    [SerializeField] private float dangerWaitTime;
    [SerializeField] private Vector3 standardScaleQuotient;
    [SerializeField] private Vector3 dangerScaleQuotient;

    private float _currentTime;
    private Vector3 _currentScale;
    private Vector3 _startScale;
    private float _maxScaleX;
    
    private float _currentWaitTime;
    private Vector3 _currentScaleQuotient;

    private bool _isScaleIncrease;

    private void Start()
    {
        _startScale = gameObject.transform.localScale;
        _currentScale = _startScale;
        _maxScaleX = standardMaxIncreaseScaleX * _startScale.x;
        _isScaleIncrease = true;

        _currentWaitTime = standardWaitTime;
        _currentScaleQuotient = standardScaleQuotient;
    }

    public void IncreaseHeardBeat()
    {
        _currentWaitTime = dangerWaitTime;
        _currentScaleQuotient = dangerScaleQuotient;
        _maxScaleX = dangerMaxIncreaseScaleX * _startScale.x;
    }

    private void Update()
    {
        if (_currentWaitTime < _currentTime)
        {
            if (_isScaleIncrease)
            {
                _currentScale += _currentScaleQuotient * Time.deltaTime;
                
                if (_currentScale.x < _maxScaleX)
                {
                    gameObject.transform.localScale = _currentScale;
                }
                else
                {
                    _isScaleIncrease = false;
                }
            }
            else
            {
                _currentScale -= _currentScaleQuotient * Time.deltaTime;
                
                if (_currentScale.x > _startScale.x)
                {
                    gameObject.transform.localScale = _currentScale;
                }
                else
                {
                    gameObject.transform.localScale = _startScale;
                    _currentScale = _startScale;
                    _isScaleIncrease = true;
                    _currentTime = 0;
                }
            }
        }
        else
        {
            _currentTime += Time.deltaTime;
        }
    }
}
