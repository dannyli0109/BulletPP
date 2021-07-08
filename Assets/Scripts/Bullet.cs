using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Ammo
{
    public Character owner;
    float bornTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bornTime += Time.deltaTime;
        if (bornTime >= owner.bulletStats.travelTime.value)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * owner.bulletStats.speed.value * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Destroy(gameObject);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            Character character = other.gameObject.GetComponent<Character>();

            if (character && character != owner)
            {
                character.hp -= owner.bulletStats.damage.value;
                Destroy(gameObject);
            }
        }
    }
}
