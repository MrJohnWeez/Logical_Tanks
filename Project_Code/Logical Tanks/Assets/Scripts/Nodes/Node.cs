using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    [SerializeField] private NodeConnectionPoint[] _nodeConnectionPoints;
    private List<NodeConnection> _nodeConnections = new List<NodeConnection>();
    private NodeManager _nodeManager;

    private void Awake()
    {
        _nodeManager = GameObject.FindObjectOfType<NodeManager>();
    }

    private void Start()
    {
        foreach(NodeConnectionPoint ncp in _nodeConnectionPoints)
        {
            ncp.OnBeginDragEvent += OnNodeConnectionPointDragged;
            ncp.OnDropEvent += OnNodeConnectionPointDropped;
            ncp.OnEndDragEvent += OnNodeConnectionPointEndDrag;
        }
    }

    private void OnDestroy()
    {
        foreach(NodeConnectionPoint ncp in _nodeConnectionPoints)
        {
            ncp.OnBeginDragEvent -= OnNodeConnectionPointDragged;
            ncp.OnDropEvent -= OnNodeConnectionPointDropped;
            ncp.OnEndDragEvent -= OnNodeConnectionPointEndDrag;
        }
    }

    private void OnNodeConnectionPointDragged(PointerEventData eventData, NodeConnectionPoint nodeConnectionPoint)
    {
        _nodeManager.NodeConnectionPointDragged(this, nodeConnectionPoint);
    }

    private void OnNodeConnectionPointDropped(PointerEventData eventData, NodeConnectionPoint nodeConnectionPoint)
    {
        _nodeManager.NodeConnectionPointDropped(this, nodeConnectionPoint);
    }

    private void OnNodeConnectionPointEndDrag(PointerEventData eventData, NodeConnectionPoint nodeConnectionPoint)
    {
        _nodeManager.NodeConnectionPointDragEnded(this, nodeConnectionPoint);
    }

    public void AddConnection(NodeConnection newConnection)
    {
        _nodeConnections.Add(newConnection);
    }
}
