using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTankNode : TankNode
{
    [Header("MoveTankNode")]
    [Range(0.1f,5.0f)]
    public float distance = 1;
    
    public override IEnumerator Execute()
    {
        SetHighlightColor(Color.yellow);
        _nicerOutline.enabled = true;
        RefreshTankList();
        foreach(TankController tc in tankControllers)
        {
            Task newTask = new Task(tc.MoveTank(distance));
            newTask.OnFinished += FinishedTask;
            tasks.Add(newTask);
        }
        yield return new WaitUntil (() => tasks.Count == 0);
        _nicerOutline.enabled = false;
    }
}
