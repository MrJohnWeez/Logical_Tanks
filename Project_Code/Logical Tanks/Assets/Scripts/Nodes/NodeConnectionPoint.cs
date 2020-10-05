using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Implement buttons programmatically instead of using built in buttons. Allows for more control IPointerEnterHandler

public class NodeConnectionPoint : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    public delegate void IPointerEvent(PointerEventData eventData, NodeConnectionPoint nodeConnectionPoint);
    public event IPointerEvent OnBeginDragEvent;
    public event IPointerEvent OnEndDragEvent;
    public event IPointerEvent OnDropEvent;
    
    public bool isConnected = false;

    public RectTransform GetRect() => _rectTransform;
    public bool IsOutNode() => _isOutNode;
    public RectTransform GetDummyTransform() => _dummyTransform;

    [SerializeField] private bool _isOutNode = true;
    private RectTransform _rectTransform;
    private RectTransform _dummyTransform;
    private Canvas _nodeCanvas = null;

    

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _nodeCanvas = GameObject.FindGameObjectWithTag("NodeCanvas").GetComponent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _dummyTransform = new GameObject("dummyTransform", typeof(RectTransform)).GetComponent<RectTransform>();
        _dummyTransform.SetParent(transform.parent);
        Debug.Log("OnBeginDrag: " + gameObject.transform.parent.name);
        OnBeginDragEvent?.Invoke(eventData, this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown: " + gameObject.transform.parent.name);
        _dummyTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(_dummyTransform.gameObject);
        Debug.Log("OnEndDrag: " + gameObject.transform.parent.name);
        OnEndDragEvent?.Invoke(eventData, this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown: " + gameObject.transform.parent.name);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp: " + gameObject.transform.parent.name);
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop: " + gameObject.transform.parent.name);
        OnDropEvent?.Invoke(eventData, this);
    }
}
