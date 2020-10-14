using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class PaneControls : MonoBehaviour, IDragHandler
{
    public Action<Vector2> OnDragEvent;

    public void OnDrag(PointerEventData eventData)
    {
        OnDragEvent?.Invoke(eventData.delta);
    }
}
