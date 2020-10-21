using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeCompiler : NodeManager
{
    [Header("NodeCompiler")]
    [SerializeField] private Node _startNode = null;
    [SerializeField] private GameObject _uiBlocker = null;
    private Task _currentTask = null;
    private Node _currentNode = null;

    public void Play()
    {
        _uiBlocker.SetActive(true);
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
                _currentTask = new Task(_currentNode.Execute());
                _currentTask.OnFinished += NextNode;
            }
            else
            {
                _uiBlocker.SetActive(false);
            }
        }
    }

    public void Stop()
    {
        Debug.Log("Stopped!");
        _currentTask.Stop();
        _uiBlocker.SetActive(false);
    }
}
