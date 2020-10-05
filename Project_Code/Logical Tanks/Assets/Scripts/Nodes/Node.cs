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
            ncp.OnPointerDownEvent += OnNodeConnectionClicked;
        }
    }

    private void OnDestroy()
    {
        foreach(NodeConnectionPoint ncp in _nodeConnectionPoints)
        {
            ncp.OnPointerDownEvent -= OnNodeConnectionClicked;
        }
    }

    private void OnNodeConnectionClicked(PointerEventData eventData, NodeConnectionPoint nodeConnectionPoint)
    {
        _nodeManager.NodeConnectionClicked(this, nodeConnectionPoint);
    }

    public void AddConnection(NodeConnection newConnection)
    {
        _nodeConnections.Add(newConnection);
    }
}
