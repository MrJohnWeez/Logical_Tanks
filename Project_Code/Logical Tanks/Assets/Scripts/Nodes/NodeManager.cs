using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public enum State
    {
        Idle,
        ConnectingNodeLinks,
        NodeSelected
    }

    [SerializeField] private GameObject _nodeBridgePrefab;
    [SerializeField] private GameObject _nodeBridgesParent;

    private State _state = State.Idle;
    private List<NodeBridge> _currentNodeBridges = new List<NodeBridge>();

    public void NodeLinkDragStarted(NodeLink nodeLink)
    {
        if (_state == State.Idle)
        {
            if (!nodeLink.HasBridges)
            {
                // Connect one side of bridge
                _state = State.ConnectingNodeLinks;
                NodeBridge newBridge = CreateNewBridgeObject();
                if(nodeLink.IsOutNode)
                    newBridge.SetStartNodeLink(nodeLink);
                else
                    newBridge.SetEndNodeLink(nodeLink);
                _currentNodeBridges.Add(newBridge);
            }
            else
            {
                // Disconnect one side of bridge
                _state = State.ConnectingNodeLinks;
                _currentNodeBridges = new List<NodeBridge>(nodeLink.Bridges);
                for(int i = 0; i < _currentNodeBridges.Count; i++)
                {
                    if(!nodeLink.IsOutNode)
                        _currentNodeBridges[i].RemoveEndNodeLink();
                    else
                        _currentNodeBridges[i].RemoveStartNodeLink();
                }
            }
        }
    }

    public void NodeLinkDropped(NodeLink nodeLink)
    {
        if (_state == State.ConnectingNodeLinks)
        {
            for(int i = _currentNodeBridges.Count - 1; i >= 0 ; i--)
            {
                if (!nodeLink.IsOutNode && nodeLink.WillBridgeBeValid(_currentNodeBridges[i].StartNodeLink))
                {
                    _currentNodeBridges[i].SetEndNodeLink(nodeLink);
                    _currentNodeBridges.RemoveAt(i);
                }
                else if (nodeLink.IsOutNode && !nodeLink.HasBridges && nodeLink.WillBridgeBeValid(_currentNodeBridges[i].EndNodeLink))
                {
                    _currentNodeBridges[i].SetStartNodeLink(nodeLink);
                    _currentNodeBridges.RemoveAt(i);
                }
            }
            _state = State.Idle;
        }
    }

    public void NodeLinkDragEnded(NodeLink nodeConnectionPoint)
    {
        for(int i = 0; i < _currentNodeBridges.Count; i++)
            Destroy(_currentNodeBridges[i].gameObject);
        _currentNodeBridges.Clear();
        _state = State.Idle;
    }

    private NodeBridge CreateNewBridgeObject()
    {
        GameObject nodeConnectionObject = Instantiate(_nodeBridgePrefab, _nodeBridgesParent.transform);
        nodeConnectionObject.transform.position = _nodeBridgesParent.transform.position;
        return nodeConnectionObject?.GetComponent<NodeBridge>();
    }
}