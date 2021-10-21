﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBullet : Augment
{
    public float explosiveRadius;

    public ExplodingBullet(ExplodingBulletData data) : base(data)
    {
        explosiveRadius = data.explosiveRadius;
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
        
    }

    public override void Shoot(Character character, Transform transform, int index)
    {
        //SoundManager.PlaySound(SoundType.Gunshot, transform.position, 1);
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
        SoundManager.PlaySound(SoundType.Gunshot, transform.position, 1, new List<string>() { "rocket" }, new List<float>() { amounts });


        AmmoPool ammoPool = AmmoPool.current;
        for (int i = 0; i < amounts; i++)
        {
            Rocket rocket;
            if (ammoPool.rocketPool.TryInstantiate(out rocket, transform.position, transform.rotation))
            {
                Vector3 forward = transform.forward;
                rocket.Init(character, forward, initialAngle + angleIncrements * i, speed, damage, size, 0, false, true, explosiveRadius);
            }
        }
    }
}