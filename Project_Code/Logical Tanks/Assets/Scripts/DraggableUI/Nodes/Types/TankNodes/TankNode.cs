using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankNode : Node
{
    public ColorID tankColor = ColorID.Green;
    protected List<TankController> tankControllers = new List<TankController>();

    protected virtual void RefreshTankList()
    {
        tankControllers.Clear();
        TankController[] controllers = GameObject.FindObjectsOfType<TankController>();
        foreach(TankController tc in controllers)
        {
            if(tc.GetColorID == tankColor)
                tankControllers.Add(tc);
        }
    }
}
