using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private List<GameObject> hearts = new List<GameObject>();

    [SerializeField] private float distanceBetweenHeartsX;
    [SerializeField] private float distanceBetweenHeartsY;
    [SerializeField] private int heartPerLineCount;
    [SerializeField] private int maxHealth;
    [SerializeField] private GameObject heartPrefab;

    private HeartAnimation _heartAnimation;
    private HeartAnimation _previousHeartAnimation;
    
    private Vector3 _currentHeartPosition;
    private int _currentHeartIndex = -1;
    private int _currentColumnIndex = -1;

    private bool _creatingHealthBar;

    public void CreateHealthBar(int health)
    {
        _creatingHealthBar = true;
        
        _currentHeartPosition = this.gameObject.transform.position;

        for (var i = 0; i < health; i++)
        {
            CreateHeart();
        }

        _heartAnimation = hearts[0].GetComponent<HeartAnimation>();
        _heartAnimation.isFirst = true;

        _creatingHealthBar = false;
    }
    
    public void AddHealth(int health)
    {
        for (var i = 0; i < health; i++)
        {
            CreateHeart();
        }
    }

    private void CreateHeart()
    {
        if (_currentHeartIndex < maxHealth - 1)
        {
            var heart = Instantiate(heartPrefab, _currentHeartPosition, Quaternion.identity, this.gameObject.transform);

            if (_currentHeartIndex != -1)
            {
                heart.GetComponent<HeartAnimation>().previousHeartAnimation = hearts[_currentHeartIndex].GetComponent<HeartAnimation>();
            }
            else
            {
                heart.GetComponent<HeartAnimation>().previousHeartAnimation = heart.GetComponent<HeartAnimation>();
            }
            
            hearts.Add(heart);

            _currentColumnIndex++;
            _currentHeartIndex++;

            if (_currentColumnIndex == heartPerLineCount)
            {
                _currentColumnIndex = 0;
            }

            if (_currentColumnIndex == heartPerLineCount - 1)
            {
                _currentHeartPosition.x = gameObject.transform.position.x;
                _currentHeartPosition.y -= distanceBetweenHeartsY;
            }
            else
            {
                _currentHeartPosition.x -= distanceBetweenHeartsX;
            }

            if (_currentHeartIndex == 1 && !_creatingHealthBar)
            {
                _heartAnimation.DecreaseHeartBeat();
            }
        }
    }
    
    public void RemoveHeart()
    {
        hearts[_currentHeartIndex].GetComponent<HeartAnimation>().isDead = true;
        hearts.RemoveAt(_currentHeartIndex);
        
        _currentHeartIndex--;
        _currentColumnIndex--;
        
        if (_currentColumnIndex == -1)
        {
            _currentColumnIndex = heartPerLineCount - 1;
        }
        
        if (_currentColumnIndex == heartPerLineCount - 2)
        {
            _currentHeartPosition.x -= distanceBetweenHeartsX * (heartPerLineCount - 1);
            _currentHeartPosition.y += distanceBetweenHeartsY;
        }
        else
        {
            _currentHeartPosition.x += distanceBetweenHeartsX;
        }
        
        if (_currentHeartIndex == 0)
        {
            _heartAnimation.IncreaseHeartBeat();
        }
    }
}
