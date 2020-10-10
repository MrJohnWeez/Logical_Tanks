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
            if (nodeLink.IsValidStartLink())
            {
                // Visually start node bridge
                _state = State.ConnectingNodeLinks;
                GameObject nodeConnectionObject = Instantiate(_nodeBridgePrefab, _nodeBridgesParent.transform);
                nodeConnectionObject.transform.position = _nodeBridgesParent.transform.position;
                NodeBridge newBridge = nodeConnectionObject.GetComponent<NodeBridge>();
                newBridge.SetStartNodeLink(nodeLink);
                _currentNodeBridges.Add(newBridge);
            }
            else if (!nodeLink.IsOutNode)
            {
                // Disconnect bridges but keep them active
                _state = State.ConnectingNodeLinks;
                _currentNodeBridges = new List<NodeBridge>(nodeLink.Bridges);
                for(int i = 0; i < _currentNodeBridges.Count; i++)
                {
                    _currentNodeBridges[i].RemoveEndNodeLink();
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
                if (nodeLink.WillBridgeBeValid(_currentNodeBridges[i].StartNodeLink))
                {
                    _currentNodeBridges[i].SetEndNodeLink(nodeLink);
                    _currentNodeBridges.RemoveAt(i);
                }
            }
            _state = State.Idle;
        }
    }

    public void NodeLinkDragEnded(NodeLink nodeConnectionPoint)
    {
        for(int i = 0; i < _currentNodeBridges.Count; i++)
        {
            Destroy(_currentNodeBridges[i].gameObject);
        }
        _currentNodeBridges.Clear();
        _state = State.Idle;
    }
}