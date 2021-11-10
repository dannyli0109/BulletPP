using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorBullet : Augment
{
    public MirrorBullet(MirrorBulletData data) : base(data)
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
        //Debug.Log("mirror amount " + index);
        if (index <= 0)
        {
            return 0;
        }
        else
        {
            return character.inventory[index - 1].GetAmounts(character, index - 1);
        }
    }

    public override float GetAngles(Character character, int index)
    {
        if (index <= 0)
        {
            return (stats.angles + tempStats.angles) * tempStatMultipliers.angles;
        }
        else
        {
            return character.inventory[index - 1].GetAngles(character, index - 1);
        }
    }

    public override Color GetColor(Character character, int index)
    {
        if (index <= 0)
        {
            return color;
        }
        else
        {
            return character.inventory[index - 1].GetColor(character, index - 1);
        }
    }

    public override float GetDamage(Character character, int index)
    {
        if (index <= 0)
        {
            return 0;
        }
        else
        {
            return character.inventory[index - 1].GetDamage(character, index - 1);
        }
    }

    public override float GetExplosiveRadius(Character character, int index)
    {
        if (index <= 0)
        {
            return 0;
        }
        else
        {
            return character.inventory[index - 1].GetExplosiveRadius(character, index - 1);
        }
    }

    public override float GetHomingRadius(Character character, int index)
    {
        if (index <= 0)
        {
            return 0;
        }
        else
        {
            return character.inventory[index - 1].GetHomingRadius(character, index - 1);
        }
    }

    public override int GetId(Character character, int index)
    {
        if (index <= 0)
        {
            return id;
        }
        else
        {
            return character.inventory[index - 1].GetId(character, index - 1);
        }
    }

    public override float GetLifeTime(Character character, int index)
    {
        if (index <= 0)
        {
            return (stats.lifeTime + tempStats.lifeTime) * tempStatMultipliers.lifeTime;
        }
        else
        {
            return character.inventory[index - 1].GetLifeTime(character, index - 1);
        }
    }

    public override float GetSize(Character character, int index)
    {
        if (index <= 0)
        {
            return (stats.size + tempStats.size) * tempStatMultipliers.size;
        }
        else
        {
            return character.inventory[index - 1].GetSize(character, index - 1);
        }
    }

    public override float GetSpeed(Character character, int index)
    {
        if (index <= 0)
        {
            return (stats.speed + tempStats.speed) * tempStatMultipliers.speed;
        }
        else
        {
            return character.inventory[index - 1].GetSpeed(character, index - 1);
        }
    }

    public override void OnAttached(Character character, int index)
    {
        
    }

    public override void Shoot(Character character, Transform transform, int index)
    {
        if (index <= 0) return;
        character.inventory[index - 1].Shoot(character, transform, index - 1);
    }
}
