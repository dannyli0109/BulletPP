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
            OnBulletDestroy(gameObject);
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

        if (homingRadius > 0 && currentTarget == null)
        {
            Collider[] hitColliders = Physics.OverlapSphere(ammoTip.transform.position, homingRadius, 1 << 12);
            int minIndex = -1;
            float minDistance = float.MaxValue;
            float minRadius = 3;
            for (int i = 0; i < hitColliders.Length; i++)
            {
                GameObject target = hitColliders[i].gameObject;
                Vector3 direction = target.transform.position - ammoTip.transform.position;
                direction = new Vector3(direction.x, 0, direction.z);

                float distanceSquared = direction.sqrMagnitude;
                if (minDistance > distanceSquared && distanceSquared > minRadius * minRadius)
                {
                    minIndex = i;
                    minDistance = distanceSquared;
                }
            }
            if (minIndex >= 0)
            {
                GameObject seekTarget = hitColliders[minIndex].gameObject;
                Vector3 seekDirection = seekTarget.transform.position - ammoTip.transform.position;
                seekDirection = new Vector3(seekDirection.x, 0, seekDirection.z);
                seekDirection.Normalize();
                Vector3 desireVelocity = seekDirection * speed;
                // homing factor
                acceleration = (desireVelocity - velocity) * 3;
            }
            else
            {
                acceleration = new Vector3(0, 0, 0);
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
            SpawnHitParticle(size);
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

        if (pierce)
        {
            SpawnHitParticle(size);
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
