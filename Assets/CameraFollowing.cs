using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    public Transform cameraHolder;
    public float cameraLerpAmount;
    public Transform target;
    // Start is called before the first frame update

    // Update is called once per frame
    void FixedUpdate()
    {
        cameraHolder.transform.position = Vector3.Lerp(cameraHolder.transform.position, new Vector3(target.position.x, 0, target.position.z), cameraLerpAmount);
    }
}
