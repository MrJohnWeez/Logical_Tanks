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
    private Node _currentNode = null;
    private Button _runButton = null;
    private Button _pauseButton = null;
    private Button _continueButton = null;
    private Button _stopButton = null;
    private CanvasGroup _stopButtonCanvasGroup = null;
    private GameManager _gameManager = null;
    private NodeCompilerState _nodeCompilerState = NodeCompilerState.Idle;
    private int _stackHeight = 0;

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
        SetCompilerState(NodeCompilerState.Running);
        _gameManager.ResetGameSpeed();
        _currentNode = _startNode;
        _currentNode.OnFinishedExecution += CurrentNodeFinished;
        _stackHeight++;
        _currentNode.Execute();
    }

    public virtual void Pause()
    {
        SetCompilerState(NodeCompilerState.Paused);
        _gameManager.Pause();
    }

    public virtual void Continue()
    {
        SetCompilerState(NodeCompilerState.Running);
        _gameManager.Continue();
    }

    public virtual void Stop()
    {
        SetCompilerState(NodeCompilerState.Idle);
        _gameManager.Stop();
    }

    public void ThrowError(string generalDesc, string advancedDesc)
    {
        // TODO: Make error menu
        Debug.Log("ThrowError: " + generalDesc + " : " + advancedDesc);
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
        if(_stackHeight > 10)
        {
            Debug.Log("State: " + _stackHeight);
            Debug.Break();
        }
        
        if (nodeThatFinished)
        {
            _stackHeight--;
            nodeThatFinished.OnFinishedExecution -= CurrentNodeFinished;
            _currentNode = nodeThatFinished.NextNode();
            if (_currentNode)
            {
                _currentNode.OnFinishedExecution += CurrentNodeFinished;
                _stackHeight++;
                _currentNode.Execute();
            }
            else
            {
                Stop();
            }

        }
        else
        {
            Stop();
        }
    }
}
