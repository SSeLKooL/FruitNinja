using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Color colorDecreaseQuotient;
    [SerializeField] private Vector3 scaleDecreaseQuotient;
    [SerializeField] private float minColorR;
    [SerializeField] private float minScaleX;

    private RectTransform _rectTransform;
    
    private Image _currentImage;
    private Color _currentColor;
    private Color _startColor;
    private Vector3 _currentScale;
    private Vector3 _startScale;

    private bool _startAnimation;
    private bool _colorAnimation;

    private void Start()
    {
        _currentImage = this.gameObject.GetComponent<Image>();
        _rectTransform = this.gameObject.GetComponent<RectTransform>();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _startAnimation = false;
        _colorAnimation = false;
        _currentImage.color = _startColor;
        _rectTransform.localScale = _startScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _startAnimation = true;
        _colorAnimation = true;
        _startScale = _rectTransform.localScale;
        _currentColor = _startColor = _currentImage.color;
        _currentScale = _startScale;
    }

    private void Update()
    {
        if (_startAnimation)
        {
            _currentColor -= colorDecreaseQuotient * Time.deltaTime;
            _currentScale -= scaleDecreaseQuotient * Time.deltaTime;

            if (_currentColor.r > minColorR)
            {
                _currentImage.color = _currentColor;
            }
            else
            {
                _colorAnimation = false;
            }

            if (_currentScale.x > minScaleX)
            {
                _rectTransform.localScale = _currentScale;
            }
            else if (!_colorAnimation)
            {
                _startAnimation = false;
            }
        }
    }
}
