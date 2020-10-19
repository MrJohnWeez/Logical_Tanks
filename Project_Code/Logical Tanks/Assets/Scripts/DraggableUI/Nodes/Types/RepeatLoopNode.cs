using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatLoopNode : Node
{
    [SerializeField] protected NodeLink loopNodeLink = null;
    [SerializeField] protected int loopTimes = 2;
    private Task _currentTask = null;
    private Node _currentNode = null;

    public override void Awake()
    {
        base.Awake();
        EnableNodeLinkInteractions(loopNodeLink);
    }

    public override void OnDestroy()
    {
        DisableNodeLinkInteractions(loopNodeLink);
    }

    public override IEnumerator Execute()
    {
        int currentIteration = 0;
        while(currentIteration < loopTimes)
        {
            _currentNode = loopNodeLink.GetNextNode();
            while(_currentNode)
            {
                _currentTask = new Task(_currentNode.Execute());
                yield return new WaitUntil(() => !_currentTask.Running);
                _currentNode = _currentNode.NextNode();
            }
            currentIteration++;
        }
        yield return null;
    }
}
