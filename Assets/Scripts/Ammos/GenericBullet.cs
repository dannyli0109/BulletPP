using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GenericBullet : Ammo
{
    void Start()
    {
        EventManager.current.onAmmoDestroy += OnBulletDestroy;
    }

    void Update()
    {
        if (GameManager.current.GetState() == GameState.Pause) return;
        if (GameManager.current.GameTransitional()) { ReturnToPool(); }
        bornTime += Time.deltaTime;
        if (bornTime >= owner.bulletStats.travelTime.value)
        {
            ReturnToPool();
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
        acceleration = new Vector3(0, 0, 0);
    }

    private void OnBulletDestroy(GameObject gameObject)
    {
        if (this.gameObject == gameObject)
        {
            SpawnHitParticle(owner.bulletStats.size.value);
            ReturnToPool();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.current.GameTransitional()) return;

        HandleAmmoHit(other);
        EventManager.current.OnAmmoDestroy(gameObject);
        //if (piercing && other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        //{
        //    if (isADesk)
        //    {
        //        Destroy(gameObject);
        //    }
        //    else
        //    {
        //        EventManager.current.OnAmmoDestroy(gameObject);
        //    }
        //}
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
