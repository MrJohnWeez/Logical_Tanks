using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class NodeInteraction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform _contentWindow;
    private Canvas _canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private RectTransform _scrollView;
     void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        _canvas = GameObject.FindGameObjectWithTag("NodeCanvas").GetComponent<Canvas>();
        _scrollView = GameObject.FindGameObjectWithTag("NodeScrollView").GetComponent<RectTransform>();
        _contentWindow = GameObject.FindGameObjectWithTag("ContentWindow").GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnBeginDrag(PointerEventData eventdata)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(eventData != null)
        {
            Vector2 delta = eventData.delta / _canvas.scaleFactor / _contentWindow.localScale;
            rectTransform.anchoredPosition += delta;

            if(!GetWorldSapceRect(_scrollView).Contains(eventData.position) || !RectContainsAnother(_contentWindow, rectTransform))
            {
                rectTransform.anchoredPosition -= delta;
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

    }

    public static bool RectContainsAnother(RectTransform rct, RectTransform another)
    {
        Vector3[] rect = new Vector3[4];
        rct.GetWorldCorners(rect);
        Vector3[] anoth = new Vector3[4];
        another.GetWorldCorners(anoth);
        return anoth[0].x >= rect[0].x && anoth[0].y >= rect[0].y && anoth[2].x <= rect[2].x && anoth[2].y <= rect[2].y;
    }

    public static Rect GetWorldSapceRect(RectTransform rt)
    {
        var r = rt.rect;
        r.center = rt.TransformPoint(r.center);
        r.size = rt.TransformVector(r.size);
        return r;
    }
}
