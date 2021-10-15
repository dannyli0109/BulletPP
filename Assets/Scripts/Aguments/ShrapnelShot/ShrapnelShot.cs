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
        return holdingShrapnel + amountOfBullets;
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
        SoundManager.PlaySound(SoundType.Gunshot, transform.position, 1);

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
            Shrapnel shrapnel;
            if (ammoPool.shrapnelAmmoPool.TryInstantiate(out shrapnel, transform.position, transform.rotation))
            {
                Vector3 forward = transform.forward;

                shrapnel.Init(character, forward, initialAngle + angleIncrements * i, speed, damage, size);
            }
        }

    }
}
