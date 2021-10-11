using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Augments/ShrapnelShot")]

public class ShrapnelShotAugment : Augment
{
    public float damage;
    public float speed;
    public float size;
    public int idToFind;

    public override void Init()
    {
        InitEvents();
    }

    public override void Shoot(Character character, Transform transform)
    {
        int holdingShrapnel = 0;
        for(int i=0; i< character.inventory.Count; i++)
        {
            if(character.inventory[i].id== idToFind)
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
                Shrapnel shrapnelComponent = shrapnel.GetComponent<Shrapnel>();
                Vector3 forward = transform.forward;
                float holdingRandom = Random.RandomRange(-2 *holdingShrapnel, 2 * holdingShrapnel);
                shrapnelComponent.Init(character, forward, holdingRandom, speed, damage, size);
            }
        }
    }
}