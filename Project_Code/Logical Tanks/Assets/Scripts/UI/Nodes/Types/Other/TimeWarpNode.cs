using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeWarpNode : Node
{
    [SerializeField] protected TimeWarpSelection timeWarpSelection = null;

    public override void Execute()
    {
        RunNodeColor(true);
        float newSpeed = timeWarpSelection.GetValue();
        if (newSpeed == 0) { nodeManager.Pause(); }
        else { Time.timeScale = newSpeed; }
        OnExecuteFinished();
    }
}
