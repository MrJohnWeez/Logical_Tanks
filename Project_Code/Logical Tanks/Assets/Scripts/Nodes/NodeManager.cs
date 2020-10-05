using System.Collections;
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
    private bool _lookingForEndNode = false;
    private NodeConnection _currentNodeConnection;
    
    private void Awake()
    {
        _nodeCanvas = GameObject.FindGameObjectWithTag("NodeCanvas");
    }

    public void NodeConnectionPointDragged(Node node, NodeConnectionPoint nodeConnectionPoint)
    {
        if (_lastSelectedConnectionNode == null && nodeConnectionPoint.IsOutNode())
        {
            _lastSelectedConnectionNode = node;
        }

        if (_lastSelectedConnectionPoint == null && nodeConnectionPoint.IsOutNode())
        {
            _lastSelectedConnectionPoint = nodeConnectionPoint;
            _lookingForEndNode = true;

            GameObject nodeConnectionObject = Instantiate(_nodeConnection, _nodeConnectionsParent.transform);
            nodeConnectionObject.transform.position = _nodeConnectionsParent.transform.position;

            _currentNodeConnection = nodeConnectionObject.GetComponent<NodeConnection>();
            _currentNodeConnection.SetEndpoints(_lastSelectedConnectionPoint.GetRect(), nodeConnectionPoint.GetDummyTransform());
            Debug.Log("Dragging connection!");
        }
    }

    public void NodeConnectionPointDropped(Node node, NodeConnectionPoint nodeConnectionPoint)
    {
        if(_lookingForEndNode && _lastSelectedConnectionPoint != nodeConnectionPoint && _lastSelectedConnectionNode != node && !nodeConnectionPoint.IsOutNode())
        {
            _currentNodeConnection.SetEndpoints(_lastSelectedConnectionPoint.GetRect(), nodeConnectionPoint.GetRect());

            _lastSelectedConnectionPoint.isConnected = true;
            nodeConnectionPoint.isConnected = true;

            _lastSelectedConnectionNode.AddConnection(_currentNodeConnection);
            node.AddConnection(_currentNodeConnection);

            _lastSelectedConnectionPoint = null;
            _lastSelectedConnectionNode = null;
            _currentNodeConnection = null;
            _lookingForEndNode = false;
            Debug.Log("Set Up connection!");
        }
    }

    public void NodeConnectionPointDragEnded(Node node, NodeConnectionPoint nodeConnectionPoint)
    {
        if(_currentNodeConnection)
        {
            Destroy(_currentNodeConnection.gameObject);
            _lastSelectedConnectionPoint = null;
            _lastSelectedConnectionNode = null;
            _lookingForEndNode = false;
        }
    }
}
