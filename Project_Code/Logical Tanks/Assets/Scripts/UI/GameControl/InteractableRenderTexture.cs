using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(AspectRatioFitter))]
public class InteractableRenderTexture : ZoomableControls, IDragHandler, IScrollHandler
{
    public Action<Vector2> OnDragEvent;
    public Action<float> OnScrollEvent;
    public Action<float> OnZoomEvent;
    private RectTransform _selfRect = null;
    private CameraManager _cameraManager = null;

    protected override void Awake()
    {
        base.Awake();
        _selfRect = (RectTransform)transform;
        _zoomSlider.minValue = CameraManager.MIN_ZOOM;
        _zoomSlider.maxValue = CameraManager.MAX_ZOOM;
        _zoomSlider.value = CameraManager.MAX_ZOOM - CameraManager.MIN_ZOOM;
        _cameraManager = GameObject.FindObjectOfType<CameraManager>();
        _cameraManager.OnZoomChanged += UpdateSlider;
    }

    private void OnDestroy() { _cameraManager.OnZoomChanged -= UpdateSlider; }
    public void OnDrag(PointerEventData eventData) { OnDragEvent?.Invoke(eventData.delta); }
    public void OnScroll(PointerEventData eventData) { OnScrollEvent?.Invoke(eventData.scrollDelta.y); }
    
    public Vector2 ConvertToOrthoWorldSpace(Vector2 textureSpaceCords, Camera camera)
    {
        // TODO: This is a hack because it only works with square cameras
        float cameraDelta = camera.orthographicSize * 2;
        Rect worldRect = _selfRect.GetWorldSapceRect();
        float width = worldRect.xMax - worldRect.xMin;
        float height = worldRect.yMax - worldRect.yMin;
        textureSpaceCords.x = Remap(textureSpaceCords.x, -width, width, -cameraDelta, cameraDelta);
        textureSpaceCords.y = Remap(textureSpaceCords.y, -height, height, -cameraDelta, cameraDelta);
        return textureSpaceCords;
    }

    protected override void OnSliderChanaged(float newValue) { OnZoomEvent?.Invoke(newValue); }
    
    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    private void UpdateSlider(float newValue)
    {
        _zoomSlider.value = newValue;
    }
}
