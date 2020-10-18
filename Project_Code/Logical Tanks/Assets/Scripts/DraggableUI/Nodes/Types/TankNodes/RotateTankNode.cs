﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTankNode : TankNode
{
    [Header("RotateTankNode")]
    [Range(1.0f,90.0f)]
    [SerializeField] private float _degrees = 90;
    
    public override IEnumerator Execute()
    {
        RefreshTankList();
        foreach(TankController tc in tankControllers)
        {
            Task newTask = new Task(tc.RotateTank(_degrees));
            newTask.OnFinished += FinishedTask;
            tasks.Add(newTask);
        }
        yield return new WaitUntil (() => tasks.Count == 0);
    }
}
