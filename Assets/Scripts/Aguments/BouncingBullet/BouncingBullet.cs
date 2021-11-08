using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBullet : Augment
{
    public int numberOfBounces;


    public BouncingBullet(BouncingBulletData data) : base(data)
    {
        numberOfBounces = data.numberOfBounces;
    }
    public override int GetId(Character character, int index)
    {
        return id;
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
        return (int)(((int)stats.amountOfBullets + (int)tempStats.amountOfBullets) * tempStatMultipliers.amountOfBullets);
    }

    public override float GetAngles(Character character, int index)
    {
        return (stats.angles + tempStats.angles) * tempStatMultipliers.angles;
    }

    public override Color GetColor(Character character, int index)
    {
        return color;
    }

    public override float GetDamage(Character character, int index)
    {
        return (stats.damage + tempStats.damage) * tempStatMultipliers.damage;
    }

    public override float GetExplosiveRadius(Character character, int index)
    {
        return (stats.explosiveRadius + tempStats.explosiveRadius) * tempStatMultipliers.explosiveRadius;
    }


    public override float GetLifeTime(Character character, int index)
    {
        return (stats.lifeTime + tempStats.lifeTime) * tempStatMultipliers.lifeTime;
    }

    public override float GetSize(Character character, int index)
    {
        return (stats.size + tempStats.size) * tempStatMultipliers.size;
    }

    public override float GetSpeed(Character character, int index)
    {
        return (stats.speed + tempStats.speed) * tempStatMultipliers.speed;
    }

    public override void OnAttached(Character character, int index)
    {
        
    }

    public override void Shoot(Character character, Transform transform, int index)
    {
        //SoundManager.PlaySound(SoundType.Gunshot, transform.position, 1);

        float initialAngle = -stats.angles / 2.0f;
        float angleIncrements;
        float amounts = GetAmounts(character, index);

        if (amounts == 1)
        {
            angleIncrements = 0;
            initialAngle = 0;
        }
        else
        {
            angleIncrements = stats.angles / (amounts - 1.0f);
        }

        SoundManager.PlaySound(SoundType.Gunshot, transform.position, 1, new List<string>() { "bounce" }, new List<float>() { amounts });

        AmmoPool ammoPool = AmmoPool.current;
        for (int i = 0; i < amounts; i++)
        {
            Bullet bullet;

            if (ammoPool.bouncingBulletPool.TryInstantiate(out bullet, transform.position, transform.rotation))
            {
                Vector3 forward = transform.forward;
  
                bullet.Init(character, forward, initialAngle + angleIncrements * i, GetSpeed(character, index), GetDamage(character, index), GetSize(character, index), GetLifeTime(character, index), numberOfBounces, false, character.nextShotIsExploded, character.nextShotIsHoming);
            }
        }
        character.nextShotIsExploded = -1;
        character.nextShotIsHoming = 0;
    }
}
