using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject fruitPrefab;
    [SerializeField] private GameObject blobPrefab;
    [SerializeField] private GameObject cutEffectPrefab;
    [SerializeField] private GameObject addedScorePrefab;

    [SerializeField] private Transform fruitParentTransform;
    [SerializeField] private Transform blobParentTransform;
    [SerializeField] private Transform cutEffectParentTransform;
    [SerializeField] private Transform addedScoreParentTransform;

    private ObjectPhysics _physicsScript;
    private Fruit _fruitScript;
    
    [SerializeField] private int scoreForExecution;
    
    [SerializeField] private float maxSpeedX;
    [SerializeField] private float minSpeedX;
    [SerializeField] private float maxSpeedY;
    [SerializeField] private float minSpeedY;
    [SerializeField] private GameObject spawnerLeft;
    [SerializeField] private GameObject spawnerRight;
    
    [SerializeField] private SpriteCutter spriteCutter;
    [SerializeField] private PlayerConfiguration playerConfiguration;
    [SerializeField] private Camera currentCamera;
    
    [SerializeField] private Sprite[] blobs;

    private Spawner _thisSpawner;

    private float _currentSpeedX;

    private float _leftX;
    private float _rightX;
    private float _y;
    
    private void Start()
    {
        _thisSpawner = this.gameObject.GetComponent<Spawner>();
        
        _leftX = spawnerLeft.transform.position.x;
        _rightX = spawnerRight.transform.position.x;
        _y = spawnerRight.transform.position.y;
    }

    private void SpawnAddedScore(float rangeX, float rangeY, Vector2 center)
    {
        var addedScorePosition = new Vector2(Random.Range(center.x - rangeX, center.x + rangeX), Random.Range(center.y - rangeY, center.y + rangeY));
        
        var addedScore = Instantiate(addedScorePrefab, addedScorePosition, Quaternion.identity, addedScoreParentTransform);
        
        addedScore.GetComponent<TextMeshProUGUI>().text = scoreForExecution.ToString();
    }

    public void ExecuteFruit(GameObject fruit, Sprite currentSprite, int spriteIndex, Vector2 firstTapPosition, Vector2 secondTapPosition, float rangeX, float rangeY)
    {
        playerConfiguration.AddScorePoints(scoreForExecution);
        var blob = Instantiate(blobPrefab, fruit.transform.position, Quaternion.identity, blobParentTransform);
        blob.GetComponent<SpriteRenderer>().sprite = blobs[spriteIndex];
        
        var cutEffect = Instantiate(cutEffectPrefab, fruit.transform.position, Quaternion.identity, cutEffectParentTransform);
        cutEffect.GetComponent<CutEffect>().SetParticles(spriteIndex);
        spriteCutter.CutObject(fruit, fruitParentTransform, currentSprite, spriteIndex, firstTapPosition,secondTapPosition, fruit.GetComponent<ObjectPhysics>().direction, playerConfiguration);

        SpawnAddedScore(rangeX, rangeY, fruit.transform.position);
        
        Destroy(fruit);
    }

    public void SpawnObject()
    {
        _currentSpeedX = Random.Range(minSpeedX, maxSpeedX);
        
        var fruit = Instantiate(fruitPrefab, new Vector3(Random.Range(_leftX, _rightX), _y, 0), Quaternion.identity, fruitParentTransform);

        _physicsScript = fruit.GetComponent<ObjectPhysics>();
        _fruitScript = fruit.GetComponent<Fruit>();
            
        _physicsScript.direction = new Vector3(_currentSpeedX, Random.Range(minSpeedY, maxSpeedY), 0);
        _physicsScript.playerConfiguration = _fruitScript.PlayerConfiguration = playerConfiguration;
        _fruitScript.currentCamera = currentCamera;
        _fruitScript.spawner = _thisSpawner;
    }
    
}
