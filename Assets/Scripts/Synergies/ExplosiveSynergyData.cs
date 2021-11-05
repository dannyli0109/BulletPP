using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Synergies/ExplosiveSynergy")]
public class ExplosiveSynergyData : SynergyData
{
    // Increase Explosive Radius by 50 / 100%;
    // Increase Explosive Damage by 50 / 100%;
    public override Synergy Create()
    {
        return new ExplosiveSynergy(this);
    }
}
