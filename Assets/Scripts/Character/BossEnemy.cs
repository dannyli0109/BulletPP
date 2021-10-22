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
    public float projectileSpeed;
    public int skillOneRepeatTime;
    int shootDirection;


    // skill two
    public int maxAmountOfSnipers;
    int amountOfSnipers;

    // skill three
    //public GameObject laserPrfab;
    public bool usingLaser;

    // moving
    public GameObject[] bossCoverObjects;
    public Vector3[] bossCoverSetPos;
    public GameObject bossHitBox;
    public int savedBossPos;
    public float bossSpeed;
    public GameObject prefabCover;
    public Vector3 belowAmount;
    public float moveHitBoxSpeed;
    public Vector3 holdFirstBossTransform;

    public Vector3 holdingCenter;
    public List<float> pointsWhereBossGoesIntoHiding;
    public float currentHidingTime;
    public float hidingTime;

    public float subPosedToBeY;

    public override void Start()
    {
        base.Start();
     //   finalDestination = target.transform.position;
        agent.acceleration = setAcceleration;
        amountOfSnipers = 0;
        holdingCenter = this.transform.position;
        holdFirstBossTransform = this.gameObject.transform.position;
        for (int i=0; i< bossCoverObjects.Length; i++)
        {
           GameObject holdingObject = Instantiate(prefabCover,mapGenerationScript.rooms[mapGenerationScript.currentRoomInside].thisPrefabInfo.enemySpawnPoint[i+1].position, prefabCover.transform.rotation);
           bossCoverObjects[i] = holdingObject;
            bossCoverSetPos[i] = holdingObject.transform.position;
        }
        bossHitBox.transform.position = bossCoverObjects[0].transform.position;
    }

    public override void Init(Player target, Transform cam, AmmoPool ammoPool, float healthPercentageIncrease, float SpeedPercentageIncrease)
    {
        base.Init(target, cam, ammoPool,healthPercentageIncrease, SpeedPercentageIncrease);
        EventManager.current.enemyDeath += ReceiveEnemyDeath;
        InitSpellQueue();

        decision = new Decision()
        {
            action = () => { },
            condition = () =>
            {
                return true;
                //return InRange(tooFarToShoot);
            },
            trueBranch = new Decision()
            {
                action = () => { },
                condition = () =>
                {
                    return true;
                    //return InLineOfSight(60);
                },
                trueBranch = ToShoot(),
                falseBranch = ToMove()
            },
            falseBranch = new Decision()
            {
                action = () => { },
                condition = () =>
                {
                    return true;
                    //return InRange(1000);
                },
                trueBranch = ToMove(),
                falseBranch = null
            }
        };
    }

    public override void Update()
    {
        DebugXY();
        if (GameManager.current.GameTransitional() || GameManager.current.GetState() == GameState.Pause)
        {
            agent.enabled = false;
            return;
        }
        else
        {
            agent.enabled = true;
        }

        if (pointsWhereBossGoesIntoHiding.Count > 0)
        {
           // Debug.Log("Hp: "+ hp +" next hiding  "+ pointsWhereBossGoesIntoHiding[pointsWhereBossGoesIntoHiding.Count - 1]);
            if (hp <= pointsWhereBossGoesIntoHiding[pointsWhereBossGoesIntoHiding.Count - 1])
            {
                pointsWhereBossGoesIntoHiding.RemoveAt(pointsWhereBossGoesIntoHiding.Count - 1);
                currentHidingTime = hidingTime;
                Debug.Log("Hide");
                moveBossHitbox();
            }
        }

        if (hp <= 0)
        {
            EventManager.current.ReceiveGold(gold);
            Destroy(gameObject);
        }
        updateBossHitbox();
        agent.speed = 0;

        decision.MakeDecision();

        if (usingLaser && currentHidingTime <= 0)
        {
            Aiming();
        }
        else
        {
            NotAiming();
        }

        UpdateRotation();

        timeSinceFired += Time.deltaTime;
        currentHidingTime -= Time.deltaTime;

        if (currentHidingTime > 0)
        {
            subPosedToBeY = Mathf.Clamp(subPosedToBeY - Time.deltaTime * 10, -4, bossCoverSetPos[0].y);
        }
        else
        {
            subPosedToBeY = Mathf.Clamp(subPosedToBeY + Time.deltaTime * 10, -4, bossCoverSetPos[0].y);
        }
            transform.position = new Vector3(transform.position.x, subPosedToBeY, transform.position.z);
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

        // Boss move
        {
            spellTime.Add(moveHitBoxSpeed);
            spellQueue.Add(moveBossHitbox);
            spellTime.Add(moveHitBoxSpeed);
        }

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

        // Boss move
        {
            spellTime.Add(moveHitBoxSpeed);
            spellQueue.Add(moveBossHitbox);
            spellTime.Add(moveHitBoxSpeed);
        }

        // skill 2
        {
            spellTime.Add(0);
            spellQueue.Add(Skill2);
        }

        // Boss move
        {
            spellTime.Add(moveHitBoxSpeed);
         spellQueue.Add(moveBossHitbox);
            spellTime.Add(moveHitBoxSpeed);
        }

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

        // Boss move
        {
            spellTime.Add(moveHitBoxSpeed);
            spellQueue.Add(moveBossHitbox);
            spellTime.Add(moveHitBoxSpeed);
        }

        // skill 3
        {
            spellTime.Add(0);
            spellQueue.Add(ToggleLaserOn);
            spellTime.Add(2);
            spellQueue.Add(ToggleLaserOff);
            spellTime.Add(0);
            spellQueue.Add(Skill3);
        }

        // Boss move
        {
            spellTime.Add(moveHitBoxSpeed);
            spellQueue.Add(moveBossHitbox);
            spellTime.Add(moveHitBoxSpeed);
        }

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

        // Boss move
        {
            spellTime.Add(moveHitBoxSpeed);
            spellQueue.Add(moveBossHitbox);
            spellTime.Add(moveHitBoxSpeed);
        }


        // skill 2
        {
            spellTime.Add(0);
            spellQueue.Add(Skill2);
        }

        // Boss move
        {
            spellTime.Add(moveHitBoxSpeed);
            spellQueue.Add(moveBossHitbox);
            spellTime.Add(moveHitBoxSpeed);
        }

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

        // Boss move
        {
            spellTime.Add(moveHitBoxSpeed);
            spellQueue.Add(moveBossHitbox);
            spellTime.Add(moveHitBoxSpeed);
        }

        // skill 3
        {
            spellTime.Add(0);
            spellQueue.Add(ToggleLaserOn);
            spellTime.Add(3);
            spellQueue.Add(ToggleLaserOff);
            spellTime.Add(0);
            spellQueue.Add(Skill3);
        }

        // Boss move
        {
            spellTime.Add(moveHitBoxSpeed);
            spellQueue.Add(moveBossHitbox);
            spellTime.Add(moveHitBoxSpeed);
        }

        index = 0;
    }

    public void RandomIndex()
    {
        shootDirection = UnityEngine.Random.Range(0, 4);
    }

    public void Skill1()
    {
       // Debug.Log("Skill1");  
        ShootFromWall(shootDirection);
    }

    public void DebugXY()
    {
        //float offsetX = (max.x - min.x) / (float)amounts;
        //float offsetY = (max.y - min.y) / (float)amounts;
        Debug.DrawLine(holdingCenter + new Vector3(min.x, 0, min.y), holdingCenter + new Vector3(max.x, 0, min.y),Color.red,0.1f);
        Debug.DrawLine(holdingCenter + new Vector3(max.x, 0, min.y), holdingCenter + new Vector3(max.x, 0, max.y), Color.yellow, 0.1f);
        Debug.DrawLine(holdingCenter + new Vector3(max.x, 0, max.y), holdingCenter + new Vector3(min.x, 0, max.y), Color.blue, 0.1f);
        Debug.DrawLine(holdingCenter + new Vector3(min.x, 0,max.y), holdingCenter + new Vector3(min.x, 0, min.y), Color.green, 0.1f);
    }

    public void ShootFromWall(int direction)
    {
        //direction = 0;
        // direciton 0, 1, 2, 3
        // form top, right, bottom, left
        int[] dirX = { 0, -1, 0, 1 };
        int[] dirY = { -1, 0, 1, 0 };

        Debug.Log("doing skill 1 at "+direction);
        float offsetX = (max.x - min.x) / (float)amounts;
        float offsetY = (max.y - min.y) / (float)amounts;
        for (int i = 0; i < amounts; i++)
        {
            float posX;
            float posY;
            if (direction == 0)
            {
                // top down
                posX =holdingCenter.x + min.x + offsetX * i;
                posY = holdingCenter.z + max.y;
            }
            else if (direction == 1)
            {
                // right left
                posX =holdingCenter.x +max.x;
                posY = holdingCenter.z + min.y + offsetY * i;
            }
            else if (direction == 2)
            {
                // bottom up
                posX = holdingCenter.x +min.x + offsetX * i;
                posY = holdingCenter.z + min.y;
            }
            else
            {
                // left right
                posX = holdingCenter.x + min.x;
                posY = holdingCenter.z + min.y + offsetY * i;
            }

            //  ShootBullet(new Vector3(max.x-min.x, 2, min.y), new Vector3(0, 0, 1), 0, 0, projectileSpeed, Vector3.zero, 1, bulletStats.size.value);
            Debug.DrawLine(new Vector3(posX, 1.2f, posY), new Vector3(posX, bulletContainer.position.y, posY) + new Vector3(dirX[direction], 0, dirY[direction]) * 10, Color.cyan, 1.0f);
            ShootBullet(new Vector3(posX, 1, posY), new Vector3(dirX[direction], 0, dirY[direction]), 0, 0, projectileSpeed, Vector3.zero, 1, 1);

        }
    }

    public void Skill2()
    {
        Debug.Log("Skill2");
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

    public void moveBossHitbox()
    {
       // Debug.Log("hitbox");
        savedBossPos = UnityEngine.Random.Range(0, 5);
        bossHitBox.transform.position = bossCoverObjects[savedBossPos].transform.position;
      //  bossCoverTransforms[newPos].position;
    }

    public void updateBossHitbox()
    {
        for (int i=0; i<5; i++)
        {
            if(i == savedBossPos)
            {
                if (currentHidingTime <= 0)
                {
                    bossCoverObjects[i].transform.position = Vector3.MoveTowards(bossCoverObjects[i].transform.position, bossCoverSetPos[i] + belowAmount, Time.deltaTime * bossSpeed);
                    //Debug.Log(Vector3.MoveTowards(bossHitBox.transform.position, bossCoverSetPos[i], Time.deltaTime * bossSpeed));
                   // bossHitBox.transform.position = Vector3.MoveTowards(bossHitBox.transform.position, bossCoverSetPos[i], Time.deltaTime * bossSpeed);
                    // subPosedToBeY= boss
                   // Debug.Log(bossHitBox.transform.position);
                }
                else
                {
                    bossCoverObjects[i].transform.position = Vector3.MoveTowards(bossCoverObjects[i].transform.position, bossCoverSetPos[i], Time.deltaTime * bossSpeed);
                   // bossHitBox.transform.position = Vector3.MoveTowards(bossHitBox.transform.position, bossCoverSetPos[i]+belowAmount, Time.deltaTime * bossSpeed);
                }
            }
            else
            {
                bossCoverObjects[i].transform.position = Vector3.MoveTowards(bossCoverObjects[i].transform.position, bossCoverSetPos[i], Time.deltaTime * bossSpeed);
            }
        }
        //transform.position = new Vector3(transform.position.x, -1, transform.position.z);
    
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
        float laserLength = 100.0f;
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

        Debug.Log("Skill3");
        // shoot laser
        GameObject laser = Instantiate(laserPrefab, bulletContainer.position, bulletContainer.rotation);
        Laser laserComponent = laser.GetComponent<Laser>();
        laserComponent.Init(this, bulletContainer.forward, 0, 10, 1, 1);
    }
}
