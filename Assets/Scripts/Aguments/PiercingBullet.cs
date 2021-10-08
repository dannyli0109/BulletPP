using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Augments/PiercingBullet")]

public class PiercingBullet : Augment
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
        SoundManager.PlaySound(SoundType.Gunshot, transform.position, 1);
        AmmoPool ammoPool = AmmoPool.current;
        PiercingAmmo holdingPiercingAmmo;
        if (ammoPool.piercingAmmoPool.TryInstantiate(out holdingPiercingAmmo, transform.position, transform.rotation))
        {

            PiercingAmmo piercingAmmoComponent = holdingPiercingAmmo.GetComponent<PiercingAmmo>();
            Vector3 forward = transform.forward;
            piercingAmmoComponent.Init(character, forward, 0, speed, damage, size);
        }
    }
}

