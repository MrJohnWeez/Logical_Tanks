using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

[RequireComponent(typeof(UILineRenderer))]
public class NodeBridge : MonoBehaviour
{
    public NodeLink GetStartNodeLink => _startNodeLink;
    public NodeLink GetEndNodeLink => _endNodeLink;

    private const float PERCENTAGE_OF_DISTANCE = 0.4f;
    private const float MAX_DISTANCE = 640f;
    private UILineRenderer _uILineRenderer;
    private RectTransform _startRect = null;
    private RectTransform _endRect = null;
    private NodeLink _startNodeLink = null;
    private NodeLink _endNodeLink = null;
    private Vector3 _prevStartRectPos;
    private Vector3 _prevEndRectPos;

    private void Awake()
    {
        _uILineRenderer = GetComponent<UILineRenderer>();
    }

    public void Create(NodeLink startLink, NodeLink endLink)
    {
        _startNodeLink = startLink;
        _endNodeLink = endLink;
        startLink.AddNodeBridge(this);
        endLink.AddNodeBridge(this);
        _startRect = startLink.GetRect();
        _endRect = endLink.GetRect();
    }

    public void Remove()
    {
        _startNodeLink?.RemoveNodeBridge(this);
        _endNodeLink?.RemoveNodeBridge(this);
    }

    private void OnDestroy()
    {
        Remove();
    }

    public void SetTempBridge(NodeLink startLink)
    {
        _startNodeLink = startLink;
        _startRect = startLink.GetRect();
        _endRect = startLink.GetDummyTransform();
    }

    public void ReplaceWithTempBridge()
    {
        // Remove end connection but keep it active
        _endNodeLink?.RemoveNodeBridge(this);
        _endNodeLink = null;
        _endRect = _startNodeLink.GetDummyTransform();
    }

    private void Update()
    {
        // Determine if an update needs to happen
        if (_prevStartRectPos != _startRect.position || _prevEndRectPos != _endRect.position)
        {
            // Must assign a new Vector2 to update lineRenderer
            Vector2[] points = new Vector2[4];
            points[0] = transform.InverseTransformPoint(_startRect.position);
            points[3] = transform.InverseTransformPoint(_endRect.position);
            // Make bezier curve smooth
            float distance = Vector2.Distance(points[0], points[3]);
            distance = Mathf.Clamp(distance, 0, MAX_DISTANCE);
            Vector2 startLocal = Vector3.Normalize(transform.InverseTransformDirection(_startRect.up));
            points[1].x = points[0].x + distance * PERCENTAGE_OF_DISTANCE * startLocal.x;
            points[1].y = points[0].y + distance * PERCENTAGE_OF_DISTANCE * startLocal.y;
            Vector2 endLocal = Vector3.Normalize(transform.InverseTransformDirection(_endRect.up));
            points[2].x = points[3].x + distance * PERCENTAGE_OF_DISTANCE * endLocal.x;
            points[2].y = points[3].y + distance * PERCENTAGE_OF_DISTANCE * endLocal.y;
            // Set new points to render
            _uILineRenderer.Points = points;
            _prevStartRectPos = _startRect.position;
            _prevEndRectPos = _endRect.position;
        }
    }
}
