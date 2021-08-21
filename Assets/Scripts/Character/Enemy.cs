using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : Character
{
    #region Movement
    public Vector3 finalDestination;
    public Vector3 nextDestination;

    public float smoothingRange;

    public float speed;
    public float desiredFurthestRange;
    public float favouredRange;
    public float closestRange;

    public float minWaitingTime;
    public float maxWaitingTime;
    public float currentWaitingTime;

    public float nextDistSegmentLength;
    public float walkingOffsetRadius;
    public float wanderingOffsetRadius;

    public float wallAvoidAmount;
    public bool touchingWall;
    public float enemyAvoidAmount;

    public float fullStopMovingTime;
    public float currentStopMovingTime;

    #endregion

    #region Shooting Decisions
    public float maxCreationShootingWaitTime;
    public float minCreationShootingWaitTime;
    public float currentShootingWaitTime;
    public int currentVolleySize;
    public float currentReloadTime;
    public float currentTimeBetweenVolley;

    public int clipSizeForForcedShot;

    public float upperShootingRange; //dist from the target where they will stop shooting
    public float lowerShootingRange; // dist from target closer where they will stop shooting

    public int maxVolleySize = 1; // how many bullets should be shot at once
    public int minVolleySize = 1;

    public float timeBetweenVolley;
    public float bulletReloadTime;

    public int currentBulletClipSize;

    public bool DEBUG;
    #endregion

    public GameObject target;
    public NavMeshAgent agent;

    float angle;
    public override void Start()
    {
        base.Start();
        GetNewDestination(false);
        currentBulletClipSize = (int)bulletStats.maxClip.value;
        currentShootingWaitTime = UnityEngine.Random.Range(minCreationShootingWaitTime,maxCreationShootingWaitTime);
    }

    public override void Update()
    {
        if (GameManager.current.gameState == GameState.Shop) return;
        HandleMovement();
        base.Update();
        if (!target) return;

        UpdateAnimation();

        timeSinceFired += Time.deltaTime;

        HandleDecidingToShoot();

    }

    void GetNewDestination(bool direct)
    {
        // if in the normal range
        Vector3 directionTowardstarget = Vector3.Normalize(new Vector3(target.transform.position.x - this.transform.position.x, 0, target.transform.position.z - this.transform.position.z));
        float distFromTarget = Vector3.Distance(this.transform.position, target.transform.position);
        float distFromFinal = Vector3.Distance(this.transform.position, finalDestination);

        if (distFromTarget < desiredFurthestRange && distFromTarget > closestRange)
        {
            if (smoothingRange > distFromFinal)
            {
                finalDestination = this.transform.position + Vector3.Normalize(new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1))) * wanderingOffsetRadius;
            }

        }
        else
        {
           if(distFromTarget < closestRange)
            {
                finalDestination = target.transform.position - directionTowardstarget * favouredRange;
                // FinalDestination = target.transform.position - new Vector3(directionTowardstarget.x, 0, directionTowardstarget.y) * FavouredRange;
               // Debug.Log(directionTowardstarget);
            }
            else
            {
                finalDestination = target.transform.position - directionTowardstarget * favouredRange;
            }

        }
       
        nextDestination = Vector3.MoveTowards(this.transform.position, finalDestination, nextDistSegmentLength);

        if (!direct)
        {
            Vector3 wanderingOffset = Vector3.Normalize(new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)));
            nextDestination += wanderingOffset * walkingOffsetRadius;
        }
    }

    
    protected void UpdateAnimation()
    {
        Vector2 current = new Vector2(transform.position.x, transform.position.z);
        Vector2 targetPos = new Vector2(target.transform.position.x, target.transform.position.z);
        angle = Util.AngleBetweenTwoPoints(targetPos, current) + 90;
        transform.localRotation = Quaternion.Euler(new Vector3(0f, angle, 0f));

        float sin = Mathf.Sin(angle * Mathf.Deg2Rad);
        float cos = Mathf.Cos(angle * Mathf.Deg2Rad);

        float tx = nextDestination.x;
        float ty = nextDestination.z;

        Vector2 movemntRotated;
        movemntRotated.x = (cos * tx) - (sin * ty);
        movemntRotated.y = (sin * tx) + (cos * ty);

        // movemntRotated = new Vector2(tx, ty);
        movemntRotated = movemntRotated.normalized;

        animator.SetFloat("x", movemntRotated.x);
        animator.SetFloat("y", movemntRotated.y);
    }
    

    public void HandleMovement()
    {
        float distFromTarget = Vector3.Distance(this.transform.position, target.transform.position);

        if (distFromTarget < closestRange|| distFromTarget> desiredFurthestRange)
        {
            GetNewDestination(true);
          
        }

        float distFromNext = Vector3.Distance(this.transform.position, nextDestination);
        Vector3 directionTowardstarget = Vector3.Normalize(new Vector3( target.transform.position.x- this.transform.position.x , 0, target.transform.position.z - this.transform.position.z));

        if (currentWaitingTime <= 0)
        {
            if (currentStopMovingTime<=0)
            {

                Debug.DrawLine(this.transform.position, this.transform.position + directionTowardstarget, Color.cyan);

                // if we're too close, make a new place to go
                if(distFromNext< smoothingRange)
                {
                    if(distFromTarget < desiredFurthestRange)
                    {
                        // if close enough
                        currentWaitingTime = Random.Range(minWaitingTime, maxWaitingTime);
                    }
                    else
                    {

                    }
                    GetNewDestination(false);
                }
                // are we at the right place , get a new place
                //this.transform.position = Vector3.MoveTowards(this.transform.position, nextDestination, speed * Time.deltaTime);
                agent.speed = speed;
                agent.SetDestination(target.transform.position);
                //if (agent.hasPath)
                //{
                //}
                //this.transform.position = Vector3.MoveTowards(this.transform.position, nextDestination, speed * Time.deltaTime);

                Debug.DrawLine(this.transform.position, finalDestination, Color.green);
                Debug.DrawLine(this.transform.position, nextDestination,Color.blue);

            }
            else
            {
                currentStopMovingTime -= Time.deltaTime;
                if (currentStopMovingTime <= 0)
                {
                    GetNewDestination(false);
                }
            }
        }
        else
        {
            currentWaitingTime -= Time.deltaTime;
        }

    }

    public override void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletContainer);
        bullet.transform.SetParent(null);
        Vector3 scale = bullet.transform.localScale;
        bullet.transform.localScale = scale * bulletStats.size.value;
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        bulletComponent.owner = this;
    }

    public void HandleDecidingToShoot()
    {
        currentTimeBetweenVolley -= Time.deltaTime;
        float distFromTarget = Vector3.Distance(this.transform.position, target.transform.position);
        // if time is left, reduce it from  next

        if (reloading)
        {
            if (currentReloadTime <= 0)
            {
               // Debug.Log("reload");
                currentBulletClipSize = Mathf.Clamp(currentBulletClipSize + 1, 0, (int)bulletStats.maxClip.value);

                reloading = false;
                currentReloadTime = stats.reloadTime.value;
            }
            else
            {
                currentReloadTime -= Time.deltaTime;
            }
        }
        else if (currentShootingWaitTime <= 0)
        {
            if (currentVolleySize > 0 && currentBulletClipSize>0)
            {
                // while you can still fire keep doing it 
                Shoot();
                currentVolleySize--;
                currentBulletClipSize--;
                timeSinceFired = 0;
             //   Debug.Log("Shoot");
                currentShootingWaitTime = stats.timeBetweenShots.value;
                if (currentVolleySize == 0)
                {
                    currentShootingWaitTime = timeBetweenVolley;
                }
            }
            else
            {
                if (currentBulletClipSize >= clipSizeForForcedShot)
                {
                    if (currentTimeBetweenVolley <= 0)
                    {
                        if (currentVolleySize == 0)
                        {
                            currentVolleySize = Mathf.Clamp(Random.Range(minVolleySize, maxVolleySize+1),0,(int)bulletStats.maxClip.value);
                           // Debug.Log("shooting");
                            currentShootingWaitTime = stats.timeBetweenShots.value;
                        }
                    }
                }
                // if have too many bullets fire or in right range
                if (currentBulletClipSize>0 && distFromTarget > lowerShootingRange && distFromTarget < upperShootingRange)
                {
                    if (currentTimeBetweenVolley <= 0)
                    {
                        if (currentVolleySize == 0)
                        {
                            // set how many bullets to fire, 
                            currentVolleySize = Mathf.Clamp(Random.Range(minVolleySize, maxVolleySize+1), 0, (int)bulletStats.maxClip.value);
                           // Debug.Log("shooting");
                            currentShootingWaitTime = stats.timeBetweenShots.value;
                        }
                    }
                }
                else
                {
                    reloading = true;
                    currentReloadTime = stats.reloadTime.value;
                }
            }
        }
        else
        {
            currentShootingWaitTime -= Time.deltaTime;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
      
    }

    private void OnTriggerStay(Collider other)
    {
        // enem
        if (other.gameObject.layer == 12)
        {
            Vector3 directionAwayFromWall = Vector3.Normalize(new Vector3(this.transform.position.x - other.gameObject.transform.position.x, 0, this.transform.position.z - other.gameObject.transform.position.z));
            Debug.DrawLine(this.transform.position, this.transform.position + directionAwayFromWall * wallAvoidAmount, Color.red, 2);
            nextDestination += directionAwayFromWall *enemyAvoidAmount*Time.deltaTime;
            currentWaitingTime = 0;

        }
        // walls
        if (other.gameObject.layer == 10)
        {
            Vector3 directionAwayFromWall = Vector3.Normalize(new Vector3(this.transform.position.x - other.gameObject.transform.position.x, 0, this.transform.position.z - other.gameObject.transform.position.z));
            Debug.DrawLine(this.transform.position, this.transform.position + directionAwayFromWall * wallAvoidAmount, Color.red, 2);
            nextDestination += directionAwayFromWall * wallAvoidAmount * Time.deltaTime; ;
            currentWaitingTime = 0;
            touchingWall = true;
            float dist = Vector3.Distance(this.transform.position, target.transform.position);
            if (dist < closestRange)
            {
                currentStopMovingTime = fullStopMovingTime;
                Debug.Log(dist + " touching walls");
            }
        }
        else
        {
            touchingWall = false;   
        }
    }

    private void OnTriggerExit(Collider other)
    {
        touchingWall = false;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public virtual void Init(GameObject target, Transform cam)
    {
        this.target = target;
        transform.GetChild(0).gameObject.GetComponent<Billboard>().cam = cam;
    }
}
