using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Augments/StandardBullet")]
public class StandardBullet : Augment
{
    public int damage;
    public float speed;
    public float size;
    public override void Init()
    {
        InitEvents();
    }

    public override void Shoot(Character character, Transform transform)
    {
        AmmoPool ammoPool = AmmoPool.current;
        Bullet bullet;
        if (ammoPool.bulletPool.TryInstantiate(out bullet, transform.position, transform.rotation))
        {
            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            Vector3 forward = transform.forward;
            bulletComponent.Init(character, forward, 0, speed, damage, size);
        }
    }
}
