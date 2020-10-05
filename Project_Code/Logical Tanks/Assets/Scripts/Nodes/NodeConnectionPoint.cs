using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Implement buttons programmatically instead of using built in buttons. Allows for more control IPointerEnterHandler

public class NodeConnectionPoint : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    public delegate void IPointerEvent(PointerEventData eventData, NodeConnectionPoint nodeConnectionPoint);
    public event IPointerEvent OnPointerDownEvent;
    public event IPointerEvent OnPointerUpEvent;
    
    public bool isConnected = false;

    public RectTransform GetRect() => _rectTransform;
    public bool IsOutNode() => _isOutNode;

    [SerializeField] private bool _isOutNode = true;
    private RectTransform _rectTransform;
    

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag: " + gameObject.transform.parent.name);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown: " + gameObject.transform.parent.name);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag: " + gameObject.transform.parent.name);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown: " + gameObject.transform.parent.name);
        OnPointerDownEvent?.Invoke(eventData, this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp: " + gameObject.transform.parent.name);
        OnPointerUpEvent?.Invoke(eventData, this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop: " + gameObject.transform.parent.name);
    }
}
