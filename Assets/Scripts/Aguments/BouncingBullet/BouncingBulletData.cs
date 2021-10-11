using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Augments/BouncingBullet")]
public class BouncingBulletData : AugmentData
{
    public int numberOfBounces;

    public override Augment Create()
    {
        return new BouncingBullet(this);
    }

   
}
