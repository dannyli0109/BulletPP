
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpellQueueRunner : SpellQueueEnemy
{
    float currentAngle;

    bool exploding;
    public float ExplodingTime;
    float currentExplodingTime;

    public override void Start()
    {
        base.Start();
    }

    public override void Init(Player target, Transform cam, AmmoPool ammoPool, float healthPercentageIncrease, float SpeedPercentageIncrease)
    {
        base.Init(target, cam, ammoPool, healthPercentageIncrease, SpeedPercentageIncrease);

        InitSpellQueue();

        decision = new Decision()
        {
            action = () => { },
            condition = () =>
            {
                return InRange(100);
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
                    return InRange(2);
                },
                trueBranch = ToMove(),
                falseBranch = null
            }
        };
    }

    public override void InitSpellQueue()
    {
        spellQueue = new List<Action>();
        spellTime = new List<float>();

        spellQueue.Add(() => {
            // ShootBullets(2, 0, transform.forward, 180, 3.5f, 3);
            doNothing();
        });
        spellTime.Add(5.0f);

        index = 0;
    }

    public override void Update()
    {
        if (GameManager.current.GameTransitional() || GameManager.current.GetState() == GameState.Pause)
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

        agent.speed = 0;
        decision.MakeDecision();
        if (exploding)
        {
            handleExploding();
            agent.speed = 0;
        }
        else
        {
            HandleMoving();
        }

        UpdateAnimation();

        timeSinceFired += Time.deltaTime;
    }

    public override void HandleMoving()
    {
        agent.speed = speed;

        // too close move back
        if (InRange(tooClose))
        {
            Debug.Log("exploding");
            exploding = true;
            agent.speed = 0;
        }
        else
        {
            finalDestination = target.transform.position;
        }
        Debug.DrawLine(transform.position, finalDestination, Color.red, 0.1f);
        if (hp > 0)
        {
            agent?.SetDestination(finalDestination);
        }
        //Debug.Log("dist " + Vector3.Distance(transform.position, finalDestination));
    }

    public void CreateAOE()
    {
        Debug.Log("AOE");
        Vector3 pos = new Vector3(transform.position.x, 0.01f, transform.position.z);
        AOEDamage aoeDamage = Instantiate(aoePrefab, pos, Quaternion.identity);

       aoeDamage.Init(rocketStats.radius.value, rocketStats.damage.value, 1 << 12|1<<11);

    }

    public void doNothing()
    {

    }

    public void handleExploding()
    {

      //  Debug.Log(currentExplodingTime);
        currentExplodingTime += Time.deltaTime;
        if(currentExplodingTime> ExplodingTime)
        {
            EventManager.current.ReceiveGold(gold);
            Destroy(gameObject);
        }
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
        CreateAOE();
        base.OnDestroy();
    }

}
