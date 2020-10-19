using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeCompiler : MonoBehaviour
{
    [SerializeField] private Node _startNode = null;
    private Task _currentTask = null;
    private Node _currentNode = null;

    public void Play()
    {
        Debug.Log("Playing!");
        _currentNode = _startNode;
        _currentTask = new Task(_currentNode.Execute());
        _currentTask.OnFinished += NextNode;
    }

    private void NextNode(Task task, bool forceStopped)
    {
        if(!forceStopped)
        {
            _currentNode = _currentNode.NextNode();
            if(_currentNode != null)
            {
                Debug.Log("Running node: " + _currentNode.name);
                _currentTask = new Task(_currentNode.Execute());
                _currentTask.OnFinished += NextNode;
            }
            else { Debug.Log("Finished!"); }
        }
    }

    public void Stop()
    {
        Debug.Log("Stopped!");
        _currentTask.Stop();
    }
}
