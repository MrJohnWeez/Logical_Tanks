using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotToFunctionNode : FunctionNode
{
    public override IEnumerator Execute()
    {
        SetThenResetColor(iterationColor, 1.0f);
        _currentNode = functionNodeLink.GetNextNode(true);
        while(_currentNode)
        {
            _currentTask = new Task(_currentNode.Execute());
            yield return new WaitUntil(() => !_currentTask.Running);
            _currentNode = _currentNode.NextNode();
        }
        yield return null;
    }
}
