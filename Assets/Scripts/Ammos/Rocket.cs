using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Ammo
{
    float currentSpeed;
    void Start()
    {
        currentSpeed = owner.rocketStats.speed.value;
        EventManager.current.onAmmoDestroy += OnRocketDestroy;
    }

    public override void Init(Character owner, Vector3 forward, float angle, Vector3 offset, float speed, Vector3 acceleration, float damage, float size, float lifeTime, int bounces, bool pierce, float explosionRadius,  float homingRadius)
    {
        base.Init(owner, forward, angle, offset, speed, acceleration, damage, size, lifeTime, bounces, pierce, explosionRadius,  homingRadius);
        currentSpeed = owner.rocketStats.speed.value;
    }

    void Update()
    {
        if (GameManager.current.GetState() == GameState.Pause) return;
        if (GameManager.current.GameTransitional()) ReturnToPool();
        bornTime += Time.deltaTime;
        if (bornTime >= owner.rocketStats.travelTime.value)
        {
            ReturnToPool();
            Explode();
        }

        if (timesBounced < owner.rocketStats.amountOfBounces.value)
        {
            BounceOffAmmo();
        }

      //  Debug.Log(currentSpeed);
    }

    private void FixedUpdate()
    {
        if (GameManager.current.GetState() == GameState.Pause) return;
        currentSpeed += owner.rocketStats.acceleration.value * Time.fixedDeltaTime;

        if (owner.rocketStats.heatSeeking.value > 0)
        {
            transform.position += transform.forward * currentSpeed / 2 * Time.fixedDeltaTime;

            transform.position += transform.forward * currentSpeed * Time.fixedDeltaTime;
            float currentDistFromOwner = Vector2.Distance(new Vector2(owner.transform.position.x, owner.transform.position.z), new Vector2(transform.position.x, transform.position.z)) + currentSpeed * 2 * Time.fixedDeltaTime;
            Vector3 desiredPos = owner.gameObject.transform.position + owner.bulletContainer.forward * currentDistFromOwner;
            if (currentDistFromOwner > 3)
            {
            }
            owner.TransformBeingUsedBecauseFunctionsNeedATranform.transform.position = new Vector3(desiredPos.x, transform.position.y, desiredPos.z);
            owner.TransformBeingUsedBecauseFunctionsNeedATranform.transform.rotation = owner.transform.rotation;
            transform.LookAt(owner.TransformBeingUsedBecauseFunctionsNeedATranform);
            transform.position += transform.forward * currentSpeed / 2 * Time.fixedDeltaTime;
        }
        else
        {
            velocity += acceleration * Time.fixedDeltaTime;
            transform.forward = velocity.normalized;
            transform.position += velocity * Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.current.GameTransitional()) return;
        HandleAmmoHit(other);
        EventManager.current.OnAmmoDestroy(gameObject);
    }

    private void OnRocketDestroy(GameObject gameObject)
    {
        if (this.gameObject == gameObject)
        {
            //SpawnHitParticle(owner.rocketStats.size.value);
            ReturnToPool();
            //Explode();
        }
    }

    private void OnDestroy()
    {
        EventManager.current.onAmmoDestroy -= OnRocketDestroy;
    }

    public override void PlayImpactSound(Vector3 position)
    {
        SoundManager.PlaySound(SoundType.RocketHit, position, 1);

    }

    //public override float GetDamage()
    //{
    //    return owner.rocketStats.damage.value * owner.stats.damageMultiplier.value;
    //}

    //public override void Init(Character owner, float angle)
    //{
    //    this.owner = owner;
    //    transform.SetParent(null);
    //    transform.localScale = new Vector3(owner.bulletStats.size.value, owner.bulletStats.size.value, owner.bulletStats.size.value);
    //    transform.localRotation = Quaternion.Euler(new Vector3(0f, angle + transform.localRotation.eulerAngles.y, 0f));
    //}
}
