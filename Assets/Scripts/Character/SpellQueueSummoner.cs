using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpellQueueSummoner : SpellQueueEnemy
{
    float currentAngle;
    bool usingLaser;
    public Transform gunPoint;

    bool exploding;
    public float ExplodingTime;
    float currentExplodingTime;

    public override void Start()
    {
        base.Start();
    }

    public override void Init(float health, Player target, Transform cam, AmmoPool ammoPool, float healthPercentageIncrease, float SpeedPercentageIncrease)
    {
        base.Init(health, target, cam, ammoPool, healthPercentageIncrease, SpeedPercentageIncrease);

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
            mapGenerationScript.SpawnSwarms(transform.position, 0);
            teleport();
        });
        spellTime.Add(3.0f);

        spellQueue.Add(() => {
            ToggleLaser(true);
        });
        spellTime.Add(0.7f);

        spellQueue.Add(() => {
            ToggleLaser(false);
        });
        spellTime.Add(0.9f);

        spellTime.Add(3.0f);

        spellQueue.Add(() => {
            ToggleLaser(true);
        });
        spellTime.Add(0.7f);

        spellQueue.Add(() => {
            ToggleLaser(false);
        });
        spellTime.Add(0.9f);

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

        decision.MakeDecision();
            HandleMoving();
        if (exploding)
        {
            handleExploding();
          
        }

        UpdateAnimation();
        UpdatelaserSight();

        timeSinceFired += Time.deltaTime;
    }

    public override void HandleMoving()
    {
        agent.speed = speed;

        if (hp > 0)
        {
            agent?.SetDestination(finalDestination);
        }
        //Debug.Log("dist " + Vector3.Distance(transform.position, finalDestination));
    }

    public void CreateAOE()
    {
      //  Debug.Log("AOE");
        Vector3 pos = new Vector3(transform.position.x, 0.01f, transform.position.z);
        AOEDamage aoeDamage = Instantiate(aoePrefab, pos, Quaternion.identity);

        aoeDamage.Init(rocketStats.radius.value, rocketStats.damage.value, 1 << 12 | 1 << 11);

    }

    public void doNothing()
    {

    }

    public void teleport()
    {
        Debug.Log("teleport");
        transform.position = mapGenerationScript.ReturnNormalEnemyPosInRoom();
    }

    public void ToggleLaser(bool on)
    {
        Debug.Log("toggle laser");
        usingLaser = on;
    }

    public void UpdatelaserSight()
    {
        thisLineRenderer.useWorldSpace = true;
        if (usingLaser)
        {
            Vector3 lookDir = (gunPoint.forward) * 48;
            thisLineRenderer.SetPosition(0, gunPoint.position);
            thisLineRenderer.SetPosition(1, gunPoint.position + lookDir);
        }
        else
        {
            // Debug.Log("off");
            Vector3 lookDir = (gunPoint.forward) * 12;
            thisLineRenderer.SetPosition(0, gunPoint.position);
            thisLineRenderer.SetPosition(1, gunPoint.position);
        }

    }

    public void handleExploding()
    {

        //  Debug.Log(currentExplodingTime);
        currentExplodingTime += Time.deltaTime;
        if (currentExplodingTime > ExplodingTime)
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

       // Vector2 movemntRotated;
       // movemntRotated.x = (cos * tx) - (sin * ty);
       // movemntRotated.y = (sin * tx) + (cos * ty);
       //
       // // movemntRotated = new Vector2(tx, ty);
       // movemntRotated = movemntRotated.normalized;
      //
      //  animator.SetFloat("x", movemntRotated.x);
      //  animator.SetFloat("y", movemntRotated.y);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }
}