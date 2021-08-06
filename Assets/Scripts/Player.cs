using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : Character
{
    #region movementStats
    public CharacterStat dashAmount;
    public CharacterStat timeBetweenDashs;
    public Vector2 lastMovementDirection;
    public float currentTimeBetweenDashes;

    public CharacterStat immunityFromDashing;
    public CharacterStat immunityFromDamage;

    #endregion

    #region Gun Stats
    public CharacterStat outOfCombatReloadTime;
    public float currentReloadTime;

    #endregion

    #region clipSize
    public int currentBulletClip;
    public int currentGrenadeClip;
    public int currentRocketClip;

    #endregion

    #region AmmoUI
    public GameObject bulletsUI;
    public TextMeshProUGUI bulletAmmoText;
    public GameObject grenadesUI;
    public TextMeshProUGUI grenadeAmmoText;
    public GameObject rocketsUI;
    public TextMeshProUGUI rocketAmmoText;
    public GameObject laserUI;
    public TextMeshProUGUI laserFuelText;

    public Color32 filledClipColor;
    public Color32 emptyClipColor;
    #endregion

    public BTSManager thisBTSManager;

    public CharacterController characterController;

    float angle;
    Vector2 movement;

    public MapGeneration mapGenerationScript;

    public override void Start()
    {
        EventManager.current.receiveGold += ReceiveGold;

        currentBulletClip = (int)bulletStats.maxClip.value;
        currentGrenadeClip = (int)grenadeStats.maxClip.value;
        currentRocketClip = (int)rocketStats.maxClip.value;
        currentLaserFuel = laserStats.maxClip.value;
        base.Start();
        UpdatePlayerUI();
    }

    public override void Update()
    {
        if (hp <= 0)
        {
            Debug.Log("Player Load lose");
            thisBTSManager.LoadLoseGameScene();
        }
        if (GameManager.current.gameState == GameState.Shop) return;
        base.Update();
        HandleRotation();
        HandleMovement();
        HandleShooting();
        UpdateAnimation();
        HandleDashing();

        HandleReload();
        currentImmunityFrame -= Time.deltaTime;
        UpdateAllLasers();
    }

    private void FixedUpdate()
    {
        if (GameManager.current.gameState == GameState.Shop) return;
        MoveCharacter();
    }

    protected void ReceiveGold(float amount)
    {
        Debug.Log(amount + " gold");
        gold += amount;
        Debug.Log("total " + gold);         
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
        if (Input.GetKeyDown(KeyCode.Space) && currentTimeBetweenDashes<=0)
        {
            currentTimeBetweenDashes = timeBetweenDashs.value;
            currentImmunityFrame = immunityFromDashing.value;
            characterController.Move(new Vector3(lastMovementDirection.x * dashAmount.value,0,lastMovementDirection.y * dashAmount.value));
        }
    }

    void HandleShooting()
    {
        timeSinceFired += Time.deltaTime;
        if (reloading) return;
        if (Input.GetMouseButtonDown(0) || (timeSinceFired > timeBetweenShots.value && Input.GetMouseButton(0)))
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

            UpdatePlayerUI();
        }

        if (Input.GetMouseButton(0))
        {
            ShootLaser(-1, true);
        }
    }

    void ShootLaser(int id, bool UsesFuel)
    {
        //Debug.Log("Shooting laser");
        if (currentLaserFuel >0)
        {

        laserSustained = true;
        currentLazerLength = Mathf.Clamp(currentLazerLength + lazerGrowthSpeed * Time.deltaTime, 0, laserStats.maxLaserLength.value);
        currentLazerWidth = Mathf.Clamp(currentLazerWidth + lazerWidthGrowth * Time.deltaTime, 0, laserStats.maxLaserWidth.value);

        Vector3 lookDir = gunTip.forward * currentLazerLength;
        thisLineRenderer.SetPosition(0, gunTip.position);
        thisLineRenderer.SetPosition(1, gunTip.position + lookDir);
        thisLineRenderer.SetWidth(currentLazerWidth, currentLazerWidth);

        LazerCollider.GetComponent<BoxCollider>().center = new Vector3(0, 1.3f, currentLazerLength / 2);
        LazerCollider.GetComponent<BoxCollider>().size = new Vector3(currentLazerWidth, 1, currentLazerLength);
        currentLaserFuel = Mathf.Clamp(currentLaserFuel - Time.deltaTime, 0, laserStats.maxClip.value);
        }
    }

    void UpdateAllLasers()
    {
        if (!laserSustained)
        {
            currentLazerLength = Mathf.Clamp(currentLazerLength - lazerRecoilSpeed * Time.deltaTime, 0, laserStats.maxLaserLength.value);
            currentLazerWidth = Mathf.Clamp(currentLazerWidth - lazerWidthGrowth * Time.deltaTime, 0, laserStats.maxLaserWidth.value);

            Vector3 lookDir = gunTip.forward * currentLazerLength;
            thisLineRenderer.SetPosition(0, gunTip.position);
            thisLineRenderer.SetPosition(1, gunTip.position + lookDir);
            thisLineRenderer.SetWidth(currentLazerWidth, currentLazerWidth);

            LazerCollider.GetComponent<BoxCollider>().center = new Vector3(0, 1.3f, currentLazerLength / 2);
            LazerCollider.GetComponent<BoxCollider>().size = new Vector3(currentLazerWidth, 1, currentLazerLength);
        }
        laserSustained = false;
    }

    void HandleReload()
    {
        /*
        if (
            currentBulletClip == bulletStats.maxClip.value && 
            currentGrenadeClip == grenadeStats.maxClip.value && 
            currentRocketClip == rocketStats.maxClip.value && 
            currentLaserFuel == maxLaserFuel.value
            )
        {
            // we have max clips
            reloading = false;
        }
        else
        {
            //if( !mapGenerationScript.InCombat)
            //{
            //    Reloading = true;
            //}
            if (Input.GetKeyDown(KeyCode.R) && !reloading)
            {
                reloading = !reloading; // swap status
                currentReloadTime = 0;
            }
            if (reloading)
            {
                currentReloadTime += Time.deltaTime;
                // float holdingTime = reloadTime.value;
                //if (!mapGenerationScript.InCombat)
                //{
                //    holdingTime = outOfCombatReloadTime.value;
                //}
                // currentLaserFuel = Mathf.Clamp(currentLaserFuel += Time.deltaTime, 0, maxLaserFuel.value);
                if (currentReloadTime >= reloadTime.value)
                {
                    // currentReloadTime = 0;
                    // check if you actually get

                    
                    if (currentBulletClip < bulletStats.maxClip.value)
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
                    

                    currentReloadTime = 0;
                    currentBulletClip = (int)bulletStats.maxClip.value;
                    currentGrenadeClip = (int)grenadeStats.maxClip.value;
                    currentRocketClip = (int)rocketStats.maxClip.value;
                    currentLaserFuel = maxLaserFuel.value;
                    reloading = false;
                    UpdatePlayerUI();
                }
            }
        }
        */

        if (
                currentBulletClip == 0 &&
                currentGrenadeClip == 0 &&
                currentRocketClip == 0 &&
                currentLaserFuel <= 0
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
            if (currentReloadTime >= reloadTime.value)
            {
                currentReloadTime = 0;
                currentBulletClip = (int)bulletStats.maxClip.value;
                currentGrenadeClip = (int)grenadeStats.maxClip.value;
                currentRocketClip = (int)rocketStats.maxClip.value;
                currentLaserFuel = laserStats.maxClip.value;
                reloading = false;
                UpdatePlayerUI();
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
        characterController.Move(new Vector3(movement.x * moveSpeed.value * Time.fixedDeltaTime, 0, movement.y * moveSpeed.value * Time.fixedDeltaTime));
    }

    void UpdatePlayerUI()
    {
        if (bulletStats.maxClip.value > 0)
        {
            bulletsUI.SetActive(true);
            bulletAmmoText.text = currentBulletClip.ToString();
            if (currentBulletClip == 0)
            {
                bulletAmmoText.color = emptyClipColor;
            }
            else
            {
                bulletAmmoText.color = filledClipColor;
            }
        }
        else
        {
            bulletsUI.SetActive(false);
        }

        if (grenadeStats.maxClip.value > 0)
        {
           grenadesUI.SetActive(true);
            grenadeAmmoText.text = currentGrenadeClip.ToString();
            if (currentGrenadeClip == 0)
            {
               grenadeAmmoText.color = emptyClipColor;
            }
            else
            {
                grenadeAmmoText.color = filledClipColor;
            }
        }
        else
        {
            grenadesUI.SetActive(false);
        }

        if (rocketStats.maxClip.value > 0)
        {
            rocketsUI.SetActive(true);
            rocketAmmoText.text = currentRocketClip.ToString();
            if (currentRocketClip == 0)
            {
               rocketAmmoText.color = emptyClipColor;
            }
            else
            {
                rocketAmmoText.color = filledClipColor;
            }
        }
        else
        {
            rocketsUI.SetActive(false);
        }

        if (laserStats.maxClip.value > 0)
        {
            laserUI.SetActive(true);
            laserFuelText.text = currentLaserFuel.ToString("F");
            if (currentLaserFuel == 0)
            {
               laserFuelText.color = emptyClipColor;
            }
            else
            {
                laserFuelText.color = filledClipColor;
            }
        }
        else
        {
            laserUI.SetActive(false);
        }


    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

}