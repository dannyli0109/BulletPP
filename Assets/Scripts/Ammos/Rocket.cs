using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Ammo
{
    public AOEDamage aoePrefab;
    float currentSpeed;
    void Start()
    {
        currentSpeed = owner.rocketStats.speed.value;
        EventManager.current.onAmmoDestroy += OnRocketDestroy;
    }

    void Update()
    {
        if (GameManager.current.gameState == GameState.Shop) Destroy(gameObject);
        bornTime += Time.deltaTime;
        if (bornTime >= owner.rocketStats.travelTime.value)
        {
            Destroy(gameObject);
        }

        if (timesBounced < owner.rocketStats.amountOfBounces.value)
        {
            BounceOffAmmo();
        }
    }
    private void FixedUpdate()
    {
        currentSpeed += owner.rocketStats.acceleration.value * Time.fixedDeltaTime;

        if (owner.rocketStats.heatSeeking.value > 0)
        {
            Debug.Log(owner.rocketStats.heatSeeking.value);
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
            transform.position += transform.forward * currentSpeed * Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.current.gameState == GameState.Shop) return;
        HandleAmmoHit(other);
        EventManager.current.OnAmmoDestroy(gameObject);
    }

    private void OnRocketDestroy(GameObject gameObject)
    {
        if (this.gameObject == gameObject)
        {
            SpawnHitParticle(owner.rocketStats.size.value);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        Vector3 pos = new Vector3(transform.position.x, 1.01f, transform.position.z);
        AOEDamage aoeDamage = Instantiate(aoePrefab, pos, Quaternion.identity);
        if (owner.gameObject.layer == 11)
        {
            aoeDamage.Init(owner.rocketStats.radius.value, owner.rocketStats.damage.value, 1 << 12);
        }
        else
        {
            aoeDamage.Init(owner.rocketStats.radius.value, owner.rocketStats.damage.value, 1 << 11);
        }
        EventManager.current.onAmmoDestroy -= OnRocketDestroy;
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
