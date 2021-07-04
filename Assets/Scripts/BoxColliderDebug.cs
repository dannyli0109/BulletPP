using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxColliderDebug : MonoBehaviour
{
    LineRenderer lineRenderer;
    BoxCollider2D boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 boxSize = boxCollider.size;
        Vector3 p1 = new Vector3(transform.position.x - boxSize.x / 2, transform.position.y + boxSize.y / 2, 0);
        Vector3 p2 = new Vector3(transform.position.x + boxSize.x / 2, transform.position.y + boxSize.y / 2, 0);
        Vector3 p3 = new Vector3(transform.position.x + boxSize.x / 2, transform.position.y - boxSize.y / 2, 0);
        Vector3 p4 = new Vector3(transform.position.x - boxSize.x / 2, transform.position.y - boxSize.y / 2, 0);


        // Debug.DrawLine

        lineRenderer.SetPosition(0, p1);
        lineRenderer.SetPosition(1, p2);
        // lineRenderer.SetPosition(2, p3);
        // lineRenderer.SetPosition(3, p4);
    }
}
