using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPool : MonoBehaviour
{
    public Bullet bulletPrefab;
    public Pool<Bullet> bulletPool;
    public int bulletCount;

    public Grenade grenadePrefab;
    public Pool<Grenade> grenadePool;
    public int grenadeCount;

    public Rocket rocketPrefab;
    public Pool<Rocket> rocketPool;
    public int rocketCount;

    public Bullet enemyBulletPrefab;
    public Pool<Bullet> enemyBulletPool;
    public int enemyBulletCount;

    // Start is called before the first frame update
    void Start()
    {
        bulletPool = new Pool<Bullet>(bulletPrefab, bulletCount);
        foreach(Bullet bullet in bulletPool.available) bullet.gameObject.transform.SetParent(transform);

        grenadePool = new Pool<Grenade>(grenadePrefab, grenadeCount);
        foreach (Grenade grenade in grenadePool.available) grenade.gameObject.transform.SetParent(transform);

        rocketPool = new Pool<Rocket>(rocketPrefab, rocketCount);
        foreach (Rocket rocket in rocketPool.available) rocket.gameObject.transform.SetParent(transform);


        enemyBulletPool = new Pool<Bullet>(enemyBulletPrefab, enemyBulletCount);
        foreach (Bullet enemyBullet in enemyBulletPool.available) enemyBullet.gameObject.transform.SetParent(transform);

    }
}
