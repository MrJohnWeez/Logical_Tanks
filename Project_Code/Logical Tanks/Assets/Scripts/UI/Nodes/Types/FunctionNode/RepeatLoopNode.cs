using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatLoopNode : FunctionNode
{
    [SerializeField] protected FloatSelection floatSelection = null;

    public override IEnumerator Execute()
    {
        int currentIteration = 0;
        int loopTimes = (int)floatSelection.GetValue();
        while(currentIteration < loopTimes)
        {
            if(currentIteration != 0)
                yield return new WaitForSeconds(0.5f);

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
