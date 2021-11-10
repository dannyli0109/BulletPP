using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
public class PiercingAmmo : Ammo
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
        if (bornTime >= lifeTime)
        {
            ReturnToPool();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.current.GetState() == GameState.Pause) return;

        if (homingRadius > 0)
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
                transform.rotation = Quaternion.LookRotation(direction);
                direction.Normalize();
                Vector3 desireVelocity = direction * speed;
                // homing factor
                acceleration = (desireVelocity - velocity) * 3;
                break;
            }
        }

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
        // EventManager.current.OnAmmoDestroy(gameObject);

        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            //  Debug.Log("wall hit");
            //EventManager.current.OnAmmoDestroy(gameObject);
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
