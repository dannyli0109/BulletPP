using UnityEngine;

public class Player : Character
{
    // TODO: this will need to be a singleton reference
    public BTSManager thisBTSManager;
    public CharacterController characterController;
    public MapGeneration mapGenerationScript;

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

    public TrailRenderer trailRenderer;
    public float dashDuration;
    public float trailTime;
    public bool isDashing;
    public GameObject dashShield;

    public float magX;
    public float magY;
    public float cameraShakeTime;
    public AnimationCurve curve;

    Vector2 dashDirection;
    float angle;
    Vector2 movement;

   public Vector3[] lastKnownPos;
   float currentLastKnownPosTimer;

    #region scripts
    public CameraFollowing cameraFollowingScript;
    #endregion

    bool freshReload;

    public override void Start()
    {
        for (int i = 0; i < lastKnownPos.Length - 1; i++)
        {
            lastKnownPos[i] = transform.position;
        }
        //EventManager.current.receiveGold += ReceiveGold;

        currentBulletClip = (int)bulletStats.maxClip.value;
        currentGrenadeClip = (int)grenadeStats.maxClip.value;
        currentRocketClip = (int)rocketStats.maxClip.value;
        currentLaserClip = (int)laserStats.maxClip.value;
        currentBouncingBladeClip = 1;
        inventoryIndex = 0;
        inventory = new Inventory(6);

        //inventory.AddTo(AugmentManager.current.augments[0].Create(), this);
        //inventory.AddTo(AugmentManager.current.augments[0].Create(), this);
        //inventory.AddTo(AugmentManager.current.augments[0].Create(), this);
        //inventory.AddTo(AugmentManager.current.augments[0].Create(), this);
        //inventory.AddTo(AugmentManager.current.augments[0].Create(), this);
        //inventory.AddTo(AugmentManager.current.augments[0].Create(), this);
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        if (hp <= 0)
        {
            Debug.Log("Player Load lose");
            GameManager.current.ChangeStateImmdeiate(GameState.Transitional);
            mapGenerationScript.SaveNewHighscore();
            thisBTSManager.LoadLoseGameScene();

        }
        if (GameManager.current.GameTransitional()) return;  

        if (hp >= 6)
        {
            SoundManager.current.levelMusic.setParameterByName("life", 1.0f);
        }
        else
        {
            SoundManager.current.levelMusic.setParameterByName("life", hp / 6.0f);
        }

        if (!isDashing)
        {
            HandleReload();
            HandleRotation();
            HandleMovement();
            HandleShooting();
            dashShield.SetActive(false);
        }
        else
        {
            dashShield.SetActive(true);
        }
        HandleDashing();
        UpdateAnimation();

        currentImmunityFrame -= Time.deltaTime;

        Debug.DrawLine(transform.position, transform.position+(ReturnPossibleNewPosition(12, new Vector3())));
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

    //protected void ReceiveGold(float amount)
    //{
    //    gold += amount;
    //}

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

    public Vector3 ReturnPossibleNewPosition(float projectileSpeed,Vector3 ShooterPos)
    {
        Vector3 combinedLastKnownPos = new Vector3();
        for(int i=0; i< lastKnownPos.Length; i++)
        {
            combinedLastKnownPos += lastKnownPos[i];
        }
        combinedLastKnownPos= combinedLastKnownPos / lastKnownPos.Length;

        // sloppy, doesn't recheck
        float holdingDistance = Vector3.Distance(transform.position, ShooterPos); // dist from shooter
        float holdOverTime = (holdingDistance / projectileSpeed) * stats.moveSpeed.value; // how far player can get

      //  Vector3 holdingNormal = Vector3.Normalize((combinedLastKnownPos)); //direction based on last movement

        
        return combinedLastKnownPos * holdOverTime;
    }

    public void UpdateLastKnownPos(Vector3 input)
    {
        if (currentLastKnownPosTimer <= 0)
        {
        for (int i = 0; i < lastKnownPos.Length-1; i++)
        {
            lastKnownPos[lastKnownPos.Length - 1 - i] = lastKnownPos[lastKnownPos.Length - 2 - i];
        }
        lastKnownPos[0] = input;
            currentLastKnownPosTimer += 0.1f;
        }
        else
        {
            currentLastKnownPosTimer -= Time.deltaTime;
        }
    }

    void HandleRotation()
    {
        // Cast a ray from screen point
        Vector3 mousePosition = Input.mousePosition;
        //Debug.Log(mousePosition.y);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Save the info
        RaycastHit hit;
        // You successfully hit
        if (Physics.Raycast(ray, out hit, 100, 1 << 18))
        {
            Vector3 dir = hit.point - gunpointForAiming.position;
            dir.y = 0;
            transform.localRotation = Quaternion.LookRotation(dir);
            angle = transform.localRotation.eulerAngles.y;
        }
    }

    void HandleMovement()
    {
   
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();

        if (movement.x != 0 || movement.y != 0)
        {
            lastMovementDirection = movement;
            rocketExitPoint.LookAt(transform);
        }
    }

    void HandleDashing()
    {
        currentTimeBetweenDashes -= Time.deltaTime;
        trailTime += Time.deltaTime;
        if (trailTime > dashDuration)
        {
            trailRenderer.emitting = false;
            isDashing = false;
        }
        if (Input.GetKeyDown(KeyCode.Space) && currentTimeBetweenDashes <= 0 && movement.magnitude > 0.01)
        {
            animator.SetTrigger("Roll");
            SoundManager.PlaySound(SoundType.Roll, transform.position, 1);
            isDashing = true;
            trailTime = 0;
            trailRenderer.emitting = true;
            currentTimeBetweenDashes = stats.timeBetweenDashs.value;
            currentImmunityFrame = stats.immunityFromDashing.value;
            transform.position = new Vector3(transform.position.x, 0.12f, transform.position.z);

            dashDirection = movement;
            transform.localRotation = Quaternion.LookRotation(new Vector3(dashDirection.x, 0, dashDirection.y));
            angle = transform.localRotation.eulerAngles.y;
            // characterController.Move(new Vector3(lastMovementDirection.x * stats.dashAmount.value, 0, lastMovementDirection.y * stats.dashAmount.value));
            rocketExitPoint.position = transform.position;
            ResolveDashEffects();
        }

        //animator.SetBool("Roll", isDashing);

    }

    void HandleShooting()
    {
        timeSinceFired += Time.deltaTime;
        if (reloading) return;
        if (inventory.Count == 0) return;
        if (inventoryIndex >= inventory.Count) return;
        if (Input.GetMouseButtonDown(0) || (timeSinceFired > stats.timeBetweenShots.value && Input.GetMouseButton(0)))
        {
            timeSinceFired = 0;
            // do shooting logic
            inventory[inventoryIndex].Shoot(this, bulletContainer, inventoryIndex);
            inventoryIndex++;
            freshReload = false;

            if (inventoryIndex >= inventory.Count)
            {
                SoundManager.PlaySound(SoundType.Gunshot, transform.position, 1);

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 17)
        {
            // Debug.Log("health");
            hp++;
            other.gameObject.SetActive(false);

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
        //if (currentBulletClip > 0)
        //{
        //    if (stats.deskBulletStatPercentage.value > 0 && 7 < UnityEngine.Random.RandomRange(0, 10))
        //    {
        //        Debug.Log("desk");
        //        GameObject desk = Instantiate(deskBulletPrefab, bulletContainer);
        //        Ammo ammoComponent = desk.GetComponent<Ammo>();
        //        Vector3 forward = bulletContainer.forward;
        //        ammoComponent.Init(this, forward, angle, bulletStats.speed.value*stats.deskBulletStatPercentage.value, stats.damageMultiplier.value * bulletStats.damage.value * stats.deskBulletStatPercentage.value, bulletStats.size.value);
        //        desk.transform.SetParent(null);
        //        currentBulletClip--;
        //        if (freshReload)
        //        {
        //            ammoComponent.fromFreshReload = true;
        //        }
        //        return true;
        //    }
        //    else
        //    {
        //        Bullet bullet;
        //        if (ammoPool.bulletPool.TryInstantiate(out bullet, bulletContainer.position, bulletContainer.rotation))
        //        {
        //            Ammo ammoComponent = bullet.GetComponent<Ammo>();
        //            Vector3 forward = bulletContainer.forward;
        //            ammoComponent.Init(this, forward, angle, bulletStats.speed.value, stats.damageMultiplier.value * bulletStats.damage.value, bulletStats.size.value);
        //            currentBulletClip--;

        //            if (stats.bulletPiercing.value > 0)
        //            {
        //                bullet.piercing = true;
        //            }

        //            if (stats.bulletPiercing.value > 0)
        //            {
        //                bullet.piercing = true;
        //            }
        //            if (freshReload)
        //            {
        //                ammoComponent.fromFreshReload = true;
        //            }
        //            return true;
        //        }
        //    }
        //}
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
                ammoComponent.Init(this, forward, angle, rocketStats.speed.value, stats.damageMultiplier.value * rocketStats.damage.value, rocketStats.size.value, 2);
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
                ammoComponent.Init(this, forward, angle, bulletStats.speed.value, stats.damageMultiplier.value * bulletStats.damage.value, bulletStats.size.value, 2);
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
            ammoComponent.Init(this, forward, angle, laserStats.speed.value, stats.damageMultiplier.value * laserStats.damage.value, laserStats.size.value, 2);
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
            ammoComponent.Init(this, forward, angle, bouncingBladeStats.speed.value, stats.damageMultiplier.value * bouncingBladeStats.damage.value, bouncingBladeStats.size.value, 2);
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
        if (ShootBullet(UnityEngine.Random.Range(-angle, angle))) shot = true;
        if (ShootGrenade(UnityEngine.Random.Range(-angle, angle))) shot = true;
        if (ShootRocket(UnityEngine.Random.Range(-angle, angle))) shot = true;
        if (ShootLaser(UnityEngine.Random.Range(-angle, angle))) shot = true;
        if (ShootBouncingBlade(UnityEngine.Random.Range(-angle, angle))) shot = true;
        return shot;
    }

    void HandleReload()
    {

        if (inventoryIndex >= inventory.Count)
        {
            if (Input.GetMouseButton(0) && !reloading)
            {
                reloading = true; // swap status
                currentReloadTime = 0;
                SoundManager.PlaySound(SoundType.Reloading, transform.position, 1);

            }
        }

        if (Input.GetKeyDown(KeyCode.R) && !reloading)
        {
            reloading = true;
            currentReloadTime = 0;
            SoundManager.PlaySound(SoundType.Reloading, transform.position, 1);
        }

        if (reloading)
        {
            currentReloadTime += Time.deltaTime;
            if (currentReloadTime >= stats.reloadTime.value)
            {
                Reload();
            }
        }
    }

    public void Reload()
    {
        currentReloadTime = 0;
        inventoryIndex = 0;
        reloading = false;
        if (stats.extraDamageAfterReload.value > 1)
        {
            freshReload = true;
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
                ammoComponent.Init(this, forward, angle, bulletStats.speed.value, stats.damageMultiplier.value * bulletStats.damage.value, bulletStats.size.value, 2);
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
        
        if (isDashing)
        {
            characterController.Move(
                new Vector3(
                    dashDirection.x * stats.dashAmount.value * Time.fixedDeltaTime,
                    0,
                    dashDirection.y * stats.dashAmount.value * Time.fixedDeltaTime
                    )
                );
            UpdateLastKnownPos((new Vector3(dashDirection.x * stats.dashAmount.value * Time.fixedDeltaTime,0,dashDirection.y * stats.dashAmount.value * Time.fixedDeltaTime)));
        }
        else
        {
            characterController.Move(
                new Vector3(
                    movement.x * stats.moveSpeed.value * Time.fixedDeltaTime, 
                    0, 
                    movement.y * stats.moveSpeed.value * Time.fixedDeltaTime
                    )
                );
            UpdateLastKnownPos(new Vector3(movement.x * stats.moveSpeed.value * Time.fixedDeltaTime,0,movement.y * stats.moveSpeed.value * Time.fixedDeltaTime));
        }


    }

    public override void OnDestroy()
    {
        GameManager.current.ChangeStateImmdeiate(GameState.Transitional);
        base.OnDestroy();
    }

    public void SellAugment(int index)
    {
        if (GameManager.current.GetState() != GameState.Shop) return;
        Augment augment = inventory[index];
        //AugmentManager augmentManager = AugmentManager.current;


        if (inventory.RemoveAt(index))
        {
            gold += 1;
            sellAugment?.Invoke();
        }
        //    for (int i = 0; i < augmentManager.augmentDatas[augment.id].synergies.Count; i++)
        //    {
        //        SynergyData synergyData = augmentManager.augmentDatas[augment.id].synergies[i];
        //        for (int j = 0; j < synergies.Count; j++)
        //        {
        //            if (synergies[j].id == synergyData.id)
        //            {
        //                if (synergies[j].count == 1)
        //                {
        //                    synergies.RemoveAt(j);
        //                    break;
        //                }
        //                else
        //                {
        //                    synergies[j].count--;
        //                    break;
        //                }
        //            }
        //        }
        //    }

        //    gold += augmentManager.costs[augmentManager.augmentDatas[augment.id].rarity] * (augment.level + 1);
        //    sellAugment?.Invoke();
        //}
    }

    public override void ResolveOnTakenDamageEffects(Vector3 direction)
    {

        //cameraFollowingScript.MoveCameraInDirection(direction);

        //if (stats.personalSpace.value > 1)
        //{
        //CreatePlayerAOE(new Vector2(transform.position.x, transform.position.z));
        //}

        CameraShake.current.Shake(magX, magY, cameraShakeTime, curve);
    }

    public void ReceiveGold()
    {
        gold = 10;
    }
}