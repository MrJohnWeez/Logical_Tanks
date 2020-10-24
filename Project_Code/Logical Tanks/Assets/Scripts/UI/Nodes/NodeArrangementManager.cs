using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeArrangementManager : NodeMovementManager
{
    public enum NodeType
    {
        TankMove,
        TankRotate,
        TurretRotate,
        TurretShoot,
        JumpTo,
        Loop,
        Reroute
    }

    [Header("NodeArrangementManager")]
    [SerializeField] private GameObject[] _nodeBarButtons = null;
    [SerializeField] private GameObject[] _nodePrefabs = null;
    [SerializeField] private GameObject _nodeBar = null;
    [SerializeField] private GameObject _selectionBar = null;
    [SerializeField] private Button _deselectButton = null;
    [SerializeField] private Button _deleteButton = null;
    [SerializeField] private RectTransform _nodeSpawnPoint = null;
    [SerializeField] private NodeType _unlockedNodes = NodeType.TankMove;
    private Button[] _nodeSelectionButtons = null;

    protected override void Awake()
    {
        base.Awake();
        _nodeSelectionButtons = new Button[_nodeBarButtons.Length];
        for(int i = 0; i < _nodeBarButtons.Length; i++)
        {
            _nodeSelectionButtons[i] = _nodeBarButtons[i].GetComponent<Button>();
            NodeType nodeType = (NodeType)i;
            _nodeSelectionButtons[i].onClick.AddListener(() => AddNode(nodeType));
            _nodeBarButtons[i].SetActive(i <= (int)_unlockedNodes);
        }
        _deselectButton.onClick.AddListener(Deselect);
        _deleteButton.onClick.AddListener(DeleteSelected);
    }

    protected virtual void Start()
    {
        _selectionBar.SetActive(SelectedNodeCount > 0);
    }
    
    public void AddNode(NodeType nodeType)
    {
        GameObject newNode = Instantiate(_nodePrefabs[(int)nodeType], _nodesParent);
        RectTransform rt = newNode.GetComponent<RectTransform>();
        rt.localScale = _contentWindow.localScale;
        rt.position = _nodeSpawnPoint.position;
    }

    public void Deselect()
    {
        for (int i = _selectedNodes.Count - 1; i >= 0; i--) { _selectedNodes[i].SetIsSelected(false); }
    }

    public void DeleteSelected()
    {
        // Delete bridges, links, nodes that are selected
        for (int i = _selectedNodes.Count - 1; i >= 0; i--)
        {
            Node nodeToDelete = _selectedNodes[i];
            nodeToDelete.Delete();
            nodeToDelete?.SetIsSelected(false);
        }
    }

    public override void NodeSelectionChanged(Node node)
    {
        base.NodeSelectionChanged(node);
        _selectionBar.SetActive(SelectedNodeCount > 0);
    }

    protected void SetNodeBarActive(bool isActive)
    {
        _nodeBar.SetActive(isActive);
    }
}
