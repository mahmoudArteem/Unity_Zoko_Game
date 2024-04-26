using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class ButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent holdEvent = new UnityEvent();
    public UnityEvent unholdEvent = new UnityEvent();

    public void OnPointerDown(PointerEventData eventData)
    {
        holdEvent.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        unholdEvent.Invoke();
    }

}
