/// TaskManager.cs
/// Copyright (c) 2011, Ken Rockot  <k-e-n-@-REMOVE-CAPS-AND-HYPHENS-oz.gs>.  All rights reserved.
/// Everyone is granted non-exclusive license to do anything at all with this code.
///
/// This is a new coroutine interface for Unity.
///
/// The motivation for this is twofold:
///
/// 1. The existing coroutine API provides no means of stopping specific
///    coroutines; StopCoroutine only takes a string argument, and it stops
///    all coroutines started with that same string; there is no way to stop
///    coroutines which were started directly from an enumerator.  This is
///    not robust enough and is also probably pretty inefficient.
///
/// 2. StartCoroutine and friends are MonoBehaviour methods.  This means
///    that in order to start a coroutine, a user typically must have some
///    component reference handy.  There are legitimate cases where such a
///    constraint is inconvenient.  This implementation hides that
///    constraint from the user.
///
/// Example usage:
///
/// ----------------------------------------------------------------------------
/// IEnumerator MyAwesomeTask()
/// {
///     while(true) {
///         Debug.Log("Logcat iz in ur consolez, spammin u wif messagez.");
///         yield return null;
////    }
/// }
///
/// IEnumerator TaskKiller(float delay, Task t)
/// {
///     yield return new WaitForSeconds(delay);
///     t.Stop();
/// }
///
/// void SomeCodeThatCouldBeAnywhereInTheUniverse()
/// {
///     Task spam = new Task(MyAwesomeTask());
///     new Task(TaskKiller(5, spam));
/// }
/// ----------------------------------------------------------------------------
///
/// When SomeCodeThatCouldBeAnywhereInTheUniverse is called, the debug console
/// will be spammed with annoying messages for 5 seconds.
///
/// Simple, really.  There is no need to initialize or even refer to TaskManager.
/// When the first Task is created in an application, a "TaskManager" GameObject
/// will automatically be added to the scene root with the TaskManager component
/// attached.  This component will be responsible for dispatching all coroutines
/// behind the scenes.
///
/// Task also provides an event that is triggered when the coroutine exits.

using UnityEngine;
using System.Collections;
using System;

/// A Task object represents a coroutine.  Tasks can be started, paused, and stopped.
/// It is an error to attempt to start a task that has been stopped or which has
/// naturally terminated.
public class Task
{
    public Action<Task, bool> OnFinished; // Action for termination subscribers. True if call to Stop() was made.
    public bool Running { get { return task.Running; } } // Returns true if coroutine is running. Paused tasks are considered to be running.
    public bool Paused { get { return task.Paused; } } // Returns true if coroutine is currently paused.
    private TaskState task;

    /// <summary>
    /// Creates a new Task object for the given coroutine.
    /// </summary>
    /// <param name="c"></param>
    /// <param name="autoStart">If autoStart is true (default) the task 
    /// is automatically started upon construction.</param>
    public Task(IEnumerator c, bool autoStart = true)
    {
        task = TaskManager.CreateTask(c);
        task.OnFinished += TaskFinished;
        if (autoStart)
            Start();
    }

    /// <summary>
    /// Begins execution of the coroutine
    /// </summary>
    public void Start()
    {
        task.Start();
    }

    /// <summary>
    /// Discontinues execution of the coroutine at its next yield.
    /// </summary>
    public void Stop()
    {
        task.Stop();
    }

    public void Pause()
    {
        task.Pause();
    }

    public void Unpause()
    {
        task.Unpause();
    }

    void TaskFinished(bool manual)
    {
        Action<Task, bool> handler = OnFinished;
        if (handler != null)
            handler(this, manual);
    }
}
