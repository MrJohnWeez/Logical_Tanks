using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableCycledObject : ResettableObject
{
    protected GameManager gameManager = null;

    protected virtual void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }
}
