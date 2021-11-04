using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Augments/ExplodingBullet")]
public class UnstableExplosionDevice_data : AugmentData
{

    public float explosiveRadius;

    public float explosiveRadiusOfNextShot;

    public override Augment Create()
    {
        return new UnstableExplosionDevice(this);
           // ExplodingBullet(this);
    }

}
