using UnityEngine;

public class BoomAnimation : MonoBehaviour
{
    public GameObject bomb;
    
    [SerializeField] private Vector3 scaleIncreaseQuotient;
    [SerializeField] private Vector3 angleIncreaseQuotient;

    private Vector3 _currentScale;
    private Vector3 _targetScale;
    private Quaternion _currentRotation;

    private bool _isIncreaseScale = true;
    
    private void Start()
    {
        _targetScale = transform.localScale;
        _currentScale = transform.localScale = Vector3.zero;
        _currentRotation = transform.rotation;
    }

    private void Update()
    {
        _currentRotation.eulerAngles += angleIncreaseQuotient * Time.deltaTime;
        transform.rotation = _currentRotation;
        
        if (_isIncreaseScale)
        {
            _currentScale += scaleIncreaseQuotient * Time.deltaTime;

            if (_currentScale.x < _targetScale.x)
            {
                gameObject.transform.localScale = _currentScale;
            }
            else
            {
                _isIncreaseScale = false;
                _targetScale = Vector3.zero;
                Destroy(bomb);
            }
        }
        else
        {
            _currentScale -= scaleIncreaseQuotient * Time.deltaTime;

            if (_currentScale.x > _targetScale.x)
            {
                gameObject.transform.localScale = _currentScale;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
