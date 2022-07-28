using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Play : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject loadScreen;
    private ShowScreen _showScreenScript;

    private void Start()
    {
        _showScreenScript = loadScreen.GetComponent<ShowScreen>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("ok");
        loadScreen.SetActive(true);
        _showScreenScript.Show("GameScene");
    }
}
