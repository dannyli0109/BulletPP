using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public Vector3 mousePos;
    public Vector3 mouseWorldPos;
    public Vector3 playerPos;
    public Camera camera;
    public GameObject model;
    public Animator playerAnimator;

    LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        // mousePos.y = mousePos.y;
        mousePos.z = camera.nearClipPlane;


        mouseWorldPos = camera.ScreenToWorldPoint(mousePos);

        playerPos = transform.position;
        
        //playerPos.z = camera.nearClipPlane;
        //playerPos.y = mouseWorldPos.y;

        Vector3 lookDir = mousePos - playerPos;
        lineRenderer.SetPosition(0, playerPos);
        lineRenderer.SetPosition(1, mouseWorldPos);


        float yRotation = Quaternion.LookRotation(lookDir, Vector3.up).eulerAngles.y;

        //atan2(v.y, v.x) * 180.0f / M_PI;

        float angle = Mathf.Atan2(lookDir.y, lookDir.x);
        float deg = Mathf.Rad2Deg * angle;
        //Debug.Log(deg);

        Vector3 eulerAngle = transform.rotation.eulerAngles;
        eulerAngle.y = deg;
        model.transform.rotation = Quaternion.Euler(eulerAngle);

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        playerAnimator.SetFloat("x", x);
        playerAnimator.SetFloat("y", y);

        // string str = "x: " + x + "y: " + y;
        // Debug.Log(str);

    }
}
