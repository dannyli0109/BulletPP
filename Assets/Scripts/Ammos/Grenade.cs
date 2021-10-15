using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Ammo
{
    public float currentSpeed;
    public float slowDownSpeed;
    void Start()
    {
        EventManager.current.onAmmoDestroy += OnGrenadeDestroy;
        currentSpeed = owner.grenadeStats.speed.value;
    }
    private void FixedUpdate()
    {
        if (GameManager.current.GetState() == GameState.Pause) return;
        //transform.position += transform.forward * owner.grenadeStats.speed.value * Time.fixedDeltaTime;
        transform.position += transform.forward * currentSpeed * Time.fixedDeltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed -= Time.deltaTime, owner.grenadeStats.speed.value, currentSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.current.GameTransitional()) return;
        HandleAmmoHit(other);
        EventManager.current.OnAmmoDestroy(gameObject);
    }

    void Update()
    {
        if (GameManager.current.GetState() == GameState.Pause) return;
        if (GameManager.current.GameTransitional()) ReturnToPool();
        currentSpeed = Mathf.Clamp(currentSpeed - slowDownSpeed * Time.deltaTime, owner.grenadeStats.speed.value, currentSpeed);
        bornTime += Time.deltaTime;
        if (bornTime >= owner.grenadeStats.travelTime.value)
        {
            Debug.Log("grenade died of old age");
            ReturnToPool();
        }

        if (timesBounced < owner.grenadeStats.amountOfBounces.value)
        {
            if (BounceOffAmmo())
            {
                currentSpeed += owner.grenadeStats.bounceAdditionSpeed.value;
            }
        }
    }

    private void OnGrenadeDestroy(GameObject gameObject)
    {
        if (this.gameObject == gameObject)
        {
            SpawnHitParticle(owner.grenadeStats.size.value);
            ReturnToPool();
        }
    }

    private void OnDestroy()
    {
        EventManager.current.onAmmoDestroy -= OnGrenadeDestroy;
    }

    public override void PlayImpactSound(Vector3 position)
    {
        SoundManager.PlaySound(SoundType.BlasterHit, position, 1);

    }

    //public override float GetDamage()
    //{
    //    return owner.grenadeStats.damage.value * owner.stats.damageMultiplier.value;
    //}

    //public override void Init(Character owner, float angle)
    //{
    //    this.owner = owner;
    //    transform.SetParent(null);
    //    transform.localScale = new Vector3(owner.bulletStats.size.value, owner.bulletStats.size.value, owner.bulletStats.size.value);
    //    transform.localRotation = Quaternion.Euler(new Vector3(0f, angle + transform.localRotation.eulerAngles.y, 0f));
    //}
}
