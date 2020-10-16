using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeCompiler : MonoBehaviour
{
    List<Node> _connectedNodes = new List<Node>();
    [SerializeField] private Node _startNode = null;
    [SerializeField] private GameObject _nodesParent = null;
    private Task _currentTask = null;

    public void Play()
    {
        Debug.Log("Playing!");
        _currentTask = new Task(_startNode.Execute());
        _currentTask.OnFinished += NextNode;
    }

    private void NextNode(Task task, bool forceStopped)
    {
        _currentTask = new Task(_startNode.Execute());
        _currentTask.OnFinished += NextNode;
    }

    public void DebugPlay()
    {

    }

    public void Step()
    {

    }

    public void Stop()
    {

    }
}
