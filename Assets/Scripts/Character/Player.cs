using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Player : Character
{
    // TODO: this will need to be a singleton reference
    public BTSManager thisBTSManager;
    public CharacterController characterController;

    public float RecentlyTakenDamage; // goes up when taken damage, goes down each turn

    #region movementStats
    public Vector3 lastMovementDirection;
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

    public AmmoPool ammoPool;
    public GameEvent sellAugment;

    public Transform gunpointForAiming;

    float angle;
    Vector2 movement;

    bool freshReload;
    public override void Start()
    {
        EventManager.current.receiveGold += ReceiveGold;

        currentBulletClip = (int)bulletStats.maxClip.value;
        currentGrenadeClip = (int)grenadeStats.maxClip.value;
        currentRocketClip = (int)rocketStats.maxClip.value;
        currentLaserClip = (int)laserStats.maxClip.value;
        currentBouncingBladeClip = 1;
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        if (hp <= 0)
        {
            Debug.Log("Player Load lose");
            GameManager.current.ChangeStateImmdeiate(GameState.Transitional);
            thisBTSManager.LoadLoseGameScene();
           
        }
        if (GameManager.current.GameTransitional()) return;
        HandleRotation();
        HandleMovement();
        HandleShooting();
        UpdateAnimation();
        HandleDashing();
        HandleReload();
        currentImmunityFrame -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (GameManager.current.GameTransitional()) return;
        MoveCharacter();
    }

  public override void CheckOnDamageTrigger()
    {
        RecentlyTakenDamage++;
    }

    protected void ReceiveGold(float amount)
    {
        gold += amount;
    }

    public Vector3 GetPlayerPlaneMousePos(Vector3 aPlayerPos)
    {
        Plane plane = new Plane(Vector3.up, aPlayerPos);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float dist;
        if (plane.Raycast(ray, out dist))
        {
            return ray.GetPoint(dist);
        }
        return Vector3.zero;
    }

    void HandleRotation()
    {
        //Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        //Vector2 mouseOnScreen = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        //positionOnScreen.x = positionOnScreen.x * Screen.width - Screen.width / 2.0f;
        //positionOnScreen.y = positionOnScreen.y * Screen.height - Screen.height / 2.0f;

        //mouseOnScreen.x = mouseOnScreen.x * Screen.width - Screen.width / 2.0f;
        //mouseOnScreen.y = mouseOnScreen.y * Screen.height - Screen.height / 2.0f;

        //angle = Util.AngleBetweenTwoPoints(mouseOnScreen, positionOnScreen);
        //transform.localRotation = Quaternion.Euler(new Vector3(0f, angle, 0f));

        // Cast a ray from screen point
        Vector3 mousePosition = Input.mousePosition;
        //Debug.Log(mousePosition.y);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Save the info
        RaycastHit hit;
        // You successfully hit
        if (Physics.Raycast(ray, out hit, 100, 1 << 18))
        {
            //Debug.Log(hit.collider.gameObject.layer);
            // Find the direction to move in
            //Vector3 hitPoint = new Vector3(hit.point.x, bulletContainer.position.y, hit.point.z);

            Vector3 dir = hit.point - gunpointForAiming.position;
            dir.y = 0;
            transform.localRotation = Quaternion.LookRotation(dir);
            angle = transform.localRotation.eulerAngles.y;
            

            //float distance = Vector3.Distance(hit.point, bulletContainer.position);

            //if (distance > 0.9)
            //{
            //    transform.localRotation = Quaternion.LookRotation(dir);
            //    angle = transform.localRotation.eulerAngles.y;
            //}
        }
    }

    void HandleMovement()
    {

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();

         if(movement.x!=0|| movement.y != 0)
        {
            lastMovementDirection = movement;
            rocketExitPoint.LookAt(transform);
        }
    }

    void HandleDashing()
    {
        currentTimeBetweenDashes -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && currentTimeBetweenDashes<=0)
        {
            currentTimeBetweenDashes = stats.timeBetweenDashs.value;
            currentImmunityFrame = stats.immunityFromDashing.value;
            transform.position = new Vector3(transform.position.x, 0.12f, transform.position.z);
            characterController.Move(new Vector3(lastMovementDirection.x * stats.dashAmount.value, 0, lastMovementDirection.y * stats.dashAmount.value));
            rocketExitPoint.position = transform.position;
            ResolveDashEffects();
        }
    }

    void HandleShooting()
    {
        timeSinceFired += Time.deltaTime;
        if (reloading) return;
        if (Input.GetMouseButtonDown(0) || (timeSinceFired > stats.timeBetweenShots.value && Input.GetMouseButton(0)))
        {
            timeSinceFired = 0;
            // shoot the main one in the middle
            if (ShootAmmos(0))
            {
                SoundManager.PlaySound(SoundType.Gunshot, bulletContainer.position, 1.0f);
            }

            for (int i = 0; i < (int)AmmoType.Count; i++)
            {
                AmmoStats ammoStat = GetAmmoStats(i);
                float totalAdditionalAmmos = (ammoStat.additionalAmmo.value + stats.additionalAmmo.value);
                int bulletsOnOneSide = Mathf.CeilToInt(totalAdditionalAmmos / 2.0f);
                int count = 0;
                for (int j = 1; j <= bulletsOnOneSide; j++)
                {
                    float angle = -(stats.spreadAngle.value / (float)(bulletsOnOneSide) * (float)j) / 2.0f;
                    float increment = stats.spreadAngle.value / (float)(bulletsOnOneSide) * (float)j;
                    for (int k = 0; k < 2; k++)
                    {
                        if (count >= totalAdditionalAmmos) break;
                        angle += k * increment;
                        Shoot(i, angle);
                        count++;
                    }
                }
            }
            freshReload = false;
            /*
            if (currentBouncingBladeClip > 0 && bouncingBladeStats.maxClip.value > 0)
            {
                GameObject blade = Instantiate(bouncingBladePrefab, bulletContainer);
                Ammo ammoComponent = blade.GetComponent<Ammo>();
                ammoComponent.Init(this, 0);
                //BouncingBlade bladeComponent = blade.GetComponent<BouncingBlade>();
                //  bladeComponent.owner = this;
                currentBouncingBladeClip--;
            }
            if (currentLaserClip > 0)
            {
                GameObject laser = Instantiate(laserPrefab, bulletContainer);
                Ammo ammoComponent = laser.GetComponent<Ammo>();
                ammoComponent.Init(this, angle);
                currentLaserClip--;
            }
            */
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 17)
        {
           // Debug.Log("health");
            hp++;
            other.gameObject.transform.parent.gameObject.SetActive(false);

        }
    }

    AmmoStats GetAmmoStats(int type)
    {
        switch (type)
        {
            case 0:
                return bulletStats;
            case 1:
                return grenadeStats;
            case 2:
                return laserStats;
            case 3:
                return rocketStats;
            case 4:
                return bouncingBladeStats;
            default:
                break;
        }
        return null;
    }

    void Shoot(int type, float angle)
    {
        switch (type)
        {
            case 0:
                ShootBullet(angle);
                break;
            case 1:
                ShootGrenade(angle);
                break;
            case 2:
                ShootLaser(angle);
                break;
            case 3:
                ShootRocket(angle);
                break;
            case 4:
                ShootLaser(angle);
                break;
            case 5:
                ShootBouncingBlade(angle);
                break;
            default:
                break;
        }
    }

    bool ShootBullet(float angle)
    {
        if (currentBulletClip > 0)
        {
            Bullet bullet;
            if (ammoPool.bulletPool.TryInstantiate(out bullet, bulletContainer.position, bulletContainer.rotation))
            {
                Ammo ammoComponent = bullet.GetComponent<Ammo>();
                Vector3 forward = bulletContainer.forward;
                ammoComponent.Init(this, forward, angle, bulletStats.speed.value, stats.damageMultiplier.value * bulletStats.damage.value, bulletStats.size.value);
                currentBulletClip--;
                if (freshReload)
                {
                    ammoComponent.fromFreshReload = true;
                }
                return true;
            }
        }
        return false;
    }

    bool ShootRocket(float angle)
    {
        if (currentRocketClip > 0)
        {
            Rocket rocket;
            if (ammoPool.rocketPool.TryInstantiate(out rocket, bulletContainer.position, bulletContainer.rotation))
            {
                Ammo ammoComponent = rocket.GetComponent<Ammo>();
                Vector3 forward = bulletContainer.forward;
                ammoComponent.Init(this, forward, angle, rocketStats.speed.value, stats.damageMultiplier.value * rocketStats.damage.value, rocketStats.size.value);
                currentRocketClip--;
                if (freshReload)
                {
                    ammoComponent.fromFreshReload = true;
                }
                return true;
            }
        }
        return false;
    }

    bool ShootGrenade(float angle)
    {
        if (currentGrenadeClip > 0)
        {
            Grenade grenade;
            if (ammoPool.grenadePool.TryInstantiate(out grenade, bulletContainer.position, bulletContainer.rotation))
            {
                Ammo ammoComponent = grenade.GetComponent<Ammo>();
                Vector3 forward = bulletContainer.forward;
                ammoComponent.Init(this, forward, angle, bulletStats.speed.value, stats.damageMultiplier.value * bulletStats.damage.value, bulletStats.size.value);
                currentGrenadeClip--;
                if (freshReload)
                {
                    ammoComponent.fromFreshReload = true;
                }
                return true;
            }
        }
        return false;
    }

    bool ShootLaser(float angle)
    {
        if (currentLaserClip > 0)
        {
            GameObject laser = Instantiate(laserPrefab, bulletContainer);
            Ammo ammoComponent = laser.GetComponent<Ammo>();
            Vector3 forward = bulletContainer.forward;
            ammoComponent.Init(this, forward, angle, laserStats.speed.value, stats.damageMultiplier.value * laserStats.damage.value, laserStats.size.value);
            laser.transform.SetParent(null);
            currentLaserClip--;
            if (freshReload)
            {
                ammoComponent.fromFreshReload = true;
            }
            return true;
        }
        return false;
    }

    bool ShootBouncingBlade(float angle)
    {
       // Debug.Log("trying to shoot blade");
        if (currentBouncingBladeClip > 0 && bouncingBladeStats.maxClip.value > 0)
        {
            GameObject blade = Instantiate(bouncingBladePrefab, bulletContainer);
            Ammo ammoComponent = blade.GetComponent<Ammo>();
            Vector3 forward = bulletContainer.forward;
            ammoComponent.Init(this, forward, angle, bouncingBladeStats.speed.value, stats.damageMultiplier.value * bouncingBladeStats.damage.value, bouncingBladeStats.size.value);
            blade.transform.SetParent(null);
            currentBouncingBladeClip--;
            if (freshReload)
            {
                ammoComponent.fromFreshReload = true;
            }
            return true;
        }
        return false;
    }

    bool ShootAmmos(float angle)
    {
        bool shot = false;
        if (ShootBullet(angle)) shot = true;
        if (ShootGrenade(angle)) shot = true;
        if (ShootRocket(angle)) shot = true;
        if (ShootLaser(angle)) shot = true;
        if (ShootBouncingBlade(angle)) shot = true;
        return shot;
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
                //currentBouncingBladeClip = (int)bouncingBladeStats.maxClip.value;
                reloading = false;
                if (stats.extraDamageAfterReload.value > 1)
                {
                    freshReload = true;
                }

                // Debug.Log("Bullet " + currentBulletClip + " " + (int)bulletStats.maxClip.value + " grenade " + currentGrenadeClip + " " + (int)grenadeStats.maxClip.value + " rocket  " + currentRocketClip + " " + (int)rocketStats.maxClip.value + " current laser " + currentLaserClip + " " + (int)laserStats.maxClip.value + " blades " + currentBouncingBladeClip + " " + (int)bouncingBladeStats.maxClip.value);
            }
        }
    }

    void PartialReload()
    {
        Debug.Log("part reload");
        currentBulletClip = (int)Mathf.Clamp(currentBulletClip+1, 0, bulletStats.maxClip.value);
        currentGrenadeClip = (int)Mathf.Clamp(currentGrenadeClip + 1, 0, grenadeStats.maxClip.value);
        currentRocketClip = (int)Mathf.Clamp(currentRocketClip + 1, 0, rocketStats.maxClip.value);
        currentLaserClip = (int)Mathf.Clamp(currentLaserClip + 1, 0, laserStats.maxClip.value);
        if (stats.extraDamageAfterReload.value > 1)
        {
            freshReload = true;
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

    void ResolveDashEffects()
    {
        if (stats.reloadOnDash.value > 0)
        {
            PartialReload();
        }

        if (stats.shootRocketOnDash.value > 0)
        {
            Rocket rocket;
            if (ammoPool.rocketPool.TryInstantiate(out rocket, rocketExitPoint.position, rocketExitPoint.rotation))
            {
                Ammo ammoComponent = rocket.GetComponent<Ammo>();
                Vector3 forward = lastMovementDirection;
                ammoComponent.Init(this, forward, angle, bulletStats.speed.value, stats.damageMultiplier.value * bulletStats.damage.value, bulletStats.size.value);
            }
        }

        if (stats.personalSpace.value > 1)
        {
            CreatePlayerAOE(new Vector2(transform.position.x, transform.position.z));
        }
    }

    void MoveCharacter()
    {
        rocketExitPoint.position = transform.position;
        transform.position = new Vector3(transform.position.x, 0.12f, transform.position.z);
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
        GameManager.current.ChangeStateImmdeiate(GameState.Transitional);
        base.OnDestroy();
    }

    public void SellAugment(int index)
    {
        if (GameManager.current.GetState() != GameState.Shop) return;
        Augment augment = inventory.augments[index];
        AugmentManager augmentManager = AugmentManager.current;
  

        if (inventory.RemoveAt(index))
        {
            for (int i = 0; i < augmentManager.augmentDatas[augment.id].synergies.Count; i++)
            {
                SynergyData synergyData = augmentManager.augmentDatas[augment.id].synergies[i];
                for (int j = 0; j < synergies.Count; j++)
                {
                    if (synergies[j].id == synergyData.id)
                    {
                        if (synergies[j].count == 1)
                        {
                            synergies.RemoveAt(j);
                            break;
                        }
                        else
                        {
                            synergies[j].count--;
                            break;
                        }
                    }
                }
            }

            gold += augmentManager.costs[augmentManager.augmentDatas[augment.id].rarity] * (augment.level + 1);
            sellAugment?.Invoke();
        }
    }
}