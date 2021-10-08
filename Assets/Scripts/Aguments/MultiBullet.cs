using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Augments/MultiBullet")]
public class MultiBullet : Augment
{
    public int damage;
    public float speed;
    public float size;
    public float angles;
    public int amountOfBullets;
    public override void Init()
    {
        InitEvents();
    }

    public override void Shoot(Character character, Transform transform)
    {
        SoundManager.PlaySound(SoundType.Gunshot, transform.position, 1);
        float initialAngle = -angles / 2.0f;
        float angleIncrements;

        if (amountOfBullets == 1)
        {
            angleIncrements = 0;
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
