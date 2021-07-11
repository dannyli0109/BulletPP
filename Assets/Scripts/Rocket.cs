using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Ammo
{

    private void FixedUpdate()
    {
        transform.position += transform.forward * owner.rocketStats.speed.value * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // do contact damage
        if (TimesBounced < owner.rocketStats.amountOfBounces.value)
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
