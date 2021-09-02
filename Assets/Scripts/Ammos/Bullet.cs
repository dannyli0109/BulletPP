using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Bullet : Ammo
{
    
    void Start()
    {
        EventManager.current.onAmmoDestroy += OnBulletDestroy;
    }

    void Update()
    {
        if (GameManager.current.GamePausing()) Destroy(gameObject);
        bornTime += Time.deltaTime;
        if (bornTime >= owner.bulletStats.travelTime.value)
        {
            Destroy(gameObject);
        }

        if (timesBounced < owner.bulletStats.amountOfBounces.value)
        {
            BounceOffAmmo();
        }
    }

    private void FixedUpdate()
    {
        velocity += acceleration * Time.fixedDeltaTime;
        transform.position += velocity * Time.fixedDeltaTime;
        acceleration = new Vector3(0, 0, 0);
    }

    private void OnBulletDestroy(GameObject gameObject)
    {
        if (this.gameObject == gameObject)
        {
            SpawnHitParticle(owner.bulletStats.size.value);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.current.GamePausing()) return;

        HandleAmmoHit(other);
        EventManager.current.OnAmmoDestroy(gameObject);      
    }

    private void OnDestroy()
    {
        EventManager.current.onAmmoDestroy -= OnBulletDestroy;
    }

    //public override float GetDamage()
    //{
    //    //return owner.bulletStats.damage.value * owner.stats.damageMultiplier.value;
    //    return damage;
    //}

    //public override void Init(Character owner, float damage, float size, Vector3 velocity, Vector3 acceleration)
    //{
    //    this.owner = owner;
    //    this.damage = damage;
    //    this.velocity = velocity;
    //    this.acceleration = acceleration;
    //    transform.SetParent(null);
    //    transform.localScale = new Vector3(size, size, size);

    //    //transform.localRotation = Quaternion.Euler(new Vector3(0f, angle + transform.localRotation.eulerAngles.y, 0f));
    //}
}
