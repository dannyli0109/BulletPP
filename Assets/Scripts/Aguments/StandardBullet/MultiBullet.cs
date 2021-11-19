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
        if (stats.explosiveRadius + tempStats.explosiveRadius > 0)
        {
            return (stats.explosiveRadius + tempStats.explosiveRadius) * tempStatMultipliers.explosiveRadius;
        }
        else if (character.nextShotIsExploded > 0) return character.nextShotIsExploded * tempStatMultipliers.explosiveRadius;
        else return 0;
    }

    public override float GetHomingRadius(Character character, int index)
    {
        if (stats.homingRadius > 0) return stats.homingRadius * tempStatMultipliers.homingRadius;
        else if (tempStats.homingRadius > 0) return tempStats.homingRadius * tempStatMultipliers.homingRadius;
        else if (character.nextShotIsHoming > 0) return character.nextShotIsHoming * tempStatMultipliers.homingRadius;
        else return 0;
    }
    public override int GetId(Character character, int index)
    {
        return id;
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
        float angles = GetAngles(character, index);
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
            initialAngle += stats.intialAngleOffset;
            angleIncrements = angles / (amounts - 1.0f);
        }


        AmmoPool ammoPool = AmmoPool.current;
        for (int i = 0; i < amounts; i++)
        {
            GenericBullet bullet;
            if (id == 10)
            {
                if (ammoPool.landmineBulletPool.TryInstantiate(out bullet, transform.position, transform.rotation))
                {
                    GenericBullet bulletComponent = bullet.GetComponent<GenericBullet>();
                    Vector3 forward = transform.forward;
                    //public virtual void Init(Character owner, Vector3 forward, float angle, float offset, float speed, Vector3 acceleration, float damage, float size, int bounces)


                    bulletComponent.Init(character, forward, initialAngle + angleIncrements * i, new Vector3(0, 0, 0), GetSpeed(character, index), new Vector3(0, 0, 0), GetDamage(character, index), GetSize(character, index), GetLifeTime(character, index), 0, false, GetExplosiveRadius(character, index), GetHomingRadius(character, index));

                }
            }
            else
            {

                if (ammoPool.bulletPool.TryInstantiate(out bullet, transform.position, transform.rotation))
                {
                    GenericBullet bulletComponent = bullet.GetComponent<GenericBullet>();
                    Vector3 forward = transform.forward;
                    //public virtual void Init(Character owner, Vector3 forward, float angle, float offset, float speed, Vector3 acceleration, float damage, float size, int bounces)


                    bulletComponent.Init(character, forward, initialAngle + angleIncrements * i, new Vector3(0, 0, 0), GetSpeed(character, index), new Vector3(0, 0, 0), GetDamage(character, index), GetSize(character, index), GetLifeTime(character, index), 0, false, GetExplosiveRadius(character, index), GetHomingRadius(character, index));

                }
            }
        }
        character.nextShotIsExploded = -1;
        character.nextShotIsHoming = -1;
        
    }


}
