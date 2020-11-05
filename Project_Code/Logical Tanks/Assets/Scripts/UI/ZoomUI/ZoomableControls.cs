using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoomableControls : MonoBehaviour
{
    [SerializeField] protected Slider _zoomSlider = null;

    protected virtual void Awake() { _zoomSlider.onValueChanged.AddListener(OnSliderChanaged); }
    protected virtual void OnSliderChanaged(float newValue) { }
}
