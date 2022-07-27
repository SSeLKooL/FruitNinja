

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
    
    [SerializeField] private float startPositionY;
    
    private const string FruitName = "fruit(Clone)";

    private void Start()
    {
        _thisObject = this.gameObject;
        _currentSizeIncrease.z = Random.Range(minSizeIncrease, maxSizeIncrease);
        _angleCurrentValue = _thisObject.transform.rotation;
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

        _angleCurrentValue.eulerAngles += new Vector3(0, 0, angleIncreaseQuotient * Time.deltaTime);
        _thisObject.transform.rotation = _angleCurrentValue;

        _thisObject.transform.localScale += _currentSizeIncrease * Time.deltaTime;
    }
    
    private void Update()
    {
        Move();
        
        CheckPosition();
    }
}
