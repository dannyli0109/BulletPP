using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPool : MonoBehaviour
{
    public static AmmoPool current;


    public Bullet bulletPrefab;
    public Pool<Bullet> bulletPool;
    public int bulletCount;

    public Bullet bouncingBulletPrefab;
    public Pool<Bullet> bouncingBulletPool;
    public int bouncingBulletCount;

    public PiercingAmmo piercingAmmoPrefab;
    public Pool<PiercingAmmo> piercingAmmoPool;
    public int piercingAmmoCount;

    public Shrapnel shrapnelAmmoPrefab;
    public Pool<Shrapnel> shrapnelAmmoPool;
    public int shrapnelAmmoCount;

    public GenericBullet multiBulletPrefab;
    public Pool<GenericBullet> multiBulletPool;
    public int multiBulletCount;

    public Grenade grenadePrefab;
    public Pool<Grenade> grenadePool;
    public int grenadeCount;

    public Rocket rocketPrefab;
    public Pool<Rocket> rocketPool;
    public int rocketCount;

    public Bullet enemyBulletPrefab;
    public Pool<Bullet> enemyBulletPool;
    public int enemyBulletCount;

    public ExplosionParticle explosionParticlePrefab;
    public Pool<ExplosionParticle> explosionParticlePool;
    public int explosionParticleCount;

    private void Awake()
    {
        current = this;

        bulletPool = new Pool<Bullet>(bulletPrefab, bulletCount);
        foreach (Bullet bullet in bulletPool.available) bullet.gameObject.transform.SetParent(transform);

        bouncingBulletPool = new Pool<Bullet>(bouncingBulletPrefab, bouncingBulletCount);
        foreach (Bullet bouncingBullet in bouncingBulletPool.available) bouncingBullet.gameObject.transform.SetParent(transform);

        piercingAmmoPool = new Pool<PiercingAmmo>(piercingAmmoPrefab, piercingAmmoCount);
        foreach (PiercingAmmo piercingAmmo in piercingAmmoPool.available) piercingAmmo.gameObject.transform.SetParent(transform);

        shrapnelAmmoPool = new Pool<Shrapnel>(shrapnelAmmoPrefab, shrapnelAmmoCount);
        foreach (Shrapnel shrapnelAmmo in shrapnelAmmoPool.available) shrapnelAmmo.gameObject.transform.SetParent(transform);

        multiBulletPool = new Pool<GenericBullet>(multiBulletPrefab, multiBulletCount);
        foreach (GenericBullet multiBullet in multiBulletPool.available) multiBullet.gameObject.transform.SetParent(transform);

        grenadePool = new Pool<Grenade>(grenadePrefab, grenadeCount);
        foreach (Grenade grenade in grenadePool.available) grenade.gameObject.transform.SetParent(transform);

        rocketPool = new Pool<Rocket>(rocketPrefab, rocketCount);
        foreach (Rocket rocket in rocketPool.available) rocket.gameObject.transform.SetParent(transform);

        enemyBulletPool = new Pool<Bullet>(enemyBulletPrefab, enemyBulletCount);
        foreach (Bullet enemyBullet in enemyBulletPool.available) enemyBullet.gameObject.transform.SetParent(transform);

        explosionParticlePool = new Pool<ExplosionParticle>(explosionParticlePrefab, explosionParticleCount);
        foreach (ExplosionParticle explosionParticle in explosionParticlePool.available) explosionParticle.gameObject.transform.SetParent(transform);
    }
}
