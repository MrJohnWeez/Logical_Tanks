using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

[RequireComponent(typeof(UILineRenderer))]
public class NodeBridge : MonoBehaviour
{
    public NodeLink StartNodeLink => _startNodeLink;
    public NodeLink EndNodeLink => _endNodeLink;

    private const float PERCENTAGE_OF_DISTANCE = 0.4f;
    private const float MAX_DISTANCE = 640f;
    private UILineRenderer _uILineRenderer;
    private RectTransform _startRect = null;
    private RectTransform _endRect = null;
    private NodeLink _startNodeLink = null;
    private NodeLink _endNodeLink = null;
    private Vector3 _prevStartPos;
    private Vector3 _prevEndPos;
    private Vector3 _currStartPos;
    private Vector3 _currEndPos;

    public void RemoveAllLinks()
    {
        RemoveStartNodeLink();
        RemoveEndNodeLink();
    }

    public void SetStartNodeLink(NodeLink startLink)
    {
        _startNodeLink = startLink;
        _startNodeLink.AddNodeBridge(this);
        _startRect = startLink.Rect;
    }

    public void SetEndNodeLink(NodeLink endLink)
    {
        _endNodeLink = endLink;
        _endNodeLink.AddNodeBridge(this);
        _endRect = _endNodeLink.Rect;
    }

    public void RemoveStartNodeLink()
    {
        if (_startNodeLink)
        {
            _startNodeLink.RemoveNodeBridge(this);
            _startNodeLink = null;
            _startRect = null;
        }
    }

    public void RemoveEndNodeLink()
    {
        if (_endNodeLink)
        {
            _endNodeLink.RemoveNodeBridge(this);
            _endNodeLink = null;
            _endRect = null;
        }
    }

    private void Awake()
    {
        _uILineRenderer = GetComponent<UILineRenderer>();
    }

    private void OnDestroy()
    {
        RemoveAllLinks();
    }

    private void Update()
    {
        // Use mouse pos if rect is missing
        _currStartPos = _startRect ? _startRect.position : Input.mousePosition;
        _currEndPos = _endRect ? _endRect.position : Input.mousePosition;

        // Determine if an update needs to happen
        if (_prevStartPos != _currStartPos || _prevEndPos != _currEndPos)
        {
            // Must assign a new Vector2 to update lineRenderer
            Vector2[] points = new Vector2[4];
            points[0] = transform.InverseTransformPoint(_currStartPos);
            points[3] = transform.InverseTransformPoint(_currEndPos);
            // Make bezier curve smooth
            float distance = Vector2.Distance(points[0], points[3]);
            distance = Mathf.Clamp(distance, 0, MAX_DISTANCE);
            Vector3 startUpDirection = _startRect ? _startRect.up : Vector3.down;
            Vector2 startLocal = Vector3.Normalize(transform.InverseTransformDirection(startUpDirection));
            points[1].x = points[0].x + distance * PERCENTAGE_OF_DISTANCE * startLocal.x;
            points[1].y = points[0].y + distance * PERCENTAGE_OF_DISTANCE * startLocal.y;
            Vector3 endUpDirection = _endRect ? _endRect.up : Vector3.up;
            Vector2 endLocal = Vector3.Normalize(transform.InverseTransformDirection(endUpDirection));
            points[2].x = points[3].x + distance * PERCENTAGE_OF_DISTANCE * endLocal.x;
            points[2].y = points[3].y + distance * PERCENTAGE_OF_DISTANCE * endLocal.y;
            // Set new points to render
            _uILineRenderer.Points = points;
            _prevStartPos = _currStartPos;
            _prevEndPos = _currEndPos;
        }
    }
}
