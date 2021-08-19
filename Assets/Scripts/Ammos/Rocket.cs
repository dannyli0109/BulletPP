using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Ammo
{
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
        if (GameManager.current.gameState == GameState.Shop) return;
        HandleAmmoHit(other);
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
        EventManager.current.onAmmoDestroy -= OnRocketDestroy;
    }


    public override float GetDamage()
    {
        return owner.rocketStats.damage.value * owner.stats.damageMultiplier.value;
    }
}
