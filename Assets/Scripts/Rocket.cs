using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Ammo
{
    public GameObject bulletHitParticlePrefab;
    public Transform bulletTip;
    float bornTime = 0;

    private void FixedUpdate()
    {
        transform.position += transform.forward * owner.rocketStats.speed.value * Time.fixedDeltaTime;
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

        if (TimesBounced < owner.rocketStats.amountOfBounces.value)
        {
            gameObject.transform.Rotate(new Vector3(0, Random.Range(155, 205), 0));
            TimesBounced++;

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
}
