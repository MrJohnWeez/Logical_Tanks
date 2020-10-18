using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(NicerOutline))]
public class DraggableUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] protected bool isLocked = false;
    [SerializeField] protected bool isSelectable = true;
    [SerializeField] protected bool isDeletable = true;
    protected CanvasGroup canvasGroup = null;
    protected Canvas canvas = null;
    protected RectTransform contentWindow;
    protected NicerOutline _nicerOutline;
    private bool _isSelected = false;
    private bool _didDrag = false;
    private RectTransform _rectTransform;

    public RectTransform GetRect => _rectTransform;
    public bool IsSelected => _isSelected;

    public virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        _nicerOutline = GetComponent<NicerOutline>();
        _rectTransform = (RectTransform)transform;
    }

    public virtual void OnPointerDown(PointerEventData eventData) { _didDrag = false; }
    public virtual void OnBeginDrag(PointerEventData eventdata)
    {
        if (!isLocked)
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.6f;
            _didDrag = true;
        }
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

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (!isLocked)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
        }
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (!_didDrag && isSelectable)
        {
            SetIsSelected(!_isSelected);
        }
    }

    public virtual void SetIsSelected(bool newSelectedState)
    {
        _isSelected = newSelectedState;
        _nicerOutline.enabled = newSelectedState;
        OnSelection(_isSelected);
    }

    public virtual void OnDragged(Vector2 delta, PointerEventData eventData) { }
    public virtual void OnSelection(bool isNowSelected) { }
    protected virtual void SetHighlightColor(Color newColor) { _nicerOutline.effectColor = newColor; }
}
