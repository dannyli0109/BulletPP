using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class AugmentData : ScriptableObject
{
    [HideInInspector]
    public int id;
    [HideInInspector]
    public int index;

    public int rarity;
    public Sprite augmentIcon;
    public string augmentName;
    [TextArea(5, 10)]
    public string description;
    public int cost;
    public Color color;

    public float damage;
    public float speed;
    public float size;
    public float angles;
    public int amountOfBullets;
    public float lifeTime;

    public abstract Augment Create();


}
