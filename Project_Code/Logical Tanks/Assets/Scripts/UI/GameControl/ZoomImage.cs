using UnityEngine;
using UnityEngine.EventSystems;
 
public class ZoomImage : ZoomableControls, IScrollHandler
{
    [SerializeField] private float _minimumScale = 0.5f;
    [SerializeField] private float _initialScale = 1f;
    [SerializeField] private float _maximumScale = 3f;
    [SerializeField] private float _sensitivity = 0.1f;
    [SerializeField] private RectTransform _overrideCenter;
    [HideInInspector] private Vector3 _scale;
    private float _scaleValue = 1;
    private RectTransform _thisTransform;
    
 
    protected override void Awake()
    {
        base.Awake();
        _thisTransform = (RectTransform)transform;
        _scaleValue = _initialScale;
        _scale.Set(_scaleValue, _scaleValue, 1f);
        _thisTransform.localScale = _scale;
        _zoomSlider.minValue = -_maximumScale;
        _zoomSlider.maxValue = -_minimumScale;
        _zoomSlider.value = -_initialScale;
    }
 
    public void OnScroll(PointerEventData eventData)
    {
        Vector2 relativeMousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_thisTransform, Input.mousePosition, null, out relativeMousePosition);
        UpdateZoom(relativeMousePosition, eventData.scrollDelta.y * _sensitivity);
    }

    protected override void OnSliderChanaged(float newValue) { UpdateZoom(-newValue); }

    private void UpdateZoom(Vector2 zoomCenter, float delta)
    {
        float oldScale = _scaleValue;
        _scaleValue = Mathf.Clamp(_scaleValue + delta, _minimumScale, _maximumScale);
        float newDelta = _scaleValue - oldScale;
        _thisTransform.localScale = new Vector3(_scaleValue, _scaleValue, 1);
        _thisTransform.anchoredPosition -= (zoomCenter * newDelta);
        _zoomSlider.value = -_scaleValue;
    }

    private void UpdateZoom(float newScale)
    {
        Vector2 zoomPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_thisTransform, _overrideCenter.position, null, out zoomPos);
        float oldScale = _scaleValue;
        _scaleValue = Mathf.Clamp(newScale, _minimumScale, _maximumScale);
        _zoomSlider.value = -_scaleValue;
        float newDelta = _scaleValue - oldScale;
        _thisTransform.localScale = new Vector3(_scaleValue, _scaleValue, 1);
        _thisTransform.anchoredPosition -= (zoomPos * newDelta);
    }
}