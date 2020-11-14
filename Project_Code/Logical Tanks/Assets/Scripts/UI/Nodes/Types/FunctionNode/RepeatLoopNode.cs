using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatLoopNode : FunctionNode
{
    [SerializeField] protected FloatSelection floatSelection = null;
    private int _currentIteration = 0;
    private int _loopTimes = 0;

    public override void Execute()
    {
        _currentIteration = 0;
        _loopTimes = (int)floatSelection.GetValue();
        ExecuteFunctionNode();
    }

    protected override void OnNodeFinished(Node nodeThatFinished)
    {
        if(nodeThatFinished)
        {
            nodeThatFinished.OnFinishedExecution -= OnNodeFinished;
            _currentNode = nodeThatFinished.NextNode();
        }
        else {  _currentNode = null; }
        ExecuteCurrentNode();
    }

    protected override void ExecuteCurrentNode()
    {
        if(_currentNode)
        {
            _currentNode.OnFinishedExecution += OnNodeFinished;
            _currentNode.Execute();
        }
        else
        {
            _currentIteration++;
            if(_currentIteration < _loopTimes)
            {
               ExecuteFunctionNode();
            }
            else
            {
                OnExecuteFinished();
            }
        }
    }

    protected virtual void ExecuteFunctionNode()
    {
        RunNodeColor(true);
        _currentNode = functionNodeLink.GetNextNode(true);
        if(_currentNode)
        {
            _currentNode.OnFinishedExecution += OnNodeFinished;
            _currentNode.Execute();
        }
        else
        {
            OnNodeFinished(null);
        }
    }
}
