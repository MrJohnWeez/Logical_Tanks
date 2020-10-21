using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatLoopNode : GotToFunctionNode
{
    [SerializeField] protected int loopTimes = 2;

    public override IEnumerator Execute()
    {
        int currentIteration = 0;
        while(currentIteration < loopTimes)
        {
            SetThenResetColor(iterationColor, iterationFadeTime);
            _currentNode = functionNodeLink.GetNextNode(true);
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
