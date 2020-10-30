using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeCompiler : NodeArrangementManager
{
    [Header("NodeCompiler")]
    [SerializeField] private GameObject _runButtonGO = null;
    [SerializeField] private GameObject _stepButtonGO = null;
    [SerializeField] private GameObject _continueButtonGO = null;
    [SerializeField] private GameObject _stopButtonGO = null;
    [SerializeField] private Node _startNode = null;
    [SerializeField] private GameObject _nodeUIBlocker = null;
    [SerializeField] private GameObject _playingOverlay = null;
    private Node _currentNode = null;
    private Button _runButton = null;
    private Button _stepButton = null;
    private Button _continueButton = null;
    private CanvasGroup _continueButtonCanvasGroup = null;
    private Button _stopButton = null;
    private bool _isStepping = false;
    private bool _isPlaying = false;
    private GameManager _gameManager = null;

    protected override void Awake()
    {
        base.Awake();
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        // Set up buttons
        _runButton = _runButtonGO.GetComponent<Button>();
        _stepButton = _stepButtonGO.GetComponent<Button>();
        _continueButton = _continueButtonGO.GetComponent<Button>();
        _continueButtonCanvasGroup = _continueButtonGO.GetComponent<CanvasGroup>();
        _stopButton = _stopButtonGO.GetComponent<Button>();
        _runButton.onClick.AddListener(Play);
        _stepButton.onClick.AddListener(Step);
        _continueButton.onClick.AddListener(Continue);
        _stopButton.onClick.AddListener(Stop);
    }

    protected override void Start()
    {
        if(!_isPlaying)
        {
            base.Start();
            ChangeUIToPlaying(false);
        }
    }

    public void Play()
    {
        ChangeUIToPlaying(true);
        _currentNode = _startNode;
        _currentNode.OnFinishedExecution += CurrentNodeFinished;
        _currentNode.Execute();
    }

    private void CurrentNodeFinished(Node nodeThatFinished)
    {
        if(nodeThatFinished)
        {
            nodeThatFinished.OnFinishedExecution -= CurrentNodeFinished;
            _currentNode = nodeThatFinished.NextNode();
            if(_currentNode)
            {
                _currentNode.OnFinishedExecution += CurrentNodeFinished;
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

    public void Step()
    {
        if(!_isPlaying)
        {
            Play();
        }
        //_currentNode?.Pause();
        _isStepping = true;
        EnableContinueButton(true);
    }

    public void Continue()
    {
        if(_isStepping)
        {
            _isStepping = false;
            //_currentNode?.Continue();
            EnableContinueButton(false);
        }
    }

    public void Stop()
    {
        _currentNode?.Stop();
        ChangeUIToPlaying(false);
    }

    private void ChangeUIToPlaying(bool isNowPlaying)
    {
        Deselect();
        ShowRunButton(!isNowPlaying);
        _isPlaying = isNowPlaying;
        _nodeUIBlocker.SetActive(isNowPlaying);
        _playingOverlay.SetActive(isNowPlaying);
        SetNodeBarActive(!isNowPlaying);
        if(_isStepping && !isNowPlaying)
        {
            EnableContinueButton(false);
        }
    }

    private void ShowRunButton(bool showRunButton)
    {
        _runButtonGO.SetActive(showRunButton);
        _stopButtonGO.SetActive(!showRunButton);
    }

    private void EnableContinueButton(bool isEnabled)
    {
        _continueButtonCanvasGroup.alpha = isEnabled ? 1 : 0.2f;
        _continueButtonCanvasGroup.interactable = isEnabled;
        _continueButtonCanvasGroup.blocksRaycasts = isEnabled;
    }
}
