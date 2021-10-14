﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpeningStone : Augment
{
    public float damageIncrement;
    public int damageIncrementTimes;

    public SharpeningStone(SharpeningStoneData data) : base(data)
    {
        damageIncrement = data.damageIncrement;
        damageIncrementTimes = data.damageIncrementTimes;
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
        return amountOfBullets;
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
        if (character.inventory.Count <= 1) return;
        List<int> indices = new List<int>();
        for (int i = 0; i < character.inventory.Count; i++)
        {
            if (i == index) continue;
            indices.Add(i);
        }
        for (int i = 0; i < damageIncrementTimes; i++)
        {
            if (indices.Count == 0) break;
            int randomAvaliableIndex = Random.Range(0, indices.Count);
            int randomIndex = indices[randomAvaliableIndex];
            character.inventory[randomIndex].damage += damageIncrement;
            indices.RemoveAt(randomAvaliableIndex);
        }
    }

    public override void Shoot(Character character, Transform transform, int index)
    {
        SoundManager.PlaySound(SoundType.Gunshot, transform.position, 1);
        float initialAngle = -angles / 2.0f;
        float angleIncrements;
        float amounts = GetAmounts(character, index);

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
