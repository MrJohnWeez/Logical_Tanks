using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

// TODO:
// Make Count loop node
// Make foreach node
// Make Function node
// Make dummy node
// Make nodes pretty
// Make better menu on how to add nodes
// Animate color of node lines and nodes to signify movement

// Add Pressure plates
// Add Wall black gates
// Functional on logic gates
// Add Capacitor

// Remove colored bullets
// Runtime tank ray from turret
// Make turrets

// Set goal to get all tanks within win area
// Set up game scripts
// Make POC tutorial level with all functionalities





// List of features:
// - Move Tank Node
// - Rotate Tank Node
// - Rotate Turret Node
// - Shoot Tank Node
// - Count loop Node
// - Function Node
// - ForEach Tank color Node
// - Dummy Node
// - Pressure Plate that triggers for all
// - Pressure plate that triggers for tank color
// - Black Door raises and lowers dependent on power received
// - Logic gates (And,Or,Not)
// - Capacitor (Charges for time it is powered then discharges)
// - Turret will shoot when tank detected but does not rotate (variable cooldown)

public class Node : DraggableUI
{
    public event Action<Node> NodeSelectionChanged;
    public event Action<Node, Vector2, Vector2> OnNodeDragged;

    [Header("NodeBase")]
    [SerializeField] protected NodeLink inNodeLink = null;
    [SerializeField] protected NodeLink[] outNodeLinks = null;
    protected NodeManager nodeManager;
    protected List<Task> tasks = new List<Task>();

    public override void Awake()
    {
        base.Awake();
        nodeManager = GameObject.FindObjectOfType<NodeManager>();
        canvas = GameObject.FindGameObjectWithTag("InGameUI").GetComponent<Canvas>();
        contentWindow = GameObject.FindGameObjectWithTag("ContentWindow").GetComponent<RectTransform>();
        NodeSelectionChanged += nodeManager.NodeSelectionChanged;
        OnNodeDragged += nodeManager.NodeOnDrag;
        foreach (NodeLink ncp in outNodeLinks)
        {
            ncp.OnBeginDragEvent += nodeManager.NodeLinkDragStarted;
            ncp.OnDropEvent += nodeManager.NodeLinkDropped;
            ncp.OnEndDragEvent += nodeManager.NodeLinkDragEnded;
        }
        if (inNodeLink)
        {
            inNodeLink.OnBeginDragEvent += nodeManager.NodeLinkDragStarted;
            inNodeLink.OnDropEvent += nodeManager.NodeLinkDropped;
            inNodeLink.OnEndDragEvent += nodeManager.NodeLinkDragEnded;
        }
    }

    public virtual void OnDestroy()
    {
        NodeSelectionChanged -= nodeManager.NodeSelectionChanged;
        OnNodeDragged -= nodeManager.NodeOnDrag;
        foreach (NodeLink ncp in outNodeLinks)
        {
            ncp.OnBeginDragEvent -= nodeManager.NodeLinkDragStarted;
            ncp.OnDropEvent -= nodeManager.NodeLinkDropped;
            ncp.OnEndDragEvent -= nodeManager.NodeLinkDragEnded;
        }
        if (inNodeLink)
        {
            inNodeLink.OnBeginDragEvent -= nodeManager.NodeLinkDragStarted;
            inNodeLink.OnDropEvent -= nodeManager.NodeLinkDropped;
            inNodeLink.OnEndDragEvent -= nodeManager.NodeLinkDragEnded;
        }
        DeleteBridges();
    }

    public override void OnDragged(Vector2 delta, PointerEventData eventData) { OnNodeDragged?.Invoke(this, delta, eventData.position); }
    public override void OnSelection(bool isNowSelected) { NodeSelectionChanged?.Invoke(this); }

    public List<NodeBridge> GetAllBridges()
    {
        List<NodeBridge> nodeBridges = new List<NodeBridge>();

        foreach (NodeLink nl in outNodeLinks)
        {
            foreach (NodeBridge nb in nl.Bridges)
            {
                if (!nodeBridges.Contains(nb))
                    nodeBridges.Add(nb);
            }
        }

        if (inNodeLink)
        {
            foreach (NodeBridge nb in inNodeLink.Bridges)
            {
                if (!nodeBridges.Contains(nb))
                    nodeBridges.Add(nb);
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

    public virtual IEnumerator Execute()
    {
        yield return null;
    }

    public virtual Node NextNode()
    {
        if (outNodeLinks != null && outNodeLinks.Length == 1)
        {
            return outNodeLinks[0].GetNextNode();
        }
        return null;
    }

    protected virtual void FinishedTask(Task task, bool wasForceStopped)
    {
        task.OnFinished -= FinishedTask;
        tasks.Remove(task);
    }
}
