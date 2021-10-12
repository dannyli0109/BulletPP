using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Augments/SpreadingDevice")]
public class SpreadingDeviceData : AugmentData
{
    public int bulletIncrement;
    public int bulletIncrementTimes;

    public override Augment Create()
    {
        return new SpreadingDevice(this);
    }
}
