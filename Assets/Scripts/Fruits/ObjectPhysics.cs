using UnityEngine;

public class ObjectPhysics : MonoBehaviour
{
    [SerializeField] private float gravitation;
    [SerializeField] private float maxSizeIncrease;
    [SerializeField] private float minSizeIncrease;
    [SerializeField] private float angleIncreaseQuotient;

    public Vector3 direction = new Vector3(0, 0, 0);
    public PlayerConfiguration playerConfiguration;

    private Quaternion _angleCurrentValue;
    private Vector3 _currentSizeIncrease;

    private Transform _physicsTransform;

    private int SpeedSide;
    
    [SerializeField] private float startPositionY;
    
    private const string FruitName = "fruit(Clone)";

    private void Start()
    {
        _physicsTransform = gameObject.transform;
        
        _currentSizeIncrease.y = Random.Range(minSizeIncrease, maxSizeIncrease);
        _currentSizeIncrease.x = _currentSizeIncrease.y;
        _angleCurrentValue = _physicsTransform.rotation;
        SpeedSide = (direction.x > 0) ? 1 : -1;
    }
    
    private void CheckPosition()
    {
        if (_physicsTransform.position.y < startPositionY)
        {
            Execution();
        }
    }
    
    private void Execution()
    {
        if (gameObject.name == FruitName)
        {
            playerConfiguration.HitPlayer();
        }
        
        Destroy(gameObject);
    }

    private void Move()
    {
        _physicsTransform.position += direction * Time.deltaTime;
        direction.y -= gravitation * Time.deltaTime;

        _angleCurrentValue.eulerAngles -= new Vector3(0, 0, SpeedSide * angleIncreaseQuotient * Time.deltaTime);
        _physicsTransform.rotation = _angleCurrentValue;

        _physicsTransform.localScale += _currentSizeIncrease * Time.deltaTime;
    }
    
    private void Update()
    {
        Move();
        
        CheckPosition();
    }
}
