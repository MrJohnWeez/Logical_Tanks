using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
// ToDo:
// Delete connections: User must click on the in connection point and connection is reattached to users mouse like normal
// Select node when clicked and then options appear. Does not select node when draging node to move it around
// Delete selected node and it's connections

public class NodeLink : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    public event Action<NodeLink> OnBeginDragEvent;
    public event Action<NodeLink> OnEndDragEvent;
    public event Action<NodeLink> OnDropEvent;
    
    public RectTransform GetRect() => _rectTransform;
    public Node GetOwnerNode() => _ownerNode;
    public RectTransform GetDummyTransform() => _dummyTransform;

    public bool isConnected = false;

    [SerializeField] private bool _isOutNode = true;
    private RectTransform _rectTransform;
    private RectTransform _dummyTransform;
    private Node _ownerNode;

    private void Awake()
    {
        _rectTransform = (RectTransform)transform;
        _ownerNode = GetComponentInParent<Node>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _dummyTransform = new GameObject("dummyTransform", typeof(RectTransform)).GetComponent<RectTransform>();
        _dummyTransform.SetParent(transform.parent);
        // Debug.Log("OnBeginDrag: " + gameObject.transform.parent.name);
        OnBeginDragEvent?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("OnPointerDown: " + gameObject.transform.parent.name);
        _dummyTransform.position = Input.mousePosition;
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Debug.Log("OnDrop: " + gameObject.transform.parent.name);
        OnDropEvent?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(_dummyTransform.gameObject);
        // Debug.Log("OnEndDrag: " + gameObject.transform.parent.name);
        OnEndDragEvent?.Invoke(this);
    }

    public bool IsValidStartLink()
    {
        return _isOutNode && !isConnected;
    }

    public bool IsValidEndLink(NodeLink startNodeLink)
    {
        return !IsValidStartLink() && _ownerNode != startNodeLink.GetOwnerNode() && startNodeLink != this;
    }
}
