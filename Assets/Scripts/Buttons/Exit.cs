using UnityEngine;
using UnityEngine.EventSystems;

public class Exit : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Application.Quit();
    }
}
