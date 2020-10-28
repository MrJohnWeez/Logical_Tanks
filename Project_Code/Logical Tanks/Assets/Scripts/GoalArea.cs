using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoalArea : MonoBehaviour
{
    public Action<int> OnTankNumberChanged;
    public Action OnTankNumberCompleted;
    [SerializeField] private int _numberOfTanksToWin = 1;
    private int _numberOfTanks = 0;

    private void OnTriggerEnter(Collider other)
    {
        TankController tank = other.GetComponent<TankController>();
        if(tank)
        {
            _numberOfTanks++;
            CheckTankNumber();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        TankController tank = other.GetComponent<TankController>();
        if(tank)
        {
            _numberOfTanks--;
            CheckTankNumber();
        }
    }

    private void CheckTankNumber()
    {
        OnTankNumberChanged?.Invoke(_numberOfTanks);
        if(_numberOfTanks == _numberOfTanksToWin)
        {
            OnTankNumberCompleted?.Invoke();
            Debug.Log("You Won!");
        }
    }
}
