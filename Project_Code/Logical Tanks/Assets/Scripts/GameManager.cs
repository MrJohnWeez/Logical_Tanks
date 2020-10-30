using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float GameSpeed => _gameSpeed;
    [SerializeField] private float _gameSpeed = 1.0f;
    private float _oldGameSpeed = 1.0f;

    public void Pause()
    {
        _oldGameSpeed = _gameSpeed;
        _gameSpeed = 0;
    }

    public void Continue()
    {
        _gameSpeed = _oldGameSpeed;
    }

    public void Stop()
    {

    }
}
