using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrapnelShot : Augment
{
    public int idToFind;
    public ShrapnelShot(ShrapnelShotData data) : base(data) {
        idToFind = data.idToFind;
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
        int holdingShrapnel = 0;
        for (int i = 0; i < character.inventory.Count; i++)
        {
            if (character.inventory[i].GetId(character, i) == idToFind)
            {
                holdingShrapnel++;
            }
        }
        //return holdingShrapnel + (int)stats.amountOfBullets;

        int total = holdingShrapnel + (int)stats.amountOfBullets;
        return (int)((total + (int)tempStats.amountOfBullets) * tempStatMultipliers.amountOfBullets);
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
        //int holdingShrapnel = 0;
        //for (int i = 0; i < character.inventory.Count; i++)
        //{
        //    if (character.inventory[i].id == idToFind)
        //    {
        //        holdingShrapnel++;
        //    }
        //}

        //SoundManager.PlaySound(SoundType.Gunshot, transform.position, 1);
        //AmmoPool ammoPool = AmmoPool.current;
        //holdingShrapnel = 1 + holdingShrapnel * 2;
        //for (int i = 0; i < holdingShrapnel; i++)
        //{
        //    Shrapnel shrapnel;
        //    if (ammoPool.shrapnelAmmoPool.TryInstantiate(out shrapnel, transform.position, transform.rotation))
        //    {
        //        //Shrapnel shrapnelComponent = shrapnel.GetComponent<Shrapnel>();
        //        Vector3 forward = transform.forward;
        //        float holdingRandom = Random.Range(-2 * holdingShrapnel, 2 * holdingShrapnel);
        //        shrapnel.Init(character, forward, holdingRandom, speed, damage, size);
        //    }
        //}

        int amounts = GetAmounts(character, index);
        SoundManager.PlaySound(SoundType.Gunshot, transform.position, 1, new List<string>() { "blaster" }, new List<float>() { amounts });

        float angles = GetAngles(character, index);
        float initialAngle = -angles / 2.0f;
        float angleIncrements;

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
            GenericBullet shrapnel;
            if (ammoPool.shrapnelAmmoPool.TryInstantiate(out shrapnel, transform.position, transform.rotation))
            {
                Vector3 forward = transform.forward;

                // shrapnel.Init(character, forward, initialAngle + angleIncrements * i, speed, damage, size, lifeTime);
                //shrapnel.Init(character, forward, initialAngle + angleIncrements * i, new Vector3(0, 0, 0), stats.speed, new Vector3(0, 0, 0), stats.damage, stats.size, stats.lifeTime, 0, false, character.nextShotIsExploded, -1);
                shrapnel.Init(character, forward, initialAngle + angleIncrements * i, new Vector3(0, 0, 0), GetSpeed(character, index), new Vector3(0, 0, 0), GetDamage(character, index), GetSize(character, index), GetLifeTime(character, index), 0, false, GetExplosiveRadius(character, index), GetHomingRadius(character, index));


            }
        }
        character.nextShotIsExploded = -1;
        character.nextShotIsHoming = -1;
    }
}
