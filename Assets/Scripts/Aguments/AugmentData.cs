using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct AugmentStats
{
    public AugmentStats(float initValue)
    {
        damage = initValue;
        speed = initValue;
        size = initValue;
        angles = initValue;
        offset = initValue;
        intialAngleOffset = initValue;
        amountOfBullets = initValue;
        lifeTime = initValue;
        explosiveRadius = initValue;
        homingRadius = initValue;
    }

    public float damage;
    public float speed;
    public float size;
    public float angles;
    public float offset;
    public float intialAngleOffset;
    public float amountOfBullets;
    public float lifeTime;
    public float explosiveRadius;
    public float homingRadius;
}


public abstract class AugmentData : ScriptableObject
{
    [HideInInspector]
    public int id;
    [HideInInspector]
    public int index;

    public int rarity;
    public Sprite augmentIcon;
    public List<SynergyData> synergies;

    public string augmentName;
    [TextArea(5, 10)]
    public string description;
    public int cost;
    public Color color;

    public AugmentStats stats;

    public abstract Augment Create();


}
