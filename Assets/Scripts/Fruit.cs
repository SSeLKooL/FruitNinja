<<<<<<< Updated upstream
using System.Collections;
=======
using System;
>>>>>>> Stashed changes
using UnityEngine;

public class Fruit : MonoBehaviour
{
    private GameObject _fruit;
    [SerializeField] private float gravitation;
<<<<<<< Updated upstream
    [SerializeField] private float lifeTime;
    public Vector3 direction = new Vector3(0, 0, 0);
    private const int FramesPerSecond = 60;
    private void Start()
    {
        _fruit = this.gameObject;
        gravitation /= FramesPerSecond;
        StartCoroutine(Execution(lifeTime));
    }

    private void CheckCollider()
    {
        
=======
    [SerializeField] private float maxSizeIncrease;
    [SerializeField] private float minSizeIncrease;
    [SerializeField] private float spriteSize;
    [SerializeField] private int scoreForExecution;

    public Camera currentCamera;
    public PlayerConfiguration playerConfiguration;
    private float _range;

    public Quaternion angleIncreaseValue = Quaternion.identity;
    public Vector3 direction = new Vector3(0, 0, 0);
    
    private GameObject _fruit;
    private Quaternion _angleCurrentValue = Quaternion.identity;
    private Vector3 _currentSizeIncrease;

    private float _startPositionY;

    private void Start()
    {
        _fruit = this.gameObject;
        _startPositionY = _fruit.transform.position.y;
        _currentSizeIncrease.z = Random.Range(minSizeIncrease, maxSizeIncrease);
    }

    private void CheckCollider(Vector2 currentPosition)
    {
        _range = _fruit.transform.localScale.x * spriteSize;

        if (Math.Pow(_fruit.transform.position.x - currentPosition.x, 2) +
            Math.Pow(_fruit.transform.position.y - currentPosition.y, 2) <=
            Math.Pow(_range, 2))
        {
            ForcedExecution();
        }
    }

    private void CheckPosition()
    {
        if (_fruit.transform.position.y < _startPositionY)
        {
            Execution();
        }
    }

    private void ForcedExecution()
    {
        playerConfiguration.AddScorePoints(scoreForExecution);
        Destroy(_fruit);
>>>>>>> Stashed changes
    }

    private void Execution()
    {
        playerConfiguration.HitPlayer();
        Destroy(_fruit);
    }
    

    private void Move()
    {
<<<<<<< Updated upstream
        _fruit.transform.position += direction;

        direction.y -= gravitation;
    }
=======
        _fruit.transform.position += direction * Time.deltaTime;
        direction.y -= gravitation * Time.deltaTime;
        
        _angleCurrentValue.eulerAngles += angleIncreaseValue.eulerAngles * Time.deltaTime;
        _fruit.transform.rotation = _angleCurrentValue;

        _fruit.transform.localScale += _currentSizeIncrease * Time.deltaTime;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.touches[0];

            if (touch.phase == TouchPhase.Moved)
            {
                CheckCollider(currentCamera.ScreenToWorldPoint(touch.position));
            }
        }
        
        CheckPosition();
>>>>>>> Stashed changes

        Move();
    }
}
