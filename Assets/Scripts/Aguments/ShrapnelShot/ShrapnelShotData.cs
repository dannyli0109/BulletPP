using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Augments/ShrapnelShot")]

public class ShrapnelShotData : AugmentData
{
    public int idToFind;

    public override Augment Create()
    {
        return new ShrapnelShot(this);
    }

}