using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{

    #region movementStats
    public CharacterStat DashAmount;
    public CharacterStat timeBetweenDashs;
    public Vector2 lastMovementDirection;
    public float currentTimeBetweenDashes;

    #endregion

    #region Gun Stats
    public float currentReloadTime;

    #endregion

    #region clipSize
    public int currentBulletClip;
    public int currentGrenadeClip;
    public int currentRocketClip;

    #endregion 

    public CharacterStat ImmunityFromDashing;
    public CharacterStat ImmunityFromDamage;
    public float CurrentImmunityFrame;

    public Animator animator;
    public CharacterController characterController;

    float angle;
    Vector2 movement;

    public override void Start()
    {
        currentBulletClip = (int)bulletStats.maxClip.value;
        currentGrenadeClip = (int)grenadeStats.maxClip.value;
        currentRocketClip = (int)rocketStats.maxClip.value;
        base.Start();
        //StatModifier modifier = new StatModifier(10, StatModType.Flat);
        //AddModifier(StatType.Bullet, "size", modifier);
    }

    public override void Update()
    {
        base.Update();
        HandleRotation();
        HandleMovement();
        HandleShooting();
        UpdateAnimation();
        HandleDashing();

        HandleReload();
        CurrentImmunityFrame -= Time.deltaTime;

    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    void HandleRotation()
    {
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        positionOnScreen.x = positionOnScreen.x * Screen.width - Screen.width / 2.0f;
        positionOnScreen.y = positionOnScreen.y * Screen.height - Screen.height / 2.0f;

        mouseOnScreen.x = mouseOnScreen.x * Screen.width - Screen.width / 2.0f;
        mouseOnScreen.y = mouseOnScreen.y * Screen.height - Screen.height / 2.0f;

        angle = Util.AngleBetweenTwoPoints(mouseOnScreen, positionOnScreen) + 90;
        transform.localRotation = Quaternion.Euler(new Vector3(0f, angle, 0f));
    } 

    void HandleMovement()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();

        if(movement.x!=0|| movement.y != 0)
        {
            lastMovementDirection = movement;

        }

    }

    void HandleDashing()
    {
        currentTimeBetweenDashes -= Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && currentTimeBetweenDashes<=0)
        {
            currentTimeBetweenDashes = timeBetweenDashs.value;
            CurrentImmunityFrame = ImmunityFromDashing.value;
            Debug.Log("Dashing");
            characterController.Move(new Vector3(lastMovementDirection.x * DashAmount.value,0,lastMovementDirection.y * DashAmount.value));
        }
    }

    void HandleShooting()
    {
        timeSinceFired += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) || (timeSinceFired > TimeBetweenShots.value && Input.GetMouseButton(0)))
        {
            timeSinceFired = 0;
            Reloading = false;
            if (currentBulletClip > 0)
            {

                GameObject bullet = Instantiate(bulletPrefab, bulletContainer);
                bullet.transform.SetParent(null);
                bullet.transform.localScale = new Vector3(bulletStats.size.value, bulletStats.size.value, bulletStats.size.value);
                Bullet bulletComponent = bullet.GetComponent<Bullet>();
                bulletComponent.owner = this;
                currentBulletClip--;
            }

            if (currentGrenadeClip > 0)
            {

                GameObject grenade = Instantiate(grenadePrefab, bulletContainer);
                grenade.transform.SetParent(null);
                Grenade grenadeComponent = grenade.GetComponent<Grenade>();
                grenadeComponent.owner = this;
                currentGrenadeClip--;
            }

            if (currentRocketClip > 0)
            {

                GameObject rocket = Instantiate(rocketPrefab, bulletContainer);
                rocket.transform.SetParent(null);
                Rocket rocketComponent = rocket.GetComponent<Rocket>();
                rocketComponent.owner = this;
                currentRocketClip--;
            }
        }
    }

    void HandleReload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reloading = !Reloading; // swap status
            currentReloadTime = 0;
        }
        if (Reloading)
        {
            currentReloadTime += Time.deltaTime;   
            if(currentReloadTime> ReloadTime.value)
            {
                currentReloadTime -= ReloadTime.value;
                // check if you actually get

                if(currentBulletClip < bulletStats.maxClip.value)
                {
                    currentBulletClip++;
                }
                if (currentGrenadeClip < grenadeStats.maxClip.value)
                {
                    currentGrenadeClip++;
                }
                if (currentRocketClip < rocketStats.maxClip.value)
                {
                    currentRocketClip++;
                }

            }
        }
    }

    void UpdateAnimation()
    {
        float sin = Mathf.Sin(angle * Mathf.Deg2Rad);
        float cos = Mathf.Cos(angle * Mathf.Deg2Rad);
        float tx = movement.x;
        float ty = movement.y;

        Vector2 movemntRotated;
        movemntRotated.x = (cos * tx) - (sin * ty);
        movemntRotated.y = (sin * tx) + (cos * ty);

        animator.SetFloat("x", movemntRotated.x);
        animator.SetFloat("y", movemntRotated.y);
    }

    void MoveCharacter()
    {
        characterController.Move(new Vector3(movement.x * moveSpeed.value * Time.fixedDeltaTime, 0, movement.y * moveSpeed.value * Time.fixedDeltaTime));
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

}