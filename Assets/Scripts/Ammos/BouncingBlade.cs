using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBlade : Ammo
{
    public float currentSpeed;
    public float slowDownSpeed;
    public float currentDamage;

    void Start()
    {
        EventManager.current.onAmmoDestroy += OnBouncingBladeDestroy;
        currentSpeed = owner.bouncingBladeStats.speed.value;
        currentDamage = owner.bouncingBladeStats.damage.value;
    }
    private void FixedUpdate()
    {      
        currentSpeed = Mathf.Clamp(currentSpeed -= Time.deltaTime, owner.bouncingBladeStats.speed.value, currentSpeed);
        transform.position += transform.forward * currentSpeed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.current.GamePausing()) return;
        if (owner)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
            {
                // make sure the bullet is not hitting itself
                // EventManager.current.OnAmmoHit(this, other.gameObject);
               // EventManager.current.OnLaserHit(owner.bouncingBladeStats.damage.value, owner, other.gameObject);

            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                //EventManager.current.OnAmmoHit(this, other.gameObject);
               // EventManager.current.OnLaserHit(owner.bouncingBladeStats.damage.value, owner, other.gameObject);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //HandleAmmoHit(other);
        // Debug.Log(Time.deltaTime);
        if (owner)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
            {
                // make sure the bullet is not hitting itself
                EventManager.current.OnAmmoHit(this, other.gameObject);
               // EventManager.current.OnLaserHit(owner.bouncingBladeStats.damage.value * Time.deltaTime, owner, other.gameObject);

            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                EventManager.current.OnAmmoHit(this, other.gameObject);
               
            }
        }

    }

    void Update()
    {

        if (GameManager.current.GamePausing())
        {
            owner.RegainBouncingBlade();
            Destroy(gameObject);
        }

        currentSpeed = Mathf.Clamp(currentSpeed - slowDownSpeed * Time.deltaTime, owner.grenadeStats.speed.value, currentSpeed);
        bornTime += Time.deltaTime;
        if (bornTime >= owner.bouncingBladeStats.travelTime.value)
        {
            owner.RegainBouncingBlade();
            Destroy(gameObject);
        }

        if (timesBounced < owner.bouncingBladeStats.amountOfBounces.value)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 0.5f, (1 << 10)))
            {
                HandleAmmoHit(hit.collider);
                SpawnHitParticle(owner.grenadeStats.size.value);
                Vector3 reflectionDir = Vector3.Reflect(gameObject.transform.forward, hit.normal);
                gameObject.transform.forward = reflectionDir;
                timesBounced++;
                currentSpeed += owner.bouncingBladeStats.bounceAdditionSpeed.value;
                currentDamage += owner.bouncingBladeStats.bounceAdditionDamage.value;
            }
        }
        else
        {
            owner.RegainBouncingBlade();
            Destroy(gameObject);
        }
    }

    private void OnBouncingBladeDestroy(GameObject gameObject)
    {
        if (this.gameObject == gameObject)
        {
            owner.RegainBouncingBlade();
            SpawnHitParticle(owner.bouncingBladeStats.size.value);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        EventManager.current.onAmmoDestroy -= OnBouncingBladeDestroy;
    }

    //public override void Init(Character owner, float angle)
    //{
    //    this.owner = owner;
    //    transform.SetParent(null);
    //    transform.localScale = new Vector3(owner.bouncingBladeStats.size.value, owner.bouncingBladeStats.size.value, owner.bouncingBladeStats.size.value);
    //    transform.localRotation = Quaternion.Euler(new Vector3(0f, angle + transform.localRotation.eulerAngles.y, 0f));
    //}

    //public override float GetDamage()
    //{
    //    return owner.bouncingBladeStats.damage.value * owner.stats.damageMultiplier.value;
    //}
}
