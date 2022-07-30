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
    private Vector3 _currentHeartPosition;
    private int _currentHeartIndex = -1;
    private int _currentColumnIndex = -1;

    public void CreateHealthBar(int health)
    {
        _currentHeartPosition = this.gameObject.transform.position;
        
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
            hearts.Add(heart);

            _currentColumnIndex++;
            _currentHeartIndex++;

            if (_currentColumnIndex == heartPerLineCount - 1)
            {
                _currentHeartPosition.x = this.gameObject.transform.position.x;
                _currentHeartPosition.y -= distanceBetweenHeartsY;
                _currentColumnIndex = -1;
            }
            else
            {
                _currentHeartPosition.x -= distanceBetweenHeartsX;
            }
        }
    }
    
    public void RemoveHeart()
    {
        Destroy(hearts[_currentHeartIndex]);
        _currentHeartIndex--;
        
        if (_currentHeartIndex == 0)
        {
            hearts[_currentHeartIndex].GetComponent<HeardAnimation>().IncreaseHeardBeat();
        }
    }
}
