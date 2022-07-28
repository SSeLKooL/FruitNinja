using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour, IPointerUpHandler
{
    [SerializeField] private GameObject showScreen;
    private ShowScreen _showScreenScript;

    private void Start()
    {
        _showScreenScript = showScreen.GetComponent<ShowScreen>();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        showScreen.SetActive(true);
        _showScreenScript.Show("MainMenu");
    }
}
