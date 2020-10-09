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
        if (_state == State.Idle)
        {
            if (nodeLink.IsValidStartLink())
            {
                // visually start node bridge
                _state = State.ConnectingNodeLink;
                GameObject nodeConnectionObject = Instantiate(_nodeBridgePrefab, _nodeBridgesParent.transform);
                nodeConnectionObject.transform.position = _nodeBridgesParent.transform.position;
                _currentNodeBridge = nodeConnectionObject.GetComponent<NodeBridge>();
                _currentNodeBridge.SetTempBridge(nodeLink);
                _selectedNodeLink = nodeLink;
            }
            else if (!nodeLink.IsOutNode())
            {
                if (nodeLink.BridgeCount() == 1)
                {
                    // Remove bridge but keep active
                    _state = State.ConnectingNodeLink;
                    _currentNodeBridge = nodeLink.GetSingleNodeBridge();
                    _currentNodeBridge.ReplaceWithTempBridge();
                    _selectedNodeLink = nodeLink;
                }
                else if (nodeLink.BridgeCount() > 1)
                {
                    // Remove bridge entirely
                    Destroy(_currentNodeBridge);
                }
            }
        }
    }

    public void NodeLinkDropped(NodeLink nodeLink)
    {
        // If valid, visually and programatically finish node bridge
        if (_state == State.ConnectingNodeLink && nodeLink.IsValidEndLink(_selectedNodeLink))
        {
            _currentNodeBridge.Create(_selectedNodeLink, nodeLink);
            _selectedNodeLink = null;
            _currentNodeBridge = null;
            _state = State.Idle;
        }
    }

    public void NodeLinkDragEnded(NodeLink nodeConnectionPoint)
    {
        // Remove node bridge if canceled
        if (_currentNodeBridge)
        {
            Destroy(_currentNodeBridge.gameObject);
            _selectedNodeLink = null;
            _state = State.Idle;
        }
    }
}
