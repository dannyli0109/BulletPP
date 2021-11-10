using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Synergies/SpreadSynergyData")]
public class SpreadSynergyData : SynergyData
{
    public override Synergy Create()
    {
        return new SpreadSynergy(this);
    }
}
