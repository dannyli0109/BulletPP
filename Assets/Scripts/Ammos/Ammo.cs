using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AmmoType
{
    Bullet,
    Grenade,
    Rocket,
    Laser,
    BouncingBlade,
    Count
}

public abstract class Ammo : PooledItem
{
    public Character owner;
    public GameObject hitParticlePrefab;
    public Transform ammoTip;
    protected float bornTime = 0;
    public bool overTimeDamage;

    public bool facingOtherWay;

    #region stats
    public int timesBounced = 0;
    #endregion stats

    public float damage;
    public float size;
    public Vector3 velocity;
    public Vector3 acceleration;
    protected float speed;

    protected Vector3 currentAcceleration;

    public bool fromFreshReload;

    public float ImpactForce;
    protected int bounces;
    protected bool pierce;

    public virtual float GetDamage()
    {
        if (fromFreshReload)
        {
            Debug.Log("fresh ammo");
            return damage * owner.stats.damageMultiplier.value* owner.stats.extraDamageAfterReload.value;
        }
        else
        {
            return damage * owner.stats.damageMultiplier.value;

        }
    }

    public virtual void Init(Character owner, Vector3 forward, float angle, Vector3 offset, float speed, Vector3 acceleration, float damage, float size, int bounces, bool pierce)
    {
        this.owner = owner;
        this.damage = damage;
        this.acceleration = acceleration;
        this.size = size;
        this.speed = speed;
        this.bounces = bounces;
        this.pierce = pierce;
        transform.forward = forward;
        bornTime = 0;
        timesBounced = 0;
        transform.localScale = new Vector3(size, size, size);
        transform.localRotation = Quaternion.Euler(new Vector3(0f, angle + transform.localEulerAngles.y, 0f));
        Vector3 offsetVector = new Vector3(transform.forward.x * offset.x, transform.forward.y * offset.y, transform.forward.z * offset.z);
        transform.localPosition += offsetVector;
        velocity = transform.forward * speed;
    }

    public void Init(Character owner, Vector3 forward, float angle, float speed, float damage, float size)
    {
        Init(owner, forward, angle, Vector3.zero, speed, new Vector3(0, 0, 0), damage, size, 0, false);
    }

    public void Init(Character owner, Vector3 forward, float angle, float speed, float damage, float size, int bounces, bool pierce)
    {
        Init(owner, forward, angle, Vector3.zero, speed, new Vector3(0, 0, 0), damage, size, bounces, pierce);
    }

    protected void SpawnHitParticle(float size)
    {
        GameObject ammoParticle = Instantiate(hitParticlePrefab, ammoTip);
        ammoParticle.transform.SetParent(null);
        ammoParticle.transform.localScale = new Vector3(
            size, size, size
        );
    }

    protected bool BounceOffAmmo()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, size / 2.0f, (1 << 10)|(1<<12)))
        {
            HandleAmmoHit(hit.collider);
            SpawnHitParticle(owner.grenadeStats.size.value);
            Vector3 normal = new Vector3(hit.normal.x, 0, hit.normal.z);
            Vector3 reflectionDir = Vector3.Reflect(gameObject.transform.forward, normal);
            gameObject.transform.forward = reflectionDir;
            velocity = transform.forward * speed;
            timesBounced++;
            return true;
        }
        return false;
    }

    public virtual float GetImpactForce()
    {
        return ImpactForce;
    }

    protected void HandleAmmoHit(Collider other)
    {
        Vector2 holdingForce = new Vector2(transform.forward.x, transform.forward.z) * GetImpactForce();
        PlayImpactSound(other.gameObject.transform.position);
        if (owner)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
            {
                // make sure the bullet is not hitting itself
                EventManager.current.OnAmmoHit(this, other.gameObject, holdingForce);
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                EventManager.current.OnAmmoHit(this, other.gameObject, holdingForce);
                //HUDManager.current.damage += GetDamage();

                //if (GetType().ToString() == "Bullet")
                //{
                //    HUDManager.current.bulletDamage += GetDamage();
                //}

                //if (GetType().ToString() == "Grenade")
                //{
                //    HUDManager.current.grenadeDamage += GetDamage();
                //}

                //if (GetType().ToString() == "Rocket")
                //{
                //    HUDManager.current.rocketDamage += GetDamage();
                //}

                //if (GetType().ToString() == "Laser")
                //{
                //    HUDManager.current.laserDamage += GetDamage();
                //}


                //if (GetType().ToString() == "BouncingBlade")
                //{
                //    HUDManager.current.bouncingBladeDamage += GetDamage();
                //}
            }
        }
    }

    public abstract void PlayImpactSound(Vector3 position);
}
