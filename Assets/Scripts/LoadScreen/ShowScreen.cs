using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShowScreen : MonoBehaviour
{
    [SerializeField] private float alphaIncreaseQuotient;
    
    private Image _image;
    private Color _screenColor;

    private bool _show;

    private string _currentSceneName;
    
    private void Start()
    {
        _image = this.gameObject.GetComponent<Image>();
        _screenColor = _image.color;
    }

    public void Show(string sceneName)
    {
        _currentSceneName = sceneName;
        _show = true;
    }

    private void ShowingScreen()
    {
        if (_show)
        {
            _screenColor.a += alphaIncreaseQuotient * Time.deltaTime;
            if (_screenColor.a < 1)
            {
                _image.color = _screenColor;
            }
            else
            {
                _show = false;
                SceneManager.LoadScene(_currentSceneName);
            }
        }
    }

    private void Update()
    {
        ShowingScreen();
    }
}
