

using UnityEngine;

public class ObjectPhysics : MonoBehaviour
{
    private GameObject _thisObject;
    
    [SerializeField] private float gravitation;
    [SerializeField] private float maxSizeIncrease;
    [SerializeField] private float minSizeIncrease;
    [SerializeField] private float angleIncreaseQuotient;

    public Vector3 direction = new Vector3(0, 0, 0);
    public PlayerConfiguration playerConfiguration;

    private Quaternion _angleCurrentValue;
    private Vector3 _currentSizeIncrease;

    private int SpeedSide;
    
    [SerializeField] private float startPositionY;
    
    private const string FruitName = "fruit(Clone)";

    private void Start()
    {
        _thisObject = this.gameObject;
        _currentSizeIncrease.y = Random.Range(minSizeIncrease, maxSizeIncrease);
        _currentSizeIncrease.x = _currentSizeIncrease.y;
        _angleCurrentValue = _thisObject.transform.rotation;
        SpeedSide = (direction.x > 0) ? 1 : -1;
    }
    
    private void CheckPosition()
    {
        if (_thisObject.transform.position.y < startPositionY)
        {
            Execution();
        }
    }
    
    private void Execution()
    {
        if (_thisObject.name == FruitName)
        {
            playerConfiguration.HitPlayer();
        }
        Destroy(_thisObject);
    }

    private void Move()
    {
        _thisObject.transform.position += direction * Time.deltaTime;
        direction.y -= gravitation * Time.deltaTime;

        _angleCurrentValue.eulerAngles -= new Vector3(0, 0, SpeedSide * angleIncreaseQuotient * Time.deltaTime);
        _thisObject.transform.rotation = _angleCurrentValue;

        _thisObject.transform.localScale += _currentSizeIncrease * Time.deltaTime;
    }
    
    private void Update()
    {
        Move();
        
        CheckPosition();
    }
}
