using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Ammo
{

    void Start()
    {
        EventManager.current.onAmmoDestroy += OnRocketDestroy;
    }
    private void OnRocketDestroy(GameObject gameObject)
    {
        if (this.gameObject == gameObject)
        {
            SpawnHitParticle(owner.rocketStats.size.value);
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        transform.position += transform.forward * owner.rocketStats.speed.value * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleAmmoHit(other);

        if (timesBounced < owner.rocketStats.amountOfBounces.value)
        {
            SpawnHitParticle(owner.rocketStats.size.value);
            BounceOffAmmo();
        }
        else
        {
            EventManager.current.OnAmmoDestroy(this.gameObject);
        }
    }

    void Update()
    {
        bornTime += Time.deltaTime;
        if (bornTime >= owner.bulletStats.travelTime.value)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        EventManager.current.onAmmoDestroy -= OnRocketDestroy;
    }
}
