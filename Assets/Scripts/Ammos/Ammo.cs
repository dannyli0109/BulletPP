using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ammo : MonoBehaviour
{
    public Character owner;
    public GameObject hitParticlePrefab;
    public Transform ammoTip;
    protected float bornTime = 0;

    #region stats
    protected int timesBounced = 0;
    #endregion stats

    public abstract float GetDamage();

    protected void SpawnHitParticle(float size)
    {
        GameObject ammoParticle = Instantiate(hitParticlePrefab, ammoTip);
        ammoParticle.transform.SetParent(null);
        ammoParticle.transform.localScale = new Vector3(
            size, size, size
        );
    }

    protected void BounceOffAmmo()
    {
        gameObject.transform.Rotate(new Vector3(0, Random.Range(155, 205), 0));
        timesBounced++;
    }

    protected void HandleAmmoHit(Collider other)
    {
        if (owner)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
            {
                // make sure the bullet is not hitting itself
                EventManager.current.OnAmmoHit(this, other.gameObject);

            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                EventManager.current.OnAmmoHit(this, other.gameObject);
            }
        }
    }
}
