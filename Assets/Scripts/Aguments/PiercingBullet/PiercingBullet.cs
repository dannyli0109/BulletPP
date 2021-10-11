using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingBullet : Augment
{
    public PiercingBullet(PiercingBulletData data) : base(data)
    {

    }

    public override string description
    {
        get
        {
            return originalDesc;
        }
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
