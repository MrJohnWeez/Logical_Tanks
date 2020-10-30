using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionNode : Node
{
    [SerializeField] protected NodeLink functionNodeLink = null;
    protected Node _currentNode = null;

    protected override void Awake()
    {
        base.Awake();
        EnableNodeLinkInteractions(functionNodeLink);
    }

    protected override void OnDestroy()
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

    public override void Execute()
    {
        RunNodeColor(true);
        _currentNode = functionNodeLink.GetNextNode(true);
        ExecuteCurrentNode();
    }

    protected virtual void ExecuteCurrentNode()
    {
        if(_currentNode)
        {
            _currentNode.OnFinishedExecution += OnNodeFinished;
            _currentNode.Execute();
        }
        else { OnExecuteFinished(); }
    }

    protected virtual void OnNodeFinished(Node nodeThatFinished)
    {
        nodeThatFinished.OnFinishedExecution -= OnNodeFinished;
        _currentNode = nodeThatFinished.NextNode();
        ExecuteCurrentNode();
    }
}
