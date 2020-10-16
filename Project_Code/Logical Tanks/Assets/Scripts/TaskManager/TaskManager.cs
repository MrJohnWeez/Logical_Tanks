using UnityEngine;
using System.Collections;
using System;
class TaskManager : MonoBehaviour
{
    public static TaskManager singleton;

    public static TaskState CreateTask(IEnumerator coroutine)
    {
        if (singleton == null)
        {
            GameObject go = new GameObject("TaskManager");
            singleton = go.AddComponent<TaskManager>();
        }
        return new TaskState(coroutine);
    }
}