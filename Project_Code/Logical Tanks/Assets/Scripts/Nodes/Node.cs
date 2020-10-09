using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    [SerializeField] private NodeLink[] _nodeLinks;
    private List<NodeBridge> _nodeConnections = new List<NodeBridge>();
    private NodeManager _nodeManager;

    private void Awake()
    {
        _nodeManager = GameObject.FindObjectOfType<NodeManager>();
    }

    private void Start()
    {
        foreach(NodeLink ncp in _nodeLinks)
        {
            ncp.OnBeginDragEvent += _nodeManager.NodeLinkDragStarted;
            ncp.OnDropEvent += _nodeManager.NodeLinkDropped;
            ncp.OnEndDragEvent += _nodeManager.NodeLinkDragEnded;
        }
    }

    private void OnDestroy()
    {
        foreach(NodeLink ncp in _nodeLinks)
        {
            ncp.OnBeginDragEvent -= _nodeManager.NodeLinkDragStarted;
            ncp.OnDropEvent -= _nodeManager.NodeLinkDropped;
            ncp.OnEndDragEvent -= _nodeManager.NodeLinkDragEnded;
        }
    }

    public void AddConnection(NodeBridge newConnection)
    {
        _nodeConnections.Add(newConnection);
    }
}
