using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Bullet : Ammo
{
    public GameObject bulletHitParticlePrefab;
    public Transform bulletTip;
    float bornTime = 0;
    
    // Start is called before the first frame update
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

    private void OnTriggerEnter(Collider other)
    {
        if (this.gameObject == gameObject)
        // do contact damage
        if (TimesBounced < owner.bulletStats.amountOfBounces.value)
        {
            gameObject.transform.Rotate(new Vector3(0, Random.RandomRange(155, 205), 0));
            TimesBounced++;

        }
        else
        {
            GameObject bulletParticle = Instantiate(bulletHitParticlePrefab, bulletTip);
            bulletParticle.transform.SetParent(null);
            bulletParticle.transform.localScale = new Vector3(1 + (0.1f * (owner.bulletStats.size.value - 1)), 1 + (0.1f * (owner.bulletStats.size.value - 1)), 1 + (0.1f * (owner.bulletStats.size.value - 1)));
            Destroy(gameObject);
        }
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
        }


        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            EventManager.current.OnAmmoDestroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        EventManager.current.onAmmoDestroy -= OnBulletDestroy;
    }
}
