using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fruit : MonoBehaviour
{
    [SerializeField] private float spriteSize;

    [SerializeField] private float sliceRange;
    
    [SerializeField] private Sprite[] fruits;
    
    public Camera currentCamera;
    public Spawner spawner;
    public PlayerConfiguration PlayerConfiguration;

    private GameObject _fruit;
    
    private int _spriteIndex;

    private float _range;

    private bool _startedSlice;

    private const int PixelsPerUnit = 100;

    private Vector2 _firstTapPosition;
    private Vector2 _secondTapPosition;

    private void Start()
    {
        _fruit = this.gameObject;
        _spriteIndex = Random.Range(0, fruits.Length);
        _fruit.GetComponent<SpriteRenderer>().sprite = fruits[_spriteIndex];
        spriteSize = (fruits[_spriteIndex].rect.height + fruits[_spriteIndex].rect.width) / (4 * PixelsPerUnit);
    }

    private bool CheckCollider(Vector2 currentPosition)
    {
        _range = _fruit.transform.localScale.x * spriteSize;

        return Math.Pow(_fruit.transform.position.x - currentPosition.x, 2) +
            Math.Pow(_fruit.transform.position.y - currentPosition.y, 2) <= Math.Pow(_range, 2);
    }

    private void CheckTouch()
    {
        if (!_startedSlice)
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.touches[0];
                Debug.Log("touch");
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
                    spawner.ExecuteFruit(_fruit, fruits[_spriteIndex], _spriteIndex, _firstTapPosition, _secondTapPosition);
                }
            }
            else if (Input.GetMouseButton(0))
            {
                _secondTapPosition = currentCamera.ScreenToWorldPoint(Input.mousePosition);
                
                if (Vector2.Distance(_secondTapPosition, _firstTapPosition) > sliceRange)
                {
                    spawner.ExecuteFruit(_fruit, fruits[_spriteIndex], _spriteIndex, _firstTapPosition, _secondTapPosition);
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
