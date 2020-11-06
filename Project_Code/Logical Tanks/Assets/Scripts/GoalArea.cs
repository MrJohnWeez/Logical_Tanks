using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoalArea : MonoBehaviour
{
    public static int CurrentTanks = 0;
    public static Action<int> OnTankNumberChanged;
    public static Action OnTankNumberCompleted;
    public static int NumberOfTanksToWin = 1;

    private void OnTriggerEnter(Collider other)
    {
        TankController tank = other.GetComponent<TankController>();
        if (tank)
        {
            CurrentTanks++;
            CheckTankNumber();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        TankController tank = other.GetComponent<TankController>();
        if (tank)
        {
            CurrentTanks--;
            CheckTankNumber();
        }
    }

    private void CheckTankNumber()
    {
        OnTankNumberChanged?.Invoke(CurrentTanks);
        if (CurrentTanks == NumberOfTanksToWin) { OnTankNumberCompleted?.Invoke(); }
    }
}
