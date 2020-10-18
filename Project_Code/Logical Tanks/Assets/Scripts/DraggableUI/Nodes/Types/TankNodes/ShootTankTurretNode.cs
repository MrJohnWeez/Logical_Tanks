using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTankTurretNode : TankNode
{
    public override IEnumerator Execute()
    {
        RefreshTankList();
        foreach(TankController tc in tankControllers)
        {
            Task newTask = new Task(tc.ShootTurret());
            newTask.OnFinished += FinishedTask;
            tasks.Add(newTask);
        }
        yield return new WaitUntil (() => tasks.Count == 0);
    }
}
