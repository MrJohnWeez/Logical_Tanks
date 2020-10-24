using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTankNode : TankNode
{    
    public override IEnumerator Execute()
    {
        RunNodeColor(true);
        RefreshTankList();
        float distance = floatSelection.GetValue();
        foreach(TankController tc in tankControllers)
        {
            Task newTask = new Task(tc.MoveTank(distance));
            newTask.OnFinished += FinishedTask;
            tasks.Add(newTask);
        }
        yield return new WaitUntil (() => tasks.Count == 0);
        RunNodeColor(false);
    }
}
