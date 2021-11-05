using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Augment
{
    public int id;
    public int rarity;
    public Sprite augmentIcon;
    public List<SynergyData> synergies;
    public string augmentName;
    public int cost;
    protected Color color;

    //public float damage;
    //public float speed;
    //public float size;
    //public float lifeTime;
    //public float angles;
    //public float offset;
    //public int amountOfBullets;

    public AugmentStats stats;
    public AugmentStats tempStats = new AugmentStats(0);
    public AugmentStats tempStatMultipliers = new AugmentStats(1);

    public string originalDesc;

    public Augment (AugmentData data)
    {
        id = data.id;
        rarity = data.rarity;
        augmentIcon = data.augmentIcon;
        synergies = data.synergies;

        cost = data.cost;
        color = data.color;

        stats = data.stats;

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

    public abstract float GetSpeed(Character character, int index);

    public abstract float GetSize(Character character, int index);

    public abstract float GetAngles(Character character, int index);

    public abstract float GetLifeTime(Character character, int index);

    public abstract float GetExplosiveRadius(Character character, int index);

    public void ResetTempStats()
    {
        tempStats = new AugmentStats(0);
        tempStatMultipliers = new AugmentStats(1);
    }

    public abstract Color GetColor(Character character, int index);

    public abstract int GetId(Character character, int index);
}
