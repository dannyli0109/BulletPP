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
        if (bornTime >= lifeTime)
        {
            ReturnToPool();
            if (explodeRadius>0)
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

        if (homingRadius>0)
        {
            Collider[] hitColliders = Physics.OverlapSphere(ammoTip.transform.position, homingRadius, 1 << 12);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                GameObject target = hitColliders[i].gameObject;
                Vector3 direction = target.transform.position - ammoTip.transform.position;
                RaycastHit hit;
                Physics.Raycast(ammoTip.transform.position, transform.forward, out hit);
                if (hit.collider.gameObject.layer == 10 && hit.distance < 5)
                {
                    continue;
                }

                direction.Normalize();
                Vector3 desireVelocity = direction * speed;
                // homing factor
                acceleration = (desireVelocity - velocity) * 3;
                break;
            }
        }
        velocity += acceleration * Time.fixedDeltaTime;
        transform.forward = velocity.normalized;
        transform.position += velocity * Time.fixedDeltaTime;
    }

    private void OnBulletDestroy(GameObject gameObject)
    {
        if (this.gameObject == gameObject)
        {
            SpawnHitParticle(owner.bulletStats.size.value);
            ReturnToPool();
            if (explodeRadius>0)
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
