using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Augment : ScriptableObject
{
    [HideInInspector]
    public int id;
    public int rarity;
    public Sprite augmentIcon;
    public string augmentName;
    public int cost;
    public Color color;
    [TextArea(5, 10)]
    public string description;

    public List<Action<Ammo>> OnAttached;
    public List<Action<Ammo>> OnHit;
    public List<Action<Ammo>> OnDamage;
    public List<Action<Ammo>> OnBounce;
    public List<Action<Ammo>> OnCrit;

    public abstract void Init();

    public void InitEvents()
    {
        OnAttached = new List<Action<Ammo>>();
        OnHit = new List<Action<Ammo>>();
        OnDamage = new List<Action<Ammo>>();
        OnBounce = new List<Action<Ammo>>();
        OnCrit = new List<Action<Ammo>>();
    }

    public void AddOnAttachedEvent(Action<Ammo> action)
    {
        OnAttached.Add(action);
    }

    public void AddOnHitEvent(Action<Ammo> action)
    {
        OnHit.Add(action);
    }

    public void AddOnDamageEvent(Action<Ammo> action)
    {
        OnDamage.Add(action);
    }

    public void AddOnBounceEvent(Action<Ammo> action)
    {
        OnBounce.Add(action);
    }

    public void AddOnCritEvent(Action<Ammo> action)
    {
        OnCrit.Add(action);
    }

    public abstract void Shoot(Character character, Transform transform);
}
