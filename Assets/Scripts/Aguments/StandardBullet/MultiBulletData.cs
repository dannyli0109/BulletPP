using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Augments/MultiBullet")]
public class MultiBulletData : AugmentData
{
    public float homingValue;
    public override Augment Create()
    {
        return new MultiBullet(this);
    }

}
