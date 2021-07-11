using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Ammo
{

    void Update()
    {
        if (CurrentTimeTillRemoval <= 0)
        {
            Destroy(gameObject);
            // have explosion
        }
        else
        {
            CurrentTimeTillRemoval -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * owner.grenadeStats.speed.value * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer!= 12)
        {
        // do contact damage
        if (TimesBounced < owner.grenadeStats.amountOfBounces.value)
        {
            gameObject.transform.Rotate(new Vector3(0, Random.RandomRange(155, 205), 0));
            TimesBounced++;

        }
        else
        {
            Destroy(gameObject);


        }
        }
    }
}
