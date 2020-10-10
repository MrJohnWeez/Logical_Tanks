using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using UnityEngine.UI.Extensions;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(NicerOutline))]
[RequireComponent(typeof(CanvasGroup))]
public class Node : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public event Action<Node> SelectChanged;
    public event Action<Node, Vector2, Vector2> OnNodeDragged;

    public bool IsSelected => _isSelected;
    public RectTransform GetRect => _rectTransform;

    [SerializeField] private NodeLink[] _nodeLinks = null;
    [SerializeField] private bool _isLocked = false;
    [SerializeField] private bool _isSelectable = true;
    [SerializeField] private bool _isDeletable = true;

    private NodeManager _nodeManager;
    private RectTransform _contentWindow;
    private Canvas _canvas;
    private RectTransform _rectTransform;
    private CanvasGroup canvasGroup;
    private NicerOutline _nicerOutline;
    private bool _didDrag = false;
    private bool _isSelected = false;

    private void Awake()
    {
        _rectTransform = (RectTransform)transform;
        _nicerOutline = GetComponent<NicerOutline>();
        canvasGroup = GetComponent<CanvasGroup>();

        _nodeManager = GameObject.FindObjectOfType<NodeManager>();
        SelectChanged += _nodeManager.NodeSelectionChanged;
        OnNodeDragged += _nodeManager.NodeOnDrag;

        // TODO: Make these lines a class type instead?
        _canvas = GameObject.FindGameObjectWithTag("NodeCanvas").GetComponent<Canvas>();
        _contentWindow = GameObject.FindGameObjectWithTag("ContentWindow").GetComponent<RectTransform>();
    }

    private void Start()
    {
        foreach (NodeLink ncp in _nodeLinks)
        {
            ncp.OnBeginDragEvent += _nodeManager.NodeLinkDragStarted;
            ncp.OnDropEvent += _nodeManager.NodeLinkDropped;
            ncp.OnEndDragEvent += _nodeManager.NodeLinkDragEnded;
        }
    }

    private void OnDestroy()
    {
        foreach (NodeLink ncp in _nodeLinks)
        {
            ncp.OnBeginDragEvent -= _nodeManager.NodeLinkDragStarted;
            ncp.OnDropEvent -= _nodeManager.NodeLinkDropped;
            ncp.OnEndDragEvent -= _nodeManager.NodeLinkDragEnded;
        }
        DeleteBridges();
    }

    #region PointerEvents
    public void OnPointerDown(PointerEventData eventData) { _didDrag = false; }
    public void OnBeginDrag(PointerEventData eventdata)
    {
        if(!_isLocked)
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.6f;
            _didDrag = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData != null && !_isLocked)
        {
            Vector2 delta = eventData.delta / _canvas.scaleFactor / _contentWindow.localScale;
            OnNodeDragged?.Invoke(this, delta, eventData.position);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(!_isLocked)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_didDrag && _isSelectable)
        {
            SetIsSelected(!_isSelected);
        }
    }
    #endregion

    public List<NodeBridge> GetAllBridges()
    {
        List<NodeBridge> nodeBridges = new List<NodeBridge>();

        foreach(NodeLink nl in _nodeLinks)
        {
            foreach(NodeBridge nb in nl.Bridges)
            {
                if(!nodeBridges.Contains(nb))
                    nodeBridges.Add(nb);
            }
        }

        return nodeBridges;
    }

    public void DeleteBridges()
    {
        List<NodeBridge> nodeBridges = GetAllBridges();
        for(int i = nodeBridges.Count - 1; i >= 0; i--)
        {
            Destroy(nodeBridges[i].gameObject);
        }
    }

    public void SetIsSelected(bool newSelectedState)
    {
        _isSelected = newSelectedState;
        _nicerOutline.enabled = newSelectedState;
        SelectChanged?.Invoke(this);
    }

    public void Delete()
    {
        if(_isDeletable)
            Destroy(gameObject);
    }
}
