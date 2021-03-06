﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class DraggableUI : ColoredImage, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] protected bool isLocked = false;
    [SerializeField] protected bool isSelectable = true;
    [SerializeField] protected bool isDeletable = true;
    [SerializeField] protected bool isDuplicateable = true;
    protected CanvasGroup canvasGroup = null;
    protected Canvas canvas = null;
    protected RectTransform contentWindow;
    private bool _isSelected = false;
    private bool _didDrag = false;
    private RectTransform _rectTransform;

    public RectTransform GetRect => _rectTransform;
    public bool IsSelected => _isSelected;
    public bool IsDuplicateable => isDuplicateable;

    protected override void Awake()
    {
        base.Awake();
        canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = (RectTransform)transform;
    }

    public virtual void OnPointerDown(PointerEventData eventData) { _didDrag = false; }
    public virtual void OnBeginDrag(PointerEventData eventdata)
    {
        canvasGroup.blocksRaycasts = isLocked;
        _didDrag = !isLocked;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (eventData != null && !isLocked)
        {
            Vector2 delta = eventData.delta;
            if (canvas) delta /= canvas.scaleFactor;
            if (contentWindow) delta /= contentWindow.localScale;
            OnDragged(delta, eventData);
        }
    }

    public virtual void OnEndDrag(PointerEventData eventData) { canvasGroup.blocksRaycasts = !isLocked; }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (!_didDrag && isSelectable) { SetIsSelected(!_isSelected); }
    }

    public virtual void SetIsSelected(bool newSelectedState)
    {
        _isSelected = newSelectedState;
        if (newSelectedState)
            ChangeColor(highlightColor);
        else
            ResetColor();
        OnSelection(_isSelected);
    }

    public virtual void OnDragged(Vector2 delta, PointerEventData eventData) { }
    public virtual void OnSelection(bool isNowSelected) { }
    
}
