﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingBullet : Augment
{
    public PiercingBullet(PiercingBulletData data) : base(data)
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
        return (stats.explosiveRadius + tempStats.explosiveRadius) * tempStatMultipliers.explosiveRadius;
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

        SoundManager.PlaySound(SoundType.Gunshot, transform.position, 1, new List<string>() { "piercing" }, new List<float>() { amounts });

        AmmoPool ammoPool = AmmoPool.current;
        for (int i = 0; i < amounts; i++)
        {
            PiercingAmmo piercingAmmo;
            if (ammoPool.piercingAmmoPool.TryInstantiate(out piercingAmmo, transform.position, transform.rotation))
            {
                Vector3 forward = transform.forward;
             
                //piercingAmmo.Init(character, forward, initialAngle + angleIncrements * i, new Vector3(0, 0, 0), stats.speed, new Vector3(0, 0, 0), stats.damage, stats.size, stats.lifeTime, 0, false, character.nextShotIsExploded, -1);
                piercingAmmo.Init(character, forward, initialAngle + angleIncrements * i, new Vector3(0, 0, 0), GetSpeed(character, index), new Vector3(0, 0, 0), GetDamage(character, index), GetSize(character, index), GetLifeTime(character, index), 0, false, character.nextShotIsExploded, -1);


            }
        }
        character.nextShotIsExploded = -1;
    }
}
