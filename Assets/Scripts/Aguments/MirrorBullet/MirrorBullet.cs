using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorBullet : Augment
{
    public MirrorBullet(MirrorBulletData data) : base(data)
    {

    }

    public override string description
    {
        get
        {
            return originalDesc;
        }
    }

    public override void Shoot(Character character, Transform transform)
    {
        if (index <= 0) return;
        character.inventory[index - 1].index = index - 1;
        character.inventory[index - 1].Shoot(character, transform);
    }
}
