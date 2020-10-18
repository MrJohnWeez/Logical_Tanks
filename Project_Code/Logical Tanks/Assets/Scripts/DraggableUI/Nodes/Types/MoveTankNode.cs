using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTankNode : Node
{
    [Header("MoveTankNode")]
    public ColorID tankColor = ColorID.Green;
    public bool moveForward = true;
    [Range(1,100)]
    public int power = 50;

    [Range(0.1f,5.0f)]
    public float durration = 1;
    private List<TankController> _tankControllers = new List<TankController>();
    
    public override IEnumerator Execute()
    {
        RefreshTankList();
        foreach(TankController tc in _tankControllers)
        {
            Task newTask = new Task(tc.Move(moveForward, power, durration));
            newTask.OnFinished += FinishedTask;
            tasks.Add(newTask);
        }
        yield return new WaitUntil (() => tasks.Count == 0);
    }

    private void RefreshTankList()
    {
        _tankControllers.Clear();
        TankController[] controllers = GameObject.FindObjectsOfType<TankController>();
        foreach(TankController tc in controllers)
        {
            if(tc.GetColorID == tankColor)
                _tankControllers.Add(tc);
        }
    }
}
