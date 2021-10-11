using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Augments/MirrorBullet")]
public class MirrorBulletData : AugmentData
{

    public override Augment Create()
    {
        return new MirrorBullet(this);
    }
}