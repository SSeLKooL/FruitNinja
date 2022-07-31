using UnityEngine;

public class Blob : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    [SerializeField] private float alphaDecreaseQuotient;
    [SerializeField] private float maxRotationAngle;

    private SpriteRenderer _spriteRenderer;
    private GameObject _blob;
    private Color _blobColor;

    private Quaternion _currentRotation;

    private float _currentTime;
    
    private void Start()
    {
        _blob = this.gameObject;
        _spriteRenderer = _blob.GetComponent<SpriteRenderer>();
        _blobColor = _spriteRenderer.color;
        _currentRotation.eulerAngles = new Vector3(0, 0, Random.Range(0, maxRotationAngle));
        _blob.transform.rotation = _currentRotation;
    }
    
    private void Update()
    {
        _currentTime += Time.deltaTime;
        
        if (lifeTime < _currentTime)
        {
            _blobColor.a -= alphaDecreaseQuotient * Time.deltaTime;
            if (_blobColor.a <= 0)
            {
                Destroy(_blob);
            }
            else
            {
                _spriteRenderer.color = _blobColor;
            }
        }
    }
}
