using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpellQueueEnemy : Enemy
{
    // Start is called before the first frame update

    public List<Action> spellQueue;
    public List<float> spellTime;
    public int index = 0;


    public override void Start()
    {
        base.Start();
        // Decision decision = new Decision
        
    }

    public override void Init(GameObject target, Transform cam)
    {
        base.Init(target, cam);
        spellQueue = new List<Action>();
        spellTime = new List<float>();

        spellQueue.Add(
            () => { ShootBullets(10, 360); }
        );
        spellTime.Add(2);

        for (int i = 0; i < 3; i++)
        {
            spellQueue.Add(
                () => { ShootBullets(1, 0); }
            );
            if (i == 0)
            {
                spellTime.Add(2);
            }
            else
            {
                spellTime.Add(0.5f);
            }
        }
        index = 0;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (GameManager.current.gameState == GameState.Shop) return;

        if (hp <= 0)
        {
            EventManager.current.ReceiveGold(gold);
            Destroy(gameObject);
        }

        UpdateAnimation();
        
        if (timeSinceFired >= spellTime[index % spellTime.Count])
        {
            spellQueue[index % spellQueue.Count]();
            timeSinceFired = 0;
            index++;
        }

        timeSinceFired += Time.deltaTime;
    }


    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void ShootBullets(int amount, float angle)
    {
        if (amount == 1) ShootBullet(0);
        float angleIncrement = angle / (float)(amount - 1);
        float currentAngle = -angle / 2.0f;
        for (int i = 0; i < amount; i++)
        {
            ShootBullet(currentAngle);
            currentAngle += angleIncrement;
        }
    }

    void ShootBullet(float angle)
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletContainer);
        Ammo ammoComponent = bullet.GetComponent<Ammo>();
        ammoComponent.Init(this, angle);
    }

    public Decision DecideToShoot(float range)
    {
        Decision decision = new Decision()
        {
            action = () => { },
            condition = () =>
            {
                return Vector3.Distance(transform.position, target.transform.position) <= range;
            },
            trueBranch = ToShoot(),
            falseBranch = null
        };
        return decision;
    }

    public Decision ToShoot()
    {
        Decision decision = new Decision()
        {
            action = () => {
                
            },
            condition = null,
            trueBranch = null,
            falseBranch = null
        };
        return decision;
    }

}
