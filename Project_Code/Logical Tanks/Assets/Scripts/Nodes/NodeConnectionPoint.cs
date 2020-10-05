using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Implement buttons programmatically instead of using built in buttons. Allows for more control IPointerEnterHandler

public class NodeConnectionPoint : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public delegate void IPointerEvent(PointerEventData eventData, NodeConnectionPoint nodeConnectionPoint);
    public event IPointerEvent OnPointerDownEvent;
    public event IPointerEvent OnPointerUpEvent;
    public bool isOutNode = true;

    public RectTransform GetTangentRect() => _tangentRect;
    public RectTransform GetRect() => _rectTransform;

    [SerializeField] private RectTransform _tangentRect;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPointerDownEvent?.Invoke(eventData, this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnPointerUpEvent?.Invoke(eventData, this);
    }
}
