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
    public float minCooldown = 1.0f;
    public float maxCooldown = 3.0f;
    public float viewAngle = 60.0f;

    Decision decision;

    public override void Start()
    {
        base.Start();


    }

    public override void Init(GameObject target, Transform cam)
    {
        base.Init(target, cam);
        InitSpellQueue();

        decision = new Decision()
        {
            action = () => { },
            condition = () =>
            {
                return InRange(10);
            },
            trueBranch = new Decision()
            {
                action = () => { },
                condition = () =>
                {
                    return InLineOfSight(60);
                },
                trueBranch = ToShoot(),
                falseBranch = ToMove()
            },
            falseBranch = new Decision()
            {
                action = () => { },
                condition = () =>
                {
                    return InRange(30);
                },
                trueBranch = ToMove(),
                falseBranch = null
            }
        };
    }

    public void InitSpellQueue()
    {
        spellQueue = new List<Action>();
        spellTime = new List<float>();

        Vector3 forawrd = bulletContainer.forward;
        for (int i = 0; i < 3; i++)
        {
            int j = i;
            spellQueue.Add(
                () => {
                    ShootBullets(10, (float)j * 30.0f, forawrd, 360, 3, 5);
                }
            );

            if (i == 0)
            {
                spellTime.Add(GetCooldown());
            }
            else
            {
                spellTime.Add(0.8f);
            }
        }
        for (int i = 0; i < 10; i++)
        {
            spellQueue.Add(
                () => { ShootBullets(1, 0, 0, 5, 5); }
            );
            if (i == 0)
            {
                spellTime.Add(GetCooldown());
            }
            else
            {
                spellTime.Add(0.5f);
            }
        }
        index = 0;
    }

    public float GetCooldown()
    {
        return UnityEngine.Random.Range(minCooldown, maxCooldown);
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


        agent.speed = 0;
        decision.MakeDecision();
        UpdateAnimation();

        timeSinceFired += Time.deltaTime;
    }

    protected override void UpdateAnimation()
    {
        Vector2 current = new Vector2(transform.position.x, transform.position.z);
        Vector2 targetPos = new Vector2(target.transform.position.x, target.transform.position.z);
        angle = Util.AngleBetweenTwoPoints(targetPos, current) + 90;
        transform.localRotation = Quaternion.Euler(new Vector3(0f, angle, 0f));

        float sin = Mathf.Sin(angle * Mathf.Deg2Rad);
        float cos = Mathf.Cos(angle * Mathf.Deg2Rad);

        Vector3 velocity = agent.velocity;
        float tx = velocity.x;
        float ty = velocity.z;

        Vector2 movemntRotated;
        movemntRotated.x = (cos * tx) - (sin * ty);
        movemntRotated.y = (sin * tx) + (cos * ty);

        // movemntRotated = new Vector2(tx, ty);
        movemntRotated = movemntRotated.normalized;

        animator.SetFloat("x", movemntRotated.x);
        animator.SetFloat("y", movemntRotated.y);
    }


    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void ShootBullets(int amount, float initialAngle, float angle, float speed, float size)
    {
        if (amount == 1) ShootBullet(initialAngle, speed, stats.damageMultiplier.value * bulletStats.damage.value, size);
        float angleIncrement = angle / (float)(amount - 1);
        float currentAngle = -angle / 2.0f;
        for (int i = 0; i < amount; i++)
        {
            ShootBullet(currentAngle + initialAngle, speed, stats.damageMultiplier.value * bulletStats.damage.value, size);
            currentAngle += angleIncrement;
        }
    }

    public void ShootBullets(int amount, float initialAngle, Vector3 forward, float angle, float speed, float size)
    {
        if (amount == 1) ShootBullet(forward, initialAngle, speed, stats.damageMultiplier.value * bulletStats.damage.value, size);
        float angleIncrement = angle / (float)(amount - 1);
        float currentAngle = -angle / 2.0f;
        for (int i = 0; i < amount; i++)
        {
            ShootBullet(forward, currentAngle + initialAngle, speed, stats.damageMultiplier.value * bulletStats.damage.value, size);
            currentAngle += angleIncrement;
        }
    }

    void ShootBullet(float angle, float offset, float speed, Vector3 acceleration, float damage, float size)
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletContainer);
        Ammo ammoComponent = bullet.GetComponent<Ammo>();
        Vector3 forward = bulletContainer.forward;
        ammoComponent.Init(this, forward, angle, offset, speed, acceleration, damage, size);
    }

    void ShootBullet(float angle, float speed, float damage, float size)
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletContainer);
        Ammo ammoComponent = bullet.GetComponent<Ammo>();
        Vector3 forward = bulletContainer.forward;
        ammoComponent.Init(this, forward, angle, speed, damage, size);
    }

    void ShootBullet(Vector3 forward, float angle, float speed, float damage, float size)
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletContainer);
        Ammo ammoComponent = bullet.GetComponent<Ammo>();
        ammoComponent.Init(this, forward, angle, speed, damage, size);
    }

    public bool InLineOfSight(float viewAngle)
    {
        Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
        if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
        {
            float distanceToTarget = Vector3.Distance(agent.transform.position, target.transform.position);
            if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, 1 << 10))
            {
                return true;
            }
        }
        return false;
    }

    public bool InRange(float range)
    {
        return Vector3.Distance(transform.position, target.transform.position) <= range;
    }

    public Decision DecideToMove(float range)
    {
        Decision decision = new Decision()
        {
            action = () => { },
            condition = () =>
            {
                return Vector3.Distance(transform.position, target.transform.position) <= range;
            },
            trueBranch = ToMove(),
            falseBranch = null
        };
        return decision;
    }

    public Decision ToMove()
    {
        Decision decision = new Decision()
        {
            action = () => {
                agent.speed = speed;
                agent.SetDestination(target.transform.position);
            },
            condition = null,
            trueBranch = null,
            falseBranch = null
        };
        return decision;
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
            falseBranch = DecideToMove(20)
        };
        return decision;
    }

    public Decision ToShoot()
    {
        Decision decision = new Decision()
        {
            action = () => {
                if (timeSinceFired >= spellTime[index % spellTime.Count])
                {
                    spellQueue[index % spellQueue.Count]();
                    timeSinceFired = 0;
                    index++;
                }
            },
            condition = null,
            trueBranch = null,
            falseBranch = null
        };
        return decision;
    }

}
