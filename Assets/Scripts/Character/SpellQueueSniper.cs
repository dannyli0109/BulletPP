using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpellQueueSniper : SpellQueueEnemy
{
    public Transform gunPoint;
    bool usinglaser;

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
                    return InRange(100);
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
            ShootBullets(2, 0, transform.forward,25, bulletStats.speed.baseValue, 4);
              });
        spellTime.Add(2.2f);

        spellQueue.Add(() =>{
               ActivateLaser(true);    
           });
        spellTime.Add(1.2f);

        spellQueue.Add( () =>{
         ActivateLaser(false);
            ShootBullets(1, 0, 0, 50, 3);
        } );
        spellTime.Add(0.6f);

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
        if (hp > 0)
        {
            HandleMoving();
        }
        UpdateAnimation();
        UpdatelaserSight();

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

    public void ActivateLaser(bool usingLaser)
    {
        usinglaser = usingLaser;
    }

    public void UpdatelaserSight()
    {
        thisLineRenderer.useWorldSpace = true;
        if (usinglaser)
        {
            Vector3 lookDir = (gunPoint.forward) * 36;
            thisLineRenderer.SetPosition(0, gunPoint.position);
            thisLineRenderer.SetPosition(1, gunPoint.position + lookDir);
        }
        else
        {
           // Debug.Log("off");
            Vector3 lookDir = (gunPoint.forward) * 12;
            thisLineRenderer.SetPosition(0, gunPoint.position);
            thisLineRenderer.SetPosition(1, gunPoint.position );
        }

    }

}
