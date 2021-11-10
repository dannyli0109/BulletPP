using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPool : MonoBehaviour
{
    public static AmmoPool current;


    public GenericBullet bulletPrefab;
    public Pool<GenericBullet> bulletPool;
    public int bulletCount;

    public GenericBullet bouncingBulletPrefab;
    public Pool<GenericBullet> bouncingBulletPool;
    public int bouncingBulletCount;

    public GenericBullet piercingAmmoPrefab;
    public Pool<GenericBullet> piercingAmmoPool;
    public int piercingAmmoCount;

    public GenericBullet shrapnelAmmoPrefab;
    public Pool<GenericBullet> shrapnelAmmoPool;
    public int shrapnelAmmoCount;

    public GenericBullet rocketPrefab;
    public Pool<GenericBullet> rocketPool;
    public int rocketCount;

    public GenericBullet enemyBulletPrefab;
    public Pool<GenericBullet> enemyBulletPool;
    public int enemyBulletCount;

    public ExplosionParticle explosionParticlePrefab;
    public Pool<ExplosionParticle> explosionParticlePool;
    public int explosionParticleCount;

    public ParticleHandler bulletParticlePrefab;
    public Pool<ParticleHandler> bulletParticlePool;
    public int bulletParticleCount;

    private void Awake()
    {
        current = this;

        bulletPool = new Pool<GenericBullet>(bulletPrefab, bulletCount);
        foreach (GenericBullet bullet in bulletPool.available) bullet.gameObject.transform.SetParent(transform);

        bouncingBulletPool = new Pool<GenericBullet>(bouncingBulletPrefab, bouncingBulletCount);
        foreach (GenericBullet bouncingBullet in bouncingBulletPool.available) bouncingBullet.gameObject.transform.SetParent(transform);

        piercingAmmoPool = new Pool<GenericBullet>(piercingAmmoPrefab, piercingAmmoCount);
        foreach (GenericBullet piercingAmmo in piercingAmmoPool.available) piercingAmmo.gameObject.transform.SetParent(transform);

        shrapnelAmmoPool = new Pool<GenericBullet>(shrapnelAmmoPrefab, shrapnelAmmoCount);
        foreach (GenericBullet shrapnelAmmo in shrapnelAmmoPool.available) shrapnelAmmo.gameObject.transform.SetParent(transform);

        rocketPool = new Pool<GenericBullet>(rocketPrefab, rocketCount);
        foreach (GenericBullet rocket in rocketPool.available) rocket.gameObject.transform.SetParent(transform);

        enemyBulletPool = new Pool<GenericBullet>(enemyBulletPrefab, enemyBulletCount);
        foreach (GenericBullet enemyBullet in enemyBulletPool.available) enemyBullet.gameObject.transform.SetParent(transform);

        explosionParticlePool = new Pool<ExplosionParticle>(explosionParticlePrefab, explosionParticleCount);
        foreach (ExplosionParticle explosionParticle in explosionParticlePool.available) explosionParticle.gameObject.transform.SetParent(transform);

        bulletParticlePool = new Pool<ParticleHandler>(bulletParticlePrefab, bulletParticleCount);
        foreach (ParticleHandler bulletParticle in bulletParticlePool.available) bulletParticle.gameObject.transform.SetParent(transform);
    }
}
