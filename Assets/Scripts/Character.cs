using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using UnityEngine;

public enum StatType
{
    Bullet,
    Grenade,
    Rocket
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

}
[Serializable]
public class RocketStats : AmmoStats
{

}


public class Character : MonoBehaviour
{
    public CharacterStat maxHp;
    public CharacterStat moveSpeed;
    public BulletStats bulletStats;
    public GrenadeStats grenadeStats;
    public RocketStats rocketStats;

    public CharacterStat ReloadTime;
    public CharacterStat TimeBetweenShots;

    public float hp;
    public float timeSinceFired;

    public bool Reloading;

    public GameObject bulletPrefab;
    public GameObject grenadePrefab;
    public GameObject rocketPrefab;

    public Transform bulletContainer;

    public float CurrentImmunityFrame;

    public virtual void Start()
    {
        hp = maxHp.value;
        timeSinceFired = 0;
        EventManager.current.onAmmoHit += OnAmmoHit;
    }

    public virtual void Update()
    {
        if (hp <= 0)
        {
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
                if (CurrentImmunityFrame <= 0)
                {
                hp -= ammo.owner.bulletStats.damage.value;

                }
                else
                {
                    Debug.Log("near miss");
                }
               // EventManager.current.OnAmmoDestroy(ammo.gameObject);
            }
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
                    result = (CharacterStat)grenadeStats.GetType().GetField(stat).GetValue(bulletStats);
                    break;
                }
            case StatType.Rocket:
                {
                    result = (CharacterStat)rocketStats.GetType().GetField(stat).GetValue(bulletStats);
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
