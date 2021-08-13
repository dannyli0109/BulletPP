using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using UnityEngine;

public enum StatType
{
    Bullet,
    Grenade,
    Rocket,
    Laser
}

[Serializable]
public class AmmoStats
{
    public CharacterStat damage;
    public CharacterStat speed;
    public CharacterStat size;
    public CharacterStat critChance;
    public CharacterStat travelTime;
    public CharacterStat fireRate;
    public CharacterStat amountOfBounces;
    public CharacterStat maxClip;
}

[Serializable]
public class BulletStats : AmmoStats
{

}
[Serializable]
public class GrenadeStats : AmmoStats
{
    public CharacterStat BounceAdditionSpeed;
}
[Serializable]
public class RocketStats : AmmoStats
{
    public CharacterStat acceleration;
}

[Serializable]
public class LaserStats : AmmoStats
{
    public CharacterStat maxLaserLength;
    public CharacterStat maxLaserWidth;
}

[Serializable]
public class CharacterStats
{
    public CharacterStat maxHp;
    public CharacterStat moveSpeed;
    public CharacterStat reloadTime;
    public CharacterStat timeBetweenShots;
    public CharacterStat damageMultiplier;
    public CharacterStat dashAmount;
    public CharacterStat timeBetweenDashs;
    public CharacterStat immunityFromDashing;
    public CharacterStat immunityFromDamage;
    public CharacterStat outOfCombatReloadTime;
}


public abstract class Character : MonoBehaviour
{
    public CharacterStats stats;
    public BulletStats bulletStats;
    public GrenadeStats grenadeStats;
    public RocketStats rocketStats;
    public LaserStats laserStats;

    public Animator animator;
    public GameObject bulletPrefab;
    public GameObject grenadePrefab;
    public GameObject rocketPrefab;
    public GameObject laserPrefab;
    public Transform bulletContainer;

    public float hp;
    public float gold = 0;
    public List<Augment> augments = new List<Augment>();
    public float timeSinceFired;

    public bool reloading;
    public float currentImmunityFrame;

    // TODO: Move this else where
    #region Lazer
    public LineRenderer thisLineRenderer;
    #endregion
    public virtual void Start()
    {
        hp = stats.maxHp.value;
        timeSinceFired = 0;
        EventManager.current.onAmmoHit += OnAmmoHit;
        EventManager.current.onLaserHit += OnLaserHit;
    }

    public virtual void Update()
    {
        if (hp <= 0)
        {
            EventManager.current.ReceiveGold(this.gold);
            Destroy(gameObject);
        }
    }

    protected void OnAmmoHit(Ammo ammo, GameObject gameObject)
    {
        //Debug.Log(ammo);
        if (this.gameObject == gameObject)
        {
            if (ammo.owner != this)
            {
                if (currentImmunityFrame <= 0)
                {
                    hp -= ammo.GetDamage();
                }
                else
                {
                    Debug.Log("near miss");
                }
               // EventManager.current.OnAmmoDestroy(ammo.gameObject);
            }
        }
    }

    protected void OnLaserHit(float damage, Character owner, GameObject gameObjectInput)
    {
        Debug.Log("Input " +gameObjectInput);
        if (this.gameObject == gameObjectInput)
        {
            if (currentImmunityFrame <= 0)
            {
                if (owner != this)
                {
                    hp -= damage;
                    Debug.Log("deal damage to " + gameObjectInput);
                }
            }
            else
            {
                Debug.Log("near miss");
            }
            // EventManager.current.OnAmmoDestroy(ammo.gameObject);
        }
    }

    public virtual void Shoot()
    {

        GameObject bullet = Instantiate(bulletPrefab, bulletContainer);
        bullet.transform.SetParent(null);
        Vector3 scale = bullet.transform.localScale;
        bullet.transform.localScale = scale * bulletStats.size.value;
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        bulletComponent.owner = this;
    }

    public int BuyAugment(int id)
    {
        AugmentManager augmentManager = AugmentManager.current;
        AugmentData augment = augmentManager.augmentDatas[id];
        float cost = augmentManager.costs[augment.rarity];

        if (gold < cost)
        {
            return -1;
        }
        for (int i = 0; i < augments.Count; i++)
        {
            if (augments[i].id == id)
            {
                if (augments[i].count >= 9)
                {
                    return -1;
                }
                augments[i].count += 1;
                gold -= cost;
                return 1;
            }
        }
        augments.Add(new Augment() { id = id, count = 1 });
        gold -= cost;
        return 2;
    }

    public void AddModifier(string type, string stat, StatModifier modifier)
    {
        StatType statType;
        if (Enum.TryParse(type, out statType))
        {
            AddModifier(statType, stat, modifier);
        }
    }

    public void AddModifier(StatType type, string stat, StatModifier modifier)
    {
        CharacterStat result;
        switch (type)
        {
            case StatType.Bullet:
                {
                    result = (CharacterStat)bulletStats.GetType().GetField(stat).GetValue(bulletStats);
                    break;
                }
            case StatType.Grenade:
                {
                    result = (CharacterStat)grenadeStats.GetType().GetField(stat).GetValue(grenadeStats);
                    break;
                }
            case StatType.Rocket:
                {
                    result = (CharacterStat)rocketStats.GetType().GetField(stat).GetValue(rocketStats);
                    break;
                }
            case StatType.Laser:
            {
                    result = (CharacterStat)laserStats.GetType().GetField(stat).GetValue(laserStats);
                    break;
            }
            default:
                result = null;
                break;
        }

        result.AddModifier(modifier);
    }


    public virtual void OnDestroy()
    {
        EventManager.current.onAmmoHit -= OnAmmoHit;
    }
}
