using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

// TODO:
// Zoom buttons
// Set up game scripts
// Make POC tutorial level with all functionalities
// Test on WebGL

// Start making actual levels

// List of features:
// - **** Move Tank Node
// - **** Rotate Tank Node
// - **** Rotate Turret Node
// - **** Shoot Tank Node
// - **** Loop Node
// - **** JumpTo Node
// - **** ReRout Node
// - **** Pressure Plate that triggers for all
// - **** Pressure plate that triggers for tank color
// - **** Black Door raises and lowers dependent on power received
// - **** Logic gates (And,Or,Not)
// - **** Capacitor (Charges for time it is powered then discharges)
// - **** Turret will shoot but does not rotate (variable cooldown)

public class Node : DraggableUI
{
    public Action<Node> NodeSelectionChanged;
    public Action<Node, Vector2, Vector2> OnNodeDragged;
    public Action<Node> OnFinishedExecution;

    [Header("NodeBase")]
    [SerializeField] protected NodeLink inNodeLink = null;
    [SerializeField] protected NodeLink outNodeLink = null;
    protected NodeManager nodeManager;

    protected override void Awake()
    {
        base.Awake();
        nodeManager = GameObject.FindObjectOfType<NodeManager>();
        canvas = GameObject.FindGameObjectWithTag("InGameUI").GetComponent<Canvas>();
        contentWindow = GameObject.FindGameObjectWithTag("ContentWindow").GetComponent<RectTransform>();
        NodeSelectionChanged += nodeManager.NodeSelectionChanged;
        OnNodeDragged += nodeManager.NodeOnDrag;
        EnableNodeLinkInteractions(inNodeLink);
        EnableNodeLinkInteractions(outNodeLink);
    }

    protected virtual void OnDestroy()
    {
        DisableNodeLinkInteractions(inNodeLink);
        DisableNodeLinkInteractions(outNodeLink);
        NodeSelectionChanged -= nodeManager.NodeSelectionChanged;
        OnNodeDragged -= nodeManager.NodeOnDrag;
        DeleteBridges();
    }

    public override void OnDragged(Vector2 delta, PointerEventData eventData) { OnNodeDragged?.Invoke(this, delta, eventData.position); }
    public override void OnSelection(bool isNowSelected) { NodeSelectionChanged?.Invoke(this); }

    public virtual List<NodeBridge> GetAllBridges()
    {
        List<NodeBridge> nodeBridges = new List<NodeBridge>();
        if (outNodeLink)
        {
            foreach (NodeBridge nb in outNodeLink.Bridges)
            {
                if (!nodeBridges.Contains(nb)) { nodeBridges.Add(nb); }
            }
        }
        if (inNodeLink)
        {
            foreach (NodeBridge nb in inNodeLink.Bridges)
            {
                if (!nodeBridges.Contains(nb)) { nodeBridges.Add(nb); }
            }
        }
        return nodeBridges;
    }

    public void DeleteBridges()
    {
        List<NodeBridge> nodeBridges = GetAllBridges();
        for (int i = nodeBridges.Count - 1; i >= 0; i--)
        {
            Destroy(nodeBridges[i].gameObject);
        }
    }

    public void Delete()
    {
        if (isDeletable)
            Destroy(gameObject);
    }

    public virtual void Execute()
    {
        RunNodeColor(true);
        OnExecuteFinished();
    }

    public virtual void OnExecuteFinished()
    {
        RunNodeColor(false);
        OnFinishedExecution?.Invoke(this);
    }

    protected virtual void RunNodeColor(bool start)
    {
        if (start)
            ChangeColor(iterationColor);
        else
            ResetColor(iterationFadeTime);
    }

    public virtual Node NextNode() { return outNodeLink ? outNodeLink.GetNextNode(true) : null; }
    public virtual Node[] ValidateOutNodes()
    {
        return outNodeLink ? new Node[1]{outNodeLink.GetNextNode(false)} : null;
    }

    protected void EnableNodeLinkInteractions(NodeLink nodeLink)
    {
        if (nodeLink)
        {
            nodeLink.OnBeginDragEvent += nodeManager.NodeLinkDragStarted;
            nodeLink.OnDropEvent += nodeManager.NodeLinkDropped;
            nodeLink.OnEndDragEvent += nodeManager.NodeLinkDragEnded;
        }
    }

    protected void DisableNodeLinkInteractions(NodeLink nodeLink)
    {
        if (nodeLink)
        {
            nodeLink.OnBeginDragEvent -= nodeManager.NodeLinkDragStarted;
            nodeLink.OnDropEvent -= nodeManager.NodeLinkDropped;
            nodeLink.OnEndDragEvent -= nodeManager.NodeLinkDragEnded;
        }
    }
}
