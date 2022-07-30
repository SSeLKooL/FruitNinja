using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fruit : MonoBehaviour
{
    [SerializeField] private float sliceRange;

    [SerializeField] private Sprite[] fruits;

    protected Sprite CurrentSprite;
    
    public Camera currentCamera;
    public Spawner spawner;
    public PlayerConfiguration playerConfiguration;

    private GameObject _fruit;
    
    private int _spriteIndex;
    
    private float SpriteRangeX;
    private float SpriteRangeY;

    private float _rangeX;
    private float _rangeY;

    private bool _startedSlice;

    private const int PixelsPerUnit = 100;

    protected Vector2 FirstTapPosition;
    protected Vector2 SecondTapPosition;
    private Vector2 _fruitStartCenter;

    protected virtual void SetSprite()
    {
        _spriteIndex = Random.Range(0, fruits.Length);
        CurrentSprite = fruits[_spriteIndex];
        _fruit.GetComponent<SpriteRenderer>().sprite = CurrentSprite;
    }

    private void Start()
    {
        _fruit = this.gameObject;
        SetSprite();
        SpriteRangeY = (CurrentSprite.rect.height) / (2 * PixelsPerUnit);
        SpriteRangeX = (CurrentSprite.rect.width) / (2 * PixelsPerUnit);
    }

    private void SetRange()
    {
        _rangeX = gameObject.transform.localScale.x * SpriteRangeX;
        _rangeY = gameObject.transform.localScale.y * SpriteRangeY;
    }

    private bool CheckCollider(Vector2 currentPosition)
    {
        SetRange();
        
        return Math.Pow(_fruit.transform.position.x - currentPosition.x, 2) / (_rangeX * _rangeX) +
            Math.Pow(_fruit.transform.position.y - currentPosition.y, 2) / (_rangeY * _rangeY) <= 1;
    }

    protected virtual void CutBehavior()
    {
        spawner.ExecuteFruit(gameObject, CurrentSprite, _spriteIndex, FirstTapPosition, SecondTapPosition, _rangeX, _rangeY);
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
                    FirstTapPosition = currentCamera.ScreenToWorldPoint(touch.position);
                    
                    if (CheckCollider(FirstTapPosition))
                    {
                        _fruitStartCenter = gameObject.transform.position;
                        _startedSlice = true;
                    }
                }
            }
            else if (Input.GetMouseButton(0))
            {
                FirstTapPosition = currentCamera.ScreenToWorldPoint(Input.mousePosition);
                
                if (CheckCollider(FirstTapPosition))
                {
                    _fruitStartCenter = gameObject.transform.position;
                    _startedSlice = true;
                }
            }
        }
        else
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.touches[0];
            
                SecondTapPosition = currentCamera.ScreenToWorldPoint(touch.position);
                
                FirstTapPosition += _fruitStartCenter - (Vector2) gameObject.transform.position;
                _fruitStartCenter = gameObject.transform.position;
                
                if (Vector2.Distance(SecondTapPosition, FirstTapPosition) > sliceRange)
                {
                    CutBehavior();
                }
            }
            else if (Input.GetMouseButton(0))
            {
                SecondTapPosition = currentCamera.ScreenToWorldPoint(Input.mousePosition);
                
                FirstTapPosition += _fruitStartCenter - (Vector2) gameObject.transform.position;
                _fruitStartCenter = gameObject.transform.position;
                
                if (Vector2.Distance(SecondTapPosition, FirstTapPosition) > sliceRange)
                {
                    CutBehavior();
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
        if (!playerConfiguration.CheckGameStatus())
        {
            CheckTouch();
        }
    }
}
