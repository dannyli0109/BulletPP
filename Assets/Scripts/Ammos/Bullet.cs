using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Bullet : Ammo
{
    
    void Start()
    {
        EventManager.current.onAmmoDestroy += OnBulletDestroy;
    }

    void Update()
    {
        bornTime += Time.deltaTime;
        if (bornTime >= owner.bulletStats.travelTime.value)
        {
            Destroy(gameObject);
        }

        if (timesBounced < owner.bulletStats.amountOfBounces.value)
        {
            BounceOffAmmo();
        }
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * owner.bulletStats.speed.value * Time.fixedDeltaTime;
    }

    private void OnBulletDestroy(GameObject gameObject)
    {
        if (this.gameObject == gameObject)
        {
            SpawnHitParticle(owner.bulletStats.size.value);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleAmmoHit(other);
        EventManager.current.OnAmmoDestroy(gameObject);      
    }

    private void OnDestroy()
    {
        EventManager.current.onAmmoDestroy -= OnBulletDestroy;
    }

    public override float GetDamage()
    {
        return owner.bulletStats.damage.value * owner.stats.damageMultiplier.value;
    }
}
