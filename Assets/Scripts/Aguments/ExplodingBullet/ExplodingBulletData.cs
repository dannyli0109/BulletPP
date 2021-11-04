using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Augments/ExplodingBullet")]
public class ExplodingBulletData : AugmentData
{
    public float explosiveRadius;


    public override Augment Create()
    {
        return new ExplodingBullet(this);
    }

}
