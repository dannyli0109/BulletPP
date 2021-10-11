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

    public override void Shoot(Character character, Transform transform)
    {
        SoundManager.PlaySound(SoundType.Gunshot, transform.position, 1);

        AmmoPool ammoPool = AmmoPool.current;
        Rocket rocket;
        if (ammoPool.rocketPool.TryInstantiate(out rocket, transform.position, transform.rotation))
        {
            // Debug.Log("explode");
            Vector3 forward = transform.forward;
            rocket.Init(character, forward, 0, speed, damage, size);
        }
    }
}
