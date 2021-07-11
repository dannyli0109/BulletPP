using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{

    #region clipSize
    public int currentBulletClip;
    public int currentGrenadeClip;
    public int currentRocketClip;

    public int maxBulletClip = 1;
    public int maxGrenadeClip = 1;
    public int maxRocketClip = 1;
    #endregion 

    public Animator animator;
    public CharacterController characterController;

    float angle;
    Vector2 movement;

    public override void Start()
    {
        currentBulletClip = maxBulletClip;
        currentGrenadeClip = maxGrenadeClip;
        currentRocketClip = maxRocketClip;
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        HandleRotation();
        HandleMovement();
        HandleShooting();
        UpdateAnimation();
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
    }

    void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {

            if (currentBulletClip > 0)
            {

                GameObject bullet = Instantiate(bulletPrefab, bulletContainer);
                bullet.transform.SetParent(null);
                Bullet bulletComponent = bullet.GetComponent<Bullet>();
                bulletComponent.owner = this;
            }

            if (currentGrenadeClip > 0)
            {

                GameObject grenade = Instantiate(grenadePrefab, bulletContainer);
                grenade.transform.SetParent(null);
                Grenade grenadeComponent = grenade.GetComponent<Grenade>();
                grenadeComponent.owner = this;
            }

            if (currentRocketClip > 0)
            {

                GameObject rocket = Instantiate(rocketPrefab, bulletContainer);
                rocket.transform.SetParent(null);
                Rocket rocketComponent = rocket.GetComponent<Rocket>();
                rocketComponent.owner = this;
            }
        }
        else if (Input.GetMouseButton(0))
        {
            /*
            if (currentBulletClip > 0)
            {

                GameObject bullet = Instantiate(bulletPrefab, bulletContainer);
                bullet.transform.SetParent(null);
                Bullet bulletComponent = bullet.GetComponent<Bullet>();
                bulletComponent.owner = this;
            }

            if (currentGrenadeClip > 0)
            {

                GameObject grenade = Instantiate(grenadePrefab, bulletContainer);
              grenade.transform.SetParent(null);
                Grenade grenadeComponent = grenade.GetComponent<Grenade>();
                grenadeComponent.owner = this;
            }

            if (currentRocketClip > 0)
            {

                GameObject rocket = Instantiate(rocketPrefab, bulletContainer);
              rocket.transform.SetParent(null);
                Rocket rocketComponent = rocket.GetComponent<Rocket>();
                rocketComponent.owner = this;
            }
            */
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