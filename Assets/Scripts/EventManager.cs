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

    public event Action<Ammo, GameObject, Vector2> onAmmoHit;

    public event Action<float,Character, GameObject> onLaserHit;

    public event Action<float> receiveGold;

    public event Action<Vector3> enemyDeath;

    public void OnAmmoHit(Ammo ammo, GameObject obj,Vector2 impact)
    {
        onAmmoHit?.Invoke(ammo, obj,impact);
    }

    public void OnLaserHit(float damage, Character owner, GameObject obj)
    {
        onLaserHit?.Invoke(damage,owner, obj);
    }

    public void ReceiveGold(float Amount)
    {
        receiveGold?.Invoke(Amount);
    }

    public void EnemyDeath(Vector3 pos)
    {
        enemyDeath?.Invoke(pos);
    }

    public event Action<GameObject> onAmmoDestroy;
    public void OnAmmoDestroy(GameObject self)
    {
        onAmmoDestroy?.Invoke(self);
    }
}
