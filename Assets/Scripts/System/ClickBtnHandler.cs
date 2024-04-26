using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ClickBtnHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool clicked = false;

    public UnityEvent holdEvent = new UnityEvent();
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!clicked)
        {
            holdEvent.Invoke();

            clicked = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        clicked = false;
    }
}
