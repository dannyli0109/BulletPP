using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Augments/SharpeningStone")]
public class SharpeningStoneData : AugmentData
{
    public int damageIncrement;
    public int damageIncrementTimes;
    public override Augment Create()
    {
        return new SharpeningStone(this);
    }

}
