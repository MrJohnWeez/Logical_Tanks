using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableCycledObject : MonoBehaviour
{
    protected GameManager gameManager = null;

    protected virtual void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    public virtual void Stop()
    { 
        //updateCycle = false; 
    }
    public virtual void Pause()
    {
        //updateCycle = false;
    }
    public virtual void Play()
    {
       //updateCycle = true;
    }
}
