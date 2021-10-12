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

    public override void OnAttached(Character character, int index)
    {
        
    }

    public override void Shoot(Character character, Transform transform, int index)
    {
        int holdingShrapnel = 0;
        for (int i = 0; i < character.inventory.Count; i++)
        {
            if (character.inventory[i].id == idToFind)
            {
                holdingShrapnel++;
            }
        }

        SoundManager.PlaySound(SoundType.Gunshot, transform.position, 1);
        AmmoPool ammoPool = AmmoPool.current;
        holdingShrapnel = 1 + holdingShrapnel * 2;
        for (int i = 0; i < holdingShrapnel; i++)
        {
            Shrapnel shrapnel;
            if (ammoPool.shrapnelAmmoPool.TryInstantiate(out shrapnel, transform.position, transform.rotation))
            {
                //Shrapnel shrapnelComponent = shrapnel.GetComponent<Shrapnel>();
                Vector3 forward = transform.forward;
                float holdingRandom = Random.Range(-2 * holdingShrapnel, 2 * holdingShrapnel);
                shrapnel.Init(character, forward, holdingRandom, speed, damage, size);
            }
        }
    }
}
