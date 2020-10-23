using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTankTurretNode : TankNode
{    
    public override IEnumerator Execute()
    {
        RunNodeColor(true);
        RefreshTankList();
        float degrees = floatSelection.GetValue();
        foreach(TankController tc in tankControllers)
        {
            Task newTask = new Task(tc.RotateTurret(degrees));
            newTask.OnFinished += FinishedTask;
            tasks.Add(newTask);
        }
        yield return new WaitUntil (() => tasks.Count == 0);
        RunNodeColor(false);
    }
}
