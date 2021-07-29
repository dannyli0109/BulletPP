using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    #region Movement
    public Vector3 FinalDestination;
    public Vector3 NextDestination;

    public float smoothingRange;

    public float speed;
    public float DesiredFurthestRange;
    public float FavouredRange;
    public float ClosestRange;

    public float minWaitingTime;
    public float maxWaitingTime;
    public float currentWaitingTime;

    public float NextDistSegmentLength;
    public float WalkingOffsetRadius;
    public float WanderingOffsetRadius;

    public float wallAvoidAmount;
    public bool touchingWall;
    public float enemAvoidAmount;


    public float FullStopMovingTime;
    public float currentStopMovingTime;

    #endregion

    #region Shooting Decisions
    public float currentShootingWaitTime;
    public int currentVolleySize;
    public bool reloading;
    public float currentReloadTime;
    public float currentTimeBetweenVolley;

    public int ClipSizeForForcedShot;

    public float UpperShootingRange; //dist from the target where they will stop shooting
    public float lowerShootingRange; // dist from target closer where they will stop shooting

    public int MaxVolleySize = 1; // how many bullets should be shot at once
    public int MinVolleySize = 1;

    public float TimeBetweenVolley;
    public float bulletReloadTime;

    public int currentBulletClipSize;

    public bool DEBUG;

    #endregion

    public GameObject target;

    float angle;
    public override void Start()
    {
        base.Start();
        GetNewDestination(false);
        currentBulletClipSize = (int)bulletStats.maxClip.value;
    }

    void GetNewDestination(bool Direct)
    {
        // if in the normal range
        Vector3 directionTowardstarget = Vector3.Normalize(new Vector3(target.transform.position.x - this.transform.position.x, 0, target.transform.position.z - this.transform.position.z));
        float distFromTarget = Vector3.Distance(this.transform.position, target.transform.position);
        float distFromFinal = Vector3.Distance(this.transform.position, FinalDestination);

        if (distFromTarget < DesiredFurthestRange && distFromTarget > ClosestRange)
        {
            if (smoothingRange > distFromFinal)
            {
                FinalDestination = this.transform.position + Vector3.Normalize(new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1))) * WanderingOffsetRadius;
            }

        }
        else
        {
           if(distFromTarget < ClosestRange)
            {
                FinalDestination = target.transform.position - directionTowardstarget * FavouredRange;
                // FinalDestination = target.transform.position - new Vector3(directionTowardstarget.x, 0, directionTowardstarget.y) * FavouredRange;
               // Debug.Log(directionTowardstarget);
            }
            else
            {
            FinalDestination = target.transform.position - directionTowardstarget * FavouredRange; 

            }

        }
       
        NextDestination = Vector3.MoveTowards(this.transform.position, FinalDestination, NextDistSegmentLength);

        if (!Direct)
        {
            Vector3 wanderingOffset = Vector3.Normalize(new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)));
            NextDestination += wanderingOffset * WalkingOffsetRadius;
        }
    }

    public override void Update()
    {
        HandleMove();
        base.Update();
        if (!target) return;
        Vector2 current = new Vector2(transform.position.x, transform.position.z);
        Vector2 targetPos = new Vector2(target.transform.position.x, target.transform.position.z);
        angle = Util.AngleBetweenTwoPoints(targetPos, current) + 90;
        transform.localRotation = Quaternion.Euler(new Vector3(0f, angle, 0f));

        timeSinceFired += Time.deltaTime;

        HandleDecidingToShoot();

    }

    public void HandleMove()
    {
        float distFromTarget = Vector3.Distance(this.transform.position, target.transform.position);

        if (distFromTarget < ClosestRange|| distFromTarget> DesiredFurthestRange)
        {
            GetNewDestination(true);
          
        }

        float distFromNext = Vector3.Distance(this.transform.position, NextDestination);
        Vector3 directionTowardstarget = Vector3.Normalize(new Vector3( target.transform.position.x- this.transform.position.x , 0, target.transform.position.z - this.transform.position.z));

        if (currentWaitingTime <= 0)
        {
            if (currentStopMovingTime<=0)
            {

        Debug.DrawLine(this.transform.position, this.transform.position + directionTowardstarget, Color.cyan);

        // if we're too close, make a new place to go
        if(distFromNext< smoothingRange)
        {
                if(distFromTarget < DesiredFurthestRange)
                {
                    // if close enough
                    currentWaitingTime =Random.Range(minWaitingTime, maxWaitingTime);
                }
                else
                {

                }
              GetNewDestination(false);
        }
        // are we at the right place , get a new place
        this.transform.position = Vector3.MoveTowards(this.transform.position, NextDestination, speed * Time.deltaTime);

        Debug.DrawLine(this.transform.position, FinalDestination, Color.green);
        Debug.DrawLine(this.transform.position, NextDestination,Color.blue);

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
                currentReloadTime = ReloadTime.value;
            }
            else
            {
                currentReloadTime -= Time.deltaTime;
            }
        }
        else if (currentShootingWaitTime <= 0)
        {
            if (currentVolleySize > 0&&currentBulletClipSize>0)
            {
                // while you can still fire keep doing it 
                Shoot();
                currentVolleySize--;
                currentBulletClipSize--;
                timeSinceFired = 0;
             //   Debug.Log("Shoot");
                currentShootingWaitTime = TimeBetweenShots.value;
                if (currentVolleySize == 0)
                {
                    currentShootingWaitTime = TimeBetweenVolley;
                }
            }
            else
            {
                if (currentBulletClipSize >= ClipSizeForForcedShot)
                {
                    if (currentTimeBetweenVolley <= 0)
                    {
                        if (currentVolleySize == 0)
                        {
                            currentVolleySize = Mathf.Clamp(Random.Range(MinVolleySize, MaxVolleySize+1),0,(int)bulletStats.maxClip.value);
                           // Debug.Log("shooting");
                            currentShootingWaitTime = TimeBetweenShots.value;
                        }
                    }
                }
                // if have  too many bullets fire or in right range
                if (currentBulletClipSize>0 && distFromTarget > lowerShootingRange && distFromTarget < UpperShootingRange)
                {
                    if (currentTimeBetweenVolley <= 0)
                    {
                        if (currentVolleySize == 0)
                        {
                            // set how many bullets to fire, 
                            currentVolleySize = Mathf.Clamp(Random.Range(MinVolleySize, MaxVolleySize+1), 0, (int)bulletStats.maxClip.value);
                           // Debug.Log("shooting");
                            currentShootingWaitTime = TimeBetweenShots.value;
                        }
                    }
                }
                else
                {
                    reloading = true;
                    currentReloadTime = ReloadTime.value;
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
            NextDestination += directionAwayFromWall *enemAvoidAmount*Time.deltaTime;
            currentWaitingTime = 0;

        }
        // walls
        if (other.gameObject.layer == 10)
        {
            Vector3 directionAwayFromWall = Vector3.Normalize(new Vector3(this.transform.position.x - other.gameObject.transform.position.x, 0, this.transform.position.z - other.gameObject.transform.position.z));
            Debug.DrawLine(this.transform.position, this.transform.position + directionAwayFromWall * wallAvoidAmount, Color.red, 2);
            NextDestination += directionAwayFromWall * wallAvoidAmount * Time.deltaTime; ;
            currentWaitingTime = 0;
            touchingWall = true;
            float dist = Vector3.Distance(this.transform.position, target.transform.position);
            if(dist< ClosestRange)
            {
                currentStopMovingTime = FullStopMovingTime;
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
}
