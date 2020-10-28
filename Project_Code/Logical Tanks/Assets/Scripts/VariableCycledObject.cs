using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableCycledObject : MonoBehaviour
{
    protected bool updateCycle = true;
    protected float speedMultiplier = 1;

    protected virtual void Update()
    { 
        if(updateCycle)
            Cycle();
    }

    protected virtual void Cycle() {  }
    public virtual void Stop() { updateCycle = false; }
    public virtual void Pause() { updateCycle = false; }
    public virtual void Play() { updateCycle = true; }
}
