using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fruit : MonoBehaviour
{
    [SerializeField] private float gravitation;
    [SerializeField] private float lifeTime;
    [SerializeField] private float maxSizeIncrease;
    [SerializeField] private float minSizeIncrease;
    [SerializeField] private float spriteSize;
    private float _range;

    public Quaternion angleIncreaseValue = Quaternion.identity;
    public Vector3 direction = new Vector3(0, 0, 0);
    
    private GameObject _fruit;
    private Quaternion _angleCurrentValue = Quaternion.identity;
    private float _currentSizeIncrease;

    private const int FramesPerSecond = 60;
    private void Start()
    {
        _fruit = this.gameObject;
        gravitation /= FramesPerSecond;
        _currentSizeIncrease = Random.Range(minSizeIncrease, maxSizeIncrease) / FramesPerSecond;
        
        StartCoroutine(Execution(lifeTime));
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

    private void ForcedExecution()
    {
        Destroy(_fruit);
    }

    private IEnumerator Execution(float timeToWait)
    {
        while (timeToWait > 0)
        {
            timeToWait -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        
        Destroy(_fruit);
    }
    

    private void Move()
    {
        _fruit.transform.position += direction;
        direction.y -= gravitation;
        
        _angleCurrentValue.eulerAngles += angleIncreaseValue.eulerAngles;
        _fruit.transform.rotation = _angleCurrentValue;

        _fruit.transform.localScale += Vector3.one * _currentSizeIncrease;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.touches[0];

            if (touch.phase == TouchPhase.Moved)
            {
                CheckCollider(Camera.main.ScreenToWorldPoint(touch.position));
            }
        }
    }

    private void FixedUpdate()
    {
        Move();
    }
    
}
