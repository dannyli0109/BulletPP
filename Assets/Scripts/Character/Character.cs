using System;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    Character,
    Bullet,
    Grenade,
    Rocket,
    BouncingBlade,
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
    public CharacterStat additionalAmmo;
}

[Serializable]
public class BulletStats : AmmoStats
{

}

[Serializable]
public class BouncingBladeStats : AmmoStats
{
    public CharacterStat bounceAdditionSpeed;
    public CharacterStat bounceAdditionDamage;
}

[Serializable]
public class GrenadeStats : AmmoStats
{
    public CharacterStat bounceAdditionSpeed;
}
[Serializable]
public class RocketStats : AmmoStats
{
    public CharacterStat acceleration;
    public CharacterStat radius;
    public CharacterStat heatSeeking;
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
    public CharacterStat spreadAngle;
    public CharacterStat additionalAmmo;
    public CharacterStat extraBounces; // for things that bounce atleast once
    public CharacterStat damageOnBounce;
    public CharacterStat extraExplosionRange;
}

public class ModifiedStat
{
    public CharacterStat stat;
    public StatModifier modifier;
}

public abstract class Character : MonoBehaviour
{
    public CharacterStats stats;
    public BulletStats bulletStats;
    public GrenadeStats grenadeStats;
    public RocketStats rocketStats;
    public BouncingBladeStats bouncingBladeStats;
    public LaserStats laserStats;

    public Animator animator;
    public GameObject bulletPrefab;
    public GameObject grenadePrefab;
    public GameObject rocketPrefab;
    public GameObject bouncingBladePrefab;
    public GameObject laserPrefab;
    public Transform bulletContainer;

    public float hp;
    public float gold = 0;
    public float timeSinceFired;

    public bool reloading;
    public float currentImmunityFrame;
    public int currentBouncingBladeClip;

    public Transform TransformBeingUsedBecauseFunctionsNeedATranform;

    // TODO: Move this else where
    #region Lazer
    public LineRenderer thisLineRenderer;
    #endregion

    public List<Augment> augments = new List<Augment>();
    public List<Synergy> synergies = new List<Synergy>();
    public List<ModifiedStat> modifiedStats = new List<ModifiedStat>();
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
            EventManager.current.ReceiveGold(gold);
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
                  
                    if (ammo.overTimeDamage)
                    {
                        hp -= ammo.GetDamage()*Time.deltaTime;
                    }
                    else
                    {

                    hp -= ammo.GetDamage();
                    }
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
        // Debug.Log("Input " +gameObjectInput);
        if (gameObjectInput != null)
        {
            if (this.gameObject == gameObjectInput)
            {
                if (currentImmunityFrame <= 0)
                {
                    if (owner != this)
                    {
                        hp -= damage;
                        Debug.Log(hp + " deal damage to " + gameObjectInput + " " + damage);
                    }
                }
                else
                {
                    Debug.Log("near miss");
                }
                // EventManager.current.OnAmmoDestroy(ammo.gameObject);
            }
        }
        else
        {

        }
    }

  public  void RegainBouncingBlade()
    {
        if (bouncingBladeStats.maxClip.value > 0)
        {
           // Debug.Log("bouncing blade");
            currentBouncingBladeClip++;
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

                int levelBefore = augments[i].level;
                augments[i].count += 1;
                int levelAfter = augments[i].level;
                gold -= cost;

                if (levelBefore != levelAfter)
                {
                    return 3;
                }
                return 1;
            }
        }
        augments.Add(new Augment() { id = id, count = 1 });

        for (int i = 0; i < augmentManager.augmentDatas[id].synergies.Count; i++)
        {
            SynergyData synergyData = augmentManager.augmentDatas[id].synergies[i];

            bool existingSyngy = false;
            for (int j = 0; j < synergies.Count; j++)
            {
                if (synergies[j].id == synergyData.id)
                {
                    synergies[j].count++;
                    existingSyngy = true;
                    break;
                }
            }
            if (!existingSyngy)
            {
                synergies.Add(new Synergy()
                {
                    id = synergyData.id,
                    count = 1,
                    breakPoints = synergyData.breakpoints
                });
            }
        }
        // when obtain new augment, also update synergy list
        gold -= cost;
        return 2;
    }

    public int GetAugmentLevel(int id)
    {
        int level = 0;
        for (int i = 0; i < augments.Count; i++)
        {
            if (augments[i].id == id)
            {
                level = augments[i].level;
                break;
            }
        }
        return level;
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
            case StatType.BouncingBlade:
                result = (CharacterStat)bouncingBladeStats.GetType().GetField(stat).GetValue(bouncingBladeStats);
                break;
            case StatType.Character:
            {
                    result = (CharacterStat)stats.GetType().GetField(stat).GetValue(stats);
                    break;
            }
            default:
                result = null;
                break;
        }

        modifiedStats.Add(new ModifiedStat()
        {
            stat = result,
            modifier = modifier
        });
        result.AddModifier(modifier);
    }

    public void RemoveAllModifiers()
    {
        for (int i = 0; i < modifiedStats.Count; i++)
        {
            modifiedStats[i].stat.RemoeModifier(modifiedStats[i].modifier);
        }
        modifiedStats.Clear();
    }


    public virtual void OnDestroy()
    {
        EventManager.current.onAmmoHit -= OnAmmoHit;
    }
}
