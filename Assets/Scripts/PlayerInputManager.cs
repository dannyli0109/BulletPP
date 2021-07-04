using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public Vector2 positionOnScreen;
    public Vector2 mouseOnScreen;
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
        positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
        float angle = AngleBetweenTwoPoints(mouseOnScreen, positionOnScreen) + 90;
        transform.localRotation = Quaternion.Euler(new Vector3(0f, angle, 0f));

        Vector3 lookDir = transform.transform.forward * 1000;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + lookDir);

        Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        dir.Normalize();

        // rotate x and y so that it matches the forward axis
        float sin = Mathf.Sin(angle * Mathf.Deg2Rad);
        float cos = Mathf.Cos(angle * Mathf.Deg2Rad);
        float tx = dir.x;
        float ty = dir.y;
        dir.x = (cos * tx) - (sin * ty);
        dir.y = (sin * tx) + (cos * ty);

        Debug.Log(dir);

        playerAnimator.SetFloat("x", dir.x);
        playerAnimator.SetFloat("y", dir.y);

    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(-(a.y - b.y), a.x - b.x) * Mathf.Rad2Deg;
    }
}
