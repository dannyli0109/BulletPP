using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Augment
{
    public int id;
    public int rarity;
    public Sprite augmentIcon;
    public string augmentName;
    public int cost;
    public Color color;

    public float damage;
    public float speed;
    public float size;
    public float lifeTime;
    public float angles;
    public int amountOfBullets;

    public string originalDesc;

    public Augment (AugmentData data)
    {
        id = data.id;
        rarity = data.rarity;
        augmentIcon = data.augmentIcon;
        cost = data.cost;
        color = data.color;

        damage = data.damage;
        speed = data.speed;
        size = data.size;
        lifeTime = data.lifeTime;
        angles = data.angles;
        amountOfBullets = data.amountOfBullets;

        originalDesc = data.description;
    }

    public abstract string description
    {
        get;
    }
    public abstract void OnAttached(Character character, int index);

    public abstract void Shoot(Character character, Transform transform, int index);
}
