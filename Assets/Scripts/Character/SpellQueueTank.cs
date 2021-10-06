using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpellQueueTank : SpellQueueEnemy
{
    float currentAngle;

    public override void Start()
    {
        base.Start();
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
        for(int i=0; i < 3; i++)
        {
        spellQueue.Add(() => {
            currentAngle += 5;
            ShootBullets(12,currentAngle , transform.forward,360, bulletStats.speed.baseValue, 3);
              });
        spellTime.Add(0.6f);

        }
        spellTime.Add(2.5f);

        for (int i=0; i<12; i++)
        {
            Debug.Log(i);
            float holdingAngle = i * 35.0f;
            spellQueue.Add(() => {
              //  Debug.Log(currentAngle + i * 180);
                ShootBullets(2, holdingAngle, transform.forward, 180, bulletStats.speed.baseValue, 3);
            });
            spellTime.Add(0.3f);
           
        }
        spellTime.Add(4.0f);

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

        HandleMoving();

        UpdateAnimation();

        timeSinceFired += Time.deltaTime;
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
        base.OnDestroy();
    }

  
}
