using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoalArea : MonoBehaviour
{
    public static Action OnTankEnter;
    public static Action OnTankExit;

    private void OnTriggerEnter(Collider other)
    {
        TankController tank = other.GetComponent<TankController>();
        if (tank) { OnTankEnter?.Invoke(); }
    }

    private void OnTriggerExit(Collider other)
    {
        TankController tank = other.GetComponent<TankController>();
        if (tank) { OnTankExit?.Invoke(); }
    }
}
