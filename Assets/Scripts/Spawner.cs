using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Fruit fruitPrefab;
    [SerializeField] private float maxSpeedX;
    [SerializeField] private float minSpeedX;
    [SerializeField] private float maxSpeedY;
    [SerializeField] private float minSpeedY;
    [SerializeField] private GameObject spawnerLeft;
    [SerializeField] private GameObject spawnerRight;
    
    [SerializeField] private PlayerConfiguration playerConfiguration;
    [SerializeField] private Camera currentCamera;

    private float _leftX;
    private float _rightX;
    private float _y;
    
    private void Start()
    {
        _leftX = spawnerLeft.transform.position.x;
        _rightX = spawnerRight.transform.position.x;
        _y = spawnerRight.transform.position.y;
    }

    public void SpawnObject()
    {
        var fruit = Instantiate(fruitPrefab, new Vector3(Random.Range(_leftX, _rightX), _y, 0), Quaternion.identity);
<<<<<<< Updated upstream
        fruit.direction = new Vector3(Random.Range(minSpeedX, maxSpeedX), Random.Range(minSpeedY, maxSpeedY), 0);
=======
        fruit.direction = new Vector3(_currentSpeedX, Random.Range(minSpeedY, maxSpeedY), 0);
        fruit.angleIncreaseValue.eulerAngles = new Vector3(0, 0, -_currentSpeedX * angleIncreaseQuotient);
        fruit.playerConfiguration = playerConfiguration;
        fruit.currentCamera = currentCamera;
>>>>>>> Stashed changes
    }
    
}
