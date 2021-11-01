using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadingDevice : Augment
{
    public int bulletIncrement;
    public int bulletIncrementTimes;

    public SpreadingDevice(SpreadingDeviceData data) : base(data)
    {
        bulletIncrement = data.bulletIncrement;
        bulletIncrementTimes = data.bulletIncrementTimes;
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
        for (int i = 0; i < bulletIncrementTimes; i++)
        {
            if (indices.Count == 0) break;
            int randomAvaliableIndex = Random.Range(0, indices.Count);
            int randomIndex = indices[randomAvaliableIndex];
            character.inventory[randomIndex].amountOfBullets += bulletIncrement;
            OnAmmoChanged(randomIndex);
            indices.RemoveAt(randomAvaliableIndex);
        }
    }

    public override void Shoot(Character character, Transform transform, int index)
    {
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

        SoundManager.PlaySound(SoundType.Gunshot, transform.position, 1, new List<string>() { "blaster" }, new List<float>() { amounts });

        AmmoPool ammoPool = AmmoPool.current;
        for (int i = 0; i < amounts; i++)
        {
            GenericBullet bullet;
            if (ammoPool.multiBulletPool.TryInstantiate(out bullet, transform.position, transform.rotation))
            {
                Vector3 forward = transform.forward;
                bullet.Init(character, forward, initialAngle + angleIncrements * i, new Vector3(0, 0, 0), speed, new Vector3(0, 0, 0), damage, size, lifeTime, 0, false, character.nextShotIsExploded, -1);

            }
        }
    }
}
