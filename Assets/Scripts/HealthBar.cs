using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private List<GameObject> hearts = new List<GameObject>();

    [SerializeField] private float distanceBetweenHearts;
    [SerializeField] private GameObject heartPrefab;
    private Vector3 _currentHeartPosition;
    private int _currentHeartIndex = -1;

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
        var heart = Instantiate(heartPrefab, _currentHeartPosition, Quaternion.identity, this.gameObject.transform);
        hearts.Add(heart);

        _currentHeartPosition.x -= distanceBetweenHearts;
        _currentHeartIndex++;
    }

    public void RemoveHeart()
    {
        hearts[_currentHeartIndex].SetActive(false);
        _currentHeartIndex--;
    }
}
