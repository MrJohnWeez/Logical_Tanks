using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using UnityEngine.UI.Extensions;

// TODO:
// Organize Node classes
// Make tank shoot
// Make tank rotate
// Make tutorial level
// Make win condition (All enemy tanks are eliminated)
// Set up game scripts
// Add colored pressure plate that can activate colored land mines


[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(NicerOutline))]
[RequireComponent(typeof(CanvasGroup))]
public class Node : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public event Action<Node> SelectChanged;
    public event Action<Node, Vector2, Vector2> OnNodeDragged;
    
    [Header("NodeBase")]
    [SerializeField] protected NodeLink inNodeLink = null;
    [SerializeField] protected NodeLink[] outNodeLinks = null;
    [SerializeField] protected bool isLocked = false;
    [SerializeField] protected bool isSelectable = true;
    [SerializeField] protected bool isDeletable = true;
    protected NodeManager nodeManager;
    protected bool _isSelected = false;
    private RectTransform _contentWindow;
    private Canvas _canvas;
    private RectTransform _rectTransform;
    private CanvasGroup canvasGroup;
    private NicerOutline _nicerOutline;
    private bool _didDrag = false;
    
    public bool IsSelected => _isSelected;
    public RectTransform GetRect => _rectTransform;

    public virtual void Awake()
    {
        _rectTransform = (RectTransform)transform;
        _nicerOutline = GetComponent<NicerOutline>();
        canvasGroup = GetComponent<CanvasGroup>();

        nodeManager = GameObject.FindObjectOfType<NodeManager>();
        SelectChanged += nodeManager.NodeSelectionChanged;
        OnNodeDragged += nodeManager.NodeOnDrag;

        // TODO: Make these lines a class type instead?
        _canvas = GameObject.FindGameObjectWithTag("InGameUI").GetComponent<Canvas>();
        _contentWindow = GameObject.FindGameObjectWithTag("ContentWindow").GetComponent<RectTransform>();
    }

    public virtual void Start()
    {
        foreach (NodeLink ncp in outNodeLinks)
        {
            ncp.OnBeginDragEvent += nodeManager.NodeLinkDragStarted;
            ncp.OnDropEvent += nodeManager.NodeLinkDropped;
            ncp.OnEndDragEvent += nodeManager.NodeLinkDragEnded;
        }
        if(inNodeLink)
        {
            inNodeLink.OnBeginDragEvent += nodeManager.NodeLinkDragStarted;
            inNodeLink.OnDropEvent += nodeManager.NodeLinkDropped;
            inNodeLink.OnEndDragEvent += nodeManager.NodeLinkDragEnded;
        }
    }

    public virtual void OnDestroy()
    {
        foreach (NodeLink ncp in outNodeLinks)
        {
            ncp.OnBeginDragEvent -= nodeManager.NodeLinkDragStarted;
            ncp.OnDropEvent -= nodeManager.NodeLinkDropped;
            ncp.OnEndDragEvent -= nodeManager.NodeLinkDragEnded;
        }

        if(inNodeLink)
        {
            inNodeLink.OnBeginDragEvent -= nodeManager.NodeLinkDragStarted;
            inNodeLink.OnDropEvent -= nodeManager.NodeLinkDropped;
            inNodeLink.OnEndDragEvent -= nodeManager.NodeLinkDragEnded;
        }
        
        DeleteBridges();
    }

    #region PointerEvents
    public void OnPointerDown(PointerEventData eventData) { _didDrag = false; }
    public void OnBeginDrag(PointerEventData eventdata)
    {
        if(!isLocked)
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.6f;
            _didDrag = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData != null && !isLocked)
        {
            Vector2 delta = eventData.delta / _canvas.scaleFactor / _contentWindow.localScale;
            OnNodeDragged?.Invoke(this, delta, eventData.position);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(!isLocked)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_didDrag && isSelectable)
        {
            SetIsSelected(!_isSelected);
        }
    }
    #endregion

    public List<NodeBridge> GetAllBridges()
    {
        List<NodeBridge> nodeBridges = new List<NodeBridge>();

        foreach(NodeLink nl in outNodeLinks)
        {
            foreach(NodeBridge nb in nl.Bridges)
            {
                if(!nodeBridges.Contains(nb))
                    nodeBridges.Add(nb);
            }
        }

        if(inNodeLink)
        {
            foreach(NodeBridge nb in inNodeLink.Bridges)
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
        if(isDeletable)
            Destroy(gameObject);
    }

    public virtual IEnumerator Execute()
    {
        yield return null;
    }

    public virtual Node NextNode()
    {
        if(outNodeLinks != null && outNodeLinks.Length == 1)
        {
            return outNodeLinks[0].GetNextNode();
        }
        return null;
    }
}
