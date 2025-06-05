using UnityEngine;
using UnityEngine.EventSystems;

public class PointerHandlerButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public delegate void PointerEnterAction();
    public delegate void PointerExitAction();

    public event PointerEnterAction OnPointerEnterEvent;
    public event PointerExitAction OnPointerExitEvent;
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnterEvent?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnPointerExitEvent?.Invoke();
    }
}


