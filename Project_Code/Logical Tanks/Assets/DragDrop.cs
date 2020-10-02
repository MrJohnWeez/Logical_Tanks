using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class DragDrop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform scrollView;
    [SerializeField] private RectTransform contentWindow;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
     void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update() {
        Debug.Log("RectContainsAnother: " + RectContainsAnother(contentWindow, rectTransform));
        contentWindow.DebugDrawRect();
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
            Vector2 delta = eventData.delta / canvas.scaleFactor / contentWindow.localScale;
            rectTransform.anchoredPosition += delta;

            if(!GetWorldSapceRect(scrollView).Contains(eventData.position) || !RectContainsAnother(contentWindow, rectTransform))
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
