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
        TimeWarp,
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
    [SerializeField] private Button _duplicateButton = null;
    [SerializeField] private RectTransform _nodeSpawnPoint = null;
    [SerializeField] private NodeType _unlockedNodes = NodeType.TankMove;
    [SerializeField] private float _positionVariation = 5.0f;
    private Button[] _nodeSelectionButtons = null;
    private int _addState = 0;
    private int _nodeUUID = 0;

    protected override void Awake()
    {
        base.Awake();
        _nodeSelectionButtons = new Button[_nodeBarButtons.Length];
        for (int i = 0; i < _nodeBarButtons.Length; i++)
        {
            _nodeSelectionButtons[i] = _nodeBarButtons[i].GetComponent<Button>();
            NodeType nodeType = (NodeType)i;
            _nodeSelectionButtons[i].onClick.AddListener(() => AddNode(nodeType));
            _nodeBarButtons[i].SetActive(i <= (int)_unlockedNodes);
        }
        _deselectButton.onClick.AddListener(Deselect);
        _deleteButton.onClick.AddListener(DeleteSelected);
        _duplicateButton.onClick.AddListener(DuplicateSelected);
    }

    protected virtual void Start()
    {
        _selectionBar.SetActive(SelectedNodeCount > 0);
    }

    public void AddNode(NodeType nodeType)
    {
        _nodeUUID++;
        GameObject newNode = Instantiate(_nodePrefabs[(int)nodeType], _nodesParent, false);
        newNode.name = string.Format("Node ({0})", _nodeUUID);
        RectTransform rt = newNode.GetComponent<RectTransform>();
        Vector3 newPos = _nodeSpawnPoint.position;
        newPos.x += _addState > 1 ? _positionVariation : -_positionVariation;
        newPos.y += _addState % 2 == 0 ? _positionVariation : -_positionVariation;
        rt.position = newPos;
        _addState++;
        if (_addState > 3)
            _addState = 0;
    }

    public void DuplicateSelected()
    {
        List<Node> duplicatedNodes = new List<Node>();
        for (int i = 0; i < _selectedNodes.Count; i++)
        {
            GameObject copyThisObject = _selectedNodes[i].gameObject;
            Node copyNode = copyThisObject.GetComponent<Node>();
            if(copyNode.IsDuplicateable)
            {
                _nodeUUID++;
                GameObject newNode = Instantiate(copyThisObject, _nodesParent, false);
                newNode.name = string.Format("Node ({0})", _nodeUUID);
                RectTransform rt = newNode.GetComponent<RectTransform>();
                Vector3 newPos = copyThisObject.transform.position;
                newPos.x += _positionVariation;
                newPos.y -= _positionVariation;
                rt.position = newPos;
                Node node = newNode.GetComponent<Node>();
                node.SetStartColor(copyNode.GetStartColor());
                duplicatedNodes.Add(node);
            }
        }
        Deselect();
        for (int i = 0; i < duplicatedNodes.Count; i++) { duplicatedNodes[i].SetIsSelected(true); }
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
