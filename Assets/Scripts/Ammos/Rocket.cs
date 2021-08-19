using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Ammo
{
    public AOEDamage aoePrefab;
    float currentSpeed;
    void Start()
    {
        currentSpeed = owner.rocketStats.speed.value;
        EventManager.current.onAmmoDestroy += OnRocketDestroy;
    }

    void Update()
    {
        bornTime += Time.deltaTime;
        if (bornTime >= owner.bulletStats.travelTime.value)
        {
            Destroy(gameObject);
        }

        if (timesBounced < owner.rocketStats.amountOfBounces.value)
        {
            BounceOffAmmo();
        }
    }
    private void FixedUpdate()
    {
        currentSpeed += owner.rocketStats.acceleration.value * Time.fixedDeltaTime;
        transform.position += transform.forward * currentSpeed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        //HandleAmmoHit(other);

        EventManager.current.OnAmmoDestroy(gameObject);
    }

    private void OnRocketDestroy(GameObject gameObject)
    {
        if (this.gameObject == gameObject)
        {
            SpawnHitParticle(owner.rocketStats.size.value);
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        Vector3 pos = new Vector3(transform.position.x, 1.01f, transform.position.z);
        AOEDamage aoeDamage = Instantiate(aoePrefab, pos, Quaternion.identity);
        if (owner.gameObject.layer == 11)
        {
            aoeDamage.Init(owner.rocketStats.radius.value, owner.rocketStats.damage.value, 1 << 12);
        }
        else
        {
            aoeDamage.Init(owner.rocketStats.radius.value, owner.rocketStats.damage.value, 1 << 11);
        }
        EventManager.current.onAmmoDestroy -= OnRocketDestroy;
    }


    public override float GetDamage()
    {
        return owner.rocketStats.damage.value * owner.stats.damageMultiplier.value;
    }
}
