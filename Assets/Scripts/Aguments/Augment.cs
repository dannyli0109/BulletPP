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
    protected Color color;

    public float damage;
    public float speed;
    public float size;
    public float lifeTime;
    public float angles;
    public float offset;
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
        offset = data.offset;
        amountOfBullets = data.amountOfBullets;
        augmentName = data.augmentName;

        originalDesc = data.description;
    }

    public abstract string description
    {
        get;
    }
    public abstract void OnAttached(Character character, int index);

    public void OnDamageChanged(int index)
    {
        InventoryHUD.current.AnimateDamageText(index);
    }

    public void OnAmmoChanged(int index)
    {
        InventoryHUD.current.AnimateAmountText(index);
    }

    public abstract void Shoot(Character character, Transform transform, int index);

    public abstract float GetDamage(Character character, int index);

    public abstract int GetAmounts(Character character, int index);

    public abstract Color GetColor(Character character, int index);

    public abstract int GetId(Character character, int index);
}
