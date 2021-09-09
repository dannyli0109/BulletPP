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

    #region stats
    public int timesBounced = 0;
    #endregion stats

    public float damage;
    public float size;
    public Vector3 velocity;
    public Vector3 acceleration;

    protected Vector3 currentAcceleration;
    public float GetDamage()
    {
        return damage;
    }
    public void Init(Character owner, Vector3 forward, float angle, float offset, float speed, Vector3 acceleration, float damage, float size)
    {
        this.owner = owner;
        this.damage = damage;
        this.acceleration = acceleration;
        this.size = size;
        transform.forward = forward;
        bornTime = 0;
        //transform.SetParent(null);
        transform.localScale = new Vector3(size, size, size);
        transform.localRotation = Quaternion.Euler(new Vector3(0f, angle + transform.localEulerAngles.y, 0f));
        Vector3 offsetVector = new Vector3(transform.forward.x * offset, transform.forward.y * offset, transform.forward.z * offset);
        transform.localPosition += offsetVector;
        this.velocity = transform.forward * speed;
    }


    public void Init(Character owner, Vector3 forward, float angle, float speed, float damage, float size)
    {
        Init(owner, forward, angle, 0, speed, new Vector3(0, 0, 0), damage, size);
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
        if (Physics.Raycast(transform.position, transform.forward, out hit, 0.5f, (1 << 10)|(1<<12)))
        {
            HandleAmmoHit(hit.collider);
            SpawnHitParticle(owner.grenadeStats.size.value);
            Vector3 normal = new Vector3(hit.normal.x, 0, hit.normal.z);
            Vector3 reflectionDir = Vector3.Reflect(gameObject.transform.forward, normal);
            gameObject.transform.forward = reflectionDir;
            timesBounced++;
            return true;
        }
        return false;
    }

    protected void HandleAmmoHit(Collider other)
    {
        if (owner)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
            {
                // make sure the bullet is not hitting itself
                EventManager.current.OnAmmoHit(this, other.gameObject);
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                EventManager.current.OnAmmoHit(this, other.gameObject);
                HUDManager.current.damage += GetDamage();
                
                if (GetType().ToString() == "Bullet")
                {
                    HUDManager.current.bulletDamage += GetDamage();
                }

                if (GetType().ToString() == "Grenade")
                {
                    HUDManager.current.grenadeDamage += GetDamage();
                }

                if (GetType().ToString() == "Rocket")
                {
                    HUDManager.current.rocketDamage += GetDamage();
                }

                if (GetType().ToString() == "Laser")
                {
                    HUDManager.current.laserDamage += GetDamage();
                }


                if (GetType().ToString() == "BouncingBlade")
                {
                    HUDManager.current.bouncingBladeDamage += GetDamage();
                }
            }
        }
    }
}
