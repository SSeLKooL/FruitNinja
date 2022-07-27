using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : MonoBehaviour
{
    [SerializeField] private float alphaDecreaseQuotient;

    private SpriteRenderer _spriteRenderer;
    private GameObject _blob;
    private Color _blobColor;
    
    private void Start()
    {
        _blob = this.gameObject;
        _spriteRenderer = _blob.GetComponent<SpriteRenderer>();
        _blobColor = _spriteRenderer.color;
    }
    
    private void Update()
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
