using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
// ToDo:
// Select node when clicked and then options appear. Does not select node when draging node to move it around
// Delete selected node and it's connections

public class NodeLink : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    public event Action<NodeLink> OnBeginDragEvent;
    public event Action<NodeLink> OnEndDragEvent;
    public event Action<NodeLink> OnDropEvent;

    public RectTransform Rect => _rectTransform;
    public Node OwnerNode => _ownerNode;
    public bool HasBridges => _nodeBridges.Count != 0;
    public bool IsOutNode => _isOutNode;
    public int BridgeCount => _nodeBridges.Count;
    public List<NodeBridge> Bridges => _nodeBridges;
    
    [SerializeField] private bool _isOutNode = true;
    private List<NodeBridge> _nodeBridges = new List<NodeBridge>();
    private RectTransform _rectTransform;
    private Node _ownerNode;

    private void Awake()
    {
        _rectTransform = (RectTransform)transform;
        _ownerNode = GetComponentInParent<Node>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag: " + gameObject.transform.parent.name);
        OnBeginDragEvent?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("OnPointerDown: " + gameObject.transform.parent.name);
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Debug.Log("OnDrop: " + gameObject.transform.parent.name);
        OnDropEvent?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("OnEndDrag: " + gameObject.transform.parent.name);
        OnEndDragEvent?.Invoke(this);
    }

    public bool WillBridgeBeValid(NodeLink otherNodeLink)
    {
        return _ownerNode != otherNodeLink.OwnerNode && otherNodeLink != this;
    }

    public void AddNodeBridge(NodeBridge nodeBridge)
    {
        if(!_nodeBridges.Contains(nodeBridge))
            _nodeBridges.Add(nodeBridge);
    }

    public void RemoveNodeBridge(NodeBridge nodeBridge)
    {
        _nodeBridges.Remove(nodeBridge);
    }

    public void RemoveAllNodeBridges()
    {
        _nodeBridges.Clear();
    }
}
