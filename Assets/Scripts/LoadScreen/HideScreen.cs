using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideScreen : MonoBehaviour
{
    [SerializeField] private float alphaDecreaseQuotient;

    private Image _image;
    private Color screenColor;

    private bool _hide;
    
    private void Start()
    {
        _image = this.gameObject.GetComponent<Image>();
        screenColor = _image.color;
        screenColor.a = 1;
        _image.color = screenColor;
        _hide = true;
    }

    private void HidingScreen()
    {
        if (_hide)
        {
            screenColor.a -= alphaDecreaseQuotient * Time.deltaTime;
            
            if (screenColor.a > 0)
            {
                _image.color = screenColor;
            }
            else
            {
                _hide = false;
                this.gameObject.SetActive(false);
            }
        }
    }
    
    private void Update()
    {
        HidingScreen();
    }
}
