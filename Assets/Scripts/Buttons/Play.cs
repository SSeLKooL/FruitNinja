using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Play : MonoBehaviour, IPointerUpHandler
{
    [SerializeField] private GameObject loadScreen;
    private ShowScreen _showScreenScript;

    private void Start()
    {
        _showScreenScript = loadScreen.GetComponent<ShowScreen>();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        loadScreen.SetActive(true);
        _showScreenScript.Show("GameScene");
    }
}
