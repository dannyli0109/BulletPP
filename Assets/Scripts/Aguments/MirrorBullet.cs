using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Augments/MirrorBullet")]
public class MirrorBullet : Augment
{
    public override void Init()
    {
        InitEvents();
    }

    public override void Shoot(Character character, Transform transform)
    {
        if (index <= 0) return;
        character.inventory[index - 1].index = index - 1;
        if (index - 1 <= 0) return;
        character.inventory[index - 1].Shoot(character, transform);
    }
}