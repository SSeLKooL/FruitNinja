using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fruit : MonoBehaviour
{
    [SerializeField] private float sliceRange;

    [SerializeField] private Sprite[] fruits;
    
    public Camera currentCamera;
    public Spawner spawner;
    public PlayerConfiguration PlayerConfiguration;

    private GameObject _fruit;
    
    private int _spriteIndex;
    
    private float spriteRangeX;
    private float spriteRangeY;

    private float _rangeX;
    private float _rangeY;

    private bool _startedSlice;

    private const int PixelsPerUnit = 100;

    private Vector2 _firstTapPosition;
    private Vector2 _secondTapPosition;

    private void Start()
    {
        _fruit = this.gameObject;
        _spriteIndex = Random.Range(0, fruits.Length);
        _fruit.GetComponent<SpriteRenderer>().sprite = fruits[_spriteIndex];
        spriteRangeY = (fruits[_spriteIndex].rect.height) / (2 * PixelsPerUnit);
        spriteRangeX = (fruits[_spriteIndex].rect.width) / (2 * PixelsPerUnit);
    }

    private void SetRange()
    {
        _rangeX = _fruit.transform.localScale.x * spriteRangeX;
        _rangeY = _fruit.transform.localScale.y * spriteRangeY;
    }

    private bool CheckCollider(Vector2 currentPosition)
    {
        SetRange();
        
        return Math.Pow(_fruit.transform.position.x - currentPosition.x, 2) / (_rangeX * _rangeX) +
            Math.Pow(_fruit.transform.position.y - currentPosition.y, 2) / (_rangeY * _rangeY) <= 1;
    }

    private void CheckTouch()
    {
        if (!_startedSlice)
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.touches[0];

                if (touch.phase == TouchPhase.Moved)
                {
                    _firstTapPosition = currentCamera.ScreenToWorldPoint(touch.position);
                    
                    if (CheckCollider(_firstTapPosition))
                    {
                        _startedSlice = true;
                    }
                }
            }
            else if (Input.GetMouseButton(0))
            {
                _firstTapPosition = currentCamera.ScreenToWorldPoint(Input.mousePosition);
                
                if (CheckCollider(_firstTapPosition))
                {
                    _startedSlice = true;
                }
            }
        }
        else
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.touches[0];
            
                _secondTapPosition = currentCamera.ScreenToWorldPoint(touch.position);
                
                if (Vector2.Distance(_secondTapPosition, _firstTapPosition) > sliceRange)
                {
                    spawner.ExecuteFruit(_fruit, fruits[_spriteIndex], _spriteIndex, _firstTapPosition, _secondTapPosition, _rangeX, _rangeY);
                }
            }
            else if (Input.GetMouseButton(0))
            {
                _secondTapPosition = currentCamera.ScreenToWorldPoint(Input.mousePosition);
                
                if (Vector2.Distance(_secondTapPosition, _firstTapPosition) > sliceRange)
                {
                    spawner.ExecuteFruit(_fruit, fruits[_spriteIndex], _spriteIndex, _firstTapPosition, _secondTapPosition, _rangeX, _rangeY);
                }
            }
            else
            {
                _startedSlice = false;
            }
        }
    }

    private void Update()
    {
        if (!PlayerConfiguration.CheckGameStatus())
        {
            CheckTouch();
        }
    }
}
