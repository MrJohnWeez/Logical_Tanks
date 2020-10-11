using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeCompiler : MonoBehaviour
{
    List<Node> _connectedNodes = new List<Node>();
    [SerializeField] private Node _startNode = null;

    public void Play()
    {
        Debug.Log("Playing!");
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
