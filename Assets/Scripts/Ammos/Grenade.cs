using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Ammo
{
    float currentSpeed;
    void Start()
    {
        EventManager.current.onAmmoDestroy += OnGrenadeDestroy;
        currentSpeed = owner.grenadeStats.speed.value;
    }
    private void FixedUpdate()
    {
        //transform.position += transform.forward * owner.grenadeStats.speed.value * Time.fixedDeltaTime;
        transform.position += transform.forward * currentSpeed * Time.fixedDeltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed -= Time.deltaTime, owner.grenadeStats.speed.value, currentSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleAmmoHit(other);
        EventManager.current.OnAmmoDestroy(gameObject);
    }

    void Update()
    {
        if (GameManager.current.gameState == GameState.Shop) Destroy(gameObject);
        bornTime += Time.deltaTime;
        if (bornTime >= owner.grenadeStats.travelTime.value)
        {
            Destroy(gameObject);
        }

        if (timesBounced < owner.grenadeStats.amountOfBounces.value)
        {
            if (BounceOffAmmo())
            {
                currentSpeed += owner.grenadeStats.BounceAdditionSpeed.value;
            }
        }
    }

    private void OnGrenadeDestroy(GameObject gameObject)
    {
        if (this.gameObject == gameObject)
        {
            SpawnHitParticle(owner.grenadeStats.size.value);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        EventManager.current.onAmmoDestroy -= OnGrenadeDestroy;
    }

    public override float GetDamage()
    {
        return owner.grenadeStats.damage.value * owner.stats.damageMultiplier.value;
    }
}
