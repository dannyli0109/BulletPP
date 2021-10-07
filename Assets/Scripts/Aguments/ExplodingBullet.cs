using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Augments/ExplodingBullet")]
public class ExplodingBullet : Augment
{
    public int damage;
    public float speed;
    public float size;
    public override void Init()
    {
        InitEvents();
    }

    public override void Shoot(Character character, Transform transform)
    {
        AmmoPool ammoPool = AmmoPool.current;
        Rocket rocket;
        if (ammoPool.rocketPool.TryInstantiate(out rocket, transform.position, transform.rotation))
        {
            Debug.Log("explode");
            Rocket rocketComponent = rocket.GetComponent<Rocket>();
            Vector3 forward = transform.forward;
            rocketComponent.Init(character, forward, 0, speed, damage, size);
        }
    }
}
