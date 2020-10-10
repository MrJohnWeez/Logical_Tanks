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
    public event Action<Node> OnSelect;
    public bool IsSelected => _isSelected;

    [SerializeField] private NodeLink[] _nodeLinks;
    private NodeManager _nodeManager;
    private RectTransform _contentWindow;
    private Canvas _canvas;
    private RectTransform _rectTransform;
    private CanvasGroup canvasGroup;
    private RectTransform _scrollView;
    private NicerOutline _nicerOutline;
    private bool _didDrag = false;
    private bool _isSelected = false;

    private void Awake()
    {
        _rectTransform = (RectTransform)transform;
        _nicerOutline = GetComponent<NicerOutline>();
        canvasGroup = GetComponent<CanvasGroup>();

        _nodeManager = GameObject.FindObjectOfType<NodeManager>();
        OnSelect += _nodeManager.NodeOnSelect;

        // TODO: Make these lines a class type instead?
        _canvas = GameObject.FindGameObjectWithTag("NodeCanvas").GetComponent<Canvas>();
        _scrollView = GameObject.FindGameObjectWithTag("NodeScrollView").GetComponent<RectTransform>();
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
    }

    public void OnPointerDown(PointerEventData eventData) { _didDrag = false; }
    public void OnBeginDrag(PointerEventData eventdata)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
        _didDrag = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData != null)
        {
            Vector2 delta = eventData.delta / _canvas.scaleFactor / _contentWindow.localScale;
            _rectTransform.anchoredPosition += delta;

            if (!_scrollView.GetWorldSapceRect().Contains(eventData.position) || !_contentWindow.Contains(_rectTransform))
            {
                _rectTransform.anchoredPosition -= delta;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_didDrag)
        {
            ChangeIsSelected(!_isSelected);
            OnSelect?.Invoke(this);
        }
    }

    private void ChangeIsSelected(bool newSelectedState)
    {
        _isSelected = newSelectedState;
        _nicerOutline.enabled = newSelectedState;
    }
}
