using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{

    public Animator animator;
    public CharacterController characterController;

    float angle;
    Vector2 movement;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
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
        if (Input.GetMouseButton(0))
        {
            timeSinceFired += Time.deltaTime;

            if (timeSinceFired >= bulletStats.fireRate.value || Input.GetMouseButtonDown(0))
            {
                Shoot();
                timeSinceFired = 0;
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