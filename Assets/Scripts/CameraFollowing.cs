using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    public Transform cameraHolder;
    public float cameraLerpAmount;
    public Transform target;

    public MapGeneration mapGenerationScript;
    public Player playerScript;

    public float gunPointDistMutliplier;
    public float maxGunPointDist;

    void FixedUpdate()
    {
        if (!target) return;

        Vector2 mouseOnScreen = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        mouseOnScreen.x = mouseOnScreen.x * Screen.width - Screen.width / 2.0f;
        mouseOnScreen.y = mouseOnScreen.y * Screen.height - Screen.height / 2.0f;

        float holdingDist = Mathf.Clamp(Vector2.Distance(new Vector2(0, 0), mouseOnScreen), 0, maxGunPointDist);
        //Debug.Log(holdingDist);


        Vector3 desiredPos = new Vector3(target.position.x, 0, target.position.z) + (playerScript.bulletContainer.transform.forward * (gunPointDistMutliplier*(holdingDist/maxGunPointDist)));

         desiredPos = mapGenerationScript.ClampCameraVectorToCameraBoundsOfCurrentRoom(desiredPos);


        cameraHolder.transform.position = Vector3.Lerp(cameraHolder.transform.position, desiredPos, cameraLerpAmount);
    }
}
