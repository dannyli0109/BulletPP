using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Ammo
{
    void Start()
    {
        EventManager.current.onAmmoDestroy += OnGrenadeDestroy;
    }
    private void FixedUpdate()
    {
        transform.position += transform.forward * owner.grenadeStats.speed.value * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleAmmoHit(other);


        if (timesBounced < owner.grenadeStats.amountOfBounces.value)
        {
            SpawnHitParticle(owner.grenadeStats.size.value);
            BounceOffAmmo();
        }
        else
        {
            EventManager.current.OnAmmoDestroy(gameObject);
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
}
