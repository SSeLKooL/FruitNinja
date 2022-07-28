using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScreen : MonoBehaviour
{
    [SerializeField] private float alphaDecreaseQuotient;
    [SerializeField] private float alphaIncreaseQuotient;
    
    private Image _image;
    private Color screenColor;

    private bool show;
    private bool hide;
    
    private void Start()
    {
        _image = this.gameObject.GetComponent<Image>();
        screenColor = _image.color;
    }

    public void ShowScreen()
    {
        show = true;
    }

    public void HideScreen()
    {
        hide = true;
    }

    private void ShowingScreen()
    {
        if (show)
        {
            screenColor.a += alphaIncreaseQuotient * Time.deltaTime;
            Debug.Log(screenColor.a);
            if (screenColor.a < 1)
            {
                _image.color = screenColor;
            }
            else
            {
                show = false;
                SceneManager.LoadScene("GameScene");
            }
        }
    }
    
    private void HidingScreen()
    {
        if (hide)
        {
            screenColor.a -= alphaDecreaseQuotient * Time.deltaTime;
            if (screenColor.a > 0)
            {
                _image.color = screenColor;
            }
            else
            {
                hide = false;
            }
        }
    }
    
    private void Update()
    {
        ShowingScreen();
        HidingScreen();
    }
}
