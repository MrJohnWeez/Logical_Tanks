using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public enum State
    {
        Idle,
        ConnectingNodeLink,
        NodeSelected
    }

    [SerializeField] private GameObject _nodeBridgePrefab;
    [SerializeField] private GameObject _nodeBridgesParent;

    private State _state = State.Idle;
    private NodeLink _selectedNodeLink = null;
    private NodeBridge _currentNodeBridge;

    public void NodeLinkDragStarted(NodeLink nodeLink)
    {
        // If valid visually start node bridge
        if (_state == State.Idle && nodeLink.IsValidStartLink())
        {
            _state = State.ConnectingNodeLink;
            _selectedNodeLink = nodeLink;
            GameObject nodeConnectionObject = Instantiate(_nodeBridgePrefab, _nodeBridgesParent.transform);
            nodeConnectionObject.transform.position = _nodeBridgesParent.transform.position;
            _currentNodeBridge = nodeConnectionObject.GetComponent<NodeBridge>();
            _currentNodeBridge.SetTempConnection(_selectedNodeLink, nodeLink.GetDummyTransform());
        }
    }

    public void NodeLinkDropped(NodeLink nodeLink)
    {
        // If valid, visually and programatically finish node bridge
        if(_state == State.ConnectingNodeLink && nodeLink.IsValidEndLink(_selectedNodeLink))
        {
            _currentNodeBridge.SetConnection(_selectedNodeLink, nodeLink);
            _selectedNodeLink = null;
            _currentNodeBridge = null;
            _state = State.Idle;
        }
    }

    public void NodeLinkDragEnded(NodeLink nodeConnectionPoint)
    {
        // Remove node bridge if canceled
        if(_currentNodeBridge)
        {
            Destroy(_currentNodeBridge.gameObject);
            _selectedNodeLink = null;
            _state = State.Idle;
        }
    }
}
