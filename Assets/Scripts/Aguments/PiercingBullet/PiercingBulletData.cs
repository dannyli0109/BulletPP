using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Augments/PiercingBullet")]

public class PiercingBulletData : AugmentData
{ 

    public override Augment Create()
    {
        return new PiercingBullet(this);
    }

}

