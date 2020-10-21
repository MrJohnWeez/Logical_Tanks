using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTankTurretNode : TankNode
{
    [Header("RotateTankTurretNode")]
    [Range(-90.0f,90.0f)]
    [SerializeField] private float _degrees = 90;
    
    public override IEnumerator Execute()
    {
        RunNodeColor(true);
        RefreshTankList();
        foreach(TankController tc in tankControllers)
        {
            Task newTask = new Task(tc.RotateTurret(_degrees));
            newTask.OnFinished += FinishedTask;
            tasks.Add(newTask);
        }
        yield return new WaitUntil (() => tasks.Count == 0);
        RunNodeColor(false);
    }
}
