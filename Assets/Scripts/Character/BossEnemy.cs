using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossEnemy : SpellQueueEnemy
{
    // skill one
    public Vector2 min;
    public Vector2 max;
    public int amounts;
    public int skillOneRepeatTime;
    int shootDirection;

    // skill two
    public int maxAmountOfSnipers;
    int amountOfSnipers;

    // skill three
    //public GameObject laserPrfab;
    public bool usingLaser;


    public override void Start()
    {
        base.Start();
        finalDestination = target.transform.position;
        agent.acceleration = setAcceleration;
        amountOfSnipers = 0;
    }

    public override void Init(Player target, Transform cam, AmmoPool ammoPool)
    {
        base.Init(target, cam, ammoPool);
        EventManager.current.enemyDeath += ReceiveEnemyDeath;
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
        if (usingLaser)
        {
            Aiming();
        }
        else
        {
            NotAiming();
        }
        HandleMoving();

        UpdateAnimation();
        UpdateRotation();

        timeSinceFired += Time.deltaTime;
    }

    public override void HandleMoving()
    {
        agent.speed = speed;
        finalDestination = target.transform.position;
        if (hp > 0)
        {
            agent?.SetDestination(finalDestination);
        }
    }


    public override void InitSpellQueue()
    {
        spellQueue = new List<Action>();
        spellTime = new List<float>();


        // will only trigger once
        //shootDirection = UnityEngine.Random.Range(0, 4);

        // skill 1
        {
            spellTime.Add(0);
            spellQueue.Add(RandomIndex);
            for (int i = 0; i < skillOneRepeatTime; i++)
            {
                spellTime.Add(2);
                spellQueue.Add(Skill1);
            }
        }

        // skill 2
        {
            spellTime.Add(0);
            spellQueue.Add(Skill2);
        }

        // skill 3
        {
            spellTime.Add(0);
            spellQueue.Add(ToggleLaserOn);
            spellTime.Add(1);
            spellQueue.Add(ToggleLaserOff);
            spellTime.Add(0);
            spellQueue.Add(Skill3);
        }

        index = 0;
    }




    public void RandomIndex()
    {
        shootDirection = UnityEngine.Random.Range(0, 4);
    }

    public void Skill1()
    {
        //shootDirection = UnityEngine.Random.Range(0, 4);

        ShootFromWall(shootDirection);
    }

    public void ShootFromWall(int direction)
    {

        // direciton 0, 1, 2, 3
        // form top, right, bottom, left
        int[] dirX = { 0, -1, 0, 1 };
        int[] dirY = { -1, 0, 1, 0 };


        Debug.Log("doing skill 1");
        float offsetX = (max.x - min.x) / (float)amounts;
        float offsetY = (max.y - min.y) / (float)amounts;
        for (int i = 0; i < amounts; i++)
        {
            float posX;
            float posY;
            if (direction == 0)
            {
                // top down
                posX = transform.position.x + min.x + offsetX * i;
                posY = transform.position.z + max.y;
            }
            else if (direction == 1)
            {
                // right left
                posX = transform.position.x + max.x;
                posY = transform.position.z + min.y + offsetY * i;
            }
            else if (direction == 2)
            {
                // bottom up
                posX = transform.position.x + min.x + offsetX * i;
                posY = transform.position.z + min.y;
            }
            else
            {
                // left right
                posX = transform.position.x + min.x;
                posY = transform.position.z + min.y + offsetY * i;
            }


            ShootBullet(new Vector3(posX, bulletContainer.position.y, posY), new Vector3(dirX[direction], 0, dirY[direction]), 0, 0, 10, Vector3.zero, 1, 1);

        }
    }
    public void Skill2()
    {
        if (amountOfSnipers < maxAmountOfSnipers)
        {
            float posX = transform.position.x + UnityEngine.Random.Range(min.x, max.x);
            float posY = transform.position.z + UnityEngine.Random.Range(min.y, max.y);
            mapGenerationScript.SpawnSniper(new Vector3(posX, 0, posY), 0);
            amountOfSnipers++;
        }
        else
        {
            timeSinceFired = 100;
        }
    }


    public void ReceiveEnemyDeath(Vector3 pos)
    {
        amountOfSnipers--;
        //Debug.Log(EnemiesInEncounter);
    }

    public void ToggleLaserOn()
    {
        usingLaser = true;
    }

    public void ToggleLaserOff()
    {
        usingLaser = false;
    }


    public void Aiming()
    {


        thisLineRenderer.useWorldSpace = true;
        float laserLength = 36.0f;
        RaycastHit hitInfo;
        if (Physics.Raycast(bulletContainer.position, bulletContainer.forward, out hitInfo, laserLength, 1 << 10))
        {
            laserLength = hitInfo.distance;
        }
        Vector3 lookDir = (bulletContainer.forward) * laserLength;
        thisLineRenderer.SetPosition(0, bulletContainer.position);
        thisLineRenderer.SetPosition(1, bulletContainer.position + lookDir);
        
        //else
        //{
        //    Vector3 lookDir = (bulletContainer.forward) * 12;
        //    thisLineRenderer.SetPosition(0, bulletContainer.position);
        //    thisLineRenderer.SetPosition(1, bulletContainer.position);
        //}
    }

    public void NotAiming()
    {
        thisLineRenderer.useWorldSpace = true;
        //Vector3 lookDir = (bulletContainer.forward) * 12;
        thisLineRenderer.SetPosition(0, bulletContainer.position);
        thisLineRenderer.SetPosition(1, bulletContainer.position);
    }

    public void Skill3()
    {
        // shoot laser
        GameObject laser = Instantiate(laserPrefab, bulletContainer.position, bulletContainer.rotation);
        Laser laserComponent = laser.GetComponent<Laser>();
        laserComponent.Init(this, bulletContainer.forward, 0, 10, 1, 1);
    }
}
