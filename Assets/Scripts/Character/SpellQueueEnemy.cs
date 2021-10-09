using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpellQueueEnemy : Enemy
{
    public List<Action> spellQueue;
    public List<float> spellTime;
    public int index = 0;
    public float minCooldown = 1.0f;
    public float maxCooldown = 3.0f;
    public float viewAngle = 60.0f;

    protected Decision decision;
    public float rotationSpeed;
    //public AmmoPool ammoPool;

    public bool notUsingAnimation;

    #region MoveStats
    [Header("Move Stats")]

    public float setAcceleration;

    public float tooFarToSeePlayer; // wander
    public float tooFarToShoot; // won't shoot
    public float tooClose; // will move away
    public float closeEnoughtDodge; // will

    public float desiredRange;
    public float smoothingRange;

   public Vector3 finalDestination;
   protected Vector3 nextDestination;

    protected  Vector3 lastKnownPos;
    protected float timeStuck;

    public float EnemyAvoidanceAmount;
    #endregion

    public override void Start()
    {
        base.Start();
        finalDestination = target.transform.position;
        agent.acceleration = setAcceleration;
    }

    public override void Init(GameObject target, Transform cam, AmmoPool ammoPool)
    {
        base.Init(target, cam, ammoPool);
        InitSpellQueue();

        decision = new Decision()
        {
            action = () => { },
            condition = () =>
            {
                return InRange(tooFarToShoot);
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
                    return InRange(1000);
                },
                trueBranch = ToMove(),
                falseBranch = null
            }
        };
    }

    public virtual void InitSpellQueue()
    {
        spellQueue = new List<Action>();
        spellTime = new List<float>();

        float holdingRand = UnityEngine.Random.Range(2, 4);
        spellQueue.Add(
            () =>
            {
                ShootBullets((int)holdingRand, 0, transform.forward, 60, bulletStats.speed.baseValue, 2);
            }
        );
        spellTime.Add(2.0f);

       holdingRand = UnityEngine.Random.Range(2, 4);
        for (int i = 0; i < holdingRand; i++)
        {
            spellQueue.Add(
                () => { ShootBullets(1, 0, 0, bulletStats.speed.baseValue, 3); }
            );

            spellTime.Add(0.2f);
        }
        spellTime.Add(1.5f);
        holdingRand = UnityEngine.Random.Range(2, 4);
        for (int i = 0; i < holdingRand; i++)
        {
            spellQueue.Add(
                () => { ShootBullets(1, 0, 0, bulletStats.speed.baseValue, 3); }
            );

            spellTime.Add(0.2f);
        }
        spellTime.Add(2f);
        index = 0;
    }

    public float GetCooldown()
    {
        return UnityEngine.Random.Range(minCooldown, maxCooldown);
    }

    public override void Update()
    {
        if (GameManager.current.GameTransitional()|| GameManager.current.GetState()==GameState.Pause)
        {
            agent.enabled = false;
            return;
        }
        else
        {
            agent.enabled = true;
        }

        if (hp <= 0)
        {
            EventManager.current.ReceiveGold(gold);
            Destroy(gameObject);
        }
        else
        {
            agent.speed = 0;
            decision.MakeDecision();
            HandleMoving();
            timeSinceFired += Time.deltaTime;
        }
        UpdateAnimation();
    }

    public virtual void HandleMoving()
    {
        agent.speed = speed;
        if(lastKnownPos== transform.position)
        {
            timeStuck += Time.deltaTime;

            if (timeStuck < 0.3f)
            {
                timeStuck = 0;
                finalDestination = target.transform.position;
            }
        }
        // too close move back
        else if (InRange(tooClose))
        {
            Vector3 normalAwayFromPlayer =Vector3.Normalize( transform.position - target.transform.position);
            finalDestination = target.transform.position + (normalAwayFromPlayer * desiredRange);        
        }

        // if line of sight and close enough get a random place
        else if((InLineOfSight(60)|| InRange(closeEnoughtDodge )|| InRange(tooFarToSeePlayer)))
        {
            float distanceFromFinal = Vector3.Distance(transform.position, finalDestination);

            if (distanceFromFinal < smoothingRange)
            {
                Vector3 normalAwayFromPlayer = Vector3.Normalize(new Vector3(UnityEngine.Random.Range(0, 5), 0, UnityEngine.Random.Range(0, 5)));

                finalDestination = target.transform.position + normalAwayFromPlayer * 2;
            }
            else
            {
            }

        }
        else
        {
            finalDestination = target.transform.position;
        }

        //Debug.DrawLine(transform.position, finalDestination,Color.red, 0.1f);
        if (hp > 0)
        {
        agent?.SetDestination(finalDestination);

        }

        lastKnownPos = transform.position;
    }

    protected override void UpdateAnimation()
    {
        Vector2 current = new Vector2(transform.position.x, transform.position.z);
        Vector2 targetPos = new Vector2(target.transform.position.x, target.transform.position.z);
        //angle = Util.AngleBetweenTwoPoints(targetPos, current) + 90;

        //Vector3.MoveTowards(transform.localEulerAngles, new Vector3(0f, angle, 0f), 1);
        /// transform.localRotation = Quaternion.Euler(new Vector3(0f, angle, 0f));

        Vector3 holdingAngle = target.transform.position - transform.position;
        holdingAngle.y = 0; // keep the direction strictly horizontal
        Quaternion rotation = Quaternion.LookRotation(holdingAngle);
        // slerp to the desired rotation over time
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        float sin = Mathf.Sin(angle * Mathf.Deg2Rad);
        float cos = Mathf.Cos(angle * Mathf.Deg2Rad);

        Vector3 velocity = agent.velocity;
        float tx = velocity.x;
        float ty = velocity.z;

        if (!notUsingAnimation)
        {
        Vector2 movemntRotated;
        movemntRotated.x = (cos * tx) - (sin * ty);
        movemntRotated.y = (sin * tx) + (cos * ty);

        // movemntRotated = new Vector2(tx, ty);
        movemntRotated = movemntRotated.normalized;

        animator.SetFloat("x", movemntRotated.x);
        animator.SetFloat("y", movemntRotated.y);
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    private void OnTriggerStay(Collider other)
    {
     //   if(other.gameObject.layer== 12)
     //   {
     //       //Debug.Log("enemy");
     //       Vector3 normal = other.gameObject.transform.position - transform.position;
     //
     //       Debug.DrawLine(transform.position, transform.position + normal * 5,Color.blue,0.1f);
     //       finalDestination += normal * enemyAvoidAmount * Time.deltaTime;
     //
     //   }
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

    // TODO: FIX THIS
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
        Vector3 forward = bulletContainer.forward;
        ShootBullet(forward, angle, offset, speed, acceleration, damage, size);
    }

    void ShootBullet(float angle, float speed, float damage, float size)
    {
        ShootBullet(angle, 0, speed, new Vector3(0, 0, 0), damage, size);
    }

    void ShootBullet(Vector3 forward, float angle, float speed, float damage, float size)
    {
        ShootBullet(forward, angle, 0, speed, new Vector3(0, 0, 0), damage, size);
    }

    void ShootBullet(Vector3 forward, float angle, float offset, float speed, Vector3 acceleration, float damage, float size)
    {
        Bullet bullet;
        AmmoPool ammoPool = AmmoPool.current;
        ammoPool.enemyBulletPool.TryInstantiate(out bullet, bulletContainer.position, bulletContainer.rotation);
        Ammo ammoComponent = bullet.GetComponent<Ammo>();
        ammoComponent.Init(this, forward, angle, offset, speed, acceleration, damage, size, 0);
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
         //   Debug.Log("To Move()");
         //       agent.speed = speed;
         //       agent?.SetDestination(target.transform.position);
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
                    SoundManager.PlaySound(SoundType.GunshotEnemy, bulletContainer.position, 1);
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
