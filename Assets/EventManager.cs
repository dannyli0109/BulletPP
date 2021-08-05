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

    public event Action<Ammo, GameObject> onAmmoHit;

    public event Action<float,Character, GameObject> onLaserHit;

    public event Action<float> receiveGold;

    public void OnAmmoHit(Ammo ammo, GameObject obj)
    {
        onAmmoHit?.Invoke(ammo, obj);
    }

    public void OnLaserHit(float damage, Character owner, GameObject obj)
    {
        onLaserHit?.Invoke(damage,owner, obj);
    }

    public void ReceiveGold(float Amount)
    {
        receiveGold?.Invoke(Amount);
    }

    public event Action<GameObject> onAmmoDestroy;
    public void OnAmmoDestroy(GameObject self)
    {
        onAmmoDestroy?.Invoke(self);
    }
}
