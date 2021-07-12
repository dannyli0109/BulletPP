using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using UnityEngine;

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
                hp -= ammo.owner.bulletStats.damage.value;
                EventManager.current.OnAmmoDestroy(ammo.gameObject);
            }
        }
    }

    protected void Shoot()
    {

        GameObject bullet = Instantiate(bulletPrefab, bulletContainer);
        bullet.transform.SetParent(null);
        Vector3 scale = bullet.transform.localScale;
        bullet.transform.localScale = scale * bulletStats.size.value;
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        bulletComponent.owner = this;
    }

    public virtual void OnDestroy()
    {
        EventManager.current.onAmmoHit -= OnAmmoHit;
    }
}
