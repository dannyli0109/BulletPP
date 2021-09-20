﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpellQueueSwarm : SpellQueueEnemy
{
    float currentAngle;

    public Vector3 nextDestination;

    public float coolDownTime;

    public float swarmSegmentDist;

    public AOEDamage aoePrefab;

    public override void Start()
    {
        base.Start();
    }

    public override void Init(GameObject target, Transform cam, AmmoPool ammoPool)
    {
        base.Init(target, cam, ammoPool);
        this.ammoPool = ammoPool;

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
        if (GameManager.current.GamePausing()) return;

        if (hp <= 0)
        {
            EventManager.current.ReceiveGold(gold);
            Destroy(gameObject);
        }

        agent.speed = 0;
        decision.MakeDecision();
        HandleMoving();


        UpdateAnimation();

        timeSinceFired += Time.deltaTime;
    }

    public override void HandleMoving()
    {
        if (coolDownTime <= 0)
        {
            finalDestination = target.transform.position;
        }
        else
        {
            coolDownTime -= Time.deltaTime;
        }

        agent.speed = speed;

        float distanceFromNext = Vector3.Distance(nextDestination, transform.position);

        if (lastKnownPos == transform.position)
        {
            timeStuck += Time.deltaTime;

            if (timeStuck < 0.3f)
            {
                timeStuck = 0;

                nextDestination = finalDestination;
            }
        }
        // too close move back
        else if (InRange(tooClose))
        {
            if (coolDownTime <= 0)
            {

            Debug.Log("bite");
            CreateAOE();
            coolDownTime = 5.0f;

            Vector3 normalAwayFromPlayer = Vector3.Normalize(new Vector3(UnityEngine.Random.RandomRange(0, 5), 0, UnityEngine.Random.RandomRange(0, 5)));

            finalDestination = target.transform.position + normalAwayFromPlayer * desiredRange;
            }
            nextDestination = finalDestination;
        }
        else
        {
            if(distanceFromNext< smoothingRange)
            {
                //Debug.Log(" new path");
                Vector3 normalToNext = Vector3.Normalize(finalDestination - transform.position);

                nextDestination = transform.position + normalToNext * swarmSegmentDist + new Vector3(UnityEngine.Random.RandomRange(-1, 1), 0, UnityEngine.Random.RandomRange(-1, 1));
                Debug.DrawLine(transform.position, nextDestination, Color.cyan, 0.1f);

            }
        }

        //Debug.DrawLine(transform.position, finalDestination,Color.red, 0.1f);
        agent?.SetDestination(nextDestination);

        lastKnownPos = transform.position;
        Debug.DrawLine(transform.position, finalDestination, Color.red, 0.1f);
    }

    public void doNothing()
    {

    }

    public void CreateAOE()
    {
        Debug.Log("AOE");
        Vector3 pos = new Vector3(transform.position.x, 0.01f, transform.position.z);
        AOEDamage aoeDamage = Instantiate(aoePrefab, pos, Quaternion.identity);

        aoeDamage.Init(rocketStats.radius.value, rocketStats.damage.value, 1 << 11);

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

        //Vector2 movemntRotated;
        //movemntRotated.x = (cos * tx) - (sin * ty);
        //movemntRotated.y = (sin * tx) + (cos * ty);
        //
        //// movemntRotated = new Vector2(tx, ty);
        //movemntRotated = movemntRotated.normalized;

       // animator.SetFloat("x", movemntRotated.x);
       // animator.SetFloat("y", movemntRotated.y);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }


}