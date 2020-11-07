using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeMovementManager : MonoBehaviour
{
    public enum State
    {
        Idle,
        ConnectingNodeLinks,
        NodeSelected
    }

    [Header("NodeMovementManager")]
    [SerializeField] protected Transform _nodesParent = null;
    [SerializeField] private GameObject _nodeBridgePrefab = null;
    protected List<NodeBridge> _currentNodeBridges = new List<NodeBridge>();
    protected GameObject _nodeBridgesParent = null;
    protected List<Node> _selectedNodes = new List<Node>();
    protected RectTransform _scrollView;
    protected RectTransform _contentWindow;
    private State _state = State.Idle;

    public int SelectedNodeCount => _selectedNodes.Count;

    protected virtual void Awake()
    {
        _scrollView = GameObject.FindGameObjectWithTag("NodeScrollView").GetComponent<RectTransform>();
        _contentWindow = GameObject.FindGameObjectWithTag("ContentWindow").GetComponent<RectTransform>();
        int index = _nodesParent.GetSiblingIndex();
        _nodeBridgesParent = new GameObject("NodeBridgesParent");
        _nodeBridgesParent.transform.SetParent(_nodesParent.parent);
        _nodeBridgesParent.transform.position = _nodesParent.position;
        _nodeBridgesParent.transform.SetSiblingIndex(index);
    }

    public Node[] GetAllNodes()
    {
        return gameObject.GetComponentsInChildren<Node>();
    }

    public void NodeLinkDragStarted(NodeLink nodeLink)
    {
        if (_state == State.Idle)
        {
            if (!nodeLink.HasBridges)
            {
                // Connect one side of bridge
                _state = State.ConnectingNodeLinks;
                NodeBridge newBridge = CreateNewBridgeObject();
                if (nodeLink.IsOutNode)
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
                for (int i = 0; i < _currentNodeBridges.Count; i++)
                {
                    if (!nodeLink.IsOutNode)
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
            // Connect bridge(s) to nodeLink if valid
            for (int i = _currentNodeBridges.Count - 1; i >= 0; i--)
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

    public void NodeLinkDragEnded(NodeLink nodeLink)
    {
        // Delete any non-connected bridges
        for (int i = 0; i < _currentNodeBridges.Count; i++)
            _currentNodeBridges[i].Delete();
        _currentNodeBridges.Clear();
        _state = State.Idle;
    }

    public virtual void NodeSelectionChanged(Node node)
    {
        if (node.IsSelected)
            _selectedNodes.Add(node);
        else
            _selectedNodes.Remove(node);
    }

    public void NodeOnDrag(Node node, Vector2 delta, Vector2 pointerPos)
    {
        bool otherMovesWereValid = true;
        if (_selectedNodes.Count > 0)
        {
            // If all selected objects can be dragged move them
            for (int i = 0; i < _selectedNodes.Count; i++)
            {
                _selectedNodes[i].GetRect.anchoredPosition += delta;
                if (!RectTransformUtility.RectangleContainsScreenPoint(_scrollView, pointerPos) || !_contentWindow.Contains(_selectedNodes[i].GetRect))
                {
                    otherMovesWereValid = false;
                    // Undo all changed deltas
                    for (int y = i; y >= 0; y--)
                    {
                        _selectedNodes[y].GetRect.anchoredPosition -= delta;
                    }
                    break;
                }
            }
        }

        // Make sure to update actual dragging node if it was not selected
        if (otherMovesWereValid && (_selectedNodes.Count == 0 || !_selectedNodes.Contains(node)))
        {
            node.GetRect.anchoredPosition += delta;
            if (!RectTransformUtility.RectangleContainsScreenPoint(_scrollView, pointerPos) || !_contentWindow.Contains(node.GetRect))
            {
                node.GetRect.anchoredPosition -= delta;
            }
        }
    }

    private NodeBridge CreateNewBridgeObject()
    {
        GameObject nodeConnectionObject = Instantiate(_nodeBridgePrefab, _nodeBridgesParent.transform);
        nodeConnectionObject.transform.position = _nodeBridgesParent.transform.position;
        return nodeConnectionObject?.GetComponent<NodeBridge>();
    }
}
