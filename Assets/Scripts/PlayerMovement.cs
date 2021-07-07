using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed;
    public CharacterController characterController;
    public GameObject cameraHolder;
    Vector2 movement;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        //Debug.Log(movement)
    }

    private void FixedUpdate()
    {
        //Vector3 newPos = new Vector3(rb.position.x + movement.x * Time.fixedDeltaTime, 0, rb.position.y + movement.y * Time.fixedDeltaTime);
        //rb.MovePosition(newPos);
        ////rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        //transform.Translate(new Vector3(movement.x * moveSpeed * Time.fixedDeltaTime, 0f, movement.y * moveSpeed * Time.fixedDeltaTime));
        characterController.Move(new Vector3(movement.x * moveSpeed * Time.fixedDeltaTime, 0, movement.y * moveSpeed * Time.fixedDeltaTime));
        cameraHolder.transform.position = Vector3.Lerp(cameraHolder.transform.position, new Vector3(transform.position.x, 0, transform.position.z), 0.1f);

    }
}
