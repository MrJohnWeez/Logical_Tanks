using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public Action<float> OnGameSpeedChanged;
    public float GameSpeed => _gameSpeed;
    private float _gameSpeed = 0;
    private float _oldGameSpeed = 1.0f;
    private float _normalGameSpeed = 1.0f;

    public void ResetGameSpeed()
    {
        SetGameSpeed(_normalGameSpeed);
    }

    public void Pause()
    {
        _oldGameSpeed = _gameSpeed;
        SetGameSpeed(0);
    }

    public void Continue()
    {
        _gameSpeed = _oldGameSpeed;
        SetGameSpeed(_gameSpeed);
    }

    public void Stop()
    {
        ResettableObject[] resetObjects = GameObject.FindObjectsOfType<ResettableObject>(true);
        for (int i = resetObjects.Length - 1; i >= 0; i--) { resetObjects[i].ResetObject(); }
        SetGameSpeed(0);
    }

    public void SetGameSpeed(float newSpeed)
    {
        _gameSpeed = newSpeed;
        OnGameSpeedChanged?.Invoke(_gameSpeed);
    }
}
