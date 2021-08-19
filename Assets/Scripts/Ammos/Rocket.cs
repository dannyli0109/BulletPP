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
        if (bornTime >= owner.rocketStats.travelTime.value)
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

        if (owner.rocketStats.heatSeeking.value > 0)
        {
            Debug.Log(owner.rocketStats.heatSeeking.value);
            transform.position += transform.forward * currentSpeed / 2 * Time.fixedDeltaTime;

            transform.position += transform.forward * currentSpeed * Time.fixedDeltaTime;
            float currentDistFromOwner = Vector2.Distance(new Vector2(owner.transform.position.x, owner.transform.position.z), new Vector2(transform.position.x, transform.position.z)) + currentSpeed * 2 * Time.fixedDeltaTime;
            Vector3 desiredPos = owner.gameObject.transform.position + owner.bulletContainer.forward * currentDistFromOwner;
            if (currentDistFromOwner > 3)
            {
            }
            owner.TransformBeingUsedBecauseFunctionsNeedATranform.transform.position = new Vector3(desiredPos.x, transform.position.y, desiredPos.z);
            owner.TransformBeingUsedBecauseFunctionsNeedATranform.transform.rotation = owner.transform.rotation;
            transform.LookAt(owner.TransformBeingUsedBecauseFunctionsNeedATranform);
            transform.position += transform.forward * currentSpeed / 2 * Time.fixedDeltaTime;
        }
        else
        {
            transform.position += transform.forward * currentSpeed * Time.fixedDeltaTime;
        }
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
