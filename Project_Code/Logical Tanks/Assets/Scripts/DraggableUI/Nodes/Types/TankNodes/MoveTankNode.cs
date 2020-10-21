using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTankNode : TankNode
{
    [Header("MoveTankNode")]
    [Range(-5.0f,5.0f)]
    public float distance = 1;
    
    public override IEnumerator Execute()
    {
        RunNodeColor(true);
        RefreshTankList();
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
