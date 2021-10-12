using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBullet : Augment
{
    public float explosiveRadius;

    public ExplodingBullet(ExplodingBulletData data) : base(data)
    {
        explosiveRadius = data.explosiveRadius;
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
        SoundManager.PlaySound(SoundType.Gunshot, transform.position, 1);
        float initialAngle = -angles / 2.0f;
        float angleIncrements;

        if (amountOfBullets == 1)
        {
            angleIncrements = 0;
            initialAngle = 0;
        }
        else
        {
            angleIncrements = angles / (amountOfBullets - 1.0f);
        }


        AmmoPool ammoPool = AmmoPool.current;
        for (int i = 0; i < amountOfBullets; i++)
        {
            Rocket rocket;
            if (ammoPool.rocketPool.TryInstantiate(out rocket, transform.position, transform.rotation))
            {
                Vector3 forward = transform.forward;
                rocket.Init(character, forward, initialAngle + angleIncrements * i, speed, damage, size);
            }
        }
    }
}
