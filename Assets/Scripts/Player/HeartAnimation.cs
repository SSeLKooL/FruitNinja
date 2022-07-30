using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HeartAnimation : MonoBehaviour
{
    [SerializeField] private float standardMaxIncreaseScaleX;
    [SerializeField] private float dangerMaxIncreaseScaleX;
    [SerializeField] private float standardWaitTime;
    [SerializeField] private float dangerWaitTime;
    [SerializeField] private Vector3 standardScaleQuotient;
    [SerializeField] private Vector3 dangerScaleQuotient;

    public HeartAnimation previousHeartAnimation;
    
    private float _currentTime;
    private Vector3 _currentScale;
    private Vector3 _startScale;
    private float _maxScaleX;

    private float _currentWaitTime;
    private Vector3 _currentScaleQuotient;

    private bool _isScaleIncrease;
    
    public bool _isStartAnimation = true;
    public bool isFirst;
    public bool isWaiting;
    public bool isDead;

    private void Start()
    {
        _startScale = gameObject.transform.localScale;
        _currentScale = Vector3.zero;
        gameObject.transform.localScale = Vector3.zero;
        
        _maxScaleX = _startScale.x;

        _isScaleIncrease = true;

        _currentWaitTime = standardWaitTime;
        _currentTime = _currentWaitTime;
        
        _currentScaleQuotient = standardScaleQuotient;
    }

    public void DecreaseHeartBeat()
    {
        _isScaleIncrease = false;
        _currentWaitTime = standardWaitTime;
        _currentScaleQuotient = standardScaleQuotient;
        _maxScaleX = standardMaxIncreaseScaleX * _startScale.x;
    }

    public void IncreaseHeartBeat()
    {
        _isScaleIncrease = true;
        _currentWaitTime = dangerWaitTime;
        _currentScaleQuotient = dangerScaleQuotient;
        _maxScaleX = dangerMaxIncreaseScaleX * _startScale.x;
    }

    private void ChangeScale()
    {
        if (_currentWaitTime <= _currentTime)
        {
            isWaiting = false;
            
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
                    isWaiting = true;
                }
            }
        }
        else
        {
            _currentTime += Time.deltaTime;
        }
    }

    private void Update()
    {
        if (_isStartAnimation)
        {
            if (!_isScaleIncrease)
            {
                if ((previousHeartAnimation.isWaiting && !previousHeartAnimation._isStartAnimation) || isFirst)
                {
                    _isStartAnimation = false;
                    _maxScaleX = standardMaxIncreaseScaleX * _startScale.x;
                    _isScaleIncrease = true;
                    isWaiting = true;
                    _currentTime = previousHeartAnimation._currentTime;
                }
            }
            else
            {
                ChangeScale();
            }
        }
        else if (isDead)
        {
            if (_currentScale.x != 0)
            {
                _isScaleIncrease = false;
                _startScale.x = 0;
                _currentTime = _currentWaitTime;
            }
            else
            {
                Destroy(gameObject);
            }
            
            ChangeScale();
        }
        else
        {
            ChangeScale();
        }
    }
}
