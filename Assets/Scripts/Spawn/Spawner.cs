using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject blobPrefab;
    [SerializeField] private GameObject cutEffectPrefab;
    [SerializeField] private GameObject addedScorePrefab;

    [SerializeField] private Transform fruitParentTransform;
    [SerializeField] private Transform blobParentTransform;
    [SerializeField] private Transform effectParentTransform;
    [SerializeField] private Transform addedScoreParentTransform;

    private ObjectPhysics _physicsScript;
    private Fruit _fruitScript;

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

    public void SpawnText(float rangeX, float rangeY, Vector2 center, string text)
    {
        var textPosition = new Vector2(Random.Range(center.x - rangeX, center.x + rangeX), Random.Range(center.y - rangeY, center.y + rangeY));
        
        var spawnedText = Instantiate(addedScorePrefab, textPosition, Quaternion.identity, addedScoreParentTransform);
        
        spawnedText.GetComponent<TextMeshProUGUI>().text = text;
    }
    
    public void SpawnText(float rangeX, float rangeY, Vector2 center, float scaleMultiplier, string text)
    {
        var textPosition = new Vector2(Random.Range(center.x - rangeX, center.x + rangeX), Random.Range(center.y - rangeY, center.y + rangeY));
        
        var spawnedText = Instantiate(addedScorePrefab, textPosition, Quaternion.identity, addedScoreParentTransform);
        
        spawnedText.transform.localScale *= scaleMultiplier;
        
        spawnedText.GetComponent<TextMeshProUGUI>().text = text;
    }
    
    public void ExecuteFruit(GameObject fruit, Sprite currentSprite, int materialIndex, Vector2 firstTapPosition, Vector2 secondTapPosition)
    {
        spriteCutter.CutObject(fruit, fruitParentTransform, currentSprite, materialIndex, firstTapPosition,secondTapPosition, fruit.GetComponent<ObjectPhysics>().direction, playerConfiguration);
        Destroy(fruit);
    }

    public void SpawnBlob(int blobIndex, Vector3 position)
    {
        var blob = Instantiate(blobPrefab, position, Quaternion.identity, blobParentTransform);
        blob.GetComponent<SpriteRenderer>().sprite = blobs[blobIndex];
    }

    public GameObject SpawnEffect(GameObject effectPrefab, Vector3 position)
    { 
        var effectObject = Instantiate(effectPrefab, position, Quaternion.identity, effectParentTransform);
        return effectObject;
    }

    public void SpawnParticleCutEffect(int particleIndex, Vector3 position)
    {
        var cutEffect = Instantiate(cutEffectPrefab, position, Quaternion.identity, effectParentTransform);
        cutEffect.GetComponent<CutEffect>().SetParticles(particleIndex);
    }

    public void SpawnObject(GameObject objectPrefab)
    {
        _currentSpeedX = Random.Range(minSpeedX, maxSpeedX);
        
        var fruit = Instantiate(objectPrefab, new Vector3(Random.Range(_leftX, _rightX), _y, 0), Quaternion.identity, fruitParentTransform);

        _physicsScript = fruit.GetComponent<ObjectPhysics>();
        _fruitScript = fruit.GetComponent<Fruit>();
            
        _physicsScript.direction = new Vector3(_currentSpeedX, Random.Range(minSpeedY, maxSpeedY), 0);
        _physicsScript.playerConfiguration = _fruitScript.playerConfiguration = playerConfiguration;
        _fruitScript.currentCamera = currentCamera;
        _fruitScript.spawner = _thisSpawner;
    }
    
}
