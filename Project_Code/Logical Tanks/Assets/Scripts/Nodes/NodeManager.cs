﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    [SerializeField] private GameObject _nodeConnection;
    [SerializeField] private GameObject _nodeConnectionsParent;

    private NodeConnectionPoint _lastSelectedConnectionPoint = null;
    private Node _lastSelectedConnectionNode = null;
    private Node _lastSelectedNode = null;
    private GameObject _nodeCanvas = null;
    
    private void Awake()
    {
        _nodeCanvas = GameObject.FindGameObjectWithTag("NodeCanvas");
    }

    public void NodeConnectionClicked(Node node, NodeConnectionPoint nodeConnectionPoint)
    {
        if (_lastSelectedConnectionNode == null)
        {
            _lastSelectedConnectionNode = node;
        }

        if (_lastSelectedConnectionPoint == null)
        {
            _lastSelectedConnectionPoint = nodeConnectionPoint;
        }
        else if (_lastSelectedConnectionPoint != nodeConnectionPoint && _lastSelectedConnectionNode != node)
        {
            // Connect nodes
            ConnectNodeConnectionPoints(_lastSelectedConnectionNode, _lastSelectedConnectionPoint, node, nodeConnectionPoint);
            _lastSelectedConnectionPoint = null;
            _lastSelectedConnectionNode = null;
        }
    }

    private void ConnectNodeConnectionPoints(Node firstNode,
                                                NodeConnectionPoint firstNodeConnectionPoint,
                                                Node secondNode,
                                                NodeConnectionPoint secondNodeConnectionPoint)
    {
        GameObject nodeConnectionObject = Instantiate(_nodeConnection, _nodeConnectionsParent.transform);
        nodeConnectionObject.transform.position = _nodeConnectionsParent.transform.position;

        NodeConnection nodeConnection = nodeConnectionObject.GetComponent<NodeConnection>();
        nodeConnection.SetEndpoints(firstNodeConnectionPoint, secondNodeConnectionPoint);
        firstNode.AddConnection(nodeConnection);
        secondNode.AddConnection(nodeConnection);

        Debug.Log("Set Up connection!");
    }
}