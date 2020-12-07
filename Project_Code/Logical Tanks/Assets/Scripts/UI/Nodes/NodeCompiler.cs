using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeCompiler : NodeArrangementManager
{
    public enum NodeCompilerState
    {
        Idle,
        Running,
        Paused
    }

    [Header("NodeCompiler")]
    [SerializeField] private GameObject _runButtonGO = null;
    [SerializeField] private GameObject _pauseButtonGO = null;
    [SerializeField] private GameObject _continueButtonGO = null;
    [SerializeField] private GameObject _stopButtonGO = null;
    [SerializeField] private Node _startNode = null;
    [SerializeField] private GameObject _nodeUIBlocker = null;
    [SerializeField] private GameObject _playingOverlay = null;
    [SerializeField] private GameObject _infLoopError = null;
    [SerializeField] private GameObject _tankCollisionError = null;
    private Node _currentNode = null;
    private Button _runButton = null;
    private Button _pauseButton = null;
    private Button _continueButton = null;
    private Button _stopButton = null;
    private CanvasGroup _stopButtonCanvasGroup = null;
    private GameManager _gameManager = null;
    private NodeCompilerState _nodeCompilerState = NodeCompilerState.Idle;
    private Node _lastInvalidNode = null;

    protected override void Awake()
    {
        base.Awake();
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        // Set up buttons
        _runButton = _runButtonGO.GetComponent<Button>();
        _pauseButton = _pauseButtonGO.GetComponent<Button>();
        _continueButton = _continueButtonGO.GetComponent<Button>();
        _stopButtonCanvasGroup = _stopButtonGO.GetComponent<CanvasGroup>();
        _stopButton = _stopButtonGO.GetComponent<Button>();
        _runButton.onClick.AddListener(Run);
        _pauseButton.onClick.AddListener(Pause);
        _continueButton.onClick.AddListener(Continue);
        _stopButton.onClick.AddListener(Stop);
    }

    protected override void Start()
    {
        base.Start();
        SetCompilerState(NodeCompilerState.Idle);
    }

    public virtual void Run()
    {
        if(_nodeCompilerState != NodeCompilerState.Running)
        {
            Stop();
            SetCompilerState(NodeCompilerState.Running);
            _gameManager.ResetGameSpeed();
            if(IsNodeStructureValid(_startNode))
            {
                _currentNode = _startNode;
                _currentNode.OnFinishedExecution += CurrentNodeFinished;
                _currentNode.Execute();
            }
            else
            {
                SpawnError(_infLoopError);
                Stop();
                _lastInvalidNode.SetIsSelected(true);
                _lastInvalidNode = null;
            }
        }
    }

    public virtual void Pause()
    {
        if(_nodeCompilerState == NodeCompilerState.Running)
        {
            SetCompilerState(NodeCompilerState.Paused);
            _gameManager.Pause();
        }
    }

    public virtual void Continue()
    {
        if(_nodeCompilerState == NodeCompilerState.Paused)
        {
            SetCompilerState(NodeCompilerState.Running);
            _gameManager.Continue();
        }
    }

    public virtual void Stop()
    {
        if(_nodeCompilerState != NodeCompilerState.Idle)
        {
            Node[] allNodes = GetAllNodes();
            foreach(Node n in allNodes) { n.OnFinishedExecution = null; }
            SetCompilerState(NodeCompilerState.Idle);
            _gameManager.Stop();
        }
    }

    public void SpawnTankCollisionError() { SpawnError(_tankCollisionError); }

    private void SpawnError(GameObject errorPrefab)
    {
        GameObject errorMessage = Instantiate(errorPrefab, transform.parent, false);
        Animator animator = errorMessage.GetComponent<Animator>();
        animator.SetTrigger("Show");
        Destroy(errorMessage, 8);
    }
    private void SetCompilerState(NodeCompilerState newState)
    {
        _nodeCompilerState = newState;
        UpdateUIElements();
    }

    private void UpdateUIElements()
    {
        // Update menu elements to match current compiler state
        Deselect();
        _nodeUIBlocker.SetActive(_nodeCompilerState != NodeCompilerState.Idle);
        _playingOverlay.SetActive(_nodeCompilerState != NodeCompilerState.Idle);
        SetNodeBarActive(_nodeCompilerState == NodeCompilerState.Idle);
        _runButtonGO.SetActive(_nodeCompilerState == NodeCompilerState.Idle);
        _pauseButtonGO.SetActive(_nodeCompilerState == NodeCompilerState.Running);
        _continueButtonGO.SetActive(_nodeCompilerState == NodeCompilerState.Paused);
        _stopButtonCanvasGroup.interactable = _nodeCompilerState != NodeCompilerState.Idle;
        _stopButtonCanvasGroup.alpha = _nodeCompilerState != NodeCompilerState.Idle ? 1 : 0.2f;
    }

    private void CurrentNodeFinished(Node nodeThatFinished)
    {
        if(_currentNode == nodeThatFinished)
        {
            if (nodeThatFinished)
            {
                nodeThatFinished.OnFinishedExecution -= CurrentNodeFinished;
                _currentNode = nodeThatFinished.NextNode();
                if (_currentNode)
                {
                    _currentNode.OnFinishedExecution += CurrentNodeFinished;
                    _currentNode.Execute();
                }
            }
        }
    }

    private bool IsNodeStructureValid(Node startNode, Node blackListedNode = null)
    {
        Node checkThisNode = startNode;
        List<Node> visitedNodes = new List<Node>();
        if(blackListedNode != null) { visitedNodes.Add(blackListedNode); }
        while(checkThisNode != null)
        {
            if(visitedNodes.Contains(checkThisNode))
            {
                _lastInvalidNode = checkThisNode;
                return false;
            }
            visitedNodes.Add(checkThisNode);
            Node[] outNodes = checkThisNode.ValidateOutNodes();
            if(outNodes.Length > 1)
            {
                for(int i = 1; i < outNodes.Length; i++)
                {
                    if(!IsNodeStructureValid(outNodes[i], checkThisNode)) { return false; }
                }
            }
            checkThisNode = outNodes[0];
        }
        return true;
    }
}
