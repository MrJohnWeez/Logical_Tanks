using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTankTurretNode : TankNode
{
    public override void Execute()
    {
        base.Execute();
        _tankController?.ShootTurret();
    }
}
