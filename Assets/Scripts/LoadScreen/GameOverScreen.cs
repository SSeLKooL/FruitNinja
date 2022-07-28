using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private float alphaIncreaseQuotient;
    [SerializeField] private float screenAlpha;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject mainMenuButton;
    
    [SerializeField] private Image restartImage;
    [SerializeField] private Image mainMenuImage;
    [SerializeField] private TextMeshProUGUI[] textMeshArText;

    private Image _screenImage;
    private float _alpha;
    private Color _standardColor = new Color(1, 1, 1, 0);
    private Color _screenColor = new Color(0, 0, 0, 0);
    private Color[] _textMeshArColor;

    private bool _show;

    private void Start()
    {
        _screenImage = this.gameObject.GetComponent<Image>();
        
        _textMeshArColor = new Color[textMeshArText.Length];
        for (var i = 0; i < textMeshArText.Length; i++)
        {
            _textMeshArColor[i] = textMeshArText[i].color;
        }
    }

    public void ShowGameOverScreen()
    {
        _show = true;
        mainMenuButton.SetActive(true);
        restartButton.SetActive(true);
        _alpha = 0;
    }

    private void ShowingScreen()
    {
        if (_show)
        {
            _alpha += alphaIncreaseQuotient * Time.deltaTime;

            if (_alpha < screenAlpha)
            {
                _screenColor.a = _alpha;
                _screenImage.color = _screenColor;
            }

            if (_alpha < 1)
            {
                _standardColor.a = _alpha;
                mainMenuImage.color = _standardColor;
                restartImage.color = _standardColor;
                for (var i = 0; i < _textMeshArColor.Length; i++)
                {
                    _textMeshArColor[i].a = _alpha;
                    textMeshArText[i].color = _textMeshArColor[i];
                }
            }
            else
            {
                _show = false;
            }
            
        }
    }

    private void Update()
    {
        ShowingScreen();
    }
}
