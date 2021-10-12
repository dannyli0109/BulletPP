using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiBullet : Augment
{
    public MultiBullet(MultiBulletData data) : base(data)
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
            Bullet bullet;
            if (ammoPool.multiBulletPool.TryInstantiate(out bullet, transform.position, transform.rotation))
            {
                Bullet bulletComponent = bullet.GetComponent<Bullet>();
                Vector3 forward = transform.forward;
                bulletComponent.Init(character, forward, initialAngle + angleIncrements * i, speed, damage, size);
            }
        }
    }
}
