using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(RectTransform))]
public class NodeLink : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
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

    #region PointerEvents
    public void OnPointerDown(PointerEventData eventData) { }
    public void OnBeginDrag(PointerEventData eventData) { OnBeginDragEvent?.Invoke(this); }
    public void OnDrag(PointerEventData eventData) { }
    public void OnDrop(PointerEventData eventData) { OnDropEvent?.Invoke(this); }
    public void OnEndDrag(PointerEventData eventData) { OnEndDragEvent?.Invoke(this); }
    public void OnPointerUp(PointerEventData eventData) { }
    #endregion

    public bool WillBridgeBeValid(NodeLink otherNodeLink)
    {
        return otherNodeLink && _ownerNode != otherNodeLink.OwnerNode && otherNodeLink != this;
    }

    public void AddNodeBridge(NodeBridge nodeBridge)
    {
        if (!_nodeBridges.Contains(nodeBridge))
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

    public Node GetNextNode(bool displayCall = false)
    {
        if(_isOutNode && _nodeBridges.Count > 0)
        {
            if(displayCall)
            {
                _nodeBridges[0].SetThenResetColor(_nodeBridges[0].iterationColor, _nodeBridges[0].iterationFadeTime);
            }
            return _nodeBridges[0].EndNodeLink.OwnerNode;
        }
        return null;
    }
}
