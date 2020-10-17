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
    private List<Task> _tasks = new List<Task>();

    public override IEnumerator Execute()
    {
        Debug.Log("MoveTankNode Execute()");
        RefreshTankList();
        foreach(TankController tc in _tankControllers)
        {
            //StartCoroutine(tc.Move(moveForward, power, durration));
            Task newTask = new Task(tc.Move(moveForward, power, durration));
            newTask.OnFinished += FinishedTask;
            _tasks.Add(newTask);
        }
        yield return new WaitUntil (() => _tasks.Count == 0);
    }

    private void FinishedTask(Task task, bool wasForceStopped)
    {
        task.OnFinished -= FinishedTask;
        _tasks.Remove(task);
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
