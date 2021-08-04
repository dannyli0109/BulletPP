using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Ammo
{
    public GameObject bulletHitParticlePrefab;
    public Transform bulletTip;
    float bornTime = 0;

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
            }
        }

        if (TimesBounced < owner.grenadeStats.amountOfBounces.value)
        {
            gameObject.transform.Rotate(new Vector3(0, Random.Range(155, 205), 0));
            TimesBounced++;
            GameObject bulletParticle = Instantiate(bulletHitParticlePrefab, bulletTip);
            bulletParticle.transform.SetParent(null);
            bulletParticle.transform.localScale = new Vector3(1 + (0.1f * (owner.bulletStats.size.value - 1)), 1 + (0.1f * (owner.bulletStats.size.value - 1)), 1 + (0.1f * (owner.bulletStats.size.value - 1)));

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

    private void OnGrenadeDestroy(GameObject gameObject)
    {
        if (this.gameObject == gameObject)
        {
            GameObject bulletParticle = Instantiate(bulletHitParticlePrefab, bulletTip);
            bulletParticle.transform.SetParent(null);
            bulletParticle.transform.localScale = new Vector3(1 + (0.1f * (owner.bulletStats.size.value - 1)), 1 + (0.1f * (owner.bulletStats.size.value - 1)), 1 + (0.1f * (owner.bulletStats.size.value - 1)));
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        EventManager.current.onAmmoDestroy -= OnGrenadeDestroy;
    }
}
