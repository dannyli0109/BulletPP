using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Bullet : Ammo
{
    public GameObject bulletHitParticlePrefab;
    float bornTime = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        EventManager.current.onAmmoDestroy += OnBulletDestroy;
    }

    // Update is called once per frame
    void Update()
    {
        bornTime += Time.deltaTime;
        if (bornTime >= owner.bulletStats.travelTime.value)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * owner.bulletStats.speed.value * Time.fixedDeltaTime;
    }


    private void OnBulletDestroy(GameObject gameObject)
    {
        if (this.gameObject == gameObject)
        {
            GameObject bulletParticle = Instantiate(bulletHitParticlePrefab, transform);
            bulletParticle.transform.SetParent(null);
            bulletParticle.transform.localScale = new Vector3(owner.bulletStats.size.value, owner.bulletStats.size.value, owner.bulletStats.size.value);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            EventManager.current.OnAmmoHit(this);
        }
        EventManager.current.OnAmmoDestroy(gameObject);
    }

    private void OnDestroy()
    {
        EventManager.current.onAmmoDestroy -= OnBulletDestroy;
    }
}
