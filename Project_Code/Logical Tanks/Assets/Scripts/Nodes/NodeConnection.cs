using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

[RequireComponent(typeof(UILineRenderer))]
public class NodeConnection : MonoBehaviour
{
    private UILineRenderer _uILineRenderer;
    private RectTransform[] _linePoints;

    private void Awake()
    {
        _uILineRenderer = GetComponent<UILineRenderer>();
    }

    public void SetEndpoints(NodeConnectionPoint start, NodeConnectionPoint end)
    {
        _linePoints = new RectTransform[4];
        _linePoints[0] = start.GetRect();
        _linePoints[1] = start.GetTangentRect();
        _linePoints[2] = end.GetTangentRect();
        _linePoints[3] = end.GetRect();
    }

    private void Update()
    {
        // Must assign a new Vector2 to update lineRenderer
        Vector2[] points = new Vector2[_linePoints.Length];
        for(int i = 0; i < _linePoints.Length; i++)
        {
            Vector3 localSpacePosition = transform.InverseTransformPoint(_linePoints[i].position);
            points[i] = localSpacePosition;
        }
        _uILineRenderer.Points = points;
    }
}
