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

    public override int GetAmounts(Character character, int index)
    {
        return amountOfBullets;
    }

    public override Color GetColor(Character character, int index)
    {
        return color;
    }

    public override float GetDamage(Character character, int index)
    {
        return damage;
    }

    public override int GetId(Character character, int index)
    {
        return id;
    }

    public override void OnAttached(Character character, int index)
    {
        
    }

    public override void Shoot(Character character, Transform transform, int index)
    {
        float initialAngle = -angles / 2.0f;
        float angleIncrements;
        int amounts = GetAmounts(character, index);
        SoundManager.PlaySound(SoundType.Gunshot, transform.position, 1, new List<string>() { "blaster" }, new List<float>() { amounts });

        if (amounts == 1)
        {
            angleIncrements = 0;
            initialAngle = 0;
        }
        else
        {
            angleIncrements = angles / (amounts - 1.0f);
        }


        AmmoPool ammoPool = AmmoPool.current;
        for (int i = 0; i < amounts; i++)
        {
            GenericBullet bullet;
            if (ammoPool.multiBulletPool.TryInstantiate(out bullet, transform.position, transform.rotation))
            {
                GenericBullet bulletComponent = bullet.GetComponent<GenericBullet>();
                Vector3 forward = transform.forward;
                //public virtual void Init(Character owner, Vector3 forward, float angle, float offset, float speed, Vector3 acceleration, float damage, float size, int bounces)

                bulletComponent.Init(character, forward, initialAngle + angleIncrements * i, speed, damage, size, 0, false, false, 0) ;
            }
        }
    }
}
