using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : Character
{
    // TODO: this will need to be a singleton reference
    public BTSManager thisBTSManager;
    public CharacterController characterController;

    #region movementStats
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
    public int currentLaserClip;
    #endregion

    float angle;
    Vector2 movement;



    public override void Start()
    {
        EventManager.current.receiveGold += ReceiveGold;

        currentBulletClip = (int)bulletStats.maxClip.value;
        currentGrenadeClip = (int)grenadeStats.maxClip.value;
        currentRocketClip = (int)rocketStats.maxClip.value;
        currentLaserClip = (int)laserStats.maxClip.value;
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        if (hp <= 0)
        {
            Debug.Log("Player Load lose");
            thisBTSManager.LoadLoseGameScene();
        }
        if (GameManager.current.gameState == GameState.Shop) return;
        HandleRotation();
        HandleMovement();
        HandleShooting();
        UpdateAnimation();
        HandleDashing();

        HandleReload();
        currentImmunityFrame -= Time.deltaTime;
        //UpdateAllLasers();
    }

    private void FixedUpdate()
    {
        if (GameManager.current.gameState == GameState.Shop) return;
        MoveCharacter();
    }

    protected void ReceiveGold(float amount)
    {
        gold += amount;
    }

    void HandleRotation()
    {
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 mouseOnScreen = Camera.main.ScreenToViewportPoint(Input.mousePosition);

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
        if (Input.GetKeyDown(KeyCode.Space) && currentTimeBetweenDashes<=0)
        {
            currentTimeBetweenDashes = stats.timeBetweenDashs.value;
            currentImmunityFrame = stats.immunityFromDashing.value;
            characterController.Move(new Vector3(lastMovementDirection.x * stats.dashAmount.value,0,lastMovementDirection.y * stats.dashAmount.value));
        }
    }

    void HandleShooting()
    {
        timeSinceFired += Time.deltaTime;
        if (reloading) return;
        if (Input.GetMouseButtonDown(0) || (timeSinceFired > stats.timeBetweenShots.value && Input.GetMouseButton(0)))
        {
            timeSinceFired = 0;
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

            if (currentLaserClip > 0)
            {
                GameObject laser = Instantiate(laserPrefab, bulletContainer);
                laser.transform.SetParent(null);
                Laser laserComponent = laser.GetComponent<Laser>();
                laserComponent.owner = this;
                currentLaserClip--;
            }
        }
    }

    void HandleReload()
    {
        if (
                currentBulletClip == 0 &&
                currentGrenadeClip == 0 &&
                currentRocketClip == 0 &&
                currentLaserClip == 0
            )
        {
            if (Input.GetMouseButtonDown(0) && !reloading)
            {
                reloading = !reloading; // swap status
                currentReloadTime = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && !reloading)
        {
            reloading = !reloading; // swap status
            currentReloadTime = 0;
        }

        if (reloading)
        {
            currentReloadTime += Time.deltaTime;
            if (currentReloadTime >= stats.reloadTime.value)
            {
                currentReloadTime = 0;
                currentBulletClip = (int)bulletStats.maxClip.value;
                currentGrenadeClip = (int)grenadeStats.maxClip.value;
                currentRocketClip = (int)rocketStats.maxClip.value;
                currentLaserClip = (int)laserStats.maxClip.value;
                reloading = false;
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
        Physics.SyncTransforms(); // This is for when the player transform is set. Character controllers have a bug with getting not caring about the new tranform.
        characterController.Move(
            new Vector3(
                movement.x * stats.moveSpeed.value * Time.fixedDeltaTime, 
                0, 
                movement.y * stats.moveSpeed.value * Time.fixedDeltaTime
                )
            );

    }


    public override void OnDestroy()
    {
        base.OnDestroy();
    }

}