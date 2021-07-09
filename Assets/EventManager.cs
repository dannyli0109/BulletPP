using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager current;

    private void Awake()
    {
        current = this;
    }

    public event Action<Ammo> onAmmoHit;
    public void OnAmmoHit(Ammo ammo)
    {
        onAmmoHit?.Invoke(ammo);
    }

    public event Action<GameObject> onAmmoDestroy;
    public void OnAmmoDestroy(GameObject self)
    {
        onAmmoDestroy?.Invoke(self);
    }
}
