using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    bool wasMoving;

    public float currentSpeed;
    public float startSpeed=1;
    public float maxSpeed = 2;
    public float acceleration = 1;

    public float MaxBoostTime;
    public float currentBoostTime;
    public float initBoost;


    public CharacterController ourCharacterController;
    public CharacterController michael;
    public CharacterController shichael;

    public GameObject debugP;
    public Camera mainCamera;

    public float num;

    float xVal;
     float yVal;
    private void Start()
    {
        currentSpeed = startSpeed;
    }

    void Update()
    {
        xVal = Input.GetAxis("Horizontal");
        yVal = Input.GetAxis("Vertical");

        if (xVal != 0 || yVal != 0)
        {
            currentSpeed = Mathf.Clamp(currentSpeed += acceleration * Time.deltaTime, startSpeed, maxSpeed);

            Vector3 InputDirection = Vector3.Normalize(new Vector3(xVal, 0, yVal));
           // Debug.Log(InputDirection);


            float boost = Mathf.Clamp(Time.deltaTime, 0, currentBoostTime);
            currentBoostTime = Mathf.Clamp(currentBoostTime - Time.deltaTime, 0, MaxBoostTime);
            ourCharacterController.Move(InputDirection * (currentSpeed + currentBoostTime * initBoost) * Time.deltaTime);
            
        }
        else
        {

            currentSpeed = Mathf.Clamp(currentSpeed -= acceleration * 2 * Time.deltaTime, startSpeed, maxSpeed);
            currentBoostTime = Mathf.Clamp(currentBoostTime + Time.deltaTime, 0, MaxBoostTime);
        }
   
       
    }

    private void FixedUpdate()
    {
        /*
        if (xVal != 0 || yVal != 0)
        {
            currentSpeed = Mathf.Clamp(currentSpeed += acceleration * Time.deltaTime, startSpeed, maxSpeed);


            Vector3 InputDirection = Vector3.Normalize(new Vector3(xVal, 0, yVal));

            Debug.Log("X " + xVal + "  " + "  Y " + yVal + " acc  " + currentSpeed);

            if (currentBoostTime > 0)
            {
                float boost = Mathf.Clamp(Time.deltaTime, 0, currentBoostTime);
                currentBoostTime = Mathf.Clamp(currentBoostTime - Time.deltaTime, 0, MaxBoostTime);

                ourCharacterController.Move(InputDirection * (currentSpeed + currentBoostTime * initBoost) * Time.deltaTime);
            }
            else
            {

                ourCharacterController.Move(InputDirection * currentSpeed * Time.deltaTime);
            }
        }
        else
        {
            currentSpeed = Mathf.Clamp(currentSpeed -= acceleration * 2 * Time.deltaTime, startSpeed, maxSpeed);
            currentBoostTime = Mathf.Clamp(currentBoostTime + Time.deltaTime, 0, MaxBoostTime);
        }
        */
    }
}
