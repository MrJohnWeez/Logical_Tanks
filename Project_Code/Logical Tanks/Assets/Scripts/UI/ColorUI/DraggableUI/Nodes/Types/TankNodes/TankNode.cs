using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankNode : Node
{
    [SerializeField] protected ColorSelection colorSelection = null;
    [SerializeField] protected FloatSelection floatSelection = null;
    protected List<TankController> tankControllers = new List<TankController>();
    
    protected virtual void RefreshTankList()
    {
        tankControllers.Clear();
        TankController[] controllers = GameObject.FindObjectsOfType<TankController>();
        ColorID tankColor = colorSelection.GetColorID();
        foreach(TankController tc in controllers)
        {
            if(tc.GetColorID == tankColor)
                tankControllers.Add(tc);
        }
    }
}
