using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Ammo
{
    public GameObject bulletHitParticlePrefab;
    public Transform bulletTip;
    float bornTime = 0;
    void Update()
    {
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * owner.grenadeStats.speed.value * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // wall, character, enemy
        if (other.gameObject.layer == 10|| other.gameObject.layer == 11 || other.gameObject.layer == 12)
        {
            // do contact damage
            if (TimesBounced < owner.grenadeStats.amountOfBounces.value)
            {
                gameObject.transform.Rotate(new Vector3(0, Random.Range(155, 205), 0));
                TimesBounced++;
                Debug.Log("bounce off  " + other);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
