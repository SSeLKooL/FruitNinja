using System;
using System.Diagnostics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fruit : MonoBehaviour
{
    [SerializeField] private int scoreForExecution;
    
    [SerializeField] private float sliceRange;

    [SerializeField] private Sprite[] fruits;

    protected Sprite CurrentSprite;
    
    public Camera currentCamera;
    public Spawner spawner;
    public PlayerConfiguration playerConfiguration;
    public PlayerTouch playerTouch;
    
    private Transform _transform;
    
    private int _spriteIndex;
    
    private float _spriteRangeX;
    private float _spriteRangeY;

    protected float RangeX;
    protected float RangeY;

    private bool _startedSlice;

    private const int PixelsPerUnit = 100;

    protected Vector2 FirstTapPosition;
    protected Vector2 SecondTapPosition;

    protected virtual void SetSprite()
    {
        _spriteIndex = Random.Range(0, fruits.Length);
        CurrentSprite = fruits[_spriteIndex];
        gameObject.GetComponent<SpriteRenderer>().sprite = CurrentSprite;
    }

    private void Start()
    {
        _transform = gameObject.transform;
        SetSprite();
        _spriteRangeY = (CurrentSprite.rect.height) / (2 * PixelsPerUnit);
        _spriteRangeX = (CurrentSprite.rect.width) / (2 * PixelsPerUnit);
    }

    private void SetRange()
    {
        RangeX = gameObject.transform.localScale.x * _spriteRangeX;
        RangeY = gameObject.transform.localScale.y * _spriteRangeY;
    }

    private bool CheckCollider(Vector2 currentPosition)
    {
        SetRange();
        
        return Math.Pow(_transform.position.x - currentPosition.x, 2) / (RangeX * RangeX) +
            Math.Pow(_transform.position.y - currentPosition.y, 2) / (RangeY * RangeY) <= 1;
    }

    protected virtual void CutBehavior()
    {
        playerConfiguration.AddScorePoints(scoreForExecution);
        
        spawner.SpawnBlob(_spriteIndex, gameObject.transform.position);
        spawner.SpawnParticleCutEffect(_spriteIndex, gameObject.transform.position);
        spawner.SpawnText(RangeX, RangeY, gameObject.transform.position, scoreForExecution.ToString());
        spawner.ExecuteFruit(gameObject, CurrentSprite, _spriteIndex, FirstTapPosition, SecondTapPosition);
    }

    private void CheckTouch()
    {
        if (!_startedSlice)
        {
            if (playerTouch.isTouched)
            {
                FirstTapPosition = playerTouch.tapPosition;
                    
                if (CheckCollider(FirstTapPosition)) 
                { 
                    _startedSlice = true;
                }
            }
        }
        else
        {
            if (playerTouch.isTouched)
            {
                SecondTapPosition = playerTouch.tapPosition;
                    
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
        if (!playerConfiguration.stop)
        {
            CheckTouch();
        }
    }
}
