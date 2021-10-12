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

    public override void OnAttached(Character character, int index)
    {
        
    }

    public override void Shoot(Character character, Transform transform, int index)
    {
        if (index <= 0) return;
        character.inventory[index - 1].Shoot(character, transform, index - 1);
    }
}
