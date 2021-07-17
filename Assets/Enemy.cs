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
       
    #endregion

    public GameObject target;

    float angle;
    public override void Start()
    {
        base.Start();
        GetNewDestination(false);
    }

    void GetNewDestination(bool Direct)
    {
        // if in the normal range
        Vector3 directionTowardstarget = Vector3.Normalize(new Vector3(target.transform.position.x - this.transform.position.x, 0, target.transform.position.z - this.transform.position.z));
        float distFromTarget = Vector3.Distance(this.transform.position, target.transform.position);
        float distFromFinal = Vector3.Distance(this.transform.position, FinalDestination);

        if (distFromTarget< DesiredFurthestRange && distFromTarget > ClosestRange)
        {if (smoothingRange > distFromFinal)
            {
                FinalDestination = this.transform.position + Vector3.Normalize(new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1))) * WanderingOffsetRadius;
            }
       
        }
        else
        {
            FinalDestination = target.transform.position - directionTowardstarget * FavouredRange;
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

        if (timeSinceFired >= bulletStats.fireRate.value)
        {
         //   Shoot();
            timeSinceFired = 0;
        }
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

        Debug.Log(distFromNext);
        if (currentWaitingTime <= 0)
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

        //move towards it
        this.transform.position = Vector3.MoveTowards(this.transform.position, NextDestination, speed * Time.deltaTime);

        Debug.DrawLine(this.transform.position, FinalDestination, Color.green);
        Debug.DrawLine(this.transform.position, NextDestination,Color.blue);

        }
        else
        {
            currentWaitingTime -= Time.deltaTime;
        }

    }

    public void HandleDecidingToShoot()
    {

    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }
}
