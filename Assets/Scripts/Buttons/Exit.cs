using UnityEngine;
using UnityEngine.EventSystems;

public class Exit : MonoBehaviour, IPointerUpHandler
{
    public void OnPointerUp(PointerEventData eventData)
    {
        Application.Quit();
    }
}
