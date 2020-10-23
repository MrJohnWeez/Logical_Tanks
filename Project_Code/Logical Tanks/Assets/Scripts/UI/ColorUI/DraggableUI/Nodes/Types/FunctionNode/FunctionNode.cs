using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionNode : Node
{
    [SerializeField] protected NodeLink functionNodeLink = null;
    protected Task _currentTask = null;
    protected Node _currentNode = null;

    public override void Awake()
    {
        base.Awake();
        EnableNodeLinkInteractions(functionNodeLink);
    }

    public override void OnDestroy()
    {
        DisableNodeLinkInteractions(functionNodeLink);
        base.OnDestroy();
    }

    public override List<NodeBridge> GetAllBridges()
    {
        List<NodeBridge> nodeBridges = base.GetAllBridges();
        if(nodeBridges == null)
            nodeBridges = new List<NodeBridge>();

        if (functionNodeLink)
        {
            foreach (NodeBridge nb in functionNodeLink.Bridges)
            {
                if (!nodeBridges.Contains(nb)) { nodeBridges.Add(nb); }
            }
        }
        return nodeBridges;
    }
}
