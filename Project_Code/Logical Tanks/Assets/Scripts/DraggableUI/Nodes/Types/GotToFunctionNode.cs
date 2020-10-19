using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotToFunctionNode : Node
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

    public override IEnumerator Execute()
    {
        _currentNode = functionNodeLink.GetNextNode();
        while(_currentNode)
        {
            _currentTask = new Task(_currentNode.Execute());
            yield return new WaitUntil(() => !_currentTask.Running);
            _currentNode = _currentNode.NextNode();
        }
        yield return null;
    }
}
