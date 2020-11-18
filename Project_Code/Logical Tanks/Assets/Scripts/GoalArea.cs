using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoalArea : MonoBehaviour
{
    public static Action OnTankEnter;
    public static Action OnTankExit;
    private List<TankController> _enteredTanks = new List<TankController>();

    private void OnTriggerEnter(Collider other)
    {
        TankController tank = other.GetComponent<TankController>();
        if (tank)
        {
            _enteredTanks.Add(tank);
            OnTankEnter?.Invoke();
        }
    }

    private void Update()
    {
        for(int i = _enteredTanks.Count - 1; i >= 0; i--)
        {
            if(!_enteredTanks[i].IsReady)
            {
                _enteredTanks.RemoveAt(i);
                OnTankExit?.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        TankController tank = other.GetComponent<TankController>();
        if (tank)
        {
            _enteredTanks.Remove(tank);
            OnTankExit?.Invoke();
        }
    }
}
