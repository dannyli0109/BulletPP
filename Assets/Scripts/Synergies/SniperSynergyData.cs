using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Synergies/SniperSynergyData")]
public class SniperSynergyData : SynergyData
{
    public override Synergy Create()
    {
        return new SniperSynergy(this);
    }
}
