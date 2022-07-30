using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class AddedScore : MonoBehaviour
{
    [SerializeField] private float alphaQuotient;
    [SerializeField] private float targetIncreaseY;
    [SerializeField] private float lifeTime;
    
    private Transform _addedScoreTransform;
    private TextMeshProUGUI _textMeshProUGUI;

    private Vector3 _addedScorePosition;

    private float _posYSpeed;
    private float _targetPosY;

    private Color _currentColor;

    private bool _isIncrease = true;
    private bool _isDestroy;
    
    private void Start()
    {
        _addedScoreTransform = gameObject.transform;
        _textMeshProUGUI = gameObject.GetComponent<TextMeshProUGUI>();

        _currentColor = _textMeshProUGUI.color;
        _currentColor.a = 0;
        _textMeshProUGUI.color = _currentColor;
        
        _addedScorePosition = _addedScoreTransform.position;
        _targetPosY = _addedScorePosition.y + targetIncreaseY;
        _posYSpeed = 1 / lifeTime;
    }

    private void Animation()
    {
        _addedScorePosition.y += _posYSpeed * targetIncreaseY * Time.deltaTime;
        _addedScoreTransform.position = _addedScorePosition;
        
        if (_addedScorePosition.y >= _targetPosY)
        {
            _isDestroy = true;
        }

        if (_isIncrease)
        {
            _currentColor.a += alphaQuotient * Time.deltaTime;
            
            if (_currentColor.a < 1)
            {
                _textMeshProUGUI.color = _currentColor;
            }
            else
            {
                _isIncrease = false;
            }
        }
        else
        {
            _currentColor.a -= alphaQuotient * Time.deltaTime;
            
            if (_currentColor.a > 0)
            {
                _textMeshProUGUI.color = _currentColor;
            }
            else
            {
                _isIncrease = true;
                if (_isDestroy)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
    
    private void Update()
    {
        Animation();
    }
}
