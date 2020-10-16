using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using UnityEngine.UI.Extensions;

// TODO:
// Set up node base class to better fit dynamic nodes and then figure out the way to get the next node
// Make tutorial level
// Make win condition
// Set up game scripts
// Add colored pressure plate that can activate colored land mines


[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(NicerOutline))]
[RequireComponent(typeof(CanvasGroup))]
public class Node : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public event Action<Node> SelectChanged;
    public event Action<Node, Vector2, Vector2> OnNodeDragged;

    public bool IsSelected => _isSelected;
    public RectTransform GetRect => _rectTransform;

    [SerializeField] protected NodeLink[] _nodeLinks = null;
    [SerializeField] protected bool _isLocked = false;
    [SerializeField] protected bool _isSelectable = true;
    [SerializeField] protected bool _isDeletable = true;

    protected NodeManager _nodeManager;
    protected RectTransform _contentWindow;
    protected Canvas _canvas;
    protected RectTransform _rectTransform;
    protected CanvasGroup canvasGroup;
    protected NicerOutline _nicerOutline;
    protected bool _didDrag = false;
    protected bool _isSelected = false;

    private void Awake()
    {
        _rectTransform = (RectTransform)transform;
        _nicerOutline = GetComponent<NicerOutline>();
        canvasGroup = GetComponent<CanvasGroup>();

        _nodeManager = GameObject.FindObjectOfType<NodeManager>();
        SelectChanged += _nodeManager.NodeSelectionChanged;
        OnNodeDragged += _nodeManager.NodeOnDrag;

        // TODO: Make these lines a class type instead?
        _canvas = GameObject.FindGameObjectWithTag("InGameUI").GetComponent<Canvas>();
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

    public virtual IEnumerator Execute()
    {
        yield return null;
    }

    public virtual Node NextNode()
    {
        return null;
    }
}
