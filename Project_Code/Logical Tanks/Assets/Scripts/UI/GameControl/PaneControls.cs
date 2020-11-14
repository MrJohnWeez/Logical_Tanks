using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class PaneControls : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public Action<Vector2> OnDragEvent;
    public Vector2 orgPos = new Vector2();

    public void OnDrag(PointerEventData eventData) { OnDragEvent?.Invoke(eventData.delta); }
    public void OnPointerDown(PointerEventData eventData) { orgPos = eventData.position; }
    public void OnPointerUp(PointerEventData eventData)
    {
        Vector2 newPos = orgPos - eventData.position;
        newPos.x = Remap(newPos.x, -800, 800, -20, 20);
        newPos.y = Remap(newPos.y, -800, 800, -20, 20);
    }

    public float Remap (float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
