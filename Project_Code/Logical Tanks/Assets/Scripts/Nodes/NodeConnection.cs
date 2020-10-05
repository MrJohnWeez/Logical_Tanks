using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

[RequireComponent(typeof(UILineRenderer))]
public class NodeConnection : MonoBehaviour
{
    private const float PERCENTAGE_OF_DISTANCE = 0.4f;
    private const float MAX_DISTANCE = 640f;

    private UILineRenderer _uILineRenderer;
    private RectTransform _start;
    private RectTransform _end;

    private void Awake()
    {
        _uILineRenderer = GetComponent<UILineRenderer>();
    }

    public void SetEndpoints(RectTransform start, RectTransform end)
    {
        _start = start;
        _end = end;
    }

    private void Update()
    {
        // Must assign a new Vector2 to update lineRenderer
        Vector2[] points = new Vector2[4];

        points[0] = transform.InverseTransformPoint(_start.position);
        points[3] = transform.InverseTransformPoint(_end.position);

        // Make bezier curve smooth
        float distance = Vector2.Distance(points[0], points[3]);
        distance = Mathf.Clamp(distance, 0, MAX_DISTANCE);

        Vector2 startLocal = Vector3.Normalize(transform.InverseTransformDirection(_start.up));
        points[1].x = points[0].x + distance * PERCENTAGE_OF_DISTANCE * startLocal.x;
        points[1].y = points[0].y + distance * PERCENTAGE_OF_DISTANCE * startLocal.y;

        Vector2 endLocal = Vector3.Normalize(transform.InverseTransformDirection(_end.up));
        points[2].x = points[3].x + distance * PERCENTAGE_OF_DISTANCE * endLocal.x;
        points[2].y = points[3].y + distance * PERCENTAGE_OF_DISTANCE * endLocal.y;

        _uILineRenderer.Points = points;
    }
}
