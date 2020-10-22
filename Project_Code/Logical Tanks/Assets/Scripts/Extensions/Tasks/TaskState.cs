using UnityEngine;
using System.Collections;
using System;

public class TaskState
    {
        public bool Running { get { return running; } }
        public bool Paused { get { return paused; } }
        public Action<bool> OnFinished;
        private IEnumerator coroutine;
        private bool running;
        private bool paused;
        private bool stopped;

        public TaskState(IEnumerator c) { coroutine = c; }
        public void Pause() { paused = true; }
        public void Unpause() { paused = false; }

        public void Start()
        {
            running = true;
            TaskManager.singleton.StartCoroutine(CallWrapper());
        }

        public void Stop()
        {
            stopped = true;
            running = false;
        }

        private IEnumerator CallWrapper()
        {
            yield return null;
            IEnumerator e = coroutine;
            while (running)
            {
                if (paused)
                    yield return null;
                else
                {
                    if (e != null && e.MoveNext())
                    {
                        yield return e.Current;
                    }
                    else
                    {
                        running = false;
                    }
                }
            }

            Action<bool> handler = OnFinished;
            if (handler != null)
                handler(stopped);
        }
    }