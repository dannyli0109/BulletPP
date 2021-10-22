using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GenericBullet : Ammo
{
    private float lastTriggered;
    void Start()
    {
        EventManager.current.onAmmoDestroy += OnBulletDestroy;
        lastTriggered = 0;
    }

    void Update()
    {
        if (GameManager.current.GetState() == GameState.Pause) return;
        if (GameManager.current.GameTransitional()) { ReturnToPool(); }
        bornTime += Time.deltaTime;
        lastTriggered += Time.deltaTime;
        if (bornTime >= owner.bulletStats.travelTime.value)
        {
            ReturnToPool();
            if (explode)
            {
                Explode();
            }
        }

        if (timesBounced < bounces)
        {
            //  Debug.Log("Try to bounce");
            BounceOffAmmo();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.current.GetState() == GameState.Pause) return;
        velocity += acceleration * Time.fixedDeltaTime;
        transform.position += velocity * Time.fixedDeltaTime;
    }

    private void OnBulletDestroy(GameObject gameObject)
    {
        if (this.gameObject == gameObject)
        {
            SpawnHitParticle(owner.bulletStats.size.value);
            ReturnToPool();
            if (explode)
            {
                Explode();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.current.GameTransitional()) return;

        if (lastTriggered > 0.1f)
        {
            HandleAmmoHit(other);
            lastTriggered = 0;
        }

        if (pierce && other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            EventManager.current.OnAmmoDestroy(gameObject);
        }
        else if (pierce)
        {
            SpawnHitParticle(owner.bulletStats.size.value);
        }
        else
        {
            EventManager.current.OnAmmoDestroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        EventManager.current.onAmmoDestroy -= OnBulletDestroy;
    }

    public override void PlayImpactSound(Vector3 position)
    {
        SoundManager.PlaySound(SoundType.BlasterHit, position, 1);
    }
}
