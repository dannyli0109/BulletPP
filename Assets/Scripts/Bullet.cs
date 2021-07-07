using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Ammo
{
    public Character owner;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * owner.bulletSpeed.value * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Bullet trigger");
        Destroy(gameObject);
    }
}
