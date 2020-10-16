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

    [SerializeField] private GameObject _nodeBridgePrefab = null;
    [SerializeField] private GameObject _nodeBridgesParent = null;
    [SerializeField] private GameObject _nodePrefab = null;
    [SerializeField] private GameObject _nodesParent = null;
    [SerializeField] private RectTransform _nodeSpawnPoint = null;

    private State _state = State.Idle;
    private List<NodeBridge> _currentNodeBridges = new List<NodeBridge>();
    private List<Node> _selectedNodes = new List<Node>();
    private RectTransform _scrollView;
    private RectTransform _contentWindow;
    private NodeCompiler _nodeCompiler = null;

    private void Awake()
    {
        _scrollView = GameObject.FindGameObjectWithTag("NodeScrollView").GetComponent<RectTransform>();
        _contentWindow = GameObject.FindGameObjectWithTag("ContentWindow").GetComponent<RectTransform>();
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

    public void NodeSelectionChanged(Node node)
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
                if (!_scrollView.GetWorldSapceRect().Contains(pointerPos) || !_contentWindow.Contains(_selectedNodes[i].GetRect))
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
            if (!_scrollView.GetWorldSapceRect().Contains(pointerPos) || !_contentWindow.Contains(node.GetRect))
            {
                node.GetRect.anchoredPosition -= delta;
            }
        }
    }

    public void DeleteSelectedNodes()
    {
        // Delete bridges, links, nodes that are selected
        for (int i = _selectedNodes.Count - 1; i >= 0; i--)
        {
            Node nodeToDelete = _selectedNodes[i];
            nodeToDelete.Delete();
            nodeToDelete?.SetIsSelected(false);
        }
    }

    public void AddNode()
    {
        GameObject newNode = Instantiate(_nodePrefab, _nodesParent.transform);
        RectTransform rt = newNode.GetComponent<RectTransform>();
        rt.localScale = _contentWindow.localScale;
        rt.position = _nodeSpawnPoint.position;
        newNode.transform.SetParent(_contentWindow, true);
    }

    public void ClearSelected()
    {
        for (int i = _selectedNodes.Count - 1; i >= 0; i--)
            _selectedNodes[i].SetIsSelected(false);
    }

    private NodeBridge CreateNewBridgeObject()
    {
        GameObject nodeConnectionObject = Instantiate(_nodeBridgePrefab, _nodeBridgesParent.transform);
        nodeConnectionObject.transform.position = _nodeBridgesParent.transform.position;
        return nodeConnectionObject?.GetComponent<NodeBridge>();
    }
}