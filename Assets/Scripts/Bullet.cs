using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Bullet : Ammo
{
    public GameObject bulletHitParticlePrefab;
    public Transform bulletTip;
    float bornTime = 0;
    
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
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * owner.bulletStats.speed.value * Time.fixedDeltaTime;
    }

    private void OnBulletDestroy(GameObject gameObject)
    {
        if (this.gameObject == gameObject)
        {
            GameObject bulletParticle = Instantiate(bulletHitParticlePrefab, bulletTip);
            bulletParticle.transform.SetParent(null);
            bulletParticle.transform.localScale = new Vector3(1 + (0.1f * (owner.bulletStats.size.value - 1)), 1 + (0.1f * (owner.bulletStats.size.value - 1)), 1 + (0.1f * (owner.bulletStats.size.value - 1)));
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (TimesBounced < owner.bulletStats.amountOfBounces.value)
        {
            gameObject.transform.Rotate(new Vector3(0, Random.Range(155, 205), 0));
            TimesBounced++;

        }
        else
        {
            EventManager.current.OnAmmoDestroy(this.gameObject);
        }

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


         //   if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
         //   {
         //       EventManager.current.OnAmmoDestroy(gameObject);
         //   }
        
    }


    private void OnDestroy()
    {
        EventManager.current.onAmmoDestroy -= OnBulletDestroy;
    }
}
